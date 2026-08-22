using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Biomes;
using Fargowiltas.Content.Items.Summons.Abom;
using Fargowiltas.Content.Items.Summons.Deviantt;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.Items.Vanity;
using Fargowiltas.Content.UI.Emotes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Fargowiltas.Fargowiltas;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.NPCs.AbominationnNPC
{
    [AutoloadHead]
    public class Abominationn : ModNPC
    {
        private bool canSayDefeatQuote = true;
        private bool canSayMutantShimmerQuote = false;
        private int defeatQuoteTimer = 900;

        private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile AbomProfile;

        public override void Load()
        {
            ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
        }

        public override ITownNPCProfile TownNPCProfile() => AbomProfile;

        public static Asset<Texture2D> Cape;
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> StyxGazer;
        public static Asset<Texture2D> Arm, ArmGlow;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 2;
            NPCID.Sets.FaceEmote[NPC.type] = ModContent.EmoteBubbleType<AbominationnEmote>();

            NPCID.Sets.ShimmerTownTransform[Type] = true;
            NPCID.Sets.ShimmerTownTransform[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<SkyBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<DungeonBiome>(AffectionLevel.Dislike);

            NPC.Happiness.SetNPCAffection<Mutant>(AffectionLevel.Love);
            NPC.Happiness.SetNPCAffection<Deviantt>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.Nurse, AffectionLevel.Hate);

            NPC.AddDebuffImmunities(new List<int>()
            {
                 BuffID.Suffocation
            });

            AbomProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), null),
                new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, null)
            );

            if (!Main.dedServ)
            {
                Cape = Request<Texture2D>("Fargowiltas/Content/NPCs/AbominationnNPC/AbominationnCape");
                Glow = Request<Texture2D>("Fargowiltas/Content/NPCs/AbominationnNPC/Abominationn_glow");
                StyxGazer = Request<Texture2D>("Fargowiltas/Content/NPCs/AbominationnNPC/AbominationnStyxGazer");
                Arm = Request<Texture2D>("Fargowiltas/Content/NPCs/AbominationnNPC/AbominationnArm");
                ArmGlow = Request<Texture2D>("Fargowiltas/Content/NPCs/AbominationnNPC/AbominationnArm_glow");
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.NPCs.Abominationn.Bestiary")
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 52;
            NPC.height = 66;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = NPC.downedMoonlord ? 50 : 15;
            NPC.lifeMax = NPC.downedMoonlord ? 5000 : Main.hardMode ? 1000 : 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            Mod souls = Fargowiltas.SoulsMod;
            if (souls != null && ((bool)souls.Call("MutantAlive") || (bool)souls.Call("AbomAlive")))
            {
                return false;
            }
            return FargoServerConfig.Instance.Abom && NPC.downedGoblins && !FargoUtils.AnyBossAlive();
        }

        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void AI()
        {
            NPC.breath = 200;
            if (defeatQuoteTimer > 0)
                defeatQuoteTimer--;
            else
                canSayDefeatQuote = false;
            int mutant = NPC.FindFirstNPC(NPCType<Mutant>());
            if (mutant != -1)
            {
                if (!Main.npc[mutant].IsShimmerVariant)
                {
                    canSayMutantShimmerQuote = true;
                }
            }
        }

        public override void ModifyTypeName(ref string typeName)
        {
            typeName = Language.GetTextValue("Mods.Fargowiltas.NPCs.Abominationn.DisplayName");
        }

        public override void ChatBubblePosition(ref Vector2 position, ref SpriteEffects spriteEffects)
        {
            position.Y += 28;
            if (spriteEffects == SpriteEffects.None)
                position.X -= 12;
            else
                position.X += 12;
        }

        public override void EmoteBubblePosition(ref Vector2 position, ref SpriteEffects spriteEffects)
        {
            position.Y += 24;
            if (spriteEffects == SpriteEffects.None)
                position.X -= 8;
            else
                position.X += 8;
        }

        public override string GetChat()
        {
            Mod souls = Fargowiltas.SoulsMod;
            if (NPC.homeless && canSayDefeatQuote && (bool?)souls?.Call("DownedAbom") == true)
            {
                canSayDefeatQuote = false;
                return AbomChat("Defeat");
            }

            int mutant = NPC.FindFirstNPC(NPCType<Mutant>());
            if (mutant != -1)
            {
                if (Main.npc[mutant].IsShimmerVariant)
                {
                    if (canSayMutantShimmerQuote)
                    {
                        canSayMutantShimmerQuote = false;
                        return AbomChat("MutantShimmer");
                    }

                }
            }

            if (souls != null && Main.rand.NextBool(3) && (bool)souls.Call("StyxArmor"))
            {
                return AbomChat("StyxArmor");
            }

            List<string> dialogue = Language.FindAll(Lang.CreateDialogFilter("Mods.Fargowiltas.NPCs.Abominationn.Chat.Normal")).Select(item => item.Value).ToList();
            dialogue.Add(AbomChat("Formattable1", !Main.hardMode ? AbomChat("Formatter1PHM") : AbomChat("Formatter1HM")));

            if (Main.LocalPlayer.ZoneGraveyard)
            {
                dialogue.Add(AbomChat("Graveyard"));
            }

            int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
            if (mechanic != -1)
            {
                dialogue.Add(AbomChat("Mechanic", Main.npc[mechanic].GivenName));
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = Language.GetTextValue("Mods.Fargowiltas.NPCs.Abominationn.CancelEvent");
        }

        public const string ShopName = "Shop";

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = ShopName;
            }
            else
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    var netMessage = Mod.GetPacket();
                    netMessage.Write((byte)PacketID.ClientUpdateWorld);
                    netMessage.Send();
                }

                if (!NPC.downedTowers && NPC.LunarApocalypseIsUp)
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        var netMessage = Mod.GetPacket();
                        netMessage.Write((byte)PacketID.AbomClearEvent);
                        netMessage.Send();
                    }

                    if (Fargowiltas.IsEventOccurring)
                    {
                        Main.npcChatText = Fargowiltas.TryClearEvents() ? AbomChat("PillarFail2") : AbomChat("PillarFailCD", FargoWorld.AbomClearCD / 60);
                    }
                    else
                    {
                        Main.npcChatText = AbomChat("PillarFail");
                    }
                }

                else if (Fargowiltas.IsEventOccurring)
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        var netMessage = Mod.GetPacket();
                        netMessage.Write((byte)PacketID.AbomClearEvent);
                        netMessage.Send();
                    }

                    Main.npcChatText = Fargowiltas.TryClearEvents() ? AbomChat("Canceled") : AbomChat("CancelCD", FargoWorld.AbomClearCD / 60);
                }

                else
                {
                    Main.npcChatText = AbomChat("NoEvent");
                }
            }
        }

        public override void AddShops()
        {
            Condition siblingPylonCondition = new Condition("Mods.Fargowiltas.Conditions.SiblingPylon", () => Condition.NpcIsPresent(NPCType<Deviantt>()).Predicate.Invoke() && Condition.NpcIsPresent(NPCType<Mutant>()).Predicate.Invoke());
            var npcShop = new NPCShop(Type, ShopName)
                .Add(new Item(ItemType<PartyInvite>()) { shopCustomPrice = Item.buyPrice(copper: 10000) })
                .Add(new Item(ItemType<WeatherBalloon>()) { shopCustomPrice = Item.buyPrice(copper: 20000) })
                .Add(new Item(ItemType<Anemometer>()) { shopCustomPrice = Item.buyPrice(copper: 30000) })
                .Add(new Item(ItemType<ForbiddenScarab>()) { shopCustomPrice = Item.buyPrice(copper: 30000) })
                .Add(new Item(ItemType<SlimyBarometer>()) { shopCustomPrice = Item.buyPrice(copper: 40000) })
                .Add(new Item(ItemID.BloodMoonStarter) { shopCustomPrice = Item.buyPrice(copper: 50000) })
                .Add(new Item(ItemType<BloodSushiPlatter>()) { shopCustomPrice = Item.buyPrice(copper: 200000) }, new Condition("Mods.Fargowiltas.Conditions.BloodNautDown", () => Main.hardMode && FargoWorld.DownedBools["dreadnautilus"]))
                .Add(new Item(ItemID.GoblinBattleStandard) { shopCustomPrice = Item.buyPrice(copper: 60000) })
                .Add(new Item(ItemID.SnowGlobe) { shopCustomPrice = Item.buyPrice(copper: 150000) }, Condition.Hardmode)
                .Add(new Item(ItemID.PirateMap) { shopCustomPrice = Item.buyPrice(copper: 200000) }, Condition.DownedPirates)
                .Add(new Item(ItemType<PlunderedBooty>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.DutchmanDown", () => NPC.downedPirates && FargoWorld.DownedBools["flyingDutchman"]))
                .Add(new Item(ItemID.SolarTablet) { shopCustomPrice = Item.buyPrice(copper: 200000) }, Condition.DownedMechBossAny)
                .Add(new Item(ItemType<ForbiddenTome>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.MageDown", () => FargoWorld.DownedBools["darkMage"] || NPC.downedMechBossAny))
                .Add(new Item(ItemType<BatteredClub>()) { shopCustomPrice = Item.buyPrice(copper: 150000) }, new Condition("Mods.Fargowiltas.Conditions.OgreDown", () => FargoWorld.DownedBools["ogre"] || NPC.downedGolemBoss))
                .Add(new Item(ItemType<BetsyEgg>()) { shopCustomPrice = Item.buyPrice(copper: 400000) }, new Condition("Mods.Fargowiltas.Conditions.BetsyDown", () => FargoWorld.DownedBools["betsy"]))
                .Add(new Item(ItemID.PumpkinMoonMedallion) { shopCustomPrice = Item.buyPrice(copper: 500000) }, Condition.DownedPumpking)
                .Add(new Item(ItemType<HeadofMan>()) { shopCustomPrice = Item.buyPrice(copper: 200000) }, new Condition("Mods.Fargowiltas.Conditions.HorsemanDown", () => FargoWorld.DownedBools["headlessHorseman"]))
                .Add(new Item(ItemType<SpookyBranch>()) { shopCustomPrice = Item.buyPrice(copper: 200000) }, Condition.DownedMourningWood)
                .Add(new Item(ItemType<SuspiciousLookingScythe>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, Condition.DownedPumpking)
                .Add(new Item(ItemID.NaughtyPresent) { shopCustomPrice = Item.buyPrice(copper: 500000) }, Condition.DownedIceQueen)
                .Add(new Item(ItemType<FestiveOrnament>()) { shopCustomPrice = Item.buyPrice(copper: 200000) }, Condition.DownedEverscream)
                .Add(new Item(ItemType<NaughtyList>()) { shopCustomPrice = Item.buyPrice(copper: 200000) }, Condition.DownedSantaNK1)
                .Add(new Item(ItemType<IceKingsRemains>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, Condition.DownedIceQueen)
                .Add(new Item(ItemType<RunawayProbe>()) { shopCustomPrice = Item.buyPrice(copper: 500000) }, Condition.DownedGolem)
                .Add(new Item(ItemType<MartianMemoryStick>()) { shopCustomPrice = Item.buyPrice(copper: 300000) }, Condition.DownedMartians)
                .Add(new Item(ItemType<PillarSummon>()) { shopCustomPrice = Item.buyPrice(copper: 750000) }, new Condition("Mods.Fargowiltas.Conditions.PillarsDown", () => NPC.downedTowers))
                .Add(new Item(ItemType<AbominationnScythe>()) { shopCustomPrice = Item.buyPrice(copper: 50000) }, new Condition("Mods.Fargowiltas.Conditions.PillarsDown", () => NPC.downedTowers))
                .Add(new Item(ItemType<SiblingPylon>()), Condition.HappyEnoughToSellPylons, siblingPylonCondition)

            ;

            npcShop.Register();
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = NPC.downedMoonlord ? 150 : 20;
            knockback = NPC.downedMoonlord ? 10f : 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileType<AbominationnRocket>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<CrabSizedGlasses>(), 10));
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 28; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Pumpkin, 2.5f * hit.HitDirection, -2.5f, Scale: 0.8f);
                }

                if (!Main.dedServ)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height / 2));
                        Gore.NewGore(NPC.GetSource_Death(), pos, NPC.velocity, Find<ModGore>("Fargowiltas", $"AbomGore{i}").Type);
                    }
                }
            }
            else
            {
                for (int k = 0; k < 8; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Pumpkin, hit.HitDirection, -1f, Scale: 0.6f);
                }
            }
        }

        public override bool UsesPartyHat()
        {
            return !NPC.IsShimmerVariant;
        }

        public void HandleCapeAnimation(int frameType, bool windy = false)
        {
            if (CapeFrameX != (windy ? frameType + 1 : frameType))
            {
                CapeFrameX = (windy ? frameType + 1 : frameType);
                CapeFrameY = 0;
            }
        }

        enum CapeAnimationID
        {
            Idle,
            WindyIdle,
            Walking,
            Falling,
            Attacking
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            Tile? tile = NPC.IsABestiaryIconDummy ? null : Main.tile[(int)NPC.Center.X / 16, (int)NPC.Center.Y / 16];
            bool shouldFlourishCape = tile.HasValue && Main.WindyEnoughForKiteDrops
                && !(tile.Value.WallType > WallID.None && !WallID.Sets.AllowsWind[tile.Value.WallType])
                && (((int)NPC.Center.Y / 16)! < Main.worldSurface);

            #region cape
            switch (CapeFrameX)
            {
                case (int)CapeAnimationID.Idle:
                    {
                        CapeFrameX = CapeFrameY = 0;
                        CapeFrameCounter = 0;
                    }
                    break;

                case (int)CapeAnimationID.WindyIdle:
                    {
                        if (++CapeFrameCounter >= 6)
                        {
                            CapeFrameCounter = 0;
                            if (++CapeFrameY >= 4)
                                CapeFrameY = 0;
                        }
                    }
                    break;

                case (int)CapeAnimationID.Walking:
                    {
                        if (++CapeFrameCounter >= 6)
                        {
                            CapeFrameCounter = 0;
                            if (++CapeFrameY >= 4)
                                CapeFrameY = 0;
                        }
                    }
                    break;

                case (int)CapeAnimationID.Falling:
                    {
                        if (++CapeFrameCounter >= 6)
                        {
                            CapeFrameCounter = 0;
                            if (++CapeFrameY >= 4)
                                CapeFrameY = 0;
                        }
                    }
                    break;

                case (int)CapeAnimationID.Attacking:
                    {
                        if (++CapeFrameCounter >= 6)
                        {
                            CapeFrameCounter = 0;
                            if (++CapeFrameY >= 6)
                                CapeFrameY = 0;
                        }
                    }
                    break;

            }

            //Main.NewText(NPC.ai[0]);



            if (NPC.velocity.Y != 0) // falling
                HandleCapeAnimation((int)CapeAnimationID.Falling);

            /*
            else if (NPC.ai[0] == 10f || NPC.ai[0] == 13f)
            {
                if (++ArmFrameCounter >= 6)
                {
                    ArmFrameCounter = 0;
                    if (++ArmFrame >= 3)
                        ArmFrame = 0;
                }
                HandleCapeAnimation((int)CapeAnimationID.Attacking);
            }
            */
            else if (NPC.velocity.X != 0)
                HandleCapeAnimation((int)CapeAnimationID.Walking);
            else if (NPC.velocity.Length() == 0)
                HandleCapeAnimation((int)CapeAnimationID.Idle, shouldFlourishCape);
            #endregion

            /*
            if (++StyxFrameCounter >= 6)
            {
                StyxFrameCounter = 0;
                if (++StyxFrame >= 7)
                    StyxFrame = 2;
            }
            */

        }

        public int CapeFrameCounter, CapeFrameX, CapeFrameY;
        public int StyxFrameCounter, StyxFrame;
        public int ArmFrameCounter, ArmFrame;
        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)TownNPCProfile().GetTextureNPCShouldUse(NPC);
            Rectangle rectangle = NPC.frame;
            Vector2 origin = rectangle.Size() / 2f;
            SpriteEffects effects = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 position = NPC.Center - Main.screenPosition + new Vector2(6 * NPC.direction, 1 + NPC.gfxOffY);

            if (NPC.IsABestiaryIconDummy)
                position = NPC.Center + new Vector2(6 * NPC.direction, 1 + NPC.gfxOffY);

            Texture2D capeTexture = Cape.Value;
            Rectangle capeRect = new(54 * CapeFrameX, 72 * CapeFrameY, 54, 72);
            Vector2 capeOrigin = capeRect.Size() / 2f;

            Vector2 capePosition = position - new Vector2(31 * NPC.direction, 0);
            float CapeRotation = 0;

            sb.Draw(capeTexture, capePosition, capeRect, drawColor, CapeRotation, capeOrigin, NPC.scale, effects, 0);

            sb.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(rectangle), drawColor, NPC.rotation, origin, NPC.scale, effects, 0);
            sb.Draw(Glow.Value, position, new Microsoft.Xna.Framework.Rectangle?(rectangle), NPC.GetAlpha(Color.White), NPC.rotation, origin, NPC.scale, effects, 0);

            // if attacking
            Rectangle armRect = new(0, 72 * ArmFrame, 52, 72);
            Vector2 armOrigin = armRect.Size() / 2f;
            Vector2 armPosition = position;
            /* //todo: ?? figure out how town npc targetting works
            if (NPC.ai[0] == 10f || NPC.ai[0] == 13f)
            {
                float armRotation = 0;
                if (NPC.HasNPCTarget)
                {
                    NPC npc = Main.npc[NPC.TranslatedTargetIndex];
                    if (npc != null && npc.active)
                    {
                        armRotation = NPC.DirectionTo(npc.Center).ToRotation();
                    }
                }

                sb.Draw(Arm.Value, armPosition, armRect, NPC.GetAlpha(drawColor), armRotation, armOrigin, NPC.scale, effects, 0);
                sb.Draw(ArmGlow.Value, armPosition, armRect, NPC.GetAlpha(Color.White), armRotation, armOrigin, NPC.scale, effects, 0);
            }
            */

            Texture2D styxGazer = StyxGazer.Value;
            Rectangle styxRect = new(0, 72 * StyxFrame, 72, 72);
            Vector2 styxOrigin = styxRect.Size() / 2f;
            Vector2 styxPosition = position + new Vector2(4 * NPC.direction, 2);
            //if (StyxFrame != -1)
            //    sb.Draw(styxGazer, styxPosition, styxRect, NPC.GetAlpha(Color.White), NPC.rotation, styxOrigin, NPC.scale, effects, 0);
            return false;
        }

        private static string AbomChat(string key, params object[] args) => Language.GetTextValue($"Mods.Fargowiltas.NPCs.Abominationn.Chat.{key}", args);
    }
}
