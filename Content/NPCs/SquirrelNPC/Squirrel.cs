using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems.Collections;
using Fargowiltas.Content.Achievements;
using Fargowiltas.Content.Items.CaughtNPCs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.UI.Emotes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Fargowiltas.Fargowiltas;
using static Terraria.GameContent.LucyAxeMessage;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.NPCs.SquirrelNPC
{
    [AutoloadHead]
    public class Squirrel : ModNPC
    {
        private static Profiles.StackedNPCProfile NPCProfile;
        private static int ShimmerHeadIndex;

        public override void Load()
        {
            ShimmerHeadIndex = ModContent.GetModHeadSlot(Texture + "_Shimmer_Head");
        }

        private const string ShopName = "Shop";
        private Asset<Texture2D> EyesAsset => ModContent.Request<Texture2D>(Texture + "_Eyes");

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ExtraFramesCount[Type] = 0;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = -1;

            NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<SquirrelEmote>();

            NPCID.Sets.CannotSitOnFurniture[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection<LumberJack>(AffectionLevel.Love);

            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture)) //, Texture + "_Party")
            //new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, null)
            );

            NPCID.Sets.NPCPortraits.Add(
                Type,
                NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait($"{Texture}_Portrait")));

            //Close up portrait mode
            NPCID.Sets.NPCPortraitsCloseUpOffsets.Add(Type, new Vector2(-3f, -36f));

            NPCID.Sets.NPCPortraitsFullBodyRetroOffsets.Add(Type, new Vector2(-3, -12));

        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 34;
            NPC.height = 42;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = .25f;
            NPC.housingCategory = 1;

            AnimationType = NPCID.Squirrel;
            NPC.aiStyle = NPCAIStyleID.Passive;
        }

        public override void ChatBubblePosition(ref Vector2 position, ref SpriteEffects spriteffects)
        {
            if (!NPC.IsShimmerVariant)
                position.Y += 17f;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Mods.Fargowiltas.NPCs.Squirrel.Bestiary")
            });
        }

        public override List<string> SetNPCNameList()
        {
            string[] names =
               [Language.GetTextValue("Mods.Fargowiltas.NPCs.Squirrel.NPCName1"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Squirrel.NPCName2"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Squirrel.NPCName3"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Squirrel.NPCName4"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Squirrel.NPCName5"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Squirrel.NPCName6"),
                Language.GetTextValue("Mods.Fargowiltas.NPCs.Squirrel.NPCName7")];

            return new List<string>(names);
        }
        public override void OnSpawn(IEntitySource source)
        {
            FargoWorld.DownedBools["squirrel"] = true;
            base.OnSpawn(source);
        }
        public const int IdleThreshold = (int)(60 * 60 * 1.5f);
        public bool doIdleAnimation;
        public override void AI()
        {
            NPC.dontTakeDamage = Main.bloodMoon;
            DrawOffsetY = -2;

            if (NPC.velocity.X == 0)
            {
                if (!doIdleAnimation)
                    NPC.localAI[0]++;
                if (NPC.localAI[0] >= IdleThreshold)
                {
                    doIdleAnimation = true;
                    NPC.localAI[0] = 60 * Main.rand.NextFloat(1.5f, 2f) * -1f;
                }
            }

            // prevent squirrel from running away during the idle animation
            if (doIdleAnimation || idleLoops != 0)
                NPC.ai[1]++;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            if (FargoUtils.AnyBossAlive() || !FargoServerConfig.Instance.Squirrel)
            {
                return false;
            }
            if (FargoWorld.DownedBools["squirrel"])
            {
                return true;
            }
            Mod souls = Fargowiltas.SoulsMod;
            if (souls == null && NPC.downedSlimeKing)
                return true;

            if (souls?.TryFind("TopHatSquirrelCaught", out ModItem modItem) == true &&
                Main.player.Any(p => p.active && p.HasItem(modItem.Type)))
            {
                return true;
            }

            return false;
        }

        public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
        {
            boundingBox = new((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
        }

        public override string GetChat()
        {
            bool hasCritterCompanionship = false; //Main.LocalPlayer.dontHurtCritters;
            if (hasCritterCompanionship)
            {
                return Main.rand.Next(3) switch
                {
                    0 => SquirrelChat("CritterCompanionship1"),
                    1 => SquirrelChat("CritterCompanionship2"),
                    _ => SquirrelChat("CritterCompanionship3"),
                };
            }
            return Main.rand.Next(3) switch
            {
                0 => SquirrelChat("Normal1"),
                1 => SquirrelChat("Normal2"),
                _ => SquirrelChat("Normal3"),
            };
        }

        public override void RegisterChatButtons(NPCInteractionList interactions)
        {
            interactions.InsertBefore(NPCInteractions.Shop(ShopName), NPCInteractionDatabase.CloseButton);
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName);

            if (ModContent.TryFind("FargowiltasSouls", "TopHatSquirrelCaught", out ModItem tophatSqurl))
            {
                npcShop.Add(new Item(tophatSqurl.Type) { shopCustomPrice = Item.buyPrice(copper: 100000) });
            }

            npcShop
                .Add(new Item(ItemType<GizmoParts>()))
                .Add(new Item(ItemType<EnchantedTree>()))
                .Add(new Item(ItemType<PotionCooler>()))
                .Add(new Item(ItemType<SuperDummy>()) { shopCustomPrice = Item.buyPrice(gold: 1)})
            ;

            npcShop.Register();
        }

        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override bool UsesPartyHat() => false;

        public int idleLoops;
        public override void FindFrame(int frameHeight)
        {
            // todo: internally cleanup
            /*
             * Main.NewText("x: " + FrameX);
            Main.NewText("y: " + FrameY);
            Main.NewText("idle loops: " + idleLoops);
            Main.NewText("idling: " + doIdleAnimation);
            */
            if (NPC.velocity.X != 0) // movement
            {
                FrameX = 2;
                if (++FrameCounter >= 6)
                {
                    FrameCounter = 0;
                    if (++FrameY >= Main.npcFrameCount[Type])
                        FrameY = 0;
                }

            }
            else if (doIdleAnimation) // idle animation
            {
                if (FrameX != 3 && FrameX != 4)
                    FrameX = 3;

                if (++FrameCounter >= 6)
                {
                    FrameCounter = 0;
                    ++FrameY;
                    if (FrameY >= 3 && FrameX == 3)
                    {
                        FrameY = 0;
                        FrameX++;
                    }
                    if (FrameY >= 2 && FrameX == 4)
                    {
                        FrameY = 0;
                        idleLoops++;
                    }

                    if (idleLoops >= 8)
                    {
                        doIdleAnimation = false;
                        idleLoops = 0;
                    }

                }

            }
            else if (!doIdleAnimation && (FrameX == 3 || FrameX == 4)) //idle animation end
            {
                if (FrameX != 3)
                {
                    FrameX = 3;
                    FrameY = 2;
                }

                if (++FrameCounter >= 6)
                {
                    FrameCounter = 0;
                    if (--FrameY <= 0)
                    {
                        FrameX = 1; FrameY = 0;
                    }
                }
            }
            else // still
            {
                FrameX = 1;
                FrameCounter = FrameY = 0;
            }
        }
        public int FrameCounter, FrameX, FrameY;
        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            bool bloodMoon = Main.bloodMoon;

            Rectangle frame = new(50 * FrameX, 42 * FrameY, 50, 42);
            Vector2 origin = frame.Size() / 2f;
            SpriteEffects effects = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float scale = (Main.mouseTextColor / 200f - 0.35f) * 0.3f + 0.8f;
            Vector2 position = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY) + new Vector2(0, 4);

            if (bloodMoon)
            {
                for (int j = 0; j < 12; j++)
                {
                    Vector2 afterimageOffset = (MathHelper.TwoPi * j / 12f).ToRotationVector2() * 4f + Vector2.UnitY * 3;
                    Color glowColor = new Color(255, 0, 0, 0);
                    Main.EntitySpriteDraw(texture, position + afterimageOffset - new Vector2(0, 4), frame, glowColor, NPC.rotation, origin, scale, effects, 0f);
                }
            }

            sb.Draw(texture, position, frame, drawColor, NPC.rotation, origin, NPC.scale, effects, 0);

            if (bloodMoon)
                sb.Draw(EyesAsset.Value, position, frame, Color.White * NPC.Opacity, NPC.rotation, origin, NPC.scale, effects, 0f);

            return false;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>(Mod.Name, $"TophatSquirrelGore").Type, NPC.scale);
                }
            }
        }

        public static bool CanSacrifice(Item item) => EventSacrifice(Main.LocalPlayer.HeldItem, out _, false) || Main.LocalPlayer.FargoMutant().SacrificeCount[item.type] > 0;

        private static string SquirrelChat(string key) => Language.GetTextValue($"Mods.Fargowiltas.NPCs.Squirrel.Chat.{key}");

        #region Item Feeding System
        public static bool SacrificeThing(Player player, Item item)
        {
            if (item == null || item.favorited)
                return false;
            int itemType = item.type;
            FargoPlayer modPlayer = player.FargoMutant();
            if (EventSacrifice(player.HeldItem, out int consumeCount, false) || modPlayer.SacrificeCount[itemType] > 0) // item sacrificable; do the sacrifice thing
            {
                if (player.CountItemHeld(itemType) >= consumeCount)
                {
                    Main.NewText(consumeCount);
                    for (int consume = 0; consume < consumeCount; consume++)
                    {
                        player.ConsumeItemHeld(itemType, true);
                    }
                    if (modPlayer.SacrificeCount[itemType] > 0)
                        modPlayer.SacrificeCount[itemType]--;

                    //Vector2 spawnPos = Main.MouseWorld;
                    //SoundEngine.PlaySound(a, spawnPos);

                    SoundEngine.PlaySound(SoundID.Item2, player.Center);
                    if (EventSacrifice(ContentSamples.ItemsByType[itemType], out _, true))
                    {
                        // actions happen in the EventSacrifice method
                    }
                    else
                    {
                        int multiplier = 1;
                        if (FargoItemSets.SacrificeCountDefault[itemType] == 1) // things that can only be sacrificed once give increased output
                            multiplier = 3;
                        for (int i = 0; i < multiplier; i++)
                        {
                            int result;
                            int amount;
                            if (FargoItemSets.HardmodeSacrifice[itemType] && Main.hardMode)
                                result = SacrificeResultHardmode(out amount);
                            else
                                result = SacrificeResult(out amount);
                            //Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(result, amount));
                            player.QuickSpawnItem(new EntitySource_WorldEvent(), new Item(result, amount));

                        }

                        for (int i = 0; i < 32; i++)
                        {
                            Dust.NewDust(player.Center, 1, 1, DustID.Blood);
                        }
                    }

                    //Projectile.NewProjectile(new EntitySource_WorldEvent(), spawnPos, Vector2.Zero, ModContent.ProjectileType<SacrificeProj>(), 0, 0f, Main.myPlayer, itemType);
                    return true;
                }
            }
            return false;
        }
        public static int[] SetDefaultSacrificeCount(SetFactory itemFactory)
        {
            int[] prehardmode =
            [
                // Life Crystal
                ItemID.LifeCrystal, 3,

                // king slime
                ItemID.NinjaHood, 1,
                ItemID.NinjaShirt, 1,
                ItemID.NinjaPants, 1,

                // queen bee
                ItemID.BeeGun, 1,
                ItemID.BeeKeeper, 1,
                ItemID.BeesKnees, 1,
                ItemID.HiveWand, 1,

                // demonite/crimson
                ItemID.DemonBow, 1,
                ItemID.TendonBow, 1,
                ItemID.LightsBane, 1,
                ItemID.BloodButcherer, 1,
                ItemID.FisherofSouls, 1,
                ItemID.Fleshcatcher, 1,

                ItemID.NightmarePickaxe, 1,
                ItemID.DeathbringerPickaxe, 1,
                ItemID.TheBreaker, 1,
                ItemID.FleshGrinder, 1,
                ItemID.WarAxeoftheNight, 1,
                ItemID.BloodLustCluster, 1,

                // deerclops
                ItemID.PewMaticHorn, 1,
                ItemID.WeatherPain, 1,
                ItemID.HoundiusShootius, 1,
                ItemID.LucyTheAxe, 1,

                // skeletron
                ItemID.BookofSkulls, 1,
                ItemID.SkeletronHand, 1,

                // enemy drop materials
                ItemID.TatteredCloth, 3,
                ItemID.WormTooth, 3,
                ItemID.SharkFin, 3,
                ItemID.Hook, 3,
                ItemID.BlackLens, 1,
                ItemID.AntlionMandible, 3,
                ItemID.Vine, 3,

                // event drops
                ItemID.SlimeStaff, 1,

                ItemID.Harpoon, 1,

                ItemID.BloodRainBow, 1,
                ItemID.VampireFrogStaff, 1,
                ItemID.BloodFishingRod, 1,

                // crate/orb/cabin loot
                ItemID.Musket, 1,
                ItemID.ShadowOrb, 1,
                ItemID.Vilethorn, 1,
                ItemID.BallOHurt, 1,
                ItemID.BandofStarpower, 1,

                ItemID.TheUndertaker, 1,
                ItemID.CrimsonHeart, 1,
                ItemID.CrimsonRod, 1,
                ItemID.TheRottedFork, 1,
                ItemID.PanicNecklace, 1,

                ItemID.FalconBlade, 1,

                ItemID.MagicMirror, 1,
                ItemID.BandofRegeneration, 1,
                ItemID.CloudinaBottle, 1,
                ItemID.HermesBoots, 1,
                ItemID.Mace, 1,
                ItemID.ShoeSpikes, 1,
                ItemID.FlareGun, 1,
                ItemID.LavaCharm, 1,

                ItemID.Muramasa, 1,
                ItemID.CobaltShield, 1,
                ItemID.AquaScepter, 1,
                ItemID.BlueMoon, 1,
                ItemID.MagicMissile, 1,
                ItemID.Valor, 1,
                ItemID.Handgun, 1,

                ItemID.Starfury, 1,
                ItemID.ShinyRedBalloon, 1,
                ItemID.LuckyHorseshoe, 1,

                ItemID.IceBoomerang, 1,
                ItemID.IceBlade, 1,
                ItemID.IceSkates, 1,
                ItemID.SnowballCannon, 1,
                ItemID.BlizzardinaBottle, 1,
                ItemID.FlurryBoots, 1,
                ItemID.IceMirror, 1,

                ItemID.AnkletoftheWind, 1,
                ItemID.FeralClaws, 1,
                ItemID.StaffofRegrowth, 1,
                ItemID.FiberglassFishingPole, 1,
                ItemID.Boomstick, 1,
                ItemID.FlowerBoots, 1,

                // enemy weapon drops
                ItemID.BatBat, 1,
                ItemID.ChainKnife, 1,
                ItemID.BoneSword, 1,
                ItemID.BonePickaxe, 1,
                ItemID.AntlionClaw, 1, // mandible blade
                ItemID.TentacleSpike, 1,
                ItemID.DemonScythe, 1,
                ItemID.BloodyMachete, 1,
                ItemID.BladedGlove, 1,
                ItemID.ZombieArm, 1,
                ItemID.Shackle, 1,
                ItemID.Shroomerang, 1,
                ItemID.Rally, 1,

                // event summons
                CaughtNPCItem.CaughtTownies[NPCID.Dryad], 1,
                ItemID.PinkGel, 1,
                ItemID.TissueSample, 1,
                ItemID.ShadowScale, 1,
                ModContent.ItemType<WiresPainting>(), 1,

                // wof
                ItemID.BreakerBlade, 1,
                ItemID.ClockworkAssaultRifle, 1,
                ItemID.LaserRifle, 1,
                ItemID.FireWhip, 1
            ];

            int[] hardmode =
            [

                // mimic
                ItemID.DualHook, 1,
                ItemID.MagicDagger, 1,
                ItemID.PhilosophersStone, 1,
                ItemID.TitanGlove, 1,
                ItemID.StarCloak, 1,
                ItemID.CrossNecklace, 1,

                // ice mimic
                ItemID.Frostbrand, 1,
                ItemID.IceBow, 1,
                ItemID.FlowerofFrost, 1,

                // corrupt mimic
                ItemID.ClingerStaff, 1,
                ItemID.DartRifle, 1,
                ItemID.ChainGuillotines, 1,
                ItemID.PutridScent, 1,
                ItemID.WormHook, 1,

                // crimson mimic
                ItemID.SoulDrain, 1,
                ItemID.DartPistol, 1,
                ItemID.FetidBaghnakhs, 1,
                ItemID.FleshKnuckles, 1,
                ItemID.TendonHook, 1,
                
                // hallowed mimic
                ItemID.DaedalusStormbow, 1,
                ItemID.FlyingKnife, 1,
                ItemID.CrystalVileShard, 1,
                ItemID.IlluminantHook, 1,

                // queenie
                ItemID.CrystalNinjaHelmet, 1,
                ItemID.CrystalNinjaChestplate, 1,
                ItemID.CrystalNinjaLeggings, 1,
                ItemID.Smolstar, 1,
                ItemID.QueenSlimeMountSaddle, 1,
                ItemID.QueenSlimeHook, 1,

                // plantera
                ItemID.GrenadeLauncher, 1,
                ItemID.VenusMagnum, 1,
                ItemID.NettleBurst, 1,
                ItemID.LeafBlower, 1,
                ItemID.FlowerPow, 1,
                ItemID.WaspGun, 1,
                ItemID.Seedler, 1,
                ItemID.PygmyStaff, 1,
                ItemID.ThornHook, 1,

                // golem
                ItemID.Stynger, 1,
                ItemID.PossessedHatchet, 1,
                ItemID.SunStone, 1,
                ItemID.EyeoftheGolem, 1,
                ItemID.HeatRay, 1,
                ItemID.StaffofEarth, 1,
                ItemID.GolemFist, 1,

                // oger
                ItemID.BookStaff, 1, // tome of infinite wisdom
                ItemID.DD2PhoenixBow, 1, // phantom phoenix
                ItemID.DD2SquireDemonSword, 1, // brand of the inferno
                ItemID.MonkStaffT1, 1, // sleepy octopod
                ItemID.MonkStaffT2, 1, // gassy glaive

                // betsy
                ItemID.DD2BetsyBow, 1, // aerial bane
                ItemID.DD2SquireBetsySword, 1, // flying dragon
                ItemID.MonkStaffT3, 1, // sky dragons fury
                ItemID.ApprenticeStaffT3, 1, // betsy's wrath

                // eol
                ItemID.FairyQueenMagicItem, 1, // nightglow
                ItemID.PiercingStarlight, 1,
                ItemID.RainbowWhip, 1,
                ItemID.FairyQueenRangedItem, 1, // eventide

                // duke
                ItemID.Flairon, 1,
                ItemID.BubbleGun, 1,
                ItemID.RazorbladeTyphoon, 1,
                ItemID.TempestStaff, 1,
                ItemID.Tsunami, 1,

                // enemy drops
                ItemID.BeamSword, 1,
                ItemID.Marrow, 1,
                ItemID.Uzi, 1,
                ItemID.UnholyTrident, 1,
                ItemID.IceSickle, 1,
                ItemID.FrostStaff, 1,
                // dungeon
                ItemID.Keybrand, 1,
                ItemID.ShadowbeamStaff, 1,
                ItemID.SpectreStaff, 1,
                ItemID.InfernoFork, 1,
                ItemID.RocketLauncher, 1,
                ItemID.SniperRifle, 1,
                ItemID.ShadowJoustingLance, 1,
                ItemID.TacticalShotgun, 1,
                ItemID.PaladinsHammer, 1,
                ItemID.MagnetSphere, 1,
                ItemID.MaceWhip, 1,

                // materials
                ItemID.TurtleShell, 3,
                ItemID.UnicornHorn, 3,
            ];

            return itemFactory.CreateIntSet(0, [.. prehardmode, .. hardmode]);
        }
        public readonly struct Result(int type, int amount)
        {
            public readonly int Type = type;
            public readonly int Amount = amount;
        }
        // some common values to help modded entries
        public static int OreCount = 50;
        public static double OreWeight = 0.5;

        public static int FishCount = 6;
        public static double FishWeight = 0.75;
        public static int SacrificeResult(out int amount)
        {
            WeightedRandom<Result> result = new(Main.rand.Next(int.MaxValue));

            result.Add(new(ItemID.HerbBag, 2), 1);


            Result real = result.Get();
            result.Clear();
            amount = real.Amount;
            return real.Type;

        }

        public static int SacrificeResultHardmode(out int amount)
        {
            WeightedRandom<Result> result = new(Main.rand.Next(int.MaxValue));

            // lootboxes
            result.Add(new(ItemID.HerbBag, 3), 1);

            Result real = result.Get();
            result.Clear();
            amount = real.Amount;
            return real.Type;

        }

        public static bool EventSacrifice(Item item, out int consumeCount, bool action = true)
        {
            consumeCount = 1;
            Player player = Main.LocalPlayer;
            FargoPlayer modPlayer = player.FargoMutant();
            if (!action && modPlayer.SacrificeCount[item.type] <= 0)
                return false;

            // spawn blood moon
            if (item.type == CaughtNPCItem.CaughtTownies[NPCID.Dryad])
            {
                if (action)
                {
                    modPlayer.SacrificeCount[item.type]--;
                    SoundEngine.PlaySound(SoundID.Roar);

                    //ModContent.GetInstance<NPCSacrificeAchievement>().Condition.Complete();

                    // turn it to night
                    Main.dayTime = false;
                    Main.time = 0;
                    NetMessage.SendData(MessageID.WorldData);

                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        AchievementsHelper.NotifyProgressionEvent(4);
                        Main.bloodMoon = true;
                        if (Main.GetMoonPhase() == MoonPhase.Empty)
                        {
                            Main.moonPhase = 5;
                        }
                        Main.NewText(Lang.misc[8].Value, 50, byte.MaxValue, 130);
                    }
                    else
                    {
                        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, Main.LocalPlayer.whoAmI, -10f);
                    }
                }
                return true;
            }

            // spawn slime rain
            if (item.type == ItemID.PinkGel)
            {
                consumeCount = 10;
                if (action)
                {
                    modPlayer.SacrificeCount[item.type]--;
                    if (!Main.slimeRain)
                    {
                        Main.StartSlimeRain();
                        Main.slimeWarningDelay = 1;
                        Main.slimeWarningTime = 1;
                        SoundEngine.PlaySound(SoundID.Roar);
                    }
                }
                return true;
            }

            // drop a meteor
            if (item.type == ItemID.ShadowScale || item.type == ItemID.TissueSample)
            {
                consumeCount = 10;
                if (action)
                {
                    modPlayer.SacrificeCount[ItemID.ShadowScale]--;
                    modPlayer.SacrificeCount[ItemID.TissueSample]--;

                    if (Main.netMode == NetmodeID.SinglePlayer)
                        WorldGen.dropMeteor();
                    else
                    {
                        var netMessage = Instance.GetPacket();
                        netMessage.Write((byte)PacketID.DropMeteor); // "drop a meteor" tag
                        netMessage.Send();
                    }
                }
                return true;
            }

            // wires painting; spawns 20 cats :)
            if (item.type == ModContent.ItemType<WiresPainting>())
            {
                if (action)
                {
                    modPlayer.SacrificeCount[ModContent.ItemType<WiresPainting>()]--;
                    for (int i = 0; i < 20; i++)
                    {
                        NPC.NewNPC(new EntitySource_WorldEvent(), (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, NPCID.TownCat);
                    }
                }
                return true;
            }

            // lucy the axe: wait, don't!
            if (item.type == ItemID.LucyTheAxe)
            {
                consumeCount = 1;
                if (action)
                {
                    modPlayer.SacrificeCount[item.type]--;
                    Create((MessageSource)8, player.Top, new Vector2(Main.rand.NextFloatDirection() * 7f, -2f + Main.rand.NextFloat() * -2f));
                    Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.Wood, 100));
                    Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.BorealWood, 100));
                    Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.PalmWood, 100));
                    Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.Shadewood, 100));
                    Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.Ebonwood, 100));
                    Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.RichMahogany, 100));

                    if (modPlayer.ItemHasBeenOwned[ItemID.AshWood])
                        Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.AshWood, 100));

                    if (Main.hardMode && modPlayer.ItemHasBeenOwned[ItemID.Pearlwood])
                        Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.Pearlwood, 100));

                    if (Main.hardMode && modPlayer.ItemHasBeenOwned[ItemID.SpookyWood])
                        Item.NewItem(new EntitySource_WorldEvent(), player.Center, new Item(ItemID.SpookyWood, 100));
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}