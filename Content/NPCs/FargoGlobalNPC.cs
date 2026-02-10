using System;
using System.Linq;
using System.Collections.Generic;
using Fargowiltas.Buffs;
////using Fargowiltas.Items.Vanity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Fargowiltas.Content.Items.Summons.Abom;
using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Explosives;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.Items.Summons.Deviantt;
using Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;
using Fargowiltas.Content.Items.Misc;
using static Fargowiltas.Fargowiltas;

namespace Fargowiltas.Content.NPCs
{
    public class FargoGlobalNPC : GlobalNPC
    {
        internal static int[] Bosses = [ 
            NPCID.KingSlime,
            NPCID.EyeofCthulhu,
            //NPCID.EaterofWorldsHead,
            NPCID.BrainofCthulhu,
            NPCID.QueenBee,
            NPCID.SkeletronHead,
            NPCID.QueenSlimeBoss,
            NPCID.TheDestroyer,
            NPCID.SkeletronPrime,
            NPCID.Retinazer,
            NPCID.Spazmatism,
            NPCID.Plantera,
            NPCID.Golem,
            NPCID.DukeFishron,
            NPCID.HallowBoss,
            NPCID.CultistBoss,
            NPCID.MoonLordCore,
            NPCID.MartianSaucerCore,
            NPCID.Pumpking,
            NPCID.IceQueen,
            NPCID.DD2Betsy,
            NPCID.DD2OgreT3,
            NPCID.IceGolem,
            NPCID.SandElemental,
            NPCID.Paladin,
            NPCID.Everscream,
            NPCID.MourningWood,
            NPCID.SantaNK1,
            NPCID.HeadlessHorseman,
            NPCID.PirateShip 
        ];

        public static int LastWoFIndex = -1;
        public static int WoFDirection = 0;

        internal bool PillarSpawn = true;
        internal bool SwarmActive;
        internal bool PandoraActive;
        internal bool NoLoot = false;
        //internal bool DestroyerSwarm = false;

        public static int eaterBoss = -1;
        public static int brainBoss = -1;
        public static int plantBoss = -1;
        public static int beeBoss = -1;

        public bool FirstFrame = true;
        public bool woodDrop;

        public override bool InstancePerEntity => true;

        //        public override void SetDefaults(NPC npc)
        //        {
        //            if (GetInstance<FargoConfig>().CatchNPCs)
        //            {
        //                if (npc.townNPC && npc.type < NPCID.Count && npc.type != NPCID.OldMan)
        //                {
        //                    Main.npcCatchable[npc.type] = true;
        //                    npc.catchItem = npc.type == NPCID.DD2Bartender ? (short)mod.ItemType("Tavernkeep") : (short)mod.ItemType(NPCID.GetUniqueKey(npc.type).Replace("Terraria ", string.Empty));
        //                }

        //                if (npc.type == NPCID.SkeletonMerchant)
        //                {
        //                    Main.npcCatchable[npc.type] = true;
        //                    npc.catchItem = (short)mod.ItemType("SkeletonMerchant");
        //                }
        //            }
        //        }
        public override void ResetEffects(NPC npc)
        {
            woodDrop = false;
        }
        public override bool CanHitNPC(NPC npc, NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            if (target.dontTakeDamage && target.type == NPCType<Squirrel>())
                return false;
            
            if (target.friendly && FargoServerConfig.Instance.SaferBoundNPCs && (target.type == NPCID.BoundGoblin || target.type == NPCID.BoundMechanic || target.type == NPCID.BoundWizard || target.type == NPCID.BartenderUnconscious || target.type == NPCID.GolferRescue))
                return false;
            return base.CanHitNPC(npc, target);
        }
        public override bool PreAI(NPC npc)
        {
            if (FirstFrame)
            {
                if (npc.boss && FargoServerConfig.Instance.EasySummons)
                {
                    npc.DiscourageDespawn(600); //10 seconds
                }
                FirstFrame = false;
                #region Stat Sliders
                FargoServerConfig config = FargoServerConfig.Instance;
                if ((config.EnemyHealth != 1 || config.BossHealth != 1) && !npc.townNPC && !npc.CountsAsACritter && npc.life > 10)
                {
                    float lifeFraction = npc.GetLifePercent();
                    bool boss = config.BossHealth > config.EnemyHealth && // only relevant if boss health is higher than enemy health
                        (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || config.BossApplyToAllWhenAlive && AnyBossAlive());
                    if (boss)
                        npc.lifeMax = (int)Math.Round(npc.lifeMax * config.BossHealth);
                    else
                        npc.lifeMax = (int)Math.Round(npc.lifeMax * config.EnemyHealth);
                    npc.life = (int)Math.Round(npc.lifeMax * lifeFraction);
                }
                #endregion
            }
            if (npc.boss)
            {
                boss = npc.whoAmI;
            }

            if (npc.townNPC && npc.homeTileX == -1 && npc.homeTileY == -1)
            {
                bool hasRoom = WorldGen.TownManager.HasRoom(npc.type, out Point homePoint);
                if (hasRoom && homePoint.X > 0 && homePoint.Y > 0)
                {
                    int x = homePoint.X;
                    int y = homePoint.Y - 2;
                    WorldGen.moveRoom(x, y, npc.whoAmI);
                }
            }

            switch (npc.type)
            {
                case NPCID.EaterofWorldsHead:
                    eaterBoss = npc.whoAmI;
                    break;

                case NPCID.BrainofCthulhu:
                    brainBoss = npc.whoAmI;
                    break;

                case NPCID.Plantera:
                    plantBoss = npc.whoAmI;
                    break;

                case NPCID.QueenBee:
                    beeBoss = npc.whoAmI;
                    break;

                case NPCID.CultistBoss:
                    if (npc.ai[0] == -1 && npc.ai[1] == 1) //just after spawning
                    {
                        bool foundTabletNearby = Main.npc.Any(n => n.active && n.type == NPCID.CultistTablet && npc.Distance(n.Center) < 400);
                        if (!foundTabletNearby)
                        {
                            npc.ai[1] = 360;
                            npc.netUpdate = true;
                        }
                    }
                    break;

                case NPCID.MoonLordCore:
                    if (npc.ai[0] == 2)
                    {
                        int skipPoint = 600 - 60;
                        if (npc.ai[1] < skipPoint && npc.ai[1] % 60 == 30 && NPC.CountNPCS(npc.type) > 1)
                        {
                            npc.ai[1] = skipPoint;
                            npc.netUpdate = true;
                        }
                    }
                    break;

                default:
                    break;
            }

            return base.PreAI(npc);
        }

