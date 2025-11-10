using Fargowiltas;
using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Items;
using Fargowiltas.Content.Items.CaughtNPCs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.NPCs;
using Fargowiltas.Content.Projectiles;
using Fargowiltas.Content.UI;

using Fargowiltas.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Fargowiltas.Content.Items.Tiles.EnchantedTreeTileEntity;
using static Fargowiltas.FargoSets;

namespace Fargowiltas
{
    public class Fargowiltas : Mod
    {
        internal static MutantSummonTracker summonTracker;
        internal static DevianttDialogueTracker dialogueTracker;
        internal static GlyphTracker glyphTracker;

        /// <summary>
        /// All mods that should be recognized as derivative from Fargo's Souls. <br></br>
        /// Used to check whether certain universal features should apply to items from this mod, for example Ruminate tooltips. <br></br>
        /// If your mod derives from Souls and includes Souls features, add it to this list.
        /// </summary>
        public static List<string> SoulsMods = ["FargowiltasSouls", "FargowiltasCrossmod", "FargowiltasSoulsDLC"];

        // Hotkeys
        public static ModKeybind HomeKey;

        public static ModKeybind StatKey;

        public static ModKeybind PotionTogglerKey;

        public static ModKeybind DashKey;

        public static ModKeybind SetBonusKey;


        // Swarms (Energized bosses) 
        public static bool SwarmActive;
        public static bool HardmodeSwarmActive;
        public static bool LateHardmodeSwarmActive;
        public static bool SwarmNoHyperActive;
        public static int SwarmItemsUsed;
        public static bool SwarmSetDefaults;
        public static int SwarmMinDamage
        {
            get
            {
                float dmg;
                if (HardmodeSwarmActive)
                    dmg = 63 + 2 * SwarmItemsUsed;
                else
                    dmg = 40 + 1 * SwarmItemsUsed;
                if (Main.masterMode)
                    dmg /= 1.2f;
                return (int)dmg;
            }

        }

        // Mod loaded bools
        internal static Dictionary<string, bool> ModLoaded;
        internal static Dictionary<int, string> ModRareEnemies = [];
        internal static List<Action> ModEventActions = [];
        internal static List<Func<bool>> ModEventActiveFuncs = [];

        public List<StatSheetUI.Stat> ModStats;
        public List<StatSheetUI.PermaUpgrade> PermaUpgrades;

        private string[] mods;

        public static Dictionary<int, int> AnglerPityAmounts = new(){
            {ItemID.HighTestFishingLine, 5},
            {ItemID.TackleBox, 9},
            {ItemID.AnglerEarring, 13},
            {ItemID.GoldenBugNet, 26},
            {ItemID.SuperAbsorbantSponge, 23},
            {ItemID.FishingBobber, 7},
            {ItemID.FishermansGuide, 16},
            {ItemID.Sextant, 17},
            {ItemID.WeatherRadio, 18},
            {ItemID.HoneyAbsorbantSponge, 21},
            {ItemID.BottomlessHoneyBucket, 21},
            {ItemID.FinWings, 45},
            {ItemID.HotlineFishingHook, 43},
        };

        internal static Fargowiltas Instance;

        public override uint ExtraPlayerBuffSlots => (uint)(FargoServerConfig.Instance.ExtraBuffSlots ? 22 : 0);

        public Fargowiltas()
        {
            //            Properties = new ModProperties()
            //            {
            //                Autoload = true,
            //                AutoloadGores = true,
            //                AutoloadSounds = true,
            //            }; 
            //            HookIntoLoad();
        }

        public static Mod WoTG;

        public override void Load()
        {
            Instance = this;
            ModLoader.TryGetMod("NoxusBoss", out WoTG);

            FargoUIManager.LoadUI();

			ModStats = new();
            PermaUpgrades = new List<StatSheetUI.PermaUpgrade>
            {
                new(ContentSamples.ItemsByType[ItemID.AegisCrystal], () => Main.LocalPlayer.usedAegisCrystal),
                new(ContentSamples.ItemsByType[ItemID.AegisFruit], () => Main.LocalPlayer.usedAegisFruit),
                new(ContentSamples.ItemsByType[ItemID.ArcaneCrystal], () => Main.LocalPlayer.usedArcaneCrystal),
                new(ContentSamples.ItemsByType[ItemID.Ambrosia], () => Main.LocalPlayer.usedAmbrosia),
                new(ContentSamples.ItemsByType[ItemID.GummyWorm], () => Main.LocalPlayer.usedGummyWorm),
                new(ContentSamples.ItemsByType[ItemID.GalaxyPearl], () => Main.LocalPlayer.usedGalaxyPearl),
                new(ContentSamples.ItemsByType[ItemID.ArtisanLoaf], () => Main.LocalPlayer.ateArtisanBread),
            };

            summonTracker = new MutantSummonTracker();
            dialogueTracker = new DevianttDialogueTracker();
            dialogueTracker.AddVanillaDialogue();
            glyphTracker = new GlyphTracker();

            HomeKey = KeybindLoader.RegisterKeybind(this, "Home", "Home");

            StatKey = KeybindLoader.RegisterKeybind(this, "Stat", "L");

            PotionTogglerKey = KeybindLoader.RegisterKeybind(this, "PotionToggler", "K");

            DashKey = KeybindLoader.RegisterKeybind(this, "Dash", "J");

            SetBonusKey = KeybindLoader.RegisterKeybind(this, "SetBonus", "V");


            mods =
            [
                "FargowiltasSouls", // Fargo's Souls
                "FargowiltasSoulsDLC",
                "ThoriumMod",
                "CalamityMod",
                "MagicStorage",
                "WikiThis"
            ];

            ModLoaded = new Dictionary<string, bool>();
            foreach (string mod in mods)
            {
                ModLoaded.Add(mod, false);
            }
            CaughtNPCItem.RegisterItems();

            // DD2 Banner Effect hack
            ItemID.Sets.BannerStrength = ItemID.Sets.Factory.CreateCustomSet(new ItemID.BannerEffect(1f));

            Terraria.On_Player.DoCommonDashHandle += OnVanillaDash;
            Terraria.On_Player.KeyDoubleTap += OnVanillaDoubleTapSetBonus;
            Terraria.On_Player.KeyHoldDown += OnVanillaHoldSetBonus;

            Terraria.On_Recipe.FindRecipes += FindRecipes_ElementalAssemblerGraveyardHack;
            Terraria.On_WorldGen.CountTileTypesInArea += CountTileTypesInArea_PurityTotemHack;
            Terraria.On_SceneMetrics.ExportTileCountsToMain += ExportTileCountsToMain_PurityTotemHack;
            Terraria.On_Player.HasUnityPotion += OnHasUnityPotion;
            Terraria.On_Player.TakeUnityPotion += OnTakeUnityPotion;
            Terraria.On_Player.DropTombstone += DisableTombstones;

            On_Player.ItemCheck_CheckCanUse += AllowUseSummons;
            On_Player.ItemCheck_UseBossSpawners += AllowUseSummons2EvilEdition;
            On_Player.ItemCheck_UseEventItems += AllowUseEventSummons;
            On_Player.SummonItemCheck += AllowMultipleBosses;
            On_Player.AddBuff += AddBuff;

            On_Main.DoUpdateInWorld += UpdateEnchantedTreeFruit;
            On_Main.DrawPlayers_AfterProjectiles += DrawEnchantedTrees;

            On_Main.DoDraw_UpdateCameraPosition += ScopeBinocularToggle;

            On_Item.GetShimmered += FixRecipeGroupsShimmerInteraction;

            On_Player.GetAnglerReward_Bait += AnglerPitty;
        }

