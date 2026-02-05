using System.Collections.Generic;
using System.Linq;
using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Achievements;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.Items.Vanity;
using Fargowiltas.Content.Items.Weapons;
using Fargowiltas.Content.Projectiles;
using Fargowiltas.Content.UI;
using Fargowiltas.Content.UI.Emotes;
using Fargowiltas.Content.UI.LumberjackUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.NPCs
{
    [AutoloadHead]
    public class LumberJack : ModNPC
    {
        private bool dayOver;
        private bool nightOver;


        //public override bool Autoload(ref string name)
        //{
        //    name = "LumberJack";
        //    return mod.Properties.Autoload;
        //}

        public override ITownNPCProfile TownNPCProfile()
        {
            return new LumberJackProfile();
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("LumberJack");

            Main.npcFrameCount[NPC.type] = 25;

            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 2;
            NPCID.Sets.FaceEmote[NPC.type] = ModContent.EmoteBubbleType<LumberJackEmote>();

            NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Love);

            NPC.Happiness.SetNPCAffection<Squirrel>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate);

        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.NPCs.LumberJack.Bestiary")
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 40;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = Main.hardMode ? 1000 : 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;

            //if (GetInstance<FargoConfig>().CatchNPCs)
            //{
            //    Main.npcCatchable[NPC.type] = true;
            //    NPC.catchItem = (short)mod.ItemType("LumberJack");
            //}
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            return FargoServerConfig.Instance.Lumber && FargoWorld.DownedBools.TryGetValue("lumberjack", out bool down) && down;
        }

        // Tree Shake spawn method
        public static void OnTreeShake(On_WorldGen.orig_ShakeTree orig, int i, int j)
        {
            orig(i, j);
            if (!(FargoServerConfig.Instance.Lumber && Main.rand.NextBool(10) && FargoWorld.WoodChopped >= 400 && !(FargoWorld.DownedBools.TryGetValue("lumberjack", out bool down) && down)))
                return;
            WorldGen.GetTreeBottom(i, j, out var x, out var y);
            TreeTypes treeType = WorldGen.GetTreeType(Main.tile[x, y].TileType);
            if (treeType == TreeTypes.None)
                return;
            y--;
            while (y > 10 && Main.tile[x, y].HasTile && TileID.Sets.IsShakeable[Main.tile[x, y].TileType])
            {
                y--;
            }
            y++;
            if (!WorldGen.IsTileALeafyTreeTop(x, y) || Collision.SolidTiles(x - 2, x + 2, y - 2, y + 2))
                return;

            FargoWorld.DownedBools["lumberjack"] = true;
            NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), x * 16, y * 16, NPCType<LumberJack>());
        }
        public override void Load()
        {
            On_WorldGen.ShakeTree += OnTreeShake;
        }


        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void AI()
        {
            if (!Main.dayTime)
            {
                nightOver = true;
            }

            if (Main.dayTime)
            {
                dayOver = true;
            }
        }


        public override List<string> SetNPCNameList()
        {
            string[] names =
               [Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName1"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName2"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName3"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName4"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName5"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName6"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName7"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName8"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName9"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName10"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName11"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.NPCName12")];

            return new List<string>(names);
        }

        public override string GetChat()
        {
            List<string> dialogue = Language.FindAll(Lang.CreateDialogFilter("Mods.Fargowiltas.NPCs.LumberJack.Chat.Normal")).Select(item => item.Value).ToList();
            
            int nurse = NPC.FindFirstNPC(NPCID.Nurse);
            if (nurse >= 0)
            {
                dialogue.Add(LumberChat("Nurse", Main.npc[nurse].GivenName));
            }

            Player player = Main.LocalPlayer;
            if (player.HeldItem.type == ItemID.LucyTheAxe)
            {
                dialogue.Add(LumberChat("LucyTheAxe"));
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.TreeTreasures");
        }

        public const string ShopName = "Shop";

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            Player player = Main.LocalPlayer;

            if (firstButton)
            {
                shopName = ShopName;
                return;
            }
            else
            {
                Main.npcChatText = "";
                FargoUIManager.Open<LumberJackUI>();
                return;
            }

            if (dayOver && nightOver)
            {
                string quote = "";
                int itemType;

                if (player.ZoneDesert && !player.ZoneBeach)
                {
                    quote = LumberChat("Desert");
                    itemType = Main.rand.Next(new int[] { ItemID.Scorpion, ItemID.BlackScorpion });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Cactus), ItemID.Cactus, 100);
                }
                else if (player.ZoneJungle)
                {
                    quote = LumberChat("Jungle");
                    itemType = Main.rand.Next(new int[] { ItemID.Buggy, ItemID.Sluggy, ItemID.Grubby, ItemID.Frog });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    itemType = Main.rand.Next(new int[] { ItemID.Mango, ItemID.Pineapple });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.RichMahogany), ItemID.RichMahogany, 50);
                }
                else if (player.ZoneHallow)
                {
                    quote = LumberChat("Hallow");
                    for (int i = 0; i < 5; i++)
                    {
                        itemType = Main.rand.Next(new int[] { ItemID.LightningBug, ItemID.FairyCritterBlue, ItemID.FairyCritterGreen, ItemID.FairyCritterPink });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                    }
                    itemType = Main.rand.Next(new int[] { ItemID.Starfruit, ItemID.Dragonfruit });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Pearlwood), ItemID.Pearlwood, 50);

                    //add prismatic lacewing if post plantera
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.EmpressButterfly), ItemID.EmpressButterfly, 1);
                }
                else if (player.ZoneGlowshroom && Main.hardMode)
                {
                    quote = LumberChat("GlowshroomHM");
                    itemType = Main.rand.Next(new int[] { ItemID.GlowingSnail, ItemID.TruffleWorm });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.GlowingMushroom), ItemID.GlowingMushroom, 50);
                    //add mushroom grass seeds

                }
                else if (player.ZoneCorrupt || player.ZoneCrimson)
                {
                    quote = LumberChat("Evil");
                    for (int i = 0; i < 5; i++)
                    {
                        itemType = Main.rand.Next(new int[] { ItemID.Elderberry, ItemID.BlackCurrant, ItemID.BloodOrange, ItemID.Rambutan });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                    }
                }
                else if (player.ZoneSnow)
                {
                    //penguin
                    quote = LumberChat("Snow");
                    itemType = Main.rand.Next(new int[] { ItemID.Cherry, ItemID.Plum });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.BorealWood), ItemID.BorealWood, 50);
                }
                else if (player.ZoneBeach)
                {
                    quote = LumberChat("Beach");
                    itemType = Main.rand.Next(new int[] { ItemID.Coconut, ItemID.Banana });
                    player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Seagull), ItemID.Seagull, 5);
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.PalmWood), ItemID.PalmWood, 50);
                }
                else if (player.ZoneUnderworldHeight)
                {
                    quote = LumberChat("Underworld");
                    for (int i = 0; i < 5; i++)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.AshWood), ItemID.AshWood, 50);
                        itemType = Main.rand.Next(new int[] { ItemID.HellButterfly, ItemID.MagmaSnail, ItemID.Lavafly });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                        itemType = Main.rand.Next(new int[] { ItemID.SpicyPepper, ItemID.Pomegranate });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                    }
                }
                else if (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight)
                {
                    if (Main.rand.NextBool(2))
                    {
                        quote = LumberChat("DirtRockGem");

                        for (int i = 0; i < 5; i++)
                        {
                            itemType = Main.rand.Next(new int[] { ItemID.Diamond, ItemID.Ruby, ItemID.Amethyst, ItemID.Emerald, ItemID.Sapphire, ItemID.Topaz, ItemID.Amber });
                            player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 3);

                            itemType = Main.rand.Next(new int[] { ItemID.GemSquirrelDiamond, ItemID.GemSquirrelAmber, ItemID.GemSquirrelAmethyst, ItemID.GemSquirrelEmerald, ItemID.GemSquirrelRuby, ItemID.GemSquirrelSapphire, ItemID.GemSquirrelTopaz, ItemID.GemBunnyAmber, ItemID.GemBunnyAmethyst, ItemID.GemBunnyDiamond, ItemID.GemBunnyEmerald, ItemID.GemBunnyRuby, ItemID.GemBunnySapphire, ItemID.GemBunnyTopaz });
                            player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 1);
                        }
                    }
                    else
                    {
                        quote = LumberChat("DirtRockMouse");

                        itemType = ItemID.Mouse;
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                    }
                }
                //purity, most common option likely
                else// if (player.position.Y > Main.worldSurface)
                {
                    if (Main.dayTime)
                    {
                        if (Main.WindyEnoughForKiteDrops && Main.rand.NextBool(2)) //ladybug
                        {
                            quote = LumberChat("CommonDayTimeWindy");
                            itemType = ItemID.LadyBug;
                            player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType, 5);
                        }
                        else if (Main.rand.NextBool(3)) //butterfly
                        {
                            quote = LumberChat("CommonDayTimeButterfly");
                            for (int i = 0; i < 5; i++)
                            {
                                itemType = Main.rand.Next(new int[] { ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly, ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly, ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly });
                                player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                            }
                        }
                        else if (Main.rand.NextBool(20))
                        {
                            quote = LumberChat("CommonDayTimeEucaluptusSap");
                            player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.EucaluptusSap), ItemID.EucaluptusSap);
                        }
                        else
                        {
                            quote = LumberChat("CommonDayTimeCritter");
                            for (int i = 0; i < 5; i++)
                            {
                                itemType = Main.rand.Next(new int[] { ItemID.Grasshopper, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.Bird, ItemID.BlueJay, ItemID.Cardinal });
                                player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                            }
                        }
                    }
                    else
                    {
                        quote = LumberChat("CommonNightTime");
                        player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Firefly), ItemID.Firefly);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        itemType = Main.rand.Next(new int[] { ItemID.Lemon, ItemID.Peach, ItemID.Apricot, ItemID.Grapefruit, ItemID.Apple });
                        player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                    }
                    player.QuickSpawnItem(player.GetSource_OpenItem(ItemID.Wood), ItemID.Wood, 50);
                }

                if (Main.myPlayer == player.whoAmI)
                {
                    ModContent.GetInstance<TreeTreasureAchievements.T1TreeTreasureAchievement>().Condition.Complete();
                    ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().IntCondition.Value++;
                }

                Main.npcChatText = quote;
                dayOver = false;
                nightOver = false;
            }
            else
            {
                Main.npcChatText = LumberChat("Rest");
            }
        }

        public static Condition OwnsRegionalWood(int woodID)
        {
            bool AnyoneOwnsWood()
            {
                foreach (Player player in Main.ActivePlayers)
                {
                    if (player.FargoMutant().ItemHasBeenOwned[woodID])
                        return true;
                }
                return false;
            }
            return new("Mods.Fargowiltas.Conditions.OwnsRegionalWood", AnyoneOwnsWood);
        }
        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add(new Item(ItemID.WoodPlatform) { shopCustomPrice = Item.buyPrice(copper: 5) })
                .Add(new Item(ItemID.Wood) { shopCustomPrice = Item.buyPrice(copper: 10) })
                .Add(new Item(ItemID.BorealWood) { shopCustomPrice = Item.buyPrice(copper: 10) }, OwnsRegionalWood(ItemID.BorealWood))
                .Add(new Item(ItemID.RichMahogany) { shopCustomPrice = Item.buyPrice(copper: 15) }, OwnsRegionalWood(ItemID.RichMahogany))
                .Add(new Item(ItemID.PalmWood) { shopCustomPrice = Item.buyPrice(copper: 15) }, OwnsRegionalWood(ItemID.PalmWood))
                .Add(new Item(ItemID.Ebonwood) { shopCustomPrice = Item.buyPrice(copper: 15) }, OwnsRegionalWood(ItemID.Ebonwood))
                .Add(new Item(ItemID.Shadewood) { shopCustomPrice = Item.buyPrice(copper: 15) }, OwnsRegionalWood(ItemID.Shadewood))
                .Add(new Item(ItemID.AshWood) { shopCustomPrice = Item.buyPrice(copper: 20) }, OwnsRegionalWood(ItemID.AshWood))
                .Add(new Item(ItemID.Pearlwood) { shopCustomPrice = Item.buyPrice(copper: 20) }, [OwnsRegionalWood(ItemID.Pearlwood), Condition.Hardmode])
                .Add(new Item(ItemID.SpookyWood) { shopCustomPrice = Item.buyPrice(copper: 50) }, [OwnsRegionalWood(ItemID.SpookyWood), Condition.DownedPumpking])
                .Add(new Item(ItemID.Cactus) { shopCustomPrice = Item.buyPrice(copper: 10) }, OwnsRegionalWood(ItemID.Cactus))
                .Add(new Item(ItemID.BambooBlock) { shopCustomPrice = Item.buyPrice(copper: 10) }, OwnsRegionalWood(ItemID.BambooBlock))
                .Add(new Item(ItemID.LivingWoodWand) { shopCustomPrice = Item.buyPrice(copper: 10000) })
                .Add(new Item(ItemType<LumberjackMask>()) { shopCustomPrice = Item.buyPrice(copper: 10000) })
                .Add(new Item(ItemType<LumberjackBody>()) { shopCustomPrice = Item.buyPrice(copper: 10000) })
                .Add(new Item(ItemType<LumberjackPants>()) { shopCustomPrice = Item.buyPrice(copper: 10000) })
                .Add(new Item(ItemType<LumberJaxe>()) { shopCustomPrice = Item.buyPrice(copper: 10000) })
                .Add(new Item(ItemID.SharpeningStation) { shopCustomPrice = Item.buyPrice(copper: 100000) })
                .Add(new Item(ItemType<WoodenToken>()) { shopCustomPrice = Item.buyPrice(copper: 10000) })
                ;

            npcShop.Register();
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileType<LumberJaxeProjectile>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void OnKill()
        {
            FargoWorld.DownedBools["lumberjack"] = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<LumberHat>(), 3));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, Scale: 0.8f);
                }

                if (!Main.dedServ)
                {
                    Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "LumberGore3").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "LumberGore2").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "LumberGore1").Type);
                }
            }
            else
            {
                for (int k = 0; k < hit.Damage / NPC.lifeMax * 50.0; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, Scale: 0.6f);
                }
            }
        }

        private static string LumberChat(string key, params object[] args) => Language.GetTextValue($"Mods.Fargowiltas.NPCs.LumberJack.Chat.{key}", args);
    }

    public class LumberJackProfile : ITownNPCProfile
    {
        public int RollVariation() => 0;
        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
                return Request<Texture2D>("Fargowiltas/Content/NPCs/LumberJack");
            if (npc.IsABestiaryIconDummy && npc.ForcePartyHatOn)
                return Request<Texture2D>("Fargowiltas/Content/NPCs/LumberJack_Party");

            if (npc.IsShimmerVariant)
            {
                if (npc.altTexture == 1)
                {
                    return Request<Texture2D>("Fargowiltas/Content/NPCs/Lumberjack_Shimmer_Party");
                }
                else
                {
                    return Request<Texture2D>("Fargowiltas/Content/NPCs/Lumberjack_Shimmer");
                }
            }

            if (npc.altTexture == 1)
                return Request<Texture2D>("Fargowiltas/Content/NPCs/LumberJack_Party");

            return Request<Texture2D>("Fargowiltas/Content/NPCs/LumberJack");
        }

        public int GetHeadTextureIndex(NPC npc) => GetModHeadSlot("Fargowiltas/Content/NPCs/LumberJack_Head");
    }
}