        public override void AI(NPC npc)
        {
            // Wack ghost saucers begone
            if (FargoWorld.OverloadMartians && npc.type == NPCID.MartianSaucerCore && npc.dontTakeDamage)
            {
                npc.dontTakeDamage = false;
            }
        }

        public override void ModifyShop(NPCShop shop)
        {

            #region Conditions
            //TODO: localization/proper text on conditions
            Condition angler5 = new Condition("Mods.Fargowiltas.Conditions.Angler5", () => Main.LocalPlayer.anglerQuestsFinished >= 5);
            Condition angler10 = new Condition("Mods.Fargowiltas.Conditions.Angler10", () => Main.LocalPlayer.anglerQuestsFinished >= 10);
            Condition angler15 = new Condition("Mods.Fargowiltas.Conditions.Angler15", () => Main.LocalPlayer.anglerQuestsFinished >= 15);
            Condition angler20 = new Condition("Mods.Fargowiltas.Conditions.Angler20", () => Main.LocalPlayer.anglerQuestsFinished >= 20);
            Condition angler25 = new Condition("Mods.Fargowiltas.Conditions.Angler25", () => Main.LocalPlayer.anglerQuestsFinished >= 25);
            Condition angler30 = new Condition("Mods.Fargowiltas.Conditions.Angler30", () => Main.LocalPlayer.anglerQuestsFinished >= 30);
            Condition InRockOrDirtLayerHeight = new Condition("Mods.Fargowiltas.Conditions.InRockOrDirtLayerHeight", () => (Condition.InDirtLayerHeight.IsMet() || Condition.InRockLayerHeight.IsMet()) && !(Condition.InUndergroundDesert.IsMet() || Condition.InDungeon.IsMet()));
            #endregion
            

            if (FargoServerConfig.Instance.NPCSales)
            {
                //Only use "condition" if the item has a single condition, otherwise use the "conditions" array.
                void AddItem(int itemID, int customPrice = -1, Condition condition = null, Condition[] conditions = null)
                {
                    if (condition != null)
                    {
                        conditions = [condition];
                    }
                    if (conditions != null)
                    {
                        if (customPrice != -1)
                            shop.Add(new Item(itemID) { shopCustomPrice = customPrice }, conditions);
                        else
                            shop.Add(itemID, conditions);
                    }
                    else
                    {
                        if (customPrice != -1)
                            shop.Add(new Item(itemID) { shopCustomPrice = customPrice });
                        else
                            shop.Add(itemID);
                    }
                }

                switch (shop.NpcType)
                {
                    case NPCID.PartyGirl:
                        AddItem(ItemID.SliceOfCake, condition: Condition.BirthdayParty);
                        break;

                    case NPCID.Clothier:
                        //AddItem(ItemID.PharaohsMask, Item.buyPrice(gold: 1));
                        //AddItem(ItemID.PharaohsRobe, Item.buyPrice(gold: 1));

                        //AddItem(ItemID.AnglerHat, condition: angler10);
                        //AddItem(ItemID.AnglerVest, condition: angler15);
                        //AddItem(ItemID.AnglerPants, condition: angler20);

                        //AddItem(ItemID.BlueBrick, Item.buyPrice(silver: 1));

                        //AddItem(ItemID.GreenBrick, Item.buyPrice(silver: 1));

                        //AddItem(ItemID.PinkBrick, Item.buyPrice(silver: 1));

                        AddItem(ItemType<BrittleBone>(), condition: new Condition("Mods.Fargowiltas.Conditions.BrittleBone", () => Main.LocalPlayer.inventory.Any(i => !i.IsAir && i.useAmmo == ItemID.Bone)));
                        break;

                    case NPCID.Merchant:
                        
                        //AddItem(ItemID.FuzzyCarrot, condition: angler5);
                        //AddItem(ItemID.AnglerEarring, condition: angler10);
                        //AddItem(ItemID.HighTestFishingLine, condition: angler10);
                        //AddItem(ItemID.TackleBox, condition: angler10);
                        //AddItem(ItemID.GoldenBugNet, condition: angler10);
                        //AddItem(ItemID.FishHook, condition: angler10);

                        //AddItem(ItemID.FinWings, conditions: [angler10, Condition.Hardmode]);
                        //AddItem(ItemID.SuperAbsorbantSponge, conditions: [angler10, Condition.Hardmode]); ;
                        //AddItem(ItemID.BottomlessBucket, conditions: [angler10, Condition.Hardmode]);
                        //AddItem(ItemID.HotlineFishingHook, conditions: [angler25, Condition.Hardmode]);
                        //AddItem(ItemID.GoldenFishingRod, conditions: [angler30, Condition.Hardmode]);

                        AddItem(ItemID.Seed, 3, condition: new Condition("Mods.Fargowiltas.Conditions.Seeds", () => Main.LocalPlayer.inventory.Any(i => !i.IsAir && i.useAmmo == AmmoID.Dart)));
                        break;

                    case NPCID.Painter:

                        bool decorTab = true;
                        foreach (NPCShop.Entry entry in shop.Entries)
                        {
                            if (!entry.Item.IsAir && entry.Item.type == ItemID.Paintbrush)
                            {
                                decorTab = false;
                                break;
                            }
                        }

                        if (!decorTab) 
                            break; //dont sell in normal tab to prevent overflow

                        AddItem(ItemID.BloodMoonRising, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.BoneWarp, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TheCreationoftheGuide, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TheCursedMan, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TheDestroyer, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.Dryadisque, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TheEyeSeestheEnd, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.FacingtheCerebralMastermind, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.GloryoftheFire, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.GoblinsPlayingPoker, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.GreatWave, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TheGuardiansGaze, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TheHangedMan, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.Impact, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.ThePersistencyofEyes, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.PoweredbyBirds, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TheScreamer, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.SkellingtonJSkellingsworth, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.SparkyPainting, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.SomethingEvilisWatchingYou, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.StarryNight, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TrioSuperHeroes, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.TheTwinsHaveAwoken, Item.buyPrice(gold: 1), condition: Condition.InDungeon);
                        AddItem(ItemID.UnicornCrossingtheHallows, Item.buyPrice(gold: 1), condition: Condition.InDungeon);

                        AddItem(ItemID.AmericanExplosive, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.CrownoDevoursHisLunch, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.Discover, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.FatherofSomeone, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.FindingGold, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.GloriousNight, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.GuidePicasso, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.Land, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.TheMerchant, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.NurseLisa, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.OldMiner, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.RareEnchantment, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.Sunflowers, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.TerrarianGothic, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);
                        AddItem(ItemID.Waldo, Item.buyPrice(gold: 1), condition: InRockOrDirtLayerHeight);

                        AddItem(ItemID.DarkSoulReaper, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.Darkness, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.DemonsEye, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.FlowingMagma, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.HandEarth, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.ImpFace, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.LakeofFire, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.LivingGore, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.OminousPresence, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.ShiningMoon, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.Skelehead, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);
                        AddItem(ItemID.TrappedGhost, Item.buyPrice(gold: 1), condition: Condition.InUnderworldHeight);

                        //deserttt
                        AddItem(ItemID.AndrewSphinx, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.WatchfulAntlion, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.BurningSpirit, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.JawsOfDeath, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.TheSandsOfSlime, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.SnakesIHateSnakes, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.LifeAboveTheSand, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.Oasis, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.PrehistoryPreserved, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.AncientTablet, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.Uluru, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.VisitingThePyramids, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.BandageBoy, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        AddItem(ItemID.DivineEye, Item.buyPrice(gold: 1), condition: Condition.InUndergroundDesert);
                        break;

                    case NPCID.Demolitionist:
                        AddItem(ItemType<BoomShuriken>(), Item.buyPrice(0, 0, 2, 50));
                        /*AddItem(ItemID.CopperOre, condition: Condition.Hardmode);
                        AddItem(ItemID.TinOre, condition: Condition.Hardmode);
                        AddItem(ItemID.IronOre, condition: Condition.Hardmode);
                        AddItem(ItemID.LeadOre, condition: Condition.Hardmode);
                        AddItem(ItemID.SilverOre, condition: Condition.Hardmode);
                        AddItem(ItemID.TungstenOre, condition: Condition.Hardmode);
                        AddItem(ItemID.GoldOre, condition: Condition.Hardmode);
                        AddItem(ItemID.PlatinumOre, condition: Condition.Hardmode);

                        AddItem(ItemID.Meteorite, condition: Condition.DownedPlantera);
                        AddItem(ItemID.DemoniteOre, condition: Condition.DownedPlantera);
                        AddItem(ItemID.CrimtaneOre, condition: Condition.DownedPlantera);
                        AddItem(ItemID.Hellstone, condition: Condition.DownedPlantera);

                        AddItem(ItemID.CobaltOre, condition: Condition.DownedMoonLord);
                        AddItem(ItemID.PalladiumOre, condition: Condition.DownedMoonLord);
                        AddItem(ItemID.MythrilOre, condition: Condition.DownedMoonLord);
                        AddItem(ItemID.OrichalcumOre, condition: Condition.DownedMoonLord);
                        AddItem(ItemID.AdamantiteOre, condition: Condition.DownedMoonLord);
                        AddItem(ItemID.TitaniumOre, condition: Condition.DownedMoonLord);
                        AddItem(ItemID.ChlorophyteOre, condition: Condition.DownedMoonLord);*/

                        break;

                    case NPCID.WitchDoctor:
                        bool alreadySellsTable = false;
                        foreach(NPCShop.Entry entry in shop.Entries)
                        {
                            if (!entry.Item.IsAir && entry.Item.type == ItemID.BewitchingTable)
                            {
                                alreadySellsTable = true;
                                break;
                            }
                        }

                        if (!alreadySellsTable)
                            AddItem(ItemID.BewitchingTable, condition: Condition.DownedSkeletron);
                        break;

                    case NPCID.Steampunker:
                        AddItem(ItemID.PurpleSolution, conditions: [Condition.CrimsonWorld, Condition.InGraveyard]);
                        AddItem(ItemID.RedSolution, conditions: [Condition.CorruptWorld, Condition.InGraveyard]);
                        break;

                    case NPCID.DyeTrader:
                        AddItem(ItemID.RedDye, condition: new Condition("Mods.Fargowiltas.Conditions.RedHusk", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["RedHusk"]));
                        AddItem(ItemID.OrangeDye, condition: new Condition("Mods.Fargowiltas.Conditions.OrangeBloodroot", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["OrangeBloodroot"]));
                        AddItem(ItemID.YellowDye, condition: new Condition("Mods.Fargowiltas.Conditions.YellowMarigold", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["YellowMarigold"]));
                        AddItem(ItemID.LimeDye, condition: new Condition("Mods.Fargowiltas.Conditions.LimeKelp", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["LimeKelp"]));
                        AddItem(ItemID.GreenDye, condition: new Condition("Mods.Fargowiltas.Conditions.GreenMushroom", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["GreenMushroom"]));
                        AddItem(ItemID.TealDye, condition: new Condition("Mods.Fargowiltas.Conditions.TealMushroom", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["TealMushroom"]));
                        AddItem(ItemID.CyanDye, condition: new Condition("Mods.Fargowiltas.Conditions.CyanHusk", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["CyanHusk"]));
                        AddItem(ItemID.SkyBlueDye, condition: new Condition("Mods.Fargowiltas.Conditions.SkyBlueFlower", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["SkyBlueFlower"]));
                        AddItem(ItemID.BlueDye, condition: new Condition("Mods.Fargowiltas.Conditions.BlueBerries", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["BlueBerries"]));
                        AddItem(ItemID.PurpleDye, condition: new Condition("Mods.Fargowiltas.Conditions.PurpleMucos", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["PurpleMucos"]));
                        AddItem(ItemID.VioletDye, condition: new Condition("Mods.Fargowiltas.Conditions.VioletHusk", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["VioletHusk"]));
                        AddItem(ItemID.PinkDye, condition: new Condition("Mods.Fargowiltas.Conditions.PinkPricklyPear", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["PinkPricklyPear"]));
                        AddItem(ItemID.BlackDye, condition: new Condition("Mods.Fargowiltas.Conditions.BlackInk", () => Main.LocalPlayer.GetModPlayer<FargoPlayer>().FirstDyeIngredients["BlackInk"]));

                        break;

                    case NPCID.Dryad:
                        AddItem(ItemID.NaturesGift, Item.buyPrice(gold: 10));
                        AddItem(ItemID.JungleRose, Item.buyPrice(gold: 20));

                        AddItem(ItemID.StrangePlant1, Item.buyPrice(gold: 20), condition: Condition.Hardmode);
                        AddItem(ItemID.StrangePlant2, Item.buyPrice(gold: 20), condition: Condition.Hardmode);
                        AddItem(ItemID.StrangePlant3, Item.buyPrice(gold: 20), condition: Condition.Hardmode);
                        AddItem(ItemID.StrangePlant4, Item.buyPrice(gold: 20), condition: Condition.Hardmode);
                        break;

                    case NPCID.Wizard:
                        AddItem(ItemID.SuperManaPotion, condition: Condition.DownedGolem);
                        break;

                    case NPCID.Pirate:
                        AddItem(ItemType<GoldenDippingVat>(), Item.buyPrice(gold: 35), condition: Condition.Hardmode);
                        break;
                }
            }        
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            FargoPlayer fargoPlayer = player.FargoMutant();

            if (fargoPlayer.BattleCry)
            {
                spawnRate = (int)(spawnRate * 0.1);
                maxSpawns = (int)(maxSpawns * 10f);
            }

            if (fargoPlayer.CalmingCry)
            {
                float cryStrength = 1.15f; // 1 + strength of spawn rate decrease
                const float strPerBoss = 0.15f;
                if (Main.hardMode)
                    cryStrength += strPerBoss;
                if (NPC.downedMechBossAny)
                    cryStrength += strPerBoss;
                if (NPC.downedPlantBoss)
                    cryStrength += strPerBoss;
                if (NPC.downedGolemBoss)
                    cryStrength += strPerBoss;
                if (NPC.downedAncientCultist)
                    cryStrength += strPerBoss;

                spawnRate = (int)(spawnRate * cryStrength);
                maxSpawns = (int)(maxSpawns * (1 / cryStrength));
            }

            if ((FargoWorld.OverloadGoblins || FargoWorld.OverloadPirates) && player.position.X > Main.invasionX * 16.0 - 3000 && player.position.X < Main.invasionX * 16.0 + 3000)
            {
                if (FargoWorld.OverloadGoblins)
                {
                    spawnRate = (int)(spawnRate * 0.2);
                    maxSpawns = (int)(maxSpawns * 10f);
                }
                else if (FargoWorld.OverloadPirates)
                {
                    spawnRate = (int)(spawnRate * 0.2);
                    maxSpawns = (int)(maxSpawns * 30f);
                }
            }

            if (FargoWorld.OverloadPumpkinMoon || FargoWorld.OverloadFrostMoon)
            {
                spawnRate = (int)(spawnRate * 0.2);
                maxSpawns = (int)(maxSpawns * 10f);
            }
            else if (FargoWorld.OverloadMartians)
            {
                spawnRate = (int)(spawnRate * 0.2);
                maxSpawns = (int)(maxSpawns * 30f);
            }

            if (AnyBossAlive() && FargoServerConfig.Instance.BossZen && player.Distance(Main.npc[boss].Center) < 6000)
            {
                maxSpawns = 0;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Player player = Main.LocalPlayer;

            if (FargoWorld.OverloadGoblins && player.position.X > Main.invasionX * 16.0 - 3000 && player.position.X < Main.invasionX * 16.0 + 3000)
            {
                // Literally nothing in the pool in the invasion so set everything to custom
                pool[NPCID.GoblinSummoner] = 1f;
                pool[NPCID.GoblinArcher] = 3f;
                pool[NPCID.GoblinPeon] = 5f;
                pool[NPCID.GoblinSorcerer] = 3f;
                pool[NPCID.GoblinWarrior] = 5f;
                pool[NPCID.GoblinThief] = 5f;
                pool[NPCID.GoblinScout] = 3f;
            }
            else if (FargoWorld.OverloadPirates && player.position.X > Main.invasionX * 16.0 - 3000 && player.position.X < Main.invasionX * 16.0 + 3000)
            {
                // Literally nothing in the pool in the invasion so set everything to custom
                if (NPC.CountNPCS(NPCID.PirateShip) < 4)
                {
                    pool[NPCID.PirateShip] = .5f;
                }

                pool[NPCID.Parrot] = 2f;
                pool[NPCID.PirateCaptain] = 1f;
                pool[NPCID.PirateCrossbower] = 3f;
                pool[NPCID.PirateCorsair] = 5f;
                pool[NPCID.PirateDeadeye] = 4f;
                pool[NPCID.PirateDeckhand] = 5f;
            }

            else if (FargoWorld.OverloadPumpkinMoon)
            {
                pool[NPCID.Pumpking] = 4f;
                pool[NPCID.MourningWood] = 4f;
                pool[NPCID.HeadlessHorseman] = 3f;
                pool[NPCID.Scarecrow1] = .5f;
                pool[NPCID.Scarecrow2] = .5f;
                pool[NPCID.Scarecrow3] = .5f;
                pool[NPCID.Scarecrow4] = .5f;
                pool[NPCID.Scarecrow5] = .5f;
                pool[NPCID.Scarecrow6] = .5f;
                pool[NPCID.Scarecrow7] = .5f;
                pool[NPCID.Scarecrow8] = .5f;
                pool[NPCID.Scarecrow9] = .5f;
                pool[NPCID.Scarecrow10] = .5f;
                pool[NPCID.Hellhound] = 3f;
                pool[NPCID.Poltergeist] = 3f;
                pool[NPCID.Splinterling] = 3f;
            }
            else if (FargoWorld.OverloadFrostMoon)
            {
                pool[NPCID.IceQueen] = 5f;
                pool[NPCID.Everscream] = 5f;
                pool[NPCID.SantaNK1] = 5f;
                pool[NPCID.ZombieElf] = 1f;
                pool[NPCID.ZombieElfBeard] = 1f;
                pool[NPCID.ZombieElfGirl] = 1f;
                pool[NPCID.GingerbreadMan] = 2f;
                pool[NPCID.ElfArcher] = 2f;
                pool[NPCID.Nutcracker] = 3f;
                pool[NPCID.ElfCopter] = 3f;
                pool[NPCID.Flocko] = 2f;
                pool[NPCID.Yeti] = 4f;
                pool[NPCID.PresentMimic] = 2f;
                pool[NPCID.Krampus] = 4f;
            }
            else if (FargoWorld.OverloadMartians)
            {
                pool[NPCID.MartianSaucerCore] = 1f;
                pool[NPCID.Scutlix] = 3f;
                pool[NPCID.ScutlixRider] = 2f;
                pool[NPCID.MartianWalker] = 3f;
                pool[NPCID.MartianDrone] = 2f;
                pool[NPCID.GigaZapper] = 1f;
                pool[NPCID.MartianEngineer] = 2f;
                pool[NPCID.MartianOfficer] = 2f;
                pool[NPCID.RayGunner] = 1f;
                pool[NPCID.GrayGrunt] = 1f;
                pool[NPCID.BrainScrambler] = 1f;
            }

            foreach (var spawnBooster in Main.LocalPlayer.FargoMutant().ActiveSpawnBoosters)
            {
                if (!spawnBooster.SpawnCondition.IsMet())
                    continue;
                foreach (var npcID in spawnBooster.NPCTypes)
                {
                    if (!pool.ContainsKey(npcID))
                        pool[npcID] = spawnBooster.SpawnRate;
                    else
                        pool[npcID] += spawnBooster.SpawnRate;
                }
            }
        }

        public override bool PreKill(NPC npc)
        {
            if (NoLoot)
            {
                return false;
            }

            if (Fargowiltas.SwarmActive && (npc.type == NPCID.BlueSlime || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.Creeper || npc.type >= NPCID.PirateCorsair && npc.type <= NPCID.PirateCrossbower))
            {
                return false;
            }

            if (SwarmActive && Fargowiltas.SwarmActive && Main.netMode != NetmodeID.MultiplayerClient)
            {
                switch (npc.type)
                {
                    case NPCID.KingSlime:
                        Swarm(npc, NPCID.KingSlime, NPCID.BlueSlime, ItemID.KingSlimeBossBag, ItemID.KingSlimeTrophy,  ItemType<EnergizerSlime>());
                        break;

                    case NPCID.EyeofCthulhu:
                        Swarm(npc, NPCID.EyeofCthulhu, NPCID.ServantofCthulhu, ItemID.EyeOfCthulhuBossBag, ItemID.EyeofCthulhuTrophy, ItemType<EnergizerEye>());
                        break;

                    case NPCID.EaterofWorldsHead:
                        Swarm(npc, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail, ItemID.EaterOfWorldsBossBag, ItemID.EaterofWorldsTrophy, ItemType<EnergizerWorm>());
                        break;

                    case NPCID.BrainofCthulhu:
                        Swarm(npc, NPCID.BrainofCthulhu, NPCID.Creeper, ItemID.BrainOfCthulhuBossBag, ItemID.BrainofCthulhuTrophy, ItemType<EnergizerBrain>());
                        break;

                    case NPCID.DD2DarkMageT1:
                        Swarm(npc, NPCID.DD2DarkMageT1, -1, ItemID.DefenderMedal, ItemID.BossTrophyDarkmage, ItemType<EnergizerDarkMage>());
                        break;

                    case NPCID.Deerclops:
                        Swarm(npc, NPCID.Deerclops, -1, ItemID.DeerclopsBossBag, ItemID.DeerclopsTrophy, ItemType<EnergizerDeer>());
                        break;

                    case NPCID.QueenBee:
                        Swarm(npc, NPCID.QueenBee, NPCID.BeeSmall, ItemID.QueenBeeBossBag, ItemID.QueenBeeTrophy, ItemType<EnergizerBee>());
                        break;

                    case NPCID.SkeletronHead:
                        Swarm(npc, NPCID.SkeletronHead, -1, ItemID.SkeletronBossBag, ItemID.SkeletronTrophy, ItemType<EnergizerSkele>());
                        break;

                    case NPCID.WallofFlesh:
                        Swarm(npc, NPCID.WallofFlesh, NPCID.TheHungry, ItemID.WallOfFleshBossBag, ItemID.WallofFleshTrophy, ItemType<EnergizerWall>());
                        break;

                    case NPCID.QueenSlimeBoss:
                        Swarm(npc, NPCID.QueenSlimeBoss, NPCID.QueenSlimeMinionPink, ItemID.QueenSlimeBossBag, ItemID.QueenSlimeTrophy, ItemType<EnergizerQueenSlime>());
                        break;

                    case NPCID.TheDestroyer:
                        Swarm(npc, NPCID.TheDestroyer, NPCID.Probe, ItemID.DestroyerBossBag, ItemID.DestroyerTrophy, ItemType<EnergizerDestroy>());
                        break;

                    case NPCID.Retinazer:
                        Swarm(npc, NPCID.Retinazer, -1, ItemID.TwinsBossBag, ItemID.RetinazerTrophy, ItemType<EnergizerTwins>());
                        break;

                    case NPCID.Spazmatism:
                        Swarm(npc, NPCID.Spazmatism, -1, -1, ItemID.SpazmatismTrophy, -1);
                        break;

                    case NPCID.SkeletronPrime:
                        Swarm(npc, NPCID.SkeletronPrime, -1, ItemID.SkeletronPrimeBossBag, ItemID.SkeletronPrimeTrophy, ItemType<EnergizerPrime>());
                        break;

                    case NPCID.Plantera:
                        Swarm(npc, NPCID.Plantera, NPCID.PlanterasHook, ItemID.PlanteraBossBag, ItemID.PlanteraTrophy, ItemType<EnergizerPlant>());
                        break;

                    case NPCID.Golem:
                        Swarm(npc, NPCID.Golem, NPCID.GolemHeadFree, ItemID.GolemBossBag, ItemID.GolemTrophy, ItemType<EnergizerGolem>());
                        break;

                    case NPCID.DD2Betsy:
                        Swarm(npc, NPCID.DD2Betsy, NPCID.DD2WyvernT3, ItemID.BossBagBetsy, ItemID.BossTrophyBetsy, ItemType<EnergizerBetsy>());
                        break;

                    case NPCID.DukeFishron:
                        Swarm(npc, NPCID.DukeFishron, NPCID.Sharkron, ItemID.FishronBossBag, ItemID.DukeFishronTrophy, ItemType<EnergizerFish>());
                        break;

                    case NPCID.HallowBoss:
                        Swarm(npc, NPCID.HallowBoss, -1, ItemID.FairyQueenBossBag, ItemID.FairyQueenTrophy, ItemType<EnergizerEmpress>());
                        break;

                    case NPCID.CultistBoss:
                        Swarm(npc, NPCID.CultistBoss, -1, ItemID.CultistBossBag, ItemID.AncientCultistTrophy, ItemType<EnergizerCultist>());
                        return false; // no pillar spawn

                    case NPCID.MoonLordCore:
                        Swarm(npc, NPCID.MoonLordCore, NPCID.MoonLordFreeEye, ItemID.MoonLordBossBag, ItemID.MoonLordTrophy, ItemType<EnergizerMoon>());
                        break;

                    case NPCID.DungeonGuardian:
                        Swarm(npc, NPCID.DungeonGuardian, -1, -1, ItemID.BoneKey, ItemType<EnergizerDG>());
                        break;
                }

                //return false;
            }

            if (!PandoraActive)
            {
                return base.PreKill(npc);
            }

            return false;
        }

        public override void OnKill(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Painter:
                    if (NPC.AnyNPCs(NPCID.MoonLordCore))
                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemType<EchPainting>());
                    break;

                case NPCID.DD2OgreT2:
                case NPCID.DD2OgreT3:
                    if (!DD2Event.Ongoing)
                    {
                        if (Main.rand.NextBool(14))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.BossMaskOgre);

                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] { ItemID.ApprenticeScarf, ItemID.SquireShield, ItemID.HuntressBuckler, ItemID.MonkBelt, ItemID.DD2SquireDemonSword, ItemID.MonkStaffT1, ItemID.MonkStaffT2, ItemID.BookStaff, ItemID.DD2PhoenixBow, ItemID.DD2PetGhost }));

                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.GoldCoin, Main.rand.Next(4, 7));
                    }
                    break;

                case NPCID.DD2DarkMageT1:
                case NPCID.DD2DarkMageT3:
                    if (!DD2Event.Ongoing)
                    {
                        if (Main.rand.NextBool(14))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.BossMaskDarkMage);

                        if (Main.rand.NextBool(10))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.NextBool() ? ItemID.WarTable : ItemID.WarTableBanner);

                        if (Main.rand.NextBool(6))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] { ItemID.DD2PetGato, ItemID.DD2PetDragon }));
                    }
                    break;

                case NPCID.HeadlessHorseman:
                    if (FargoUtils.ActuallyNight && !Main.pumpkinMoon)
                    {
                        if (Main.rand.NextBool(10))
                            Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.JackOLanternMask);
                    }
                    break;

                case NPCID.MourningWood:
                    if (FargoUtils.ActuallyNight && !Main.pumpkinMoon)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, ItemID.SpookyWood, 30);

                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                            ItemID.SpookyHook,
                            ItemID.SpookyTwig,
                            ItemID.StakeLauncher,
                            ItemID.CursedSapling,
                            ItemID.NecromanticScroll,
                            Main.expertMode ? ItemID.WitchBroom : ItemID.SpookyWood
                        }));
                    }
                    break;

                case NPCID.Pumpking:
                    if (FargoUtils.ActuallyNight && !Main.pumpkinMoon)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                            ItemID.TheHorsemansBlade,
                            ItemID.BatScepter,
                            ItemID.BlackFairyDust,
                            ItemID.SpiderEgg,
                            ItemID.RavenStaff,
                            ItemID.CandyCornRifle,
                            ItemID.JackOLanternLauncher,
                            ItemID.ScytheWhip
                        }));
                    }
                    break;

                case NPCID.Everscream:
                    if (FargoUtils.ActuallyNight && !Main.snowMoon)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                            ItemID.ChristmasTreeSword,
                            ItemID.ChristmasHook,
                            ItemID.Razorpine,
                            ItemID.FestiveWings
                        }));
                    }
                    break;

                case NPCID.SantaNK1:
                    if (FargoUtils.ActuallyNight && !Main.snowMoon)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                            ItemID.ElfMelter,
                            ItemID.ChainGun
                        }));
                    }
                    break;

                case NPCID.IceQueen:
                    if (FargoUtils.ActuallyNight && !Main.snowMoon)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, Main.rand.Next(new int[] {
                            ItemID.BlizzardStaff,
                            ItemID.SnowmanCannon,
                            ItemID.NorthPole,
                            ItemID.BabyGrinchMischiefWhistle,
                            ItemID.ReindeerBells
                        }));
                    }
                    break;

                default:
                    break;
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            /*switch (npc.type)
            {
                case NPCID.ZombieEskimo:
                case NPCID.ArmedZombieEskimo:
                case NPCID.Penguin:
                case NPCID.IceSlime:
                case NPCID.SpikedIceSlime:
                    npcLoot.Add(ItemDropRule.OneFromOptions(20, ItemID.EskimoHood, ItemID.EskimoCoat, ItemID.EskimoPants));
                    break;

                case NPCID.MossHornet:
                case NPCID.JungleCreeper:
                case NPCID.JungleCreeperWall:
                case NPCID.AngryTrapper:
                case NPCID.GiantTortoise:
                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ItemType<JungleChest>(), 50));
                    break;
                case NPCID.Moth:
                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ItemType<JungleChest>(), 10));
                    break;

                case NPCID.GreekSkeleton:
                    npcLoot.RemoveWhere(rule => rule is CommonDrop drop && (drop.itemId == ItemID.GladiatorHelmet || drop.itemId == ItemID.GladiatorBreastplate || drop.itemId == ItemID.GladiatorLeggings));
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ItemID.GladiatorHelmet, ItemID.GladiatorBreastplate, ItemID.GladiatorLeggings));
                    break;

                case NPCID.Merchant:
                    npcLoot.Add(ItemDropRule.Common(ItemID.MiningShirt, 8));
                    npcLoot.Add(ItemDropRule.Common(ItemID.MiningPants, 8));
                    break;

                case NPCID.Nurse:
                    npcLoot.Add(ItemDropRule.Common(ItemID.LifeCrystal, 5));
                    break;

                case NPCID.Demolitionist:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Dynamite, 2, 5, 5));
                    break;

                case NPCID.Dryad:
                    npcLoot.Add(ItemDropRule.Common(ItemID.HerbBag, 3));
                    break;

                case NPCID.DD2Bartender:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Ale, 2, 4, 4));
                    break;

                case NPCID.Cyborg:
                    npcLoot.Add(ItemDropRule.Common(ItemID.NanoBullet, 4, 30, 30));
                    break;

                case NPCID.Clothier:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Skull, 20));
                    break;

                case NPCID.Mechanic:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Wire, 5, 40, 40));
                    break;

                case NPCID.Wizard:
                    npcLoot.Add(ItemDropRule.Common(ItemID.FallenStar, 5, 5, 5));
                    break;

                case NPCID.TaxCollector:
                    npcLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 8, 10, 10));
                    break;

                case NPCID.Truffle:
                    npcLoot.Add(ItemDropRule.Common(ItemID.MushroomStatue, 8));
                    break;

                case NPCID.Angler:
                    npcLoot.Add(ItemDropRule.OneFromOptions(2, ItemID.OldShoe, ItemID.TinCan, ItemID.FishingSeaweed));
                    break;


                case NPCID.DD2OgreT2:
                case NPCID.DD2OgreT3:
                    npcLoot.Add(ItemDropRule.Common(ItemID.DefenderMedal, 1, 20, 20));
                    break;

                case NPCID.DD2DarkMageT1:
                case NPCID.DD2DarkMageT3:
                    npcLoot.Add(ItemDropRule.Common(ItemID.DefenderMedal, 1, 5, 5));
                    break;

                case NPCID.Raven:
                    npcLoot.Add(ItemDropRule.Common(ItemID.GoodieBag));
                    break;

                case NPCID.SlimeRibbonRed:
                case NPCID.SlimeRibbonGreen:
                case NPCID.SlimeRibbonWhite:
                case NPCID.SlimeRibbonYellow:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Present));
                    break;

                case NPCID.BloodZombie:
                    npcLoot.Add(ItemDropRule.OneFromOptions(200, ItemID.BladedGlove, ItemID.BloodyMachete));
                    break;

                case NPCID.Clown:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Bananarang));
                    break;

                case NPCID.MoonLordCore:
                    npcLoot.Add(ItemDropRule.Common(ItemID.MoonLordLegs, 100));
                    break;
            }*/

            base.ModifyNPCLoot(npc, npcLoot);
        }


        public override bool CheckDead(NPC npc)
        {
            // Lumber Jaxe
            if (woodDrop && !npc.SpawnedFromStatue && !npc.friendly)
            {
                int WoodType()
                {
                    if (npc.lastInteraction != 255)
                    {
                        Player p = Main.player[npc.lastInteraction];
                        if (p.ZoneUnderworldHeight) return ItemID.AshWood;
                        if (p.ZoneGlowshroom) return ItemID.GlowingMushroom;
                        if (p.ZoneRockLayerHeight) return ItemID.StoneBlock;
                        if (Main.pumpkinMoon) return ItemID.SpookyWood;
                        if (p.ZoneDesert) return ItemID.Cactus;
                        if (p.ZoneHallow) return ItemID.Pearlwood;
                        if (p.ZoneCrimson) return ItemID.Shadewood;
                        if (p.ZoneCorrupt) return ItemID.Ebonwood;
                        if (p.ZoneJungle) return ItemID.RichMahogany; 
                        if (p.ZoneSnow) return ItemID.BorealWood;
                        if (p.ZoneBeach) return ItemID.PalmWood;
                    }

                    return ItemID.Wood;
                }
                Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, WoodType(), Main.rand.Next(10, 30));
            }

            switch (npc.type)
            {
                // Avoid lunar event with cultist summon
                case NPCID.CultistBoss:
                    if (!PillarSpawn)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc2 = Main.npc[i];
                            NPC.LunarApocalypseIsUp = false;

                            if (npc2.type == NPCID.LunarTowerNebula || npc2.type == NPCID.LunarTowerSolar || npc2.type == NPCID.LunarTowerStardust || npc2.type == NPCID.LunarTowerVortex)
                            {
                                NPC.TowerActiveSolar = true;
                                npc2.active = false;
                            }

                            NPC.TowerActiveSolar = false;
                        }
                    }

                    break;

                case NPCID.GiantWormHead:
                case NPCID.DiggerHead:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "worm");
                    break;

                case NPCID.DD2OgreT2:
                case NPCID.DD2OgreT3:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, "ogre");
                    break;

                case NPCID.DD2DarkMageT1:
                case NPCID.DD2DarkMageT3:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, "darkMage");
                    break;

                case NPCID.Clown:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "clown");

                    break;

                case NPCID.BlueSlime:
                    if (npc.netID == NPCID.Pinky)
                    {

                        FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "pinky");

                    }
                    break;

                case NPCID.UndeadMiner:

                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "undeadMiner");
                    break;

                case NPCID.Tim:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "tim");
                    break;

                case NPCID.DoctorBones:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "doctorBones");
                    break;

                case NPCID.Mimic:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimic");
                    break;

                case NPCID.WyvernHead:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "wyvern");
                    break;

                case NPCID.RuneWizard:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "runeWizard");
                    break;

                case NPCID.Nymph:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "nymph");
                    break;

                case NPCID.Moth:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "moth");
                    break;

                case NPCID.RainbowSlime:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "rainbowSlime");
                    break;

                case NPCID.Paladin:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "paladin");
                    break;

                case NPCID.Medusa:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "medusa");
                    break;

                case NPCID.IceGolem:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "iceGolem");
                    break;

                case NPCID.SandElemental:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "sandElemental");
                    break;

                case NPCID.Nailhead:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "nailhead");
                    break;

                case NPCID.Mothron:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3, "rareEnemy", "mothron");
                    break;

                case NPCID.BigMimicCorruption:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimicCorrupt");
                    break;

                case NPCID.BigMimicHallow:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimicHallow");
                    break;

                case NPCID.BigMimicCrimson:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimicCrimson");
                    break;

                case NPCID.BigMimicJungle:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "mimicJungle");
                    break;

                case NPCID.GoblinSummoner:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode && NPC.downedGoblins, "rareEnemy", "goblinSummoner");
                    break;

                case NPCID.PirateShip:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, NPC.downedPirates, "flyingDutchman");
                    break;

                case NPCID.DungeonSlime:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, NPC.downedBoss3, "rareEnemy", "dungeonSlime");
                    break;

                case NPCID.PirateCaptain:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode && NPC.downedPirates, "rareEnemy", "pirateCaptain");
                    break;

                case NPCID.SkeletonSniper:
                case NPCID.TacticalSkeleton:
                case NPCID.SkeletonCommando:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "skeletonGun");
                    break;

                case NPCID.Necromancer:
                case NPCID.NecromancerArmored:
                case NPCID.DiabolistRed:
                case NPCID.DiabolistWhite:
                case NPCID.RaggedCaster:
                case NPCID.RaggedCasterOpenCoat:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "skeletonMage");
                    break;

                case NPCID.BoneLee:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, NPC.downedPlantBoss, "rareEnemy", "boneLee");
                    break;

                case NPCID.HeadlessHorseman:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, "headlessHorseman");
                    break;

                case NPCID.Pumpking:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, NPC.downedHalloweenKing, "pumpking");
                    break;

                case NPCID.MourningWood:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, NPC.downedHalloweenTree, "mourningWood");
                    break;

                case NPCID.IceQueen:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, NPC.downedChristmasIceQueen, "iceQueen");
                    break;

                case NPCID.SantaNK1:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, NPC.downedChristmasSantank, "santank");
                    break;

                case NPCID.Everscream:
                    FargoUtils.TryDowned("Abominationn", Color.Orange, NPC.downedChristmasTree, "everscream");
                    break;

                case NPCID.ZombieMerman:
                case NPCID.EyeballFlyingFish:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "zombieMerman", "eyeFish");
                    break;

                case NPCID.GoblinShark:
                case NPCID.BloodEelHead:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, Main.hardMode, "rareEnemy", "goblinShark", "bloodEel");
                    break;

                case NPCID.BloodNautilus:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "dreadnautilus");
                    break;

                case NPCID.Gnome:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "gnome");
                    break;

                case NPCID.RedDevil:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "redDevil");
                    break;

                case NPCID.GoldenSlime:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "goldenSlime");
                    break;

                case NPCID.GoblinScout:
                    FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", "goblinScout");
                    break;

                default:
                    break;
            }

            if (Fargowiltas.ModRareEnemies.ContainsKey(npc.type))
            {
                FargoUtils.TryDowned("Deviantt", Color.HotPink, "rareEnemy", Fargowiltas.ModRareEnemies[npc.type]);

            }

            if (npc.type == NPCID.DD2Betsy && !PandoraActive)
            {
                FargoUtils.PrintText(Language.GetTextValue("Announcement.HasBeenDefeated_Single", Lang.GetNPCNameValue(NPCID.DD2Betsy)), new Color(175, 75, 0));
                FargoWorld.DownedBools["betsy"] = true;
            }
            bool trojan = Fargowiltas.ModLoaded["FargowiltasSouls"] && TryFind("FargowiltasSouls", "TrojanSquirrel", out ModNPC trojanSqurrel) && npc.type == trojanSqurrel.Type;
            if (npc.boss && !trojan)
            {
                FargoWorld.DownedBools["boss"] = true;
            }

            return base.CheckDead(npc);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (FargoServerConfig.Instance.RottenEggs && projectile.type == ProjectileID.RottenEgg && npc.townNPC)
            {
                modifiers.FinalDamage *= 20;
                //damage *= 20;
            }
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            // No angler check enables luiafk compatibility
            if (FargoServerConfig.Instance.AnglerQuestInstantReset && Main.anglerQuestFinished)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.AnglerQuestSwap();
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    // Broadcast swap request to server
                    var netMessage = Mod.GetPacket();
                    netMessage.Write((byte)PacketID.AnglerReset);
                    netMessage.Send();
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (!npc.canDisplayBuffs) 
                return;

            if (woodDrop && Main.rand.NextBool(10))
            {
                int WoodDustType()
                {
                    if (npc.lastInteraction != 255)
                    {
                        Player p = Main.player[npc.lastInteraction];
                        if (p.ZoneUnderworldHeight) return Main.rand.NextBool(3) ? DustID.Torch : DustID.Smoke;
                        if (p.ZoneGlowshroom) return DustID.Bone; //yes this is the actual dust it uses
                        if (p.ZoneRockLayerHeight) return DustID.Stone;
                        if (Main.pumpkinMoon) return DustID.SpookyWood;
                        if (p.ZoneDesert) return DustID.t_Cactus;
                        if (p.ZoneHallow) return DustID.Pearlwood;
                        if (p.ZoneCrimson) return DustID.Shadewood;
                        if (p.ZoneCorrupt) return DustID.Ebonwood;
                        if (p.ZoneJungle) return DustID.RichMahogany;
                        if (p.ZoneSnow) return DustID.BorealWood;
                        if (p.ZoneBeach) return DustID.PalmWood;
                    }
                    return DustID.WoodFurniture;
                }
                Dust.NewDustDirect(npc.position, npc.width, npc.height, WoodDustType());
            }
        }

        private void SpawnBoss(NPC npc, int boss)
        {
            int spawn;

            if (SwarmActive)
            {
                if (npc.type == NPCID.WallofFlesh)
                {
                    NPC currentWoF = Main.npc[LastWoFIndex];
                    int startingPos = (int)currentWoF.position.X;
                    spawn = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), startingPos + 400 * WoFDirection, (int)currentWoF.position.Y, NPCID.WallofFlesh, 0);
                    if (spawn != Main.maxNPCs)
                    {
                        Main.npc[spawn].GetGlobalNPC<FargoGlobalNPC>().SwarmActive = true;
                        LastWoFIndex = spawn;
                    }
                }
                else
                {
                    spawn = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)npc.position.X + Main.rand.Next(-1000, 1000), (int)npc.position.Y + Main.rand.Next(-400, -100), boss);

                    if (spawn != Main.maxNPCs)
                    {
                        Main.npc[spawn].GetGlobalNPC<FargoGlobalNPC>().SwarmActive = true;
                        NetMessage.SendData(MessageID.SyncNPC, number: boss);
                    }
                }
            }
            else
            {
                // Pandora
                int random;

                do
                {
                    random = Main.rand.Next(Bosses);
                }
                while (NPC.CountNPCS(random) >= 4);

                spawn = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)npc.position.X + Main.rand.Next(-1000, 1000), (int)npc.position.Y + Main.rand.Next(-400, -100), random);
                if (spawn != Main.maxNPCs)
                {
                    Main.npc[spawn].GetGlobalNPC<FargoGlobalNPC>().PandoraActive = true;
                    NetMessage.SendData(MessageID.SyncNPC, number: random);
                }
            }
        }

        private void Swarm(NPC npc, int boss, int minion, int bossbag, int trophy, int reward)
        {
            if (bossbag >= 0 && bossbag != ItemID.DefenderMedal)
            {
                int stack = Fargowiltas.SwarmItemsUsed * 5 - 1;
                if (npc.type == NPCID.CultistBoss)
                    stack += 1;
                npc.DropItemInstanced(npc.Center, npc.Size, bossbag, itemStack: stack);
            }
            else if (bossbag >= 0 && bossbag == ItemID.DefenderMedal)
            {
                npc.DropItemInstanced(npc.Center, npc.Size, bossbag, itemStack: 5 * (Fargowiltas.SwarmItemsUsed * 5 - 1));
            }

            // Drop swarm reward for every 10 items used
            if (Fargowiltas.SwarmItemsUsed >= 10 && reward > 0)
                Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, reward, Stack: Fargowiltas.SwarmItemsUsed / 10);
                    

            //drop trophy for every 3 items
            if (Fargowiltas.SwarmItemsUsed >= 3 && trophy > 0)
                Item.NewItem(npc.GetSource_Loot(), npc.Hitbox, trophy, Stack: Fargowiltas.SwarmItemsUsed / 3);

            if (minion != -1)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == minion)
                    {
                        Main.npc[i].SimpleStrikeNPC(Main.npc[i].lifeMax, -Main.npc[i].direction, true, 0, null, false, 0, true);
                        //Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, -Main.npc[i].direction, true);
                    }
                }
            }
        }

        public static void SpawnWalls(Player player)
        {
            int startingPos;

            if (LastWoFIndex == -1)
            {
                startingPos = (int)player.position.X;
            }
            else
            {
                startingPos = (int)Main.npc[LastWoFIndex].position.X;
            }

            Vector2 pos = player.position;

            if (WoFDirection == 0)
            {
                //1 is to the right, -1 is left
                WoFDirection = player.position.X / 16 > Main.maxTilesX / 2 ? 1 : -1;
            }

            int wof = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), startingPos + 400 * WoFDirection, (int)pos.Y, NPCID.WallofFlesh, 0);
            Main.npc[wof].GetGlobalNPC<FargoGlobalNPC>().SwarmActive = true;

            LastWoFIndex = wof;
        }

        public static bool SpecificBossIsAlive(ref int bossID, int bossType)
        {
            if (bossID != -1)
            {
                if (Main.npc[bossID].active && Main.npc[bossID].type == bossType)
                {
                    return true;
                }
                else
                {
                    bossID = -1;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static int boss = -1;

        public static bool AnyBossAlive()
        {
            if (boss == -1)
                return false;

            NPC npc = Main.npc[boss];

            if (npc.active && npc.type != NPCID.MartianSaucerCore && (npc.boss || npc.type == NPCID.EaterofWorldsHead))
                return true;
            boss = -1;
            return false;
        }
    }
}