using Fargowiltas.Common;
using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Biomes;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Summons.Mutant;
using Fargowiltas.Content.Items.Summons.SwarmSummons;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.NPCs.AbominationnNPC;
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
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.NPCs
{
    [AutoloadHead]
    public class Mutant : ModNPC
    {
        private static int shopNum = 1;

        internal bool spawned;
        private bool canSayDefeatQuote = true;
        private int defeatQuoteTimer = 900;


        public override ITownNPCProfile TownNPCProfile()
        {
            return new MutantProfile();
        }

        //public override bool Autoload(ref string name)
        //{
        //    name = "Mutant";
        //    return true;// Mod.Properties.Autoload;
        //}

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.FaceEmote[NPC.type] = ModContent.EmoteBubbleType<MutantEmote>();

            NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = -1f,
                Direction = -1,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<SkyBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike);

            NPC.Happiness.SetNPCAffection<Abominationn>(AffectionLevel.Love);
            NPC.Happiness.SetNPCAffection<Deviantt>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection<LumberJack>(AffectionLevel.Dislike);

            NPC.AddDebuffImmunities(new List<int>()
            {
                 BuffID.Suffocation
            });
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.NPCs.Mutant.Bestiary")
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
            NPC.lifeMax = NPC.downedMoonlord ? 5000 : Main.hardMode ? 1000 : 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;

            if (FargoServerConfig.Instance.CatchNPCs)
            {
                Main.npcCatchable[NPC.type] = true;
                //    NPC.catchItem = (short)Mod.ItemType("Mutant");
            }


            if ((bool?)Fargowiltas.SoulsMod?.Call("DownedMutant") == true)
            {
                NPC.lifeMax = 77000;
                NPC.defense = 360;
            }
        }

        public override bool CanGoToStatue(bool toKingStatue) => true;

        public override void AI()
        {
            NPC.breath = 200;
            if (defeatQuoteTimer > 0)
                defeatQuoteTimer--;
            else
                canSayDefeatQuote = false;

            if (!spawned)
            {
                spawned = true;
                if ((bool?)Fargowiltas.SoulsMod?.Call("DownedMutant") == true)
                {
                    NPC.lifeMax = 77000;
                    NPC.life = NPC.lifeMax;
                    NPC.defense = 360;
                }
            }
            AnimationType = NPC.IsShimmerVariant ? -1 : NPCID.Guide;
            NPCID.Sets.CannotSitOnFurniture[NPC.type] = NPC.ShimmeredTownNPCs[NPC.type];
        }
        public override bool UsesPartyHat() => !NPC.IsShimmerVariant;

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            if ((bool?)Fargowiltas.SoulsMod?.Call("MutantAlive") == true)
            {
                return false;
            }

            return FargoServerConfig.Instance.Mutant && FargoWorld.DownedBools["boss"] && FargoUtils.AnyBossAlive();
        }

        public override List<string> SetNPCNameList()
        {
            string[] names =
               [Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName1"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName2"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName3"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName4"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName5"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName6"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName7"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName8"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName9"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName10"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName11"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName12"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.NPCName13")];

            return new List<string>(names);
        }

        public override string GetChat()
        {
            Mod souls = Fargowiltas.SoulsMod;
            if (NPC.homeless && canSayDefeatQuote && souls != null && (bool)souls.Call("DownedMutant"))
            {
                canSayDefeatQuote = false;

                if (FargoWorld.EternityMode)
                    return MutantChat("EternityDefeat");
                else
                    return MutantChat("Defeat");
            }

            if (souls != null && Main.rand.NextBool(4))
            {
                if ((bool)souls.Call("MutantArmor"))
                    return MutantChat("MutantArmor");
            }

            List<string> dialogue = Language.FindAll(Lang.CreateDialogFilter("Mods.Fargowiltas.NPCs.Mutant.Chat.Normal")).Select(item => item.Value).ToList();

            if (souls != null)
            {
                dialogue.AddWithCondition(MutantChat("SoulsPostML"), NPC.downedMoonlord);

                if ((bool)souls.Call("DownedMutant"))
                {
                    dialogue.Add(MutantChat("DefeatCommon"));
                }
                else if ((bool)souls.Call("DownedFishronEX") || (bool)souls.Call("DownedAbom"))
                {
                    dialogue.Add(MutantChat("DefeatAbom"));
                }
            }
            else
            {
                dialogue.Add(MutantChat("SoulsModDisabled"));
            }

            //dialogue.AddWithCondition("Why would you do this.", Fargowiltas.ModLoaded["CalamityMod"]);
            //dialogue.AddWithCondition("I feel a great imbalance in this world.", Fargowiltas.ModLoaded["CalamityMod"] && Fargowiltas.ModLoaded["ThoriumMod"]);
            //dialogue.AddWithCondition("A great choice, shame about that first desert boss thing though.", Fargowiltas.ModLoaded["ThoriumMod"]);
            dialogue.AddWithCondition(MutantChat("PumpkinMoon"), Main.pumpkinMoon);
            dialogue.AddWithCondition(MutantChat("FrostMoon"), Main.snowMoon);
            dialogue.AddWithCondition(MutantChat("SlimeRain"), Main.slimeRain);
            dialogue.AddWithCondition(MutantChat("BloodMoon1"), Main.bloodMoon);
            dialogue.AddWithCondition(MutantChat("BloodMoon2"), Main.bloodMoon);
            dialogue.AddWithCondition(MutantChat("NightTime"), !Main.dayTime);

            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
            if (BirthdayParty.PartyIsUp)
            {
                if (partyGirl >= 0)
                {
                    dialogue.Add(MutantChat("Party", Main.npc[partyGirl].GivenName));
                }

                dialogue.Add(MutantChat("PartyWithoutPartyGirl"));
            }

            int nurse = NPC.FindFirstNPC(NPCID.Nurse);
            if (nurse >= 0)
            {
                dialogue.Add(MutantChat("Nurse", Main.npc[nurse].GivenName));
            }

            int witchDoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
            if (witchDoctor >= 0)
            {
                dialogue.Add(MutantChat("WitchDoctor", Main.npc[witchDoctor].GivenName));
            }

            int dryad = NPC.FindFirstNPC(NPCID.Dryad);
            if (dryad >= 0)
            {
                dialogue.Add(MutantChat("Dryad", Main.npc[dryad].GivenName));
            }

            int stylist = NPC.FindFirstNPC(NPCID.Stylist);
            if (stylist >= 0)
            {
                dialogue.Add(MutantChat("Stylist", Main.npc[stylist].GivenName));
            }

            int truffle = NPC.FindFirstNPC(NPCID.Truffle);
            if (truffle >= 0)
            {
                dialogue.Add(MutantChat("Truffle"));
            }

            int tax = NPC.FindFirstNPC(NPCID.TaxCollector);
            if (tax >= 0)
            {
                dialogue.Add(MutantChat("TaxCollector", Main.npc[tax].GivenName));
            }

            int guide = NPC.FindFirstNPC(NPCID.Guide);
            if (guide >= 0)
            {
                dialogue.Add(MutantChat("Guide", Main.npc[guide].GivenName));
            }

            int cyborg = NPC.FindFirstNPC(NPCID.Cyborg);
            if (truffle >= 0 && witchDoctor >= 0 && cyborg >= 0 && Main.rand.NextBool(52))
            {
                dialogue.Add(MutantChat("WitchDoctorTruffleCyborg", Main.npc[witchDoctor].GivenName, Main.npc[truffle].GivenName, Main.npc[cyborg].GivenName));
            }

            if (partyGirl >= 0)
            {
                dialogue.Add(MutantChat("PartyGirl", Main.npc[partyGirl].GivenName));
            }

            int demoman = NPC.FindFirstNPC(NPCID.Demolitionist);
            if (demoman >= 0)
            {
                dialogue.Add(MutantChat("Demolitionist", Main.npc[demoman].GivenName));
            }

            int tavernkeep = NPC.FindFirstNPC(NPCID.DD2Bartender);
            if (tavernkeep >= 0)
            {
                dialogue.Add(MutantChat("Tavernkeep", Main.npc[tavernkeep].GivenName));
            }

            int dyeTrader = NPC.FindFirstNPC(NPCID.DyeTrader);
            if (dyeTrader >= 0)
            {
                dialogue.Add(MutantChat("DyeTrader", Main.npc[dyeTrader].GivenName));
            }

            return Main.rand.Next(dialogue);
        }

        private bool AnyHardmodeSummon => Main.hardMode || Fargowiltas.summonTracker.SortedSummons.Any(s => s.progression >= MutantSummonTracker.WallOfFlesh && s.downed.Invoke());
        private bool AnyPostMLSummon => NPC.downedMoonlord || Fargowiltas.summonTracker.SortedSummons.Any(s => s.progression >= MutantSummonTracker.Moonlord && s.downed.Invoke());
        private static string GetLocalization(string line) => Language.GetTextValue($"Mods.Fargowiltas.NPCs.Mutant.{line}");
        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (AnyHardmodeSummon)
            {
                button2 = GetLocalization("CycleShop");
            }
            else
            {
                shopNum = 1;
            }
            switch (shopNum)
            {
                case 1:
                    button = GetLocalization("PreHM");
                    break;

                case 2:
                    button = GetLocalization("HM");
                    break;

                default:
                    button = GetLocalization("PostML");
                    break;
            }



            if (AnyPostMLSummon)
            {
                if (shopNum >= 4)
                {
                    shopNum = 1;
                }
            }
            else
            {
                if (shopNum >= 3)
                {
                    shopNum = 1;
                }
            }
        }

        public const string ShopName1 = "Pre Hardmode Shop";
        public const string ShopName2 = "Hardmode Shop";
        public const string ShopName3 = "Post Moon Lord Shop";

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                switch (shopNum)
                {
                    case 1:
                        shopName = ShopName1;
                        break;
                    case 2:
                        shopName = ShopName2;
                        break;
                    default:
                        shopName = ShopName3;
                        break;
                }
            }
            else if (!firstButton && AnyHardmodeSummon)
            {
                shopNum++;
            }
        }

        public override void AddShops()
        {
            var npcShop1 = new NPCShop(Type, ShopName1)
                .Add(new Item(ItemType<Overloader>()) { shopCustomPrice = Item.buyPrice(copper: 400000) }, Condition.InExpertMode)
                .Add(new Item(ItemType<ModeToggle>()));

            if (Fargowiltas.SoulsMod?.TryFind("Masochist", out ModItem masochist) == true)
            {
                npcShop1.Add(new Item(masochist.Type) { shopCustomPrice = Item.buyPrice(copper: 10000) }); //mutants gift
            }

            foreach (MutantSummonInfo summon in Fargowiltas.summonTracker.SortedSummons)
            {
                //phm
                if (summon.progression <= MutantSummonTracker.WallOfFlesh)
                {
                    npcShop1.Add(new Item(summon.itemId) { shopCustomPrice = Item.buyPrice(copper: summon.price) }, new Condition("Mods.Fargowiltas.Conditions.DownedTheBoss", summon.downed));
                }
            }

            var npcShop2 = new NPCShop(Type, ShopName2)
                 .Add(new Item(ItemType<Overloader>()) { shopCustomPrice = Item.buyPrice(copper: 400000) }, Condition.InExpertMode);

            foreach (MutantSummonInfo summon in Fargowiltas.summonTracker.SortedSummons)
            {
                //hm
                if (summon.progression > MutantSummonTracker.WallOfFlesh && summon.progression <= MutantSummonTracker.Moonlord)
                {
                    npcShop2.Add(new Item(summon.itemId) { shopCustomPrice = Item.buyPrice(copper: summon.price) }, new Condition("Mods.Fargowiltas.Conditions.DownedTheBoss", summon.downed));
                }
            }

            var npcShop3 = new NPCShop(Type, ShopName3)
                 .Add(new Item(ItemType<Overloader>()) { shopCustomPrice = Item.buyPrice(copper: 400000) }, Condition.InExpertMode);

            foreach (MutantSummonInfo summon in Fargowiltas.summonTracker.SortedSummons)
            {
                //post ml
                if (summon.progression > MutantSummonTracker.Moonlord)
                {
                    npcShop3.Add(new Item(summon.itemId) { shopCustomPrice = Item.buyPrice(copper: summon.price) }, new Condition("Mods.Fargowiltas.Conditions.DownedTheBoss", summon.downed));
                }
            }

            npcShop3.Add(new Item(ItemType<AncientSeal>()) { shopCustomPrice = Item.buyPrice(copper: 100000000) });

            npcShop1.Add(new Item(ItemType<SiblingPylon>()), Condition.HappyEnoughToSellPylons, Condition.NpcIsPresent(NPCType<Abominationn>()), Condition.NpcIsPresent(NPCType<Deviantt>()));
            npcShop2.Add(new Item(ItemType<SiblingPylon>()), Condition.HappyEnoughToSellPylons, Condition.NpcIsPresent(NPCType<Abominationn>()), Condition.NpcIsPresent(NPCType<Deviantt>()));
            npcShop3.Add(new Item(ItemType<SiblingPylon>()), Condition.HappyEnoughToSellPylons, Condition.NpcIsPresent(NPCType<Abominationn>()), Condition.NpcIsPresent(NPCType<Deviantt>()));

            npcShop1.Register();
            npcShop2.Register();
            npcShop3.Register();
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
            if (!ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls))
            {
                return;
            }

            if (shopName.Replace($"{Mod.Name}/{DisplayName}/", "") is ShopName1 or ShopName2 or ShopName3)
            {
                foreach (var item in items)
                {
                    if (item is null || item.IsAir)
                    {
                        continue;
                    }

                    float modifier = 1f;
                    if ((bool)fargoSouls.Call("MutantDiscountCard"))
                    {
                        modifier -= 0.2f;
                    }

                    if ((bool)fargoSouls.Call("MutantPact"))
                    {
                        modifier -= 0.3f;
                    }

                    item.shopCustomPrice = (int)((item.shopCustomPrice ?? item.GetStoreValue()) * modifier);
                }
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            if ((bool?)Fargowiltas.SoulsMod?.Call("DownedMutant") == true)
            {
                damage = 700;
                knockback = 7f;
            }
            else if (NPC.downedMoonlord)
            {
                damage = 250;
                knockback = 6f;
            }
            else if (Main.hardMode)
            {
                damage = 60;
                knockback = 5f;
            }
            else
            {
                damage = 20;
                knockback = 4f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            if (NPC.downedMoonlord)
            {
                cooldown = 1;
            }
            else if (Main.hardMode)
            {
                cooldown = 20;
                randExtraCooldown = 25;
            }
            else
            {
                cooldown = 30;
                randExtraCooldown = 30;
            }
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            Mod souls = Fargowiltas.SoulsMod;
            if (souls != null && (bool)souls.Call("DownedMutant") && souls.TryFind("MutantSpearThrownFriendly", out ModProjectile penetrator))
            {
                projType = penetrator.Type;
            }
            else if (NPC.downedMoonlord)
            {
                projType = ProjectileType<PhantasmalEyeProjectile>();
            }
            else if (Main.hardMode)
            {
                projType = ProjectileType<MechEyeProjectile>();
            }
            else
            {
                projType = ProjectileType<EyeProjectile>();
            }

            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            if ((bool?)Fargowiltas.SoulsMod?.Call("DownedMutant") == true)
            {
                multiplier = 25f;
                randomOffset = 0f;
            }
            else
            {
                multiplier = 12f;
                randomOffset = 2f;
            }
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
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "MutantGore3").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "MutantGore2").Type);

                    pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                    Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", "MutantGore1").Type);
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

        private const int SquirrelFrameCount = 6;
        private int SquirrelFrame = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)TownNPCProfile().GetTextureNPCShouldUse(NPC);
            Rectangle rectangle = NPC.frame;
            if (NPC.IsShimmerVariant)
            {
                NPC.spriteDirection = NPC.direction;
                int height = 56; //we unfortunately have to explicitly set this value due to Restrictions
                if (NPC.velocity.X == 0)
                {
                    SquirrelFrame = 0;
                }
                else
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 5)
                    {
                        NPC.frameCounter = 0;
                        SquirrelFrame++;
                        if (SquirrelFrame >= SquirrelFrameCount)
                        {
                            SquirrelFrame = 1;
                        }
                    }
                }
                rectangle.X = 0;
                rectangle.Y = height * SquirrelFrame;
                rectangle.Width = texture.Width;
                rectangle.Height = height;
            }
            SpriteEffects effects = SpriteEffects.None;
            Vector2 origin2 = NPC.frame.Size() / 2f;
            if (NPC.IsShimmerVariant)
                effects = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY - 4), new Microsoft.Xna.Framework.Rectangle?(rectangle), NPC.GetAlpha(drawColor), NPC.rotation, origin2, NPC.scale, effects, 0f);
            return false;
        }

        private static string MutantChat(string key, params object[] args) => Language.GetTextValue($"Mods.Fargowiltas.NPCs.Mutant.Chat.{key}", args);
    }

    public class MutantProfile : ITownNPCProfile
    {
        public int RollVariation() => 0;
        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            if (npc.IsABestiaryIconDummy)
                return Request<Texture2D>("Fargowiltas/Content/NPCs/Mutant");

            if (npc.IsShimmerVariant)
            {
                if (npc.altTexture == 1)
                {
                    return Request<Texture2D>("Fargowiltas/Content/NPCs/Mutant_Shimmer_Party");
                }
                else
                {
                    return Request<Texture2D>("Fargowiltas/Content/NPCs/Mutant_Shimmer");
                }
            }
            if (npc.direction == -1)
                return ModContent.Request<Texture2D>("Fargowiltas/Content/NPCs/Mutant");
            else
                return ModContent.Request<Texture2D>("Fargowiltas/Content/NPCs/MutantRight");
        }

        public int GetHeadTextureIndex(NPC npc) => GetModHeadSlot("Fargowiltas/Content/NPCs/Mutant_Head");
    }
}
