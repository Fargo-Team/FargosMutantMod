using Fargowiltas.Utilities.Extensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems
{
    
    public class PotionToggleBackend : ToggleBackend
    {
        public readonly static string ConfigPath = Path.Combine(Main.SavePath, "ModConfigs", "Fargowiltas_PotionToggles.json");
        public Preferences Config;

        public Dictionary<int, PotionToggle> Toggles = [];

        public const int CustomPresetCount = 3;
        public List<int>[] CustomPresets = new List<int>[CustomPresetCount];

        public bool Initialized;

        //not doing it in player.initialize because multiplayer clones players which makes new togglers which tries config load which has high overhead (lag)
        public override void TryLoad()
        {
            if (Initialized)
                return;

            Initialized = true;

            Config = new Preferences(ConfigPath);

            Toggles = PotionToggleLoader.LoadedToggles;

            if (!Main.dedServ)
            {
                if (!Config.Load())
                    Save();
            }

            //TODO: figure out how to extract a plain list from json, only using Dict rn because i know it can be loaded from json
            for (int i = 0; i < CustomPresets.Length; i++)
            {
                var toggleUnpack = Config.Get<Dictionary<string, bool>>($"CustomPresetsOff{i + 1}", null);
                if (toggleUnpack != null)
                {
                    List<int> disabledPotions = [];
                    foreach (int itemID in PotionToggleLoader.LoadedToggles.Keys.ToList())
                    {
                        string key;
                        if (itemID < ItemID.Count)
                        {
                            key = itemID.ToString();
                        }
                        else if (ContentSamples.ItemsByType[i] is Item item && item.ModItem is ModItem modItem)
                        {
                            key = modItem.FullName;
                        }
                        else // how?
                            continue;
                        if (toggleUnpack.ContainsKey(key))
                            disabledPotions.Add(itemID);
                    }
                    CustomPresets[i] = disabledPotions;
                }
            }
        }

        public override void Save()
        {
            if (!Initialized)
                return;

            if (!Main.dedServ)
            {
                //Config.Put("CanPlayMaso", CanPlayMaso);

                //Config.Put(TogglesByPlayer, ParsePackedToggles());

                //TogglerPosition = FargoUIManager.SoulToggler.GetPositionAsPoint();
                //Config.Put("TogglerPosition", UnpackPosition());

                for (int i = 0; i < CustomPresets.Length; i++)
                {
                    if (CustomPresets[i] == null)
                        continue;

                    Dictionary<string, bool> togglesOff = new(CustomPresets.Length);
                    foreach (int itemID in CustomPresets[i])
                    {
                        string key;
                        if (itemID < ItemID.Count)
                        {
                            key = itemID.ToString();
                        }
                        else if (ContentSamples.ItemsByType[i] is Item item && item.ModItem is ModItem modItem)
                        {
                            key = modItem.FullName;
                        }
                        else // how?
                            continue;
                        togglesOff[key] = false;
                    }
                    Config.Put($"CustomPresetsOff{i + 1}", togglesOff);
                }

                Config.Save();
            }
        }

        public override void LoadPlayerToggles(Player player)
        {
            FargoPlayer modPlayer = player.FargoMutant();
            if (!Initialized)
                return;

            Toggles = PotionToggleLoader.LoadedToggles;
            SetAll(true);

            foreach (int itemID in modPlayer.DisabledPotionToggles)
                Main.LocalPlayer.SetPotionToggleValue(itemID, false);

            foreach (KeyValuePair<int, PotionToggle> entry in Toggles)
                modPlayer.PotionTogglesToSync[entry.Key] = entry.Value.ToggleBool;
        }

        public override void SetAll(bool value)
        {
            foreach (PotionToggle toggle in Toggles.Values)
            {
                Main.LocalPlayer.SetPotionToggleValue(toggle.ItemID, value);
            }
        }

        public override void SomeEffects()
        {
            
        }

        public override void MinimalEffects()
        {
            
        }

        public override void SaveCustomPreset(int slot)
        {
            var togglesOff = new List<int>();
            foreach (KeyValuePair<int, PotionToggle> entry in Toggles)
            {
                if (!Toggles[entry.Key].ToggleBool)
                    togglesOff.Add(entry.Key);
            }

            if (!Main.dedServ)
            {
                CustomPresets[slot - 1] = togglesOff;
                //Save(); 
                Main.NewText(Language.GetTextValue("Mods.Fargowiltas.UI.SavedToSlot", slot), Color.Yellow);
            }
        }

        public override void LoadCustomPreset(int slot)
        {
            List<int> togglesOff = CustomPresets[slot - 1];
            if (togglesOff == null)
            {
                Main.NewText(Language.GetTextValue("Mods.Fargowiltas.UI.NoTogglesFound", slot), Color.Yellow);
                return;
            }

            FargoPlayer modPlayer = Main.LocalPlayer.FargoMutant();
            modPlayer.DisabledPotionToggles = [.. togglesOff];

            LoadPlayerToggles(Main.LocalPlayer);
            modPlayer.DisabledPotionToggles.Clear();
        }
    }
}