        private static IEnumerable<Item> GetWormholes(Player self) =>
            self.inventory
                .Concat(self.bank.item)
                .Concat(self.bank2.item)
                .Where(x => x.type == ItemID.WormholePotion);

        private static void OnTakeUnityPotion(Terraria.On_Player.orig_TakeUnityPotion orig, Player self)
        {
            var wormholes = GetWormholes(self).ToList();

            if (
                FargoServerConfig.Instance.UnlimitedPotionBuffsOn120
                && wormholes.Select(x => x.stack).Sum() >= 30
            )
            {
                return;
            }

            // Can't be empty as we're gated by HasUnityPotion
            Item pot = wormholes.First();

            pot.stack -= 1;

            if (pot.stack <= 0)
                pot.SetDefaults(0, false);
        }

        private static void DisableTombstones(Terraria.On_Player.orig_DropTombstone orig, Player self, long coinsOwned, NetworkText deathText, int hitDirection)
        {
            if (FargoServerConfig.Instance.DisableTombstones)
                return;

            orig(self, coinsOwned, deathText, hitDirection);
        }

        private static bool OnHasUnityPotion(Terraria.On_Player.orig_HasUnityPotion orig, Player self)
        {
            return GetWormholes(self).Select(x => x.stack).Sum() > 0;
        }

        private static void FindRecipes_ElementalAssemblerGraveyardHack(
            Terraria.On_Recipe.orig_FindRecipes orig,
            bool canDelayCheck)
        {
            bool oldZoneGraveyard = Main.LocalPlayer.ZoneGraveyard;

            if (!Main.gameMenu && Main.LocalPlayer.active && Main.LocalPlayer.GetModPlayer<FargoPlayer>().ElementalAssemblerNearby > 0)
                Main.LocalPlayer.ZoneGraveyard = true;

            orig(canDelayCheck);

            Main.LocalPlayer.ZoneGraveyard = oldZoneGraveyard;
        }

        //for town npc housing check, independent from player biome
        private static void CountTileTypesInArea_PurityTotemHack(
            Terraria.On_WorldGen.orig_CountTileTypesInArea orig,
            int[] tileTypeCounts, int startX, int endX, int startY, int endY)
        {
            orig(tileTypeCounts, startX, endX, startY, endY);

            if (tileTypeCounts[ModContent.TileType<PurityTotemSheet>()] > 0)
            {
                const int sunflowerWeight = 5;
                tileTypeCounts[TileID.Sunflower] += PurityTotemSheet.TILES_NEGATED / sunflowerWeight;
            }
        }

        //for current biome
        private void ExportTileCountsToMain_PurityTotemHack(
            Terraria.On_SceneMetrics.orig_ExportTileCountsToMain orig,
            SceneMetrics self)
        {
            orig(self);

            //for visible biome effect
            if (self.GetTileCount((ushort)ModContent.TileType<PurityTotemSheet>()) > 0)
            {
                const int tilesNegated = PurityTotemSheet.TILES_NEGATED;

                //reduce biome counts, floor at zero
                self.BloodTileCount = Math.Max(self.BloodTileCount - tilesNegated, 0);
                self.EvilTileCount = Math.Max(self.EvilTileCount - tilesNegated, 0);
                self.GraveyardTileCount = Math.Max(self.GraveyardTileCount - tilesNegated, 0);

                //reenable if disabled by graveyard
                if (self.GetTileCount(TileID.Sunflower) > 0)
                    self.HasSunflower = true;
            }
        }



        public override void Unload()
        {
            Terraria.On_Player.DoCommonDashHandle -= OnVanillaDash;
            Terraria.On_Player.KeyDoubleTap -= OnVanillaDoubleTapSetBonus;
            Terraria.On_Player.KeyHoldDown -= OnVanillaHoldSetBonus;

            Terraria.On_Recipe.FindRecipes -= FindRecipes_ElementalAssemblerGraveyardHack;
            Terraria.On_WorldGen.CountTileTypesInArea -= CountTileTypesInArea_PurityTotemHack;
            Terraria.On_SceneMetrics.ExportTileCountsToMain -= ExportTileCountsToMain_PurityTotemHack;
            Terraria.On_Player.HasUnityPotion -= OnHasUnityPotion;
            Terraria.On_Player.TakeUnityPotion -= OnTakeUnityPotion;
            Terraria.On_Player.DropTombstone -= DisableTombstones;

            On_Player.ItemCheck_CheckCanUse -= AllowUseSummons;
            On_Player.ItemCheck_UseBossSpawners -= AllowUseSummons2EvilEdition;
            On_Player.ItemCheck_UseEventItems -= AllowUseEventSummons;
            On_Player.SummonItemCheck -= AllowMultipleBosses;
            On_Player.AddBuff -= AddBuff;

            On_Main.DoUpdateInWorld -= UpdateEnchantedTreeFruit;
            On_Main.DrawPlayers_AfterProjectiles -= DrawEnchantedTrees;

            On_Main.DoDraw_UpdateCameraPosition -= ScopeBinocularToggle;

            On_Item.GetShimmered -= FixRecipeGroupsShimmerInteraction;

            summonTracker = null;
            dialogueTracker = null;
            glyphTracker = null;
            

            HomeKey = null;
            StatKey = null;
            PotionTogglerKey = null;
            DashKey = null;
            SetBonusKey = null;
            mods = null;
            ModLoaded = null;

            Instance = null;
        }

