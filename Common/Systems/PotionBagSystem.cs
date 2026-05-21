using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items;
using Fargowiltas.Content.NPCs;
using Fargowiltas.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;

namespace Fargowiltas.Common.Systems
{
    public class PotionBagSystem : ModSystem
    {
        #region Save Data
        public override void SaveWorldData(TagCompound tag)
        {
            var list = new List<TagCompound>();
            foreach (var potion in _potions)
            {
                list.Add(new TagCompound()
                {
                    { "ItemID", potion.Key },
                    { "Count", potion.Value }
                });
            }
            tag["StoredPotions"] = list;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            _potions = [];
            if (tag.TryGet<IList<TagCompound>>("StoredPotions", out var list))
            {
                foreach (var potion in list)
                {
                    _potions.Add(potion.Get<ItemDefinition>("ItemID"), potion.Get<int>("Count"));
                }
            }
            base.LoadWorldData(tag);
        }
        #endregion

        private static Dictionary<ItemDefinition, int> _potions = [];
        public static Dictionary<ItemDefinition, int> Potions => _potions.Where(p => !p.Key.IsUnloaded).ToDictionary();

        public static int MaxPotions => FargoServerConfig.Instance.UnlimitedPotionBuffsAmount;

        #region Netcode
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Potions.Count);
            foreach (var potion in Potions)
            {
                writer.Write(potion.Key.Type);
                writer.Write(potion.Value);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            Dictionary<ItemDefinition, int> newList = [];
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                ItemDefinition id = new(reader.ReadInt32());
                int count = reader.ReadInt32();
                newList[id] = count;
            }
            _potions = newList;
            Main.LocalPlayer.FargoMutant().NeedRefreshCooler = true;
        }
        #endregion

        /// <summary>
        /// Checks whether the potion of the given type has the full amount needed or not.
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="remainingNeeded">The amount of potions still needed to fill</param>
        /// <returns> <see langword="true"/> if the potion is not full yet, <see langword="false"/> otherwise</returns>
        public static bool PotionRequiresMore(int itemID, out int remainingNeeded)
        {
            if (_potions.TryGetValue(new(itemID), out int current))
                remainingNeeded = MaxPotions - current;
            else
                remainingNeeded = MaxPotions;
            return remainingNeeded > 0;
        }

        /// <summary>
        /// Adds the given amount of the given potion type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        public static void AddPotion(int type, int count)
        {
            ItemDefinition def = new(type);
            if (_potions.TryGetValue(def, out int currCount))
            {
                _potions[def] = currCount + count;
            }
            else
            {
                _potions.Add(def, count);
            }
        }

        /// <summary>
        /// Applies all buffs from the potion cooler to the given player
        /// </summary>
        /// <param name="player"></param>
        public static void ApplyCoolerBuffs(Player player)
        {
            FargoPlayer fargoPlayer = player.FargoMutant();
            fargoPlayer.ActiveFlask = -1;

            foreach (var potion in PotionBagSystem.Potions)
            {
                PotionBagSystem.TryApplyBuff(potion.Key.Type, player);
            }

            // update potion toggler
            foreach (var potToggle in PotionToggleLoader.LoadedToggles.Values)
            {
                if (player.HasBuff(potToggle.BuffID))
                {
                    fargoPlayer.ActivePotions.Add(potToggle.BuffID);
                }
                else if (player.buffImmune[potToggle.BuffID])
                {
                    fargoPlayer.ActivePotions.Remove(potToggle.BuffID);
                }
            }
        }

        /// <summary>
        /// Safely gets the count of the given type <para/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count">The count stored of the given type, or 0 if not found.</param>
        /// <returns>Whether the count was successfully found</returns>
        public static bool TryGetCount(int type, out int count)
        {
            ItemDefinition def = new(type);
            if (_potions.TryGetValue(def, out count))
            {
                if (count > MaxPotions)
                    _potions[def] = count = MaxPotions;
                return true;
            }
            count = 0;
            return false;
        }

        /// <summary>
        /// Whether a potion of the given type of the given count can be stored.
        /// </summary>
        /// <param name="type">The type of potion to consume</param>
        /// <param name="count">The amount of potions to consume</param>
        /// <param name="consumeAmount">How many potions would actually be consumed</param>
        /// <param name="leftovers">The remaining count that will be left after consumption (can be negative)</param>
        /// <returns></returns>
        public static bool CanConsumePotion(int type, int count, out int consumeAmount, out int leftovers)
        {
            if (PotionBagSystem.PotionRequiresMore(type, out int requiredCount))
            {
                leftovers = count - requiredCount;
                consumeAmount = leftovers <= 0 ? count : requiredCount;
                return true;
            }
            leftovers = count;
            consumeAmount = 0;
            return false;
        }

        /// <summary>
        /// Attempts to apply the buff of the given item type to the player
        /// </summary>
        /// <param name="type"></param>
        /// <param name="player"></param>
        public static void TryApplyBuff(int type, Player player)
        {
            Item item = new(type);
            TryGetCount(type, out int count);

            if (item.IsAir || !FargoServerConfig.Instance.PotionCooler || (FargoServerConfig.Instance.UnlimitedPotionBuffs is UnlimitedBuffSelections.BossOnly && !FargoGlobalNPC.AnyBossAlive()))
                return;

            if (count >= MaxPotions && item.buffType != 0 && item.buffTime >= 60 * 60 * 2)
            {
                player.FargoMutant().ActivePotions.Add(item.buffType);

                if (player.FargoMutant().PotionToggler.Toggles.Any(t => t.Value.BuffID == item.buffType && t.Value.ToggleBool))
                {
                    if (BuffID.Sets.IsAFlaskBuff[item.buffType])
                    {
                        // Only allow one flask
                        if (player.FargoMutant().ActiveFlask == -1)
                            player.FargoMutant().ActiveFlask = type;
                        else
                            return;
                    }
                    int duration = item.buffType == BuffID.Lucky ? item.buffTime : 2;
                    player.AddBuff(item.buffType, duration);
                }
            }
        }

        public static void EmptyBag()
        {
            _potions.Clear();
        }
    }
}
