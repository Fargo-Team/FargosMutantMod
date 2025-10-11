using Microsoft.Xna.Framework;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas
{
    internal static class FargoUtils
    {
        public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        public static bool EternityMode => Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool) ModLoader.GetMod("FargowiltasSouls").Call("EternityMode");
        public static bool HasAnyItem(this Player player, params int[] itemIDs) => itemIDs.Any(itemID => player.HasItem(itemID));

        public static bool ActuallyNight => !Main.dayTime || Main.remixWorld;
        public static FargoPlayer FargoMutant(this Player player) => player.GetModPlayer<FargoPlayer>();

        public static void AddWithCondition<T>(this List<T> list, T type, bool condition)
        {
            if (condition)
            {
                list.Add(type);
            }
        }
        public static void AddDebuffImmunities(this NPC npc, List<int> debuffs)
        {
            foreach (int buffType in debuffs)
            {
                NPCID.Sets.SpecificDebuffImmunity[npc.type][buffType] = true;
            }
        }
        public static void TryDowned(string seller, Color color, params string[] names)
        {
            TryDowned(seller, color, true, names);
        }

        // condition is so that display text is hidden if the kill is done early, BUT the kill is still counted
        // e.g. kill an enemy early, whose spawner is sold in hm, then get into hm, then spawner is unlocked
        // however, text is hidden on that first kill so people don't think it's sold right away
        public static void TryDowned(string seller, Color color, bool conditions, params string[] names)
        {
            bool update = false;

            foreach (string name in names)
            {
                if (!FargoWorld.DownedBools[name])
                {
                    FargoWorld.DownedBools[name] = true;
                    update = true;
                }
            }

            if (update)
            {
                seller = Language.GetTextValue($"Mods.Fargowiltas.NPCs.{seller}.DisplayName");
                string text = Language.GetTextValue("Mods.Fargowiltas.MessageInfo.NewItemUnlocked", seller);
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    if (conditions)
                        Main.NewText(text, color);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    if (conditions)
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
                    NetMessage.SendData(MessageID.WorldData); //sync world
                }
            }
        }

        public static void PrintText(string text)
        {
            PrintText(text, Color.White);
        }

        public static void PrintText(string text, Color color)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, color);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            }
        }

        public static void PrintText(string text, int r, int g, int b) => PrintText(text, new Color(r, g, b));

        public static void PrintLocalization(string fargoKey, params object[] args) => PrintText(Language.GetTextValue($"Mods.Fargowiltas.{fargoKey}", args));

        public static void PrintLocalization(string fargoKey, Color color, params object[] args) => PrintText(Language.GetTextValue($"Mods.Fargowiltas.{fargoKey}", args), color);

        public static void SpawnBossNetcoded(Player player, int bossType)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // If the player using the item is the client
                // (explicitely excluded serverside here)
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    NPC.SpawnOnPlayer(player.whoAmI, bossType);
                }
                else
                {
                    // If the player is in multiplayer, request a spawn
                    // This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, set in NPC code
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: bossType);
                }
            }
        }

        public static bool AnyBossAlive()
        {
            return Main.npc.Any(npc => npc.active && (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.DD2Betsy));
        }

        #region tiles
        //helper methods copied from tml wiki
        /// <summary>
        /// Atttempts to find the top-left corner of a multitile at location (<paramref name="x"/>, <paramref name="y"/>)
        /// </summary>
        /// <param name="x">The tile X-coordinate</param>
        /// <param name="y">The tile Y-coordinate</param>
        /// <returns>The tile location of the multitile's top-left corner, or the input location if no tile is present or the tile is not part of a multitile</returns>
        public static Point16 GetTopLeftTileInMultitile(int x, int y)
        {
            if (x > Main.maxTilesX || y > Main.maxTilesY || x < 0 || y < 0)
            {
                return new(0, 0);
            }
            Tile tile = Main.tile[x, y];

            int frameX = 0;
            int frameY = 0;

            if (tile.HasTile)
            {
                int style = 0, alt = 0;
                TileObjectData.GetTileInfo(tile, ref style, ref alt);
                TileObjectData data = TileObjectData.GetTileData(tile.TileType, style, alt);

                if (data != null)
                {
                    int size = 16 + data.CoordinatePadding;

                    frameX = tile.TileFrameX % (size * data.Width) / size;
                    frameY = tile.TileFrameY % (size * data.Height) / size;
                }
            }

            return new Point16(x - frameX, y - frameY);
        }

        /// <summary>
        /// Uses <seealso cref="GetTopLeftTileInMultitile(int, int)"/> to try to get the entity bound to the multitile at (<paramref name="i"/>, <paramref name="j"/>).
        /// </summary>
        /// <typeparam name="T">The type to get the entity as</typeparam>
        /// <param name="i">The tile X-coordinate</param>
        /// <param name="j">The tile Y-coordinate</param>
        /// <param name="entity">The found <typeparamref name="T"/> instance, if there was one.</param>
        /// <returns><see langword="true"/> if there was a <typeparamref name="T"/> instance, or <see langword="false"/> if there was no entity present OR the entity was not a <typeparamref name="T"/> instance.</returns>
        public static bool TryGetTileEntityAs<T>(int i, int j, out T entity) where T : TileEntity
        {
            Point16 origin = GetTopLeftTileInMultitile(i, j);

            // TileEntity.ByPosition is a Dictionary<Point16, TileEntity> which contains all placed TileEntity instances in the world
            // TryGetValue is used to both check if the dictionary has the key, origin, and get the value from that key if it's there
            if (TileEntity.ByPosition.TryGetValue(origin, out TileEntity existing) && existing is T existingAsT)
            {
                entity = existingAsT;
                return true;
            }

            entity = null;
            return false;
        }
        #endregion
    }
}