        public override void PostSetupContent()
        {
            try
            {
                foreach (string mod in mods)
                {
                    ModLoaded[mod] = ModLoader.TryGetMod(mod, out Mod otherMod);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Fargowiltas PostSetupContent Error: " + e.StackTrace + e.Message);
            }

			FargoUIManager.InitializeUI();

			if (ModLoader.TryGetMod("Wikithis", out Mod wikithis) && !Main.dedServ)
            {
                wikithis.Call("AddModURL", this, "https://fargosmods.wiki.gg/wiki/{}");

                // You can also use call ID for some calls!
                //wikithis.Call(0, this, "https://examplemod.wiki.gg/wiki/{}");

                // Alternatively, you can use this instead, if your wiki is on terrariamods.fandom.com
                //wikithis.Call(0, this, "https://terrariamods.fandom.com/wiki/Example_Mod/{}");
                //wikithis.Call("AddModURL", this, "https://terrariamods.fandom.com/wiki/Example_Mod/{}");

                // If there wiki on other languages (such as russian, spanish, chinese, etch), then you can also call that:
                //wikithis.Call(0, this, "https://examplemod.wiki.gg/zh/wiki/{}", GameCulture.CultureName.Chinese)

                // If you want to replace default icon for your mod, then call this. Icon should be 30x30, either way it will be cut.
                //wikithis.Call("AddWikiTexture", this, ModContent.Request<Texture2D>(pathToIcon));
                //wikithis.Call(3, this, ModContent.Request<Texture2D>(pathToIcon));
            }

            //            Mod censusMod = ModLoader.GetMod("Census");
            //            if (censusMod != null)
            //            {
            //                censusMod.Call("TownNPCCondition", NPCType("Deviantt"), "Defeat any rare enemy or... embrace eternity");
            //                censusMod.Call("TownNPCCondition", NPCType("Mutant"), "Defeat any boss or miniboss");
            //                censusMod.Call("TownNPCCondition", NPCType("LumberJack"), $"Chop down enough trees");
            //                censusMod.Call("TownNPCCondition", NPCType("Abominationn"), "Clear any event");
            //                Mod fargoSouls = ModLoader.GetMod("FargowiltasSouls");
            //                if (fargoSouls != null)
            //                {
            //                    censusMod.Call("TownNPCCondition", NPCType("Squirrel"), $"Have a Top Hat Squirrel ([i:{fargoSouls.ItemType("TophatSquirrel")}]) in your inventory");
            //                }
            //            }

            //foreach (KeyValuePair<int, int> npc in CaughtNPCItem.CaughtTownies)
            //    Main.RegisterItemAnimation(npc.Key, new DrawAnimationVertical(6, Main.npcFrameCount[npc.Value]));

            //            /*Mod soulsMod = ModLoader.GetMod("FargowiltasSouls");
            //            if (soulsMod != null)
            //            {
            //                if (!ModRareEnemies.ContainsKey(soulsMod.NPCType("BabyGuardian")))
            //                    ModRareEnemies.Add(soulsMod.NPCType("BabyGuardian"), "babyGuardian");
            //            }*/
        }

        public override object Call(params object[] args)
        {
            try
            {
                string code = args[0].ToString();

                switch (code)
                {
                    //case "DebuffDisplay":
                    //    ModContent.GetInstance<FargoConfig>().DebuffDisplay = (bool)args[1];
                    //    break;
                    case "AddIndestructibleRectangle":
                        {
                            if (args[1].GetType() == typeof(Rectangle))
                            {
                                Rectangle rectangle = (Rectangle)args[1];
                                FargoGlobalProjectile.CannotDestroyRectangle.Add(rectangle);
                            }
                        }
                        break;
                    case "AddIndestructibleTileType":
                        {
                            if (args[1].GetType() == typeof(int))
                            {
                                int tile = (int)args[1];
                                FargoSets.Tiles.InstaCannotDestroy[tile] = true;
                            }
                        }
                        break;
                    case "AddIndestructibleWallType":
                        {
                            if (args[1].GetType() == typeof(int))
                            {
                                int wall = (int)args[1];
                                FargoSets.Walls.InstaCannotDestroy[wall] = true;
                            }
                        }
                        break;
                    case "AddEvilAltar":
                        {
                            if (args[1].GetType() == typeof(int))
                            {
                                int tile = (int)args[1];
                                FargoSets.Tiles.EvilAltars[tile] = true;
                            }
                        }
                        break;
                    case "AddStat":
                        {
                            if (args[1].GetType() != typeof(int))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[1] must be of type int");
                            if (args[2].GetType() != typeof(Func<string>))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[2] must be of type Func<string>");

                            int itemID = (int)args[1];
                            Func<string> TextFunction = (Func<string>)args[2];
                            ModStats.Add(new StatSheetUI.Stat(itemID, TextFunction));
                        }
                        break;
                    case "AddPermaUpgrade":
                        {
                            if (args[1].GetType() != typeof(Item))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[1] must be of type Item");
                            if (args[2].GetType() != typeof(Func<bool>))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[2] must be of type Func<bool>");

                            Item item = (Item)args[1];
                            Func<bool> ConsumedFunction = (Func<bool>)args[2];
                            PermaUpgrades.Add(new StatSheetUI.PermaUpgrade(item, ConsumedFunction));
                        }
                        break;
                    case "SwarmActive":
                        return SwarmActive;

                    case "AddSummon":
                        {
                            if (summonTracker.SummonsFinalized)
                                throw new Exception($"Call Error: Summons must be added before AddRecipes");

                            int itemId;
                            int funcIndex;
                            if (args[2].GetType() == typeof(string))
                            {
                                //Logger.Warn("Fargowiltas: You should provide the summon item ID instead of strings (mod name) and (item name)!");
                                itemId = ModContent.Find<ModItem>(Convert.ToString(args[2]), Convert.ToString(args[3])).Type;
                                funcIndex = 4;
                            }
                            else
                            {
                                itemId = Convert.ToInt32(args[2]);
                                funcIndex = 3;
                            }

                            summonTracker.AddSummon(
                                Convert.ToSingle(args[1]),
                                itemId,
                                args[funcIndex] as Func<bool>,
                                Convert.ToInt32(args[funcIndex + 1])
                            );
                        }
                        break;

                    case "AddAbominationnEvent":
                        {
                            if (args[1].GetType() != typeof(Action))
                                throw new Exception("\"Call Error (Fargo Mutant Mod AddAbominationnEvent): args[1] must be of type Action");

                            ModEventActions.Add((Action)args[1]);

                            if (args[2].GetType() != typeof(Func<bool>))
                                throw new Exception("\"Call Error (Fargo Mutant Mod AddAbominationnEvent): args[2] must be of type Func<bool>");

                            ModEventActiveFuncs.Add((Func<bool>)args[2]);
                        }
                        break;

                    //                    case "AddEventSummon":
                    //                        if (summonTracker.SummonsFinalized)
                    //                            throw new Exception($"Call Error: Event summons must be added before AddRecipes");

                    //                        summonTracker.AddEventSummon(
                    //                            Convert.ToSingle(args[1]),
                    //                            args[2] as string,
                    //                            args[3] as string,
                    //                            args[4] as Func<bool>,
                    //                            Convert.ToInt32(args[5])
                    //                        );
                    //                        break;

                    //                    case "GetDownedEnemy":
                    //                        if (FargoWorld.DownedBools.ContainsKey(args[1] as string) && FargoWorld.DownedBools[args[1] as string])
                    //                            return true;
                    //                        return false;
                    case "AddDevianttHelpDialogue":
                        if (args[4].GetType() == typeof(string) && args[4].ToString().Length > 0)
                            dialogueTracker.AddDialogue(args[1] as string, (byte)args[2], args[3] as Predicate<string>, args[4] as string);
                        else
                            dialogueTracker.AddDialogue(args[1] as string, (byte)args[2], args[3] as Predicate<string>);

                        break;

                    case "LowRenderProj":
                        ((Projectile)args[1]).GetGlobalProjectile<FargoGlobalProjectile>().lowRender = true;
                        break;

                    case "DoubleTapDashDisabled":
                        return FargoClientConfig.Instance.DoubleTapDashDisabled;

                    case "AddCaughtNPC":
                        {
                            if (args[1].GetType() != typeof(string))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddCaughtNPC): args[1] must be of type string");
                            if (args[2].GetType() != typeof(int))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddCaughtNPC): args[2] must be of type int");
                            if (args[3].GetType() != typeof(string))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddCaughtNPC): args[3] must be of type string");
                            string internalName = (string)args[1];
                            int id = (int)args[2];
                            string modName = (string)args[3];
                            CaughtNPCItem item = new(internalName, id);
                            ModLoader.GetMod(modName).AddContent(item);
                            CaughtNPCItem.CaughtTownies.Add(id, item.Type);
                        }
                        break;
                    case "AddGlyph":
                        {
                            if (glyphTracker.GlyphsFinalized)
                                throw new Exception($"Call Error: Glyphs must be added before AddRecipes");

                            if (args[1].GetType() != typeof(string))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddGlyph): args[1] must be of type string");
                            if (args[2].GetType() != typeof(string))
                                throw new Exception($"Call Error (Fargo Mutant Mod AddGlyph): args[2] must be of type string");
                            string key = (string)args[1];
                            string fileName = (string)args[2];
                            GlyphRegistry.Register(key, fileName);
                        }
                        break;
                }

            }
            catch (Exception e)
            {
                Logger.Error("Call Error: " + e.StackTrace + e.Message);
            }

            return base.Call(args);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte messageType = reader.ReadByte();

            switch (messageType)
            {
                case 0:
                    FargoNet.HandlePacket(reader, messageType);
                    break;
                // Regal statue
                case 1:
                    {
                        if (whoAmI >= 0 && whoAmI < FargoWorld.CurrentSpawnRateTile.Length)
                        {
                            FargoWorld.CurrentSpawnRateTile[whoAmI] = reader.ReadBoolean();
                        }
                    }
                    break;

                // Abominationn clear events
                case 2:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        if (IsEventOccurring)
                        {
                            TryClearEvents();
                            NetMessage.SendData(MessageID.WorldData);
                        }
                    }

                    break;

                // Angler reset
                case 3:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        Main.AnglerQuestSwap();
                    }
                    break;

                // Sync npc max life
                case 4:
                    {
                        int n = reader.ReadInt32();
                        int lifeMax = reader.ReadInt32();
                        if (Main.netMode == NetmodeID.MultiplayerClient && n >= 0 && n < Main.maxNPCs)
                            Main.npc[n].lifeMax = lifeMax;
                    }
                    break;

                // Kill super dummies
                case 5:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            if (Main.npc[i] != null && Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<SuperDummyNPC>())
                            {
                                NPC npc = Main.npc[i];
                                npc.life = 0;
                                npc.HitEffect();
                                Main.npc[i].SimpleStrikeNPC(int.MaxValue, 0, false, 0, null, false, 0, true);
                                //Main.npc[i].StrikeNPCNoInteraction(int.MaxValue, 0, 0, false, false, false);

                                if (Main.netMode == NetmodeID.Server)
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
                            }
                        }
                    }
                    break;

                //client requested server to update world
                case 6:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData);
                    }
                    break;

                //client requested server to broadcast battle cry message
                case 7:
                    {
                        bool isBattle = reader.ReadBoolean();
                        int p = reader.ReadInt32();
                        bool cry = reader.ReadBoolean();
                        BattleCry.GenerateText(isBattle, Main.player[p], cry);
                    }
                    break;

                //client sync battle cry states to others
                case 8:
                    {
                        int p = reader.ReadInt32();
                        Main.player[p].GetModPlayer<FargoPlayer>().BattleCry = reader.ReadBoolean();
                        Main.player[p].GetModPlayer<FargoPlayer>().CalmingCry = reader.ReadBoolean();
                    }
                    break;

                case 9: // sync death fruit health
                    {
                        int p = (int)reader.ReadByte();
                        int deathFruitHealth = reader.ReadByte();
                        if (p >= 0 && p < Main.maxPlayers && Main.player[p].active)
                        {
                            Main.player[p].GetModPlayer<FargoPlayer>().DeathFruitHealth = deathFruitHealth;
                        }
                    }
                    break;
                case 10: // drop a meteor
                    {
                        if (Main.netMode == NetmodeID.Server)
                            WorldGen.dropMeteor();
                    }
                    break;
                case 11:
                    {
                        int treeindex = reader.ReadInt32();
                        FargoUtils.TryGetTileEntityAs(EnchantedTreeSheet.EnchantedTrees[treeindex].X, EnchantedTreeSheet.EnchantedTrees[treeindex].Y, out EnchantedTreeTileEntity tree);
                        tree.ItemType = reader.ReadInt32();
                        tree.Prefix = reader.ReadInt32();
                        int fruitlength = reader.ReadInt32();

                        tree.Fruits = [];
                        for (int i = 0; i < fruitlength; i++)
                        {
                            Fruit fruit = new Fruit(reader.ReadInt32(), reader.ReadVector2(), reader.ReadVector2(), reader.ReadVector2(), reader.ReadInt32(), reader.ReadInt32());
                            fruit.grabCooldown = reader.ReadInt32();
                            fruit.despawnTimer = reader.ReadSingle();
                            tree.Fruits.Add(fruit);
                        }
                        if (Main.dedServ)
                        {
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, tree.ID, tree.Position.X, tree.Position.Y);
                        }
                    }
                    break;
                case 12:
                    {
                        EnchantedTreeSheet.EnchantedTrees = [];
                        int arrayLength = reader.ReadInt32();
                        for (int m = 0; m < arrayLength; m++)
                        {
                            EnchantedTreeSheet.EnchantedTrees.Add(new Point16(reader.ReadInt32(), reader.ReadInt32()));
                        }
                        if (Main.dedServ)
                        {
                            FargoNet.SendEnchantedTreesListPacket();
                        }
                    }
                    break;
                case 13: // Sync potion toggles
                    {
                        Player player = Main.player[reader.ReadByte()];
                        FargoPlayer modPlayer = player.FargoMutant();
                        byte count = reader.ReadByte();
                        List<int> keys = PotionToggleLoader.LoadedToggles.Keys.ToList();

                        for (int i = 0; i < count; i++)
                        {
                            modPlayer.PotionToggler.Toggles[keys[i]].ToggleBool = reader.ReadBoolean();
                        }
                    }
                    break;
                case 14: // Sync one potion toggle
                    {
                        Player player = Main.player[reader.ReadByte()];
                        player.SetPotionToggleValue(reader.ReadInt32(), reader.ReadBoolean());
                    }
                    break;
                default:
                    break;
            }
        }

        internal static bool IsEventOccurring =>
            Main.invasionType != 0
            || Main.pumpkinMoon
            || Main.snowMoon
            || Main.eclipse
            || Main.bloodMoon
            || Main.WindyEnoughForKiteDrops
            || Main.IsItRaining
            || Main.IsItStorming
            || Main.slimeRain
            || BirthdayParty.PartyIsUp
            || DD2Event.Ongoing
            || Sandstorm.Happening
            || (NPC.downedTowers && (NPC.LunarApocalypseIsUp || NPC.ShieldStrengthTowerNebula >= 0 || NPC.ShieldStrengthTowerSolar >= 0 || NPC.ShieldStrengthTowerStardust >= 0 || NPC.ShieldStrengthTowerVortex >= 0))
            || ModEventActiveFuncs.Any(f => f.Invoke());

        internal static bool TryClearEvents()
        {
            bool canClearEvent = FargoWorld.AbomClearCD <= 0;
            if (canClearEvent)
            {
                if (Main.invasionType != 0)
                {
                    Main.invasionType = 0;
                    FargoUtils.PrintLocalization("MessageInfo.CancelEvent", new Color(175, 75, 255));
                }

                if (Main.pumpkinMoon)
                {
                    Main.pumpkinMoon = false;
                    FargoUtils.PrintLocalization("MessageInfo.CancelPumpkinMoon", new Color(175, 75, 255));
                }

                if (Main.snowMoon)
                {
                    Main.snowMoon = false;
                    FargoUtils.PrintLocalization("MessageInfo.CancelFrostMoon", new Color(175, 75, 255));
                }

                if (Main.eclipse)
                {
                    Main.eclipse = false;
                    FargoUtils.PrintLocalization("MessageInfo.CancelEclipse", new Color(175, 75, 255));
                }

                if (Main.bloodMoon)
                {
                    Main.bloodMoon = false;
                    FargoUtils.PrintLocalization("MessageInfo.CancelBloodMoon", new Color(175, 75, 255));
                }

                if (Main.WindyEnoughForKiteDrops)
                {
                    Main.windSpeedTarget = 0;
                    Main.windSpeedCurrent = 0;
                    FargoUtils.PrintLocalization("MessageInfo.CancelWindyDay", new Color(175, 75, 255));
                }

                if (Main.slimeRain)
                {
                    Main.StopSlimeRain();
                    Main.slimeWarningDelay = 1;
                    Main.slimeWarningTime = 1;
                }

                if (BirthdayParty.PartyIsUp)
                    BirthdayParty.CheckNight();

                if (DD2Event.Ongoing && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    DD2Event.StopInvasion();
                    FargoUtils.PrintLocalization("MessageInfo.CancelOOA", new Color(175, 75, 255));
                }

                if (Sandstorm.Happening)
                {
                    Sandstorm.Happening = false;
                    Sandstorm.TimeLeft = 0;
                    Sandstorm.IntendedSeverity = 0;
                    FargoUtils.PrintLocalization("MessageInfo.CancelSandstorm", new Color(175, 75, 255));
                }

                if (NPC.downedTowers && (NPC.LunarApocalypseIsUp || NPC.ShieldStrengthTowerNebula >= 0 || NPC.ShieldStrengthTowerSolar >= 0 || NPC.ShieldStrengthTowerStardust >= 0 || NPC.ShieldStrengthTowerVortex >= 0))
                {
                    NPC.LunarApocalypseIsUp = false;
                    NPC.ShieldStrengthTowerNebula = 0;
                    NPC.ShieldStrengthTowerSolar = 0;
                    NPC.ShieldStrengthTowerStardust = 0;
                    NPC.ShieldStrengthTowerVortex = 0;

                    // Purge all towers
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        if (Main.npc[i].active
                            && (Main.npc[i].type == NPCID.LunarTowerNebula || Main.npc[i].type == NPCID.LunarTowerSolar
                            || Main.npc[i].type == NPCID.LunarTowerStardust || Main.npc[i].type == NPCID.LunarTowerVortex))
                        {
                            Main.npc[i].dontTakeDamage = false;
                            Main.npc[i].GetGlobalNPC<FargoGlobalNPC>().NoLoot = true;
                            Main.npc[i].StrikeInstantKill();
                            //Main.npc[i].StrikeNPCNoInteraction(int.MaxValue, 0f, 0);
                        }
                    }
                    FargoUtils.PrintLocalization("MessageInfo.CancelLunarEvent", new Color(175, 75, 255));
                }

                if (Main.IsItRaining || Main.IsItStorming)
                {
                    Main.StopRain();
                    Main.cloudAlpha = 0;
                    if (Main.netMode == NetmodeID.Server)
                        Main.SyncRain();
                    FargoUtils.PrintLocalization("MessageInfo.CancelRain", new Color(175, 75, 255));
                }

                FargoWorld.AbomClearCD = 7200;

                foreach (Action action in ModEventActions)
                {
                    action.Invoke();
                }
            }

            //foreach (MutantSummonInfo summon in summonTracker.EventSummons)
            //{
            //    if ((bool)ModLoader.GetMod(summon.modSource).Call("AbominationnClearEvents", canClearEvent))
            //    {
            //        eventOccurring = true;
            //    }
            //}

            return canClearEvent;
        }

        // SpawnBoss(player, mod.NPCType("MyBoss"), true, 0, 0, "DerpyBoi 2", false);
        internal static void SpawnBoss(Player player, int bossType, bool spawnMessage = true, int overrideDirection = 0, int overrideDirectionY = 0, string overrideDisplayName = "", bool namePlural = false)
        {
            if (overrideDirection == 0)
            {
                overrideDirection = Main.rand.NextBool(2) ? -1 : 1;
            }

            if (overrideDirectionY == 0)
            {
                overrideDirectionY = -1;
            }

            Vector2 npcCenter = player.Center + new Vector2(MathHelper.Lerp(500f, 800f, (float)Main.rand.NextDouble()) * overrideDirection, 800f * overrideDirectionY);
            SpawnBoss(player, bossType, spawnMessage, npcCenter, overrideDisplayName, namePlural);
        }

        // SpawnBoss(player, mod.NPCType("MyBoss"), true, player.Center + new Vector2(0, 800f), "DerpFromBelow", false);
        internal static int SpawnBoss(Player player, int bossType, bool spawnMessage = true, Vector2 npcCenter = default, string overrideDisplayName = "", bool namePlural = false)
        {
            if (npcCenter == default)
            {
                npcCenter = player.Center;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)npcCenter.X, (int)npcCenter.Y, bossType);
                Main.npc[npcID].Center = npcCenter;
                Main.npc[npcID].netUpdate2 = true;

                if (spawnMessage)
                {
                    string npcName = !string.IsNullOrEmpty(Main.npc[npcID].GivenName) ? Main.npc[npcID].GivenName : overrideDisplayName;
                    //if ((npcName == null || string.IsNullOrEmpty(npcName)) && Main.npc[npcID].modNPC != null)
                    //{
                    //    npcName = Main.npc[npcID].modNPC.DisplayName.GetDefault();
                    //}

                    if (namePlural)
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.HaveAwoken", npcName), 175, 75);
                        }
                        else
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.HaveAwoken", npcName), new Color(175, 75, 255));
                        }
                    }
                    else
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            Main.NewText(Language.GetTextValue("Announcement.HasAwoken", npcName), 175, 75);
                        }
                        else
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", npcName), new Color(175, 75, 255));
                        }
                    }
                }
            }
            else
            {
                FargoNet.SendNetMessage(FargoNet.SummonNPCFromClient, (byte)player.whoAmI, (short)bossType, spawnMessage, (int)npcCenter.X, (int)npcCenter.Y, overrideDisplayName, namePlural);
            }

            return 200;
        }
        private static void OnVanillaDash(Terraria.On_Player.orig_DoCommonDashHandle orig, Terraria.Player player, out int dir, out bool dashing, Player.DashStartAction dashStartAction)
        {
            if (FargoClientConfig.Instance.DoubleTapDashDisabled)
            {
                player.dashTime = 0;
                /*
                if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                {
                    Main.NewText(calamity);
                    if (calamity.TryFind("CalamityPlayer", out ModPlayer modPlayer))
                    {
                        FieldInfo dashTimeMod = modPlayer.GetType().GetField("dashTimeMod");
                        Main.NewText(dashTimeMod.Name);
                        if (dashTimeMod != null)
                            dashTimeMod.SetValue(modPlayer, 0);
                    }
                }
                */
            }


            orig.Invoke(player, out dir, out dashing, dashStartAction);

            if (player.whoAmI == Main.myPlayer && DashKey.JustPressed && !player.CCed)
            {
                InputManager modPlayer = player.GetModPlayer<InputManager>();
                if (player.controlRight && player.controlLeft)
                {
                    dir = modPlayer.latestXDirPressed;
                }
                else if (player.controlRight)
                {
                    dir = 1;
                }
                else if (player.controlLeft)
                {
                    dir = -1;
                }
                if (dir == 0) // this + commented out below because changed to not have an effect when not holding any movement keys; primarily so it's affected by stun effects
                    return;
                //else if (modPlayer.latestXDirReleased != 0)
                //{
                //    dir = modPlayer.latestXDirReleased;
                //}
                //else
                //{
                //    dir = player.direction;
                //}
                player.direction = dir;
                dashing = true;
                if (player.dashTime > 0)
                {
                    player.dashTime--;
                }
                if (player.dashTime < 0)
                {
                    player.dashTime++;
                }
                if ((player.dashTime <= 0 && player.direction == -1) || (player.dashTime >= 0 && player.direction == 1))
                {
                    player.dashTime = 15;
                    return;
                }
                dashing = true;
                player.dashTime = 0;
                player.timeSinceLastDashStarted = 0;
                if (dashStartAction != null)
                    dashStartAction?.Invoke(dir);
            }

        }
        private static void OnVanillaDoubleTapSetBonus(On_Player.orig_KeyDoubleTap orig, Player player, int keyDir)
        {
            if (!FargoClientConfig.Instance.DoubleTapSetBonusDisabled || SetBonusKey.JustPressed)
            {
                orig.Invoke(player, keyDir);
            }
        }
        private static void OnVanillaHoldSetBonus(On_Player.orig_KeyHoldDown orig, Player player, int keyDir, int holdTime)
        {
            if (!FargoClientConfig.Instance.DoubleTapSetBonusDisabled || SetBonusKey.Current)
            {
                orig.Invoke(player, keyDir, holdTime);
            }
        }

        private bool AllowUseSummons(On_Player.orig_ItemCheck_CheckCanUse orig, Player self, Item item)
        {
            if (FargoGlobalItem.AlwaysUsableVanillaSummons.Contains(item.type) && ModContent.GetInstance<FargoServerConfig>().EasySummons)
            {
                if (!((item.type == ItemID.BloodMoonStarter && Main.bloodMoon) ||
                    (item.type == ItemID.NaughtyPresent && Main.snowMoon) ||
                    (item.type == ItemID.PumpkinMoonMedallion && Main.pumpkinMoon) ||
                    (item.type == ItemID.GoblinBattleStandard && Main.invasionType == InvasionID.GoblinArmy) ||
                    (item.type == ItemID.SolarTablet && Main.eclipse) ||
                    (item.type == ItemID.PirateMap && Main.invasionType == InvasionID.PirateInvasion) ||
                    (item.type == ItemID.SnowGlobe && Main.invasionType == InvasionID.SnowLegion)))
                {
                    return true;
                }
            }
            return orig(self, item);
        }
        private bool AllowMultipleBosses(On_Player.orig_SummonItemCheck orig, Player self, Item item)
        {
            if (ModContent.GetInstance<FargoServerConfig>().EasySummons && self.itemAnimation == self.itemAnimationMax)
            {
                return true;
            }
            return orig(self, item);
        }

        private void AddBuff(Terraria.On_Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet, bool foodHack)
        {
            self.FargoMutant().ActivePotions.Add(type);
            orig(self, type, timeToAdd, quiet, foodHack);
        }

        private void AllowUseEventSummons(On_Player.orig_ItemCheck_UseEventItems orig, Player self, Item item)
        {
            if (!ModContent.GetInstance<FargoServerConfig>().EasySummons)
            {
                orig(self, item);
                return;
            }
            bool day = Main.dayTime;
            bool hardmode = Main.hardMode;
            bool dd2event = DD2Event.Ongoing;
            bool pumpkin = Main.pumpkinMoon;
            bool frost = Main.snowMoon;
            int lifecrystals = self.ConsumedLifeCrystals;
            if (self.ItemTimeIsZero && self.itemAnimation > 0)
            {
                if (FargoGlobalItem.NightSettingSummons.Contains(item.type))
                {
                    Main.dayTime = false;
                }
                if (item.type == ItemID.SolarTablet)
                {
                    Main.dayTime = true;
                    Main.hardMode = true;
                }
                if (item.type == ItemID.PumpkinMoonMedallion)
                {
                    DD2Event.Ongoing = false;
                    Main.snowMoon = false;
                }
                if (item.type == ItemID.NaughtyPresent)
                {
                    DD2Event.Ongoing = false;
                    Main.pumpkinMoon = false;
                }
                if (item.type == ItemID.GoblinBattleStandard || item.type == ItemID.PirateMap || item.type == ItemID.SnowGlobe)
                {
                    if (self.ConsumedLifeCrystals < 5) self.ConsumedLifeCrystals = 5;
                }
                if (item.type == ItemID.PirateMap || item.type == ItemID.SnowGlobe)
                {
                    Main.hardMode = true;
                }
                //with this one its just easier to redo the whole thing
                if (item.type == ItemID.CelestialSigil)
                {
                    SoundEngine.PlaySound(SoundID.Roar, self.position);
                    self.ApplyItemTime(item);
                    if (Main.netMode == NetmodeID.SinglePlayer)
                        WorldGen.StartImpendingDoom(60);
                    else
                        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, self.whoAmI, -8f);
                    return;
                }

            }
            orig(self, item);

            //Main.dayTime = day;
            //DD2Event.Ongoing = dd2event;
            //Main.pumpkinMoon = pumpkin;
            //Main.snowMoon = frost;
            Main.hardMode = hardmode;
            self.ConsumedLifeCrystals = lifecrystals;
        }

        private void AllowUseSummons2EvilEdition(On_Player.orig_ItemCheck_UseBossSpawners orig, Player self, int onWhichPlayer, Item item)
        {
            if (!ModContent.GetInstance<FargoServerConfig>().EasySummons)
            {
                orig(self, onWhichPlayer, item);
                return;
            }
            bool day = Main.dayTime;
            if (self.ItemTimeIsZero && self.itemAnimation > 0)
            {
                if (FargoGlobalItem.NightSettingSummons.Contains(item.type))
                {
                    Main.dayTime = false;
                }
                if (item.type == ItemID.SolarTablet)
                {
                    Main.dayTime = true;
                }
                if (item.type == ItemID.WormFood)
                {
                    self.ZoneCorrupt = true;
                }
                if (item.type == ItemID.BloodySpine)
                {
                    self.ZoneCrimson = true;
                }
                if (item.type == ItemID.Abeemination)
                {
                    self.ZoneJungle = true;
                    self.ZoneRockLayerHeight = true;
                }
                if (item.type == ItemID.DeerThing)
                {
                    self.ZoneSnow = true;
                }
                if (item.type == ItemID.QueenSlimeCrystal)
                {
                    self.ZoneHallow = true;
                }
            }
            orig(self, onWhichPlayer, item);
            //Main.dayTime = day;

        }
        private void DrawEnchantedTrees(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
        {
            EnchantedTreeTileEntity.DrawEnchantedTrees();
            orig(self);
        }

        private void UpdateEnchantedTreeFruit(On_Main.orig_DoUpdateInWorld orig, Main self, System.Diagnostics.Stopwatch sw)
        {
            orig(self, sw);
            EnchantedTreeTileEntity.UpdateEnchantedTrees();
        }

        private static void ScopeBinocularToggle(On_Main.orig_DoDraw_UpdateCameraPosition orig)
        {
            bool scopeCheck = false;
            var p = Main.LocalPlayer;
            bool config = FargoClientConfig.Instance.DisableScopeView;

            if (Main.myPlayer >= 0 && Main.myPlayer < 255 && p.active && !p.dead && p.HeldItem != null && p.HeldItem.useAmmo > AmmoID.None && p.scope && Main.mouseRight && config)
            {
                int[] ammo = [AmmoID.Bullet, AmmoID.CandyCorn, AmmoID.Stake, AmmoID.Gel, AmmoID.Solution];

                if ((p.FargoMutant().ScopeAccessoryHidden && ammo.Contains(p.HeldItem.useAmmo)) || p.HeldItem.type == ItemID.SniperRifle)
                {
                    scopeCheck = Main.mouseRight;
                    Main.mouseRight = false;
                }
            }

            orig();

            if (scopeCheck)
            {
                Main.mouseRight = true;
                p.FargoMutant().ScopeAccessoryHidden = false;
            }
        }

        private void FixRecipeGroupsShimmerInteraction(On_Item.orig_GetShimmered orig, Item self)
        {
            if (!FargoClientConfig.Instance.AnimatedRecipeGroups)
            {
                orig(self);
                return;
            }
            foreach (Recipe recipe in Main.recipe.Where(recipe => recipe.HasResult(self.type) && recipe.acceptedGroups.Count != 0))
            {
                foreach (int groupID in recipe.acceptedGroups)
                {
                    foreach (Item material in recipe.requiredItem.Where(material => RecipeGroup.recipeGroups[groupID].ContainsItem(material.type) && material.type != RecipeGroup.recipeGroups[groupID].IconicItemId))
                    {
                        string name = material.Name;
                        int stack = material.stack;
                        material.ChangeItemType(RecipeGroup.recipeGroups[groupID].IconicItemId);
                        material.SetNameOverride(name);
                        material.stack = stack;
                    }
                }
            }
            orig(self);
        }
        private void AnglerPitty(On_Player.orig_GetAnglerReward_Bait orig, Player self, List<Item> rewardItems, IEntitySource source, int questsDone, float rarityReduction, ref GetItemSettings anglerRewardSettings)
        {
            orig(self, rewardItems, source, questsDone, rarityReduction, ref anglerRewardSettings);
            foreach (Item item in rewardItems)
            {
                if (AnglerPityAmounts.ContainsKey(item.type))
                {
                    self.FargoMutant().ItemHasBeenOwned[item.type] = true;
                }
            }
            if (FargoServerConfig.Instance.AnglerQuestPity && AnglerPityAmounts.ContainsValue(questsDone))
            {
                foreach (KeyValuePair<int, int> pair in AnglerPityAmounts)
                {
                    if (questsDone >= pair.Value  && !self.FargoMutant().ItemHasBeenOwned[pair.Key])
                    {
                        if (((pair.Key == ItemID.HotlineFishingHook || pair.Key == ItemID.FinWings) && Main.hardMode) || ((pair.Key == ItemID.HoneyAbsorbantSponge || pair.Key == ItemID.BottomlessHoneyBucket) && NPC.downedQueenBee))
                        {
                            rewardItems.Add(new Item(pair.Key));
                            self.FargoMutant().ItemHasBeenOwned[pair.Key] = true;
                        }
                        else if (!(pair.Key == ItemID.HotlineFishingHook || pair.Key == ItemID.FinWings || pair.Key == ItemID.HoneyAbsorbantSponge || pair.Key == ItemID.BottomlessHoneyBucket))
                        {
                            rewardItems.Add(new Item(pair.Key));
                            self.FargoMutant().ItemHasBeenOwned[pair.Key] = true;
                        }
                    }
                }
            }
        }

        //        private static void HookIntoLoad()
        //        {
        //            MonoModHooks.RequestNativeAccess();
        //            new Hook(
        //                typeof(ModContent).GetMethod("LoadModContent", BindingFlags.NonPublic | BindingFlags.Static),
        //                typeof(Fargowiltas).GetMethod(nameof(LoadHook), BindingFlags.NonPublic | BindingFlags.Static)).Apply();

        //            HookEndpointManager.Modify(
        //                typeof(ModContent).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Static),
        //                Delegate.CreateDelegate(typeof(ILContext.Manipulator),
        //                    typeof(Fargowiltas).GetMethod(nameof(ModifyLoading),
        //                        BindingFlags.NonPublic | BindingFlags.Static) ?? throw new Exception("Couldn't create IL manipulator.")));
        //        }

        //        private static void ModifyLoading(ILContext il)
        //        {
        //            ILCursor c = new ILCursor(il);

        //            c.GotoNext(x => x.MatchCall(typeof(ModContent), "ResizeArrays"));
        //            c.Index++;

        //            c.EmitDelegate<Action>(() =>
        //            {
        //                FieldInfo loadInfo = typeof(Mod).GetField("loading", BindingFlags.Instance | BindingFlags.NonPublic);
        //                loadInfo?.SetValue(ModLoader.GetMod("Fargowiltas"), true);

        //                /*foreach (Mod mod in ModLoader.Mods.Where(x => x != ModLoader.GetMod("Fargowiltas")))
        //                {
        //                    foreach (ModNPC npc in (typeof(Mod).GetField("npcs", BindingFlags.Instance | BindingFlags.NonPublic)
        //                        ?.GetValue(mod) as IDictionary<string, ModNPC>)?.Values ?? new ModNPC[0])
        //                    {
        //                        try
        //                        {
        //                            npc.SetDefaults();

        //                            if (npc.npc.townNPC)
        //                                CaughtNPCItem.AddAutomatic(npc.Name, npc.npc.type);
        //                        }
        //                        catch
        //                        {
        //                            // ignore
        //                        }
        //                    }
        //                }*/
        //                loadInfo?.SetValue(ModLoader.GetMod("Fargowiltas"), false);

        //                typeof(ModContent).GetMethod("ResizeArrays", BindingFlags.NonPublic | BindingFlags.Static)?
        //                    .Invoke(null, new object[] {false});
        //            });
        //        }

        //        private static void LoadHook(Action<CancellationToken, Action<Mod>> orig, CancellationToken token,
        //            Action<Mod> loadAction)
        //        {
        //            PropertyInfo modsArray = typeof(ModLoader).GetProperty("Mods", BindingFlags.Public | BindingFlags.Static);

        //            if (modsArray is null)
        //            {
        //                orig(token, loadAction);
        //                return;
        //            }

        //            // Mod[] cachedArray = modsArray.GetValue(null) as Mod[];
        //            List<Mod> tempMods = (modsArray.GetValue(null) as Mod[])?.ToList();

        //            if (tempMods is null)
        //            {
        //                orig(token, loadAction);
        //                return;
        //            }

        //            Mod mod = tempMods.First(x => x.Name.Equals("Fargowiltas"));
        //            tempMods.Remove(mod);
        //            tempMods.Add(mod);
        //            modsArray.SetValue(null, tempMods.ToArray());

        //            orig(token, loadAction);

        //            // modsArray.SetValue(null, cachedArray);
        //        }
    }
}

