using Fargowiltas.Common;
using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Biomes;
using Fargowiltas.Content.Items.Summons.Deviantt;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.Projectiles;
using Fargowiltas.Content.UI.Emotes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.NPCs
{
    [AutoloadHead]
    public class Deviantt : ModNPC
    {
        private bool canSayDefeatQuote = true;
        private int defeatQuoteTimer = 900;
        private int trolling;

        public override ITownNPCProfile TownNPCProfile()
        {
            return new DevianttProfile();
        }

        public override void SetStaticDefaults()
        {

            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.FaceEmote[NPC.type] = ModContent.EmoteBubbleType<DevianttEmote>();

            NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<SkyBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<JungleBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike);
            NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate);

            NPC.Happiness.SetNPCAffection<Mutant>(AffectionLevel.Love);
            NPC.Happiness.SetNPCAffection<Abominationn>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate);

            //new
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Suffocation] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Lovestruck] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Stinky] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            //var2

            NPC.AddDebuffImmunities(new List<int>()
            {
                BuffID.Suffocation,
                BuffID.Lovestruck,
                BuffID.Stinky,
                BuffID.OnFire
            });
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.NPCs.Deviantt.Bestiary")
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 36;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = NPC.downedMoonlord ? 50 : 15;
            NPC.lifeMax = NPC.downedMoonlord ? 2500 : Main.hardMode ? 1000 : 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Angler;

            //if (GetInstance<FargoConfig>().CatchNPCs)
            //{
            //    Main.NPCCatchable[NPC.type] = true;
            //    NPC.catchItem = (short)mod.ItemType("Deviantt");
            //}

        }
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            Mod souls = Fargowiltas.SoulsMod;
            if ((bool?)souls?.Call("DevianttAlive") == true)
                return false;

            return FargoServerConfig.Instance.Devi && !FargoUtils.AnyBossAlive()
                && ((FargoWorld.DownedBools.TryGetValue("rareEnemy", out bool value) && value)
                || FargoWorld.EternityMode);
        }

        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void AI()
        {
            /*if (Main.LocalPlayer.talkNPC == NPC.whoAmI)
            {
                FargoUIManager.Open<DevianttNPCUI>();
            }
            else
                FargoUIManager.Close<DevianttNPCUI>();*/

            NPC.breath = 200;
            if (defeatQuoteTimer > 0)
                defeatQuoteTimer--;
            else
                canSayDefeatQuote = false;

            if (++trolling > 180 * 60)
            {
                trolling = -Main.rand.Next(30 * 60);

                DoALittleTrolling();
            }
        }
        void DoALittleTrolling()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            //no trolling when possible danger
            if (FargoUtils.AnyBossAlive()
                || Main.npc.Any(n => n.active && n.damage > 0 && !n.friendly && NPC.Distance(n.Center) < 1200)
                || NPC.life < NPC.lifeMax)
                return;

            if (NPC.ai[0] == 10) //actual attack anim
                return;

            Vector2 targetPos = default;

            const float maxRange = 600f;
            float targetDistance = maxRange;

            void TryUpdateTarget(Vector2 possibleTarget)
            {
                if (targetDistance > NPC.Distance(possibleTarget)
                    && Collision.CanHitLine(NPC.Center, 0, 0, possibleTarget, 0, 0))
                {
                    Tile tileBelow = Framing.GetTileSafely(possibleTarget + 32f * Vector2.UnitY);
                    if (tileBelow.HasUnactuatedTile && Main.tileSolid[tileBelow.TileType] && !Main.tileSolidTop[tileBelow.TileType])
                    {
                        targetPos = possibleTarget;
                        targetDistance = NPC.Distance(possibleTarget);
                    }
                }
            }

            //pick a target
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].friendly && Main.npc[i].townNPC
                    && Main.npc[i].life == Main.npc[i].lifeMax
                    && i != NPC.whoAmI)
                    TryUpdateTarget(Main.npc[i].Center);
            }
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && !Main.player[i].ghost
                    && Main.player[i].statLife == Main.player[i].statLifeMax2)
                    TryUpdateTarget(Main.player[i].Center);
            }

            if (targetPos != default)
            {
                float distanceRatio = targetDistance / maxRange;

                //account for gravity
                targetPos.Y += 16f;
                targetPos.Y -= 20f * 3 * distanceRatio * distanceRatio;
                Vector2 vel = (8f + 12f * distanceRatio) * NPC.DirectionTo(targetPos);

                int type = Main.rand.NextBool() ? ProjectileID.LovePotion : ProjectileID.FoulPotion;
                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, vel, type, 0, 0f, Main.myPlayer);
                Main.projectile[p].npcProj = true;

                NPC.spriteDirection = NPC.direction = targetPos.X < NPC.Center.X ? -1 : 1;
                NPC.ai[0] = 10; //trick vanilla ai into thinking devi has just attacked, but dont actually attack
                NPC.ai[1] = NPCID.Sets.AttackTime[NPC.type] - 1; //sets localai[3] = 0 if exactly AttackTime
                NPC.localAI[3] = 300f; //counts up from zero and attacks at some threshold if left alone
                NPC.netUpdate = true;
            }
        }

        public override List<string> SetNPCNameList()
        {
            string[] names =
                [Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName1"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName2"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName3"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName4"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName5"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName6"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName7"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName8"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName9"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName10"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName11"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName12"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.NPCName13")];

            return new List<string>(names);
        }

        public override string GetChat() //=> string.Empty;
        {
            Mod souls = Fargowiltas.SoulsMod;
            if (FargoWorld.EternityMode
                && (bool?)souls?.Call("GiftsReceived") == false)
            {
                souls?.Call("GiveDevianttGifts");
                return Main.npcChatText = DeviChat("GiveGifts");
            }

            if (Main.notTheBeesWorld)
            {
                string text = DeviChat("NTB");
                int max = Main.rand.Next(10, 50);
                for (int i = 0; i < max; i++)
                    text += DeviChat("NTB" + Main.rand.Next(new string[] { "HA", "HA", "HEE", "HOO", "HEH", "HAH" }));
                text += Language.GetTextValue("Mods.Fargowiltas.MessageInfo.Common.Exclamation");
                return text;
            }

            if (souls != null && Main.rand.NextBool())
            {
                if ((bool)souls.Call("EridanusArmor"))
                    return DeviChat("EridanusArmor");

                if ((bool)souls.Call("NekomiArmor"))
                    return DeviChat("NekomiArmor");
            }

            if (NPC.homeless && canSayDefeatQuote && (bool?)souls?.Call("DownedDevi") == true)
            {
                canSayDefeatQuote = false;
                return DeviChat("Defeat");
            }

            if (Main.rand.NextBool())
            {
                if (Main.LocalPlayer.stinky)
                    return DeviChat("Stinky");

                if (Main.LocalPlayer.loveStruck)
                    return DeviChat("LoveStruck", Main.rand.Next(2, 8));

                if (Main.bloodMoon)
                    return DeviChat("BloodMoon");
            }

            List<string> dialogue = Language.FindAll(Lang.CreateDialogFilter("Mods.Fargowiltas.NPCs.Deviantt.Chat.Normal")).Select(item => item.Value).ToList();
            dialogue.Add(DeviChat("Formattable1", Main.LocalPlayer.name));

            if (Main.hardMode)
            {
                dialogue.Add(DeviChat("HM"));
            }

            int mutant = NPC.FindFirstNPC(NPCType<Mutant>());
            if (mutant != -1)
            {
                dialogue.Add(DeviChat("Mutant1", Main.npc[mutant].GivenName));
                dialogue.Add(DeviChat("Mutant2", Main.npc[mutant].GivenName));
            }

            int lumberjack = NPC.FindFirstNPC(NPCType<LumberJack>());
            if (lumberjack != -1)
            {
                dialogue.Add(DeviChat("Lumber", Main.npc[lumberjack].GivenName));
            }

            if (FargoWorld.EternityMode)
            {
                dialogue.Add(DeviChat("EternityMode"));
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            if (FargoWorld.EternityMode)
            {
                button2 = Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.HelpButton"); //(bool)ModLoader.GetMod("FargowiltasSouls").Call("GiftsReceived") ? "Help" : "Receive Gift";
            }
        }

        public const string ShopName = "Shop";

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = ShopName;
            }
            else if (FargoWorld.EternityMode)
            {
                Main.npcChatText = Fargowiltas.dialogueTracker.GetDialogue(NPC.GivenName);
            }
        }

        public override void AddShops()
        {
            AddVanillaShop();
        }

        public void AddVanillaShop()
        {
            var npcShop = new NPCShop(Type, ShopName);

            if (Fargowiltas.SoulsExtrasMod?.TryFind("PandorasBox", out ModItem pandorasBox) == true)
            {
                npcShop.Add(new Item(pandorasBox.Type));
            }

            npcShop
                .Add(new Item(ItemType<WormSnack>()) { shopCustomPrice = Item.buyPrice(copper: 20000) }, new Condition("Mods.Fargowiltas.Conditions.WormDown", () => FargoWorld.DownedBools["worm"]))
                .Add(new Item(ItemType<PinkSlimeCrown>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.PinkyDown", () => FargoWorld.DownedBools["pinky"]))
                .Add(new Item(ItemType<GoblinScrap>()) { shopCustomPrice = Item.buyPrice(copper: 10000) }, new Condition("Mods.Fargowiltas.Conditions.ScoutDown", () => FargoWorld.DownedBools["goblinScout"]))
                .Add(new Item(ItemType<Eggplant>()) { shopCustomPrice = Item.buyPrice(copper: 20000) }, new Condition("Mods.Fargowiltas.Conditions.DoctorDown", () => FargoWorld.DownedBools["doctorBones"]))
                .Add(new Item(ItemType<AttractiveOre>()) { shopCustomPrice = Item.buyPrice(copper: 30000) }, new Condition("Mods.Fargowiltas.Conditions.MinerDown", () => FargoWorld.DownedBools["undeadMiner"]))
                .Add(new Item(ItemType<HolyGrail>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.TimDown", () => FargoWorld.DownedBools["tim"]))
                .Add(new Item(ItemType<GnomeHat>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.GnomeDown", () => FargoWorld.DownedBools["gnome"]))
                .Add(new Item(ItemType<SlimyLockBox>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.DungeonSlimeDown", () => NPC.downedBoss3 && FargoWorld.DownedBools["dungeonSlime"]))
                .Add(new Item(ItemType<AthenianIdol>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.MedusaDown", () => Main.hardMode && FargoWorld.DownedBools["medusa"]))
                .Add(new Item(ItemType<ClownLicense>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.ClownDown", () => Main.hardMode && FargoWorld.DownedBools["clown"]))
                .Add(new Item(ItemType<HeartChocolate>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.NymphDown", () => FargoWorld.DownedBools["nymph"]))
                .Add(new Item(ItemType<MothLamp>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.MothDown", () => Main.hardMode && FargoWorld.DownedBools["moth"]))
                .Add(new Item(ItemType<DilutedRainbowMatter>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.RainbowSlimeDown", () => Main.hardMode && FargoWorld.DownedBools["rainbowSlime"]))
                .Add(new Item(ItemType<CloudSnack>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.WyvernDown", () => Main.hardMode && FargoWorld.DownedBools["wyvern"]))
                .Add(new Item(ItemType<RuneOrb>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.RuneDown", () => Main.hardMode && FargoWorld.DownedBools["runeWizard"]))
                .Add(new Item(ItemType<SuspiciousLookingChest>()) { shopCustomPrice = Item.buyPrice(copper: 800000) }, new Condition("Mods.Fargowiltas.Conditions.MimicDown", () => Main.hardMode && FargoWorld.DownedBools["mimic"]))
                .Add(new Item(ItemType<HallowChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicHallowDown", () => Main.hardMode && FargoWorld.DownedBools["mimicHallow"]))
                .Add(new Item(ItemType<CorruptChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicCorruptDown", () => Main.hardMode && (FargoWorld.DownedBools["mimicCorrupt"] || FargoWorld.DownedBools["mimicCrimson"])))
                .Add(new Item(ItemType<CrimsonChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicCrimsonDown", () => Main.hardMode && (FargoWorld.DownedBools["mimicCorrupt"] || FargoWorld.DownedBools["mimicCrimson"])))
                .Add(new Item(ItemType<JungleChest>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.MimicJungleDown", () => Main.hardMode && FargoWorld.DownedBools["mimicJungle"]))
                .Add(new Item(ItemType<CoreoftheFrostCore>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.IceGolemDown", () => Main.hardMode && FargoWorld.DownedBools["iceGolem"]))
                .Add(new Item(ItemType<ForbiddenForbiddenFragment>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.SandDown", () => Main.hardMode && FargoWorld.DownedBools["sandElemental"]))
                .Add(new Item(ItemType<DemonicPlushie>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.DevilDown", () => NPC.downedMechBossAny && FargoWorld.DownedBools["redDevil"]))
                .Add(new Item(ItemType<SuspiciousLookingLure>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.BloodFishDown", () => FargoWorld.DownedBools["eyeFish"] || FargoWorld.DownedBools["zombieMerman"]))
                .Add(new Item(ItemType<HemoclawCrab>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.BloodFish2Down", () => Main.hardMode && (FargoWorld.DownedBools["bloodEel"] || FargoWorld.DownedBools["goblinShark"])))
                .Add(new Item(ItemType<ShadowflameIcon>()) { shopCustomPrice = Item.buyPrice(copper: 100000) }, new Condition("Mods.Fargowiltas.Conditions.SummonerDown", () => Main.hardMode && NPC.downedGoblins && FargoWorld.DownedBools["goblinSummoner"]))
                .Add(new Item(ItemType<PirateFlag>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.PirateDown", () => Main.hardMode && NPC.downedPirates && FargoWorld.DownedBools["pirateCaptain"]))
                .Add(new Item(ItemType<Pincushion>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.NailheadDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["nailhead"]))
                .Add(new Item(ItemType<MothronEgg>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.MothronDown", () => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && FargoWorld.DownedBools["mothron"]))
                .Add(new Item(ItemType<LeesHeadband>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.LeeDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["boneLee"]))
                .Add(new Item(ItemType<GrandCross>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.PaladinDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["paladin"]))
                .Add(new Item(ItemType<AmalgamatedSkull>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.SkeleGunDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["skeletonGun"]))
                .Add(new Item(ItemType<AmalgamatedSpirit>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, new Condition("Mods.Fargowiltas.Conditions.SkeleMagesDown", () => NPC.downedPlantBoss && FargoWorld.DownedBools["skeletonMage"]))
                .Add(new Item(ItemType<SiblingPylon>()), Condition.HappyEnoughToSellPylons, Condition.NpcIsPresent(NPCType<Mutant>()), Condition.NpcIsPresent(NPCType<Abominationn>()))
            ;

            npcShop.Register();
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {

        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            if (NPC.downedMoonlord)
            {
                damage = 80;
                knockback = 4f;
            }
            else if (Main.hardMode)
            {
                damage = 40;
                knockback = 4f;
            }
            else
            {
                damage = 20;
                knockback = 2f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = NPC.downedMoonlord ? 1 : 30;
            if (!NPC.downedMoonlord)
            {
                randExtraCooldown = 30;
            }
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = NPC.downedMoonlord ? ProjectileType<FakeHeartMarkDeviantt>() : ProjectileType<FakeHeartDeviantt>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 10f;
            randomOffset = 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.8f);
                }

                if (!Main.dedServ)
                {
                    Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "DevianttGore3").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "DevianttGore2").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "DevianttGore1").Type);
                }
            }
            else
            {
                for (int k = 0; k < hit.Damage / NPC.lifeMax * 50.0; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 0.6f);
                }
            }
        }

        public override void OnKill()
        {
            if (Fargowiltas.SoulsMod?.TryFind("CosmosChampion", out ModNPC cosmosChamp) == true && NPC.AnyNPCs(cosmosChamp.Type))
                Item.NewItem(NPC.GetSource_Loot(), NPC.Hitbox, ItemType<WalkingRick>());
        }



        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int offset = -5;
            Texture2D texture = (Texture2D)TownNPCProfile().GetTextureNPCShouldUse(NPC);
            Rectangle rectangle = NPC.frame;
            Vector2 origin2 = rectangle.Size() / 2f;
            SpriteEffects effects = NPC.IsShimmerVariant ? NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if ((bool?)Fargowiltas.SoulsMod?.Call("GiftsReceived") == false)
            {


                Color color26 = Main.DiscoColor;
                color26.A = 0;

                //for (int i = 0; i < NPCID.Sets.TrailCacheLength[NPC.type]; i++)
                //{
                //    Color color27 = color26 * 0.5f;
                //    color27 *= (float)(NPCID.Sets.TrailCacheLength[NPC.type] - i) / NPCID.Sets.TrailCacheLength[NPC.type];
                //    Vector2 value4 = NPC.oldPos[i];
                //    float num165 = NPC.rotation; //NPC.oldRot[i];
                //    Main.EntitySpriteDraw(texture2D13, value4 + NPC.Size / 2f - Main.screenPosition + new Vector2(0, NPC.gfxOffY - 4), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, NPC.scale, effects, 0);
                //}

                float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.5f + 1f;
                scale *= NPC.scale;
                Main.EntitySpriteDraw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY - 4), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, NPC.rotation, origin2, scale, effects, 0);
            }

            if (NPC.IsShimmerVariant)
                offset = -3;

            if (!NPC.IsABestiaryIconDummy)
            {
                Main.EntitySpriteDraw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, offset), new Microsoft.Xna.Framework.Rectangle?(rectangle), NPC.GetAlpha(drawColor), NPC.rotation, origin2, NPC.scale, effects, 0);
                return false;
            }
            else
                return true;





        }

        private static string DeviChat(string key, params object[] args) => Language.GetTextValue($"Mods.Fargowiltas.NPCs.Deviantt.Chat.{key}", args);
    }
    public class DevianttProfile : ITownNPCProfile
    {
        public int RollVariation() => 0;
        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            if (npc.IsABestiaryIconDummy)
                return Request<Texture2D>("Fargowiltas/Content/NPCs/Deviantt");

            if (npc.IsShimmerVariant)
            {
                return Request<Texture2D>("Fargowiltas/Content/NPCs/Deviantt_Shimmer");
            }

            if (npc.direction == -1)
                return ModContent.Request<Texture2D>("Fargowiltas/Content/NPCs/Deviantt");
            else
                return ModContent.Request<Texture2D>("Fargowiltas/Content/NPCs/DevianttRight");
        }

        public int GetHeadTextureIndex(NPC npc) => GetModHeadSlot("Fargowiltas/Content/NPCs/Deviantt_Head");
    }
}
