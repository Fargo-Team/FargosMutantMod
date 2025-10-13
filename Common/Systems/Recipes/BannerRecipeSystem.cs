using Fargowiltas.Common.Configs;
using Fargowiltas.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes
{
    public class BannerRecipeSystem : ModSystem
    {
        private static int AnyPirateBanner, AnyArmoredBonesBanner, AnySlimesBanner, AnyBatBanner, AnyPureSkeletonBanner;
        private static int AnyHallowBanner, AnyCorruptBanner, AnyCrimsonBanner, AnyJungleBanner, AnySnowBanner, AnyDesertBanner;

        public override bool IsLoadingEnabled(Mod mod)
        {
            return FargoServerConfig.Instance.BannerRecipes;
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => RecipeHelper.GenerateAnyBannerRecipeGroupText("NPCName.Pirate"), ItemID.PirateDeadeyeBanner, ItemID.PirateCorsairBanner, ItemID.PirateCrossbowerBanner, ItemID.PirateBanner);
            AnyPirateBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyPirateBanner", group);

            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("ArmoredBonesBanner"), ItemID.BlueArmoredBonesBanner, ItemID.HellArmoredBonesBanner, ItemID.RustyArmoredBonesBanner);
            AnyArmoredBonesBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyArmoredBonesBanner", group);

            // Slimes (excluding ones that don't drop gel)
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.SlimeBanner),
                ItemID.SlimeBanner, ItemID.GreenSlimeBanner, ItemID.RedSlimeBanner, ItemID.PurpleSlimeBanner,
                ItemID.YellowSlimeBanner, ItemID.BlackSlimeBanner, ItemID.IceSlimeBanner, ItemID.SandSlimeBanner,
                ItemID.JungleSlimeBanner, ItemID.SpikedIceSlimeBanner, ItemID.SpikedJungleSlimeBanner, ItemID.MotherSlimeBanner,
                ItemID.UmbrellaSlimeBanner, ItemID.ToxicSludgeBanner, ItemID.CorruptSlimeBanner, ItemID.SlimerBanner,
                ItemID.CrimslimeBanner, ItemID.GastropodBanner, ItemID.IlluminantSlimeBanner, ItemID.RainbowSlimeBanner
            );
            AnySlimesBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnySlimes", group);

            // Any Hallow enemy
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyBannerRecipeGroupText("RandomWorldName_Adjective.Hallowed"),
                ItemID.PixieBanner, ItemID.UnicornBanner, ItemID.RainbowSlimeBanner, ItemID.GastropodBanner,
                ItemID.LightMummyBanner, ItemID.IlluminantBatBanner, ItemID.IlluminantSlimeBanner, ItemID.ChaosElementalBanner,
                ItemID.EnchantedSwordBanner, ItemID.BigMimicHallowBanner
            );
            AnyHallowBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyHallows", group);

            // Any Corruption enemy
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyBannerRecipeGroupText("CLI.Corrupt"),
                ItemID.EaterofSoulsBanner, ItemID.CorruptorBanner, ItemID.CorruptSlimeBanner, ItemID.SlimerBanner,
                ItemID.DevourerBanner, ItemID.WorldFeederBanner, ItemID.DarkMummyBanner, ItemID.CursedHammerBanner,
                ItemID.ClingerBanner, ItemID.BigMimicCorruptionBanner
            );
            AnyCorruptBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyCorrupts", group);

            // Any Crimson enemy
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyBannerRecipeGroupText("CLI.Crimson"),
                ItemID.BloodCrawlerBanner, ItemID.FaceMonsterBanner, ItemID.CrimeraBanner, ItemID.HerplingBanner,
                ItemID.CrimslimeBanner, ItemID.BloodJellyBanner, ItemID.BloodFeederBanner, ItemID.BloodMummyBanner,
                ItemID.CrimsonAxeBanner, ItemID.IchorStickerBanner, ItemID.FloatyGrossBanner, ItemID.BigMimicCrimsonBanner
            );
            AnyCrimsonBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyCrimsons", group);

            // Any Jungle enemy
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyBannerRecipeGroupText("RandomWorldName_Location.Jungle"),
                ItemID.PiranhaBanner, ItemID.SnatcherBanner, ItemID.JungleBatBanner, ItemID.JungleSlimeBanner,
                ItemID.DoctorBonesBanner, ItemID.AnglerFishBanner, ItemID.ArapaimaBanner, ItemID.TortoiseBanner,
                ItemID.AngryTrapperBanner, ItemID.DerplingBanner, ItemID.GiantFlyingFoxBanner, ItemID.HornetBanner,
                ItemID.SpikedJungleSlimeBanner, ItemID.JungleCreeperBanner, ItemID.MothBanner, ItemID.ManEaterBanner,
                ItemID.MossHornetBanner
            );
            AnyJungleBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyJungles", group);

            // Any Snow enemy
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyBannerRecipeGroupText("RandomWorldName_Noun.Snow"),
                ItemID.IceSlimeBanner, ItemID.ZombieEskimoBanner, ItemID.IceElementalBanner, ItemID.WolfBanner,
                ItemID.IceGolemBanner, ItemID.IceBatBanner, ItemID.SnowFlinxBanner, ItemID.SpikedIceSlimeBanner,
                ItemID.UndeadVikingBanner, ItemID.ArmoredVikingBanner, ItemID.IceTortoiseBanner, ItemID.IcyMermanBanner,
                ItemID.PigronBanner
            );
            AnySnowBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnySnows", group);

            // Any desert enemy
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyBannerRecipeGroupText("RandomWorldName_Location.Desert"),
                ItemID.VultureBanner, ItemID.MummyBanner, ItemID.BloodMummyBanner, ItemID.DarkMummyBanner,
                ItemID.LightMummyBanner, ItemID.FlyingAntlionBanner, ItemID.WalkingAntlionBanner, ItemID.LarvaeAntlionBanner,
                ItemID.AntlionBanner, ItemID.SandSlimeBanner, ItemID.TombCrawlerBanner, ItemID.DesertBasiliskBanner,
                ItemID.RavagerScorpionBanner, ItemID.DesertLamiaBanner, ItemID.DesertGhoulBanner, ItemID.DesertDjinnBanner,
                ItemID.DuneSplicerBanner, ItemID.SandElementalBanner, ItemID.SandsharkBanner, ItemID.SandsharkCorruptBanner,
                ItemID.SandsharkCrimsonBanner, ItemID.SandsharkHallowedBanner, ItemID.TumbleweedBanner
            );
            AnyDesertBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyDeserts", group);

            // Any bats
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyBannerRecipeGroupText("RandomWorldName_Noun.Bats"),
                ItemID.BatBanner, ItemID.GiantBatBanner, ItemID.GiantFlyingFoxBanner, ItemID.IceBatBanner,
                ItemID.IlluminantBatBanner, ItemID.JungleBatBanner, ItemID.HellbatBanner, ItemID.LavaBatBanner,
                ItemID.SporeBatBanner
            );
            AnyBatBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyBats", group);

            // Spore or Pure Skeletons
            group = new RecipeGroup(() => ItemXOrY(ItemID.SkeletonBanner, ItemID.SporeSkeletonBanner), ItemID.SkeletonBanner, ItemID.SporeSkeletonBanner);
            AnyPureSkeletonBanner = RecipeGroup.RegisterGroup("Fargowiltas:AnyPureSkeleton", group);
        }

        public override void AddRecipes()
        {
            AddBannerToAccessoryRecipes();
            AddBannerToArmorRecipes();
            AddBannerToCritterRecipes();
            AddBannerToFoodRecipes();
            AddBannerToFurnitureRecipes();
            AddBannerToMaterialRecipes();
            AddBannerToMiscItemRecipes();
            AddBannerToMountOrPetRecipes();
            AddBannerToWeaponRecipes();
        }

        private static void AddBannerToAccessoryRecipes()
        {
            AddBannerToItemRecipe(ItemID.BloodZombieBanner, ItemID.SharkToothNecklace);
            AddBannerToItemRecipe(ItemID.DripplerBanner, ItemID.SharkToothNecklace);
            AddBannerToItemRecipe(ItemID.FireImpBanner, ItemID.ObsidianRose);
            AddBannerToItemRecipe(ItemID.JellyfishBanner, ItemID.JellyfishNecklace);
            AddBannerToItemRecipe(ItemID.PinkJellyfishBanner, ItemID.JellyfishNecklace);
            AddBannerToItemRecipe(ItemID.HellbatBanner, ItemID.MagmaStone);
            AddBannerToItemRecipe(ItemID.RaincoatZombieBanner, ItemID.Shackle);
            AddBannerToItemRecipe(ItemID.ZombieBanner, ItemID.Shackle);
            AddBannerToItemRecipe(ItemID.ZombieEskimoBanner, ItemID.Shackle);
            AddBannerToItemRecipe(ItemID.SharkBanner, ItemID.DivingHelmet);

            // Hardmode
            AddBannerToItemRecipe(ItemID.GreenJellyfishBanner, ItemID.JellyfishNecklace, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.IceTortoiseBanner, ItemID.FrozenTurtleShell, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SkeletonArcherBanner, ItemID.MagicQuiver, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.WerewolfBanner, ItemID.MoonCharm, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.TitanGlove, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.PhilosophersStone, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.CrossNecklace, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.StarCloak, conditions: Condition.Hardmode);

            // Downed Pirates
            AddBannerGroupToItemRecipe(AnyPirateBanner, ItemID.DiscountCard, conditions: Condition.DownedPirates);
            AddBannerGroupToItemRecipe(AnyPirateBanner, ItemID.GoldRing, conditions: Condition.DownedPirates);
            AddBannerGroupToItemRecipe(AnyPirateBanner, ItemID.LuckyCoin, conditions: Condition.DownedPirates);

            // Downed Any Mech Boss
            AddBannerToItemRecipe(ItemID.CreatureFromTheDeepBanner, ItemID.NeptunesShell, conditions: Condition.DownedMechBossAny);
            AddBannerToItemRecipe(ItemID.LavaBatBanner, ItemID.MagmaStone, conditions: Condition.DownedMechBossAny);
            AddBannerToItemRecipe(ItemID.VampireBanner, ItemID.MoonStone, conditions: Condition.DownedMechBossAny);

            // Downed Plantera
            AddBannerToItemRecipe(ItemID.BoneLeeBanner, ItemID.BlackBelt, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.BoneLeeBanner, ItemID.Tabi, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.SkeletonSniperBanner, ItemID.RifleScope, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.MothronBanner, ItemID.MothronWings, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.PaladinBanner, ItemID.PaladinsShield, conditions: Condition.DownedPlantera);

            // Ankh Shield
            AddBannerToItemRecipe(ItemID.CrimsonAxeBanner, ItemID.Nazar);
            AddBannerToItemRecipe(ItemID.CursedHammerBanner, ItemID.Nazar);
            AddBannerToItemRecipe(ItemID.CursedSkullBanner, ItemID.Nazar);
            AddBannerToItemRecipe(ItemID.EnchantedSwordBanner, ItemID.Nazar);
            AddBannerToItemRecipe(ItemID.GiantCursedSkullBanner, ItemID.Nazar);
            AddBannerToItemRecipe(ItemID.HornetBanner, ItemID.Bezoar);
            AddBannerToItemRecipe(ItemID.MossHornetBanner, ItemID.Bezoar);
            AddBannerToItemRecipe(ItemID.ToxicSludgeBanner, ItemID.Bezoar);

            AddBannerToItemRecipe(ItemID.AnglerFishBanner, ItemID.AdhesiveBandage, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.WerewolfBanner, ItemID.AdhesiveBandage, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BloodMummyBanner, ItemID.Blindfold, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.CorruptSlimeBanner, ItemID.Blindfold, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.CrimslimeBanner, ItemID.Blindfold, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DarkMummyBanner, ItemID.Blindfold, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.ArmoredSkeletonBanner, ItemID.ArmorPolish, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.ClownBanner, ItemID.TrifoldMap, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.GiantBatBanner, ItemID.TrifoldMap, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.LightMummyBanner, ItemID.TrifoldMap, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.CorruptorBanner, ItemID.Vitamins, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.FloatyGrossBanner, ItemID.Vitamins, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BloodMummyBanner, ItemID.Megaphone, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DarkMummyBanner, ItemID.Megaphone, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.GreenJellyfishBanner, ItemID.Megaphone, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.PixieBanner, ItemID.Megaphone, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MummyBanner, ItemID.FastClock, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.PixieBanner, ItemID.FastClock, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.WraithBanner, ItemID.FastClock, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MedusaBanner, ItemID.PocketMirror, conditions: Condition.Hardmode);

            AddBannerToItemRecipe(ItemID.RustyArmoredBonesBanner, ItemID.AdhesiveBandage, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.BlueArmoredBonesBanner, ItemID.ArmorPolish, conditions: Condition.DownedPlantera);

            // Shellphone
            AddBannerToItemRecipe(ItemID.AngryBonesBanner, ItemID.TallyCounter);
            AddBannerToItemRecipe(ItemID.CursedSkullBanner, ItemID.TallyCounter);
            AddBannerToItemRecipe(ItemID.SkeletonMageBanner, ItemID.TallyCounter);
            AddBannerToItemRecipe(ItemID.ArmoredVikingBanner, ItemID.Compass);
            AddBannerToItemRecipe(ItemID.CrawdadBanner, ItemID.Compass);
            AddBannerToItemRecipe(ItemID.GiantShellyBanner, ItemID.Compass);
            AddBannerToItemRecipe(ItemID.MotherSlimeBanner, ItemID.Compass);
            AddBannerToItemRecipe(ItemID.PiranhaBanner, ItemID.Compass);
            AddBannerToItemRecipe(ItemID.SalamanderBanner, ItemID.Compass);
            AddBannerToItemRecipe(ItemID.SnowFlinxBanner, ItemID.Compass);
            AddBannerToItemRecipe(ItemID.UndeadVikingBanner, ItemID.Compass);
            AddBannerToItemRecipe(ItemID.BatBanner, ItemID.DepthMeter);
            AddBannerToItemRecipe(ItemID.CrawdadBanner, ItemID.DepthMeter);
            AddBannerToItemRecipe(ItemID.GiantBatBanner, ItemID.DepthMeter);
            AddBannerToItemRecipe(ItemID.GiantShellyBanner, ItemID.DepthMeter);
            AddBannerToItemRecipe(ItemID.IceBatBanner, ItemID.DepthMeter);
            AddBannerToItemRecipe(ItemID.JungleBatBanner, ItemID.DepthMeter);
            AddBannerToItemRecipe(ItemID.SalamanderBanner, ItemID.DepthMeter);
            AddBannerToItemRecipe(ItemID.SporeBatBanner, ItemID.DepthMeter);
            AddBannerToItemRecipe(ItemID.NypmhBanner, ItemID.MetalDetector);
        }

        private static void AddBannerToArmorRecipes()
        {
            // Armor
            AddBannerToItemRecipe(ItemID.AngryBonesBanner, ItemID.AncientNecroHelmet, bannerAmount: 2);
            AddBannerToItemRecipe(ItemID.EaterofSoulsBanner, ItemID.AncientShadowGreaves, 2);
            AddBannerToItemRecipe(ItemID.EaterofSoulsBanner, ItemID.AncientShadowHelmet, 2);
            AddBannerToItemRecipe(ItemID.EaterofSoulsBanner, ItemID.AncientShadowScalemail, 2);
            AddBannerToItemRecipe(ItemID.GraniteFlyerBanner, ItemID.NightVisionHelmet);
            AddBannerToItemRecipe(ItemID.GraniteGolemBanner, ItemID.NightVisionHelmet);
            AddBannerToItemRecipe(ItemID.GreekSkeletonBanner, ItemID.GladiatorBreastplate);
            AddBannerToItemRecipe(ItemID.GreekSkeletonBanner, ItemID.GladiatorHelmet);
            AddBannerToItemRecipe(ItemID.GreekSkeletonBanner, ItemID.GladiatorLeggings);
            AddBannerToItemRecipe(ItemID.HornetBanner, ItemID.AncientCobaltBreastplate, 2);
            AddBannerToItemRecipe(ItemID.HornetBanner, ItemID.AncientCobaltHelmet, 2);
            AddBannerToItemRecipe(ItemID.HornetBanner, ItemID.AncientCobaltLeggings, 2);
            AddBannerToItemRecipe(ItemID.SkeletonBanner, ItemID.AncientGoldHelmet, 2);
            AddBannerToItemRecipe(ItemID.SkeletonBanner, ItemID.AncientIronHelmet, 2);
            AddBannerToItemRecipe(ItemID.SkeletonMageBanner, ItemID.AncientNecroHelmet, bannerAmount: 2);
            AddBannerToItemRecipe(ItemID.UndeadMinerBanner, ItemID.MiningPants);
            AddBannerToItemRecipe(ItemID.UndeadMinerBanner, ItemID.MiningShirt);
            AddBannerToItemRecipe(ItemID.UndeadVikingBanner, ItemID.VikingHelmet);
            AddBannerToItemRecipe(ItemID.ArmoredVikingBanner, ItemID.VikingHelmet);

            // Vanity
            AddBannerToItemRecipe(ItemID.PiranhaBanner, ItemID.RobotHat);
            AddBannerToItemRecipe(ItemID.CorruptBunnyBanner, ItemID.BunnyHood);
            AddBannerToItemRecipe(ItemID.CrimsonBunnyBanner, ItemID.BunnyHood);
            AddBannerToItemRecipe(ItemID.RockGolemBanner, ItemID.RockGolemHead);
            AddBannerToItemRecipe(ItemID.UmbrellaSlimeBanner, ItemID.UmbrellaHat);
            AddBannerToItemRecipe(ItemID.CorruptPenguinBanner, ItemID.PedguinHat);
            AddBannerToItemRecipe(ItemID.CorruptPenguinBanner, ItemID.PedguinShirt);
            AddBannerToItemRecipe(ItemID.CorruptPenguinBanner, ItemID.PedguinPants);
            AddBannerToItemRecipe(ItemID.CrimsonPenguinBanner, ItemID.PedguinHat);
            AddBannerToItemRecipe(ItemID.CrimsonPenguinBanner, ItemID.PedguinShirt);
            AddBannerToItemRecipe(ItemID.CrimsonPenguinBanner, ItemID.PedguinPants);
            AddBannerToItemRecipe(ItemID.RaincoatZombieBanner, ItemID.RainCoat);
            AddBannerToItemRecipe(ItemID.RaincoatZombieBanner, ItemID.RainHat);
            AddBannerToItemRecipe(ItemID.ZombieEskimoBanner, ItemID.EskimoHood);
            AddBannerToItemRecipe(ItemID.ZombieEskimoBanner, ItemID.EskimoCoat);
            AddBannerToItemRecipe(ItemID.ZombieEskimoBanner, ItemID.EskimoPants);

            AddBannerToItemRecipe(ItemID.AnglerFishBanner, ItemID.RobotHat, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertDjinnBanner, ItemID.DjinnsCurse, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertLamiaBanner, ItemID.LamiaHat, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertLamiaBanner, ItemID.LamiaShirt, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertLamiaBanner, ItemID.LamiaPants, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertLamiaBanner, ItemID.MoonMask, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertLamiaBanner, ItemID.SunMask, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MummyBanner, ItemID.MummyMask, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MummyBanner, ItemID.MummyShirt, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MummyBanner, ItemID.MummyPants, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BloodMummyBanner, ItemID.MummyMask, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BloodMummyBanner, ItemID.MummyShirt, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BloodMummyBanner, ItemID.MummyPants, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DarkMummyBanner, ItemID.MummyMask, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DarkMummyBanner, ItemID.MummyShirt, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DarkMummyBanner, ItemID.MummyPants, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.LightMummyBanner, ItemID.MummyMask, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.LightMummyBanner, ItemID.MummyShirt, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.LightMummyBanner, ItemID.MummyPants, conditions: Condition.Hardmode);

            AddBannerGroupToItemRecipe(AnyPirateBanner, ItemID.SailorHat, conditions: Condition.DownedPirates);
            AddBannerGroupToItemRecipe(AnyPirateBanner, ItemID.SailorShirt, conditions: Condition.DownedPirates);
            AddBannerGroupToItemRecipe(AnyPirateBanner, ItemID.SailorPants, conditions: Condition.DownedPirates);

            AddBannerToItemRecipe(ItemID.TacticalSkeletonBanner, ItemID.SWATHelmet, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.ZombieElfBanner, ItemID.ElfHat, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.ZombieElfBanner, ItemID.ElfShirt, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.ZombieElfBanner, ItemID.ElfPants, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.ButcherBanner, ItemID.ButcherMask, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.ButcherBanner, ItemID.ButcherApron, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.ButcherBanner, ItemID.ButcherPants, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.DrManFlyBanner, ItemID.DrManFlyMask, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.DrManFlyBanner, ItemID.DrManFlyLabCoat, conditions: Condition.DownedPlantera);
        }

        private static void AddBannerToCritterRecipes()
        {
            AddBannerToItemRecipe(ItemID.BirdBanner, ItemID.Bird, resultAmount: 100);
            AddBannerToItemRecipe(ItemID.BirdBanner, ItemID.BlueJay, resultAmount: 100);
            AddBannerToItemRecipe(ItemID.BirdBanner, ItemID.Cardinal, resultAmount: 100);
            AddBannerToItemRecipe(ItemID.BunnyBanner, ItemID.Bunny, resultAmount: 100);
            AddBannerToItemRecipe(ItemID.GoldfishBanner, ItemID.Goldfish, resultAmount: 100);
            AddBannerToItemRecipe(ItemID.PenguinBanner, ItemID.Penguin, resultAmount: 100);
        }

        private static void AddBannerToFoodRecipes()
        {
            // Well Fed
            AddBannerToItemRecipe(ItemID.SalamanderBanner, ItemID.PotatoChips);
            AddBannerToItemRecipe(ItemID.CrawdadBanner, ItemID.PotatoChips);
            AddBannerToItemRecipe(ItemID.GiantShellyBanner, ItemID.PotatoChips);
            AddBannerGroupToItemRecipe(AnyPureSkeletonBanner, ItemID.MilkCarton);

            // Plenty Satisfied
            AddBannerToItemRecipe(ItemID.FlyingFishBanner, ItemID.Fries);
            AddBannerToItemRecipe(ItemID.HarpyBanner, ItemID.ChickenNugget);
            AddBannerToItemRecipe(ItemID.AntlionBanner, ItemID.BananaSplit);
            AddBannerToItemRecipe(ItemID.FlyingAntlionBanner, ItemID.BananaSplit);
            AddBannerToItemRecipe(ItemID.WalkingAntlionBanner, ItemID.BananaSplit);
            AddBannerToItemRecipe(ItemID.CursedSkullBanner, ItemID.CreamSoda);
            AddBannerToItemRecipe(ItemID.IceSlimeBanner, ItemID.IceCream);
            AddBannerToItemRecipe(ItemID.IceBatBanner, ItemID.IceCream);
            AddBannerToItemRecipe(ItemID.SpikedIceSlimeBanner, ItemID.IceCream);
            AddBannerToItemRecipe(ItemID.ManEaterBanner, ItemID.CoffeeCup);
            AddBannerToItemRecipe(ItemID.SnatcherBanner, ItemID.CoffeeCup);
            AddBannerToItemRecipe(ItemID.TumbleweedBanner, ItemID.Nachos);
            AddBannerToItemRecipe(ItemID.SharkBanner, ItemID.ShrimpPoBoy);
            AddBannerToItemRecipe(ItemID.CrabBanner, ItemID.ShrimpPoBoy);
            AddBannerToItemRecipe(ItemID.SpiderBanner, ItemID.FriedEgg);

            AddBannerToItemRecipe(ItemID.GastropodBanner, ItemID.ChocolateChipCookie, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.GiantFlyingFoxBanner, ItemID.Grapes, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DerplingBanner, ItemID.Grapes, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.AngryTrapperBanner, ItemID.CoffeeCup, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SandsharkBanner, ItemID.Nachos, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SandsharkCorruptBanner, ItemID.Nachos, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SandsharkCrimsonBanner, ItemID.Nachos, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SandsharkHallowedBanner, ItemID.Nachos, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BlackRecluseBanner, ItemID.FriedEgg, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.RavagerScorpionBanner, ItemID.FriedEgg, conditions: Condition.Hardmode);

            AddBannerToItemRecipe(ItemID.BoneLeeBanner, ItemID.CoffeeCup, resultAmount: 5, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.GiantCursedSkullBanner, ItemID.CreamSoda, conditions: Condition.DownedPlantera);

            // Exquisitely Stuffed
            AddBannerToItemRecipe(ItemID.GreekSkeletonBanner, ItemID.Pizza);
            AddBannerToItemRecipe(ItemID.MedusaBanner, ItemID.Pizza, resultAmount: 5);
            AddBannerToItemRecipe(ItemID.UndeadMinerBanner, ItemID.Steak, resultAmount: 5);
            AddBannerToItemRecipe(ItemID.BoneSerpentBanner, ItemID.Hotdog);

            AddBannerToItemRecipe(ItemID.PigronBanner, ItemID.Bacon, resultAmount: 2, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.ChaosElementalBanner, ItemID.ApplePie, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.IlluminantSlimeBanner, ItemID.ApplePie, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.IlluminantBatBanner, ItemID.ApplePie, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.EaterofSoulsBanner, ItemID.Burger);
            AddBannerToItemRecipe(ItemID.GraniteGolemBanner, ItemID.Spaghetti);
            AddBannerToItemRecipe(ItemID.GraniteFlyerBanner, ItemID.Spaghetti);
            AddBannerToItemRecipe(ItemID.IcyMermanBanner, ItemID.Milkshake, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.IceTortoiseBanner, ItemID.Milkshake, conditions: Condition.Hardmode);

            AddBannerToItemRecipe(ItemID.RedDevilBanner, ItemID.Hotdog, conditions: Condition.DownedMechBossAny);
            AddBannerToItemRecipe(ItemID.ThePossessedBanner, ItemID.Steak, resultAmount: 2, conditions: Condition.DownedMechBossAny);

            AddBannerToItemRecipe(ItemID.SkeletonCommandoBanner, ItemID.BBQRibs, resultAmount: 2, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.SkeletonSniperBanner, ItemID.BBQRibs, resultAmount: 2, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.TacticalSkeletonBanner, ItemID.BBQRibs, resultAmount: 2, conditions: Condition.DownedPlantera);
        }

        private static void AddBannerToFurnitureRecipes()
        {
            AddBannerGroupToItemRecipe(AnyCorruptBanner, ItemID.MeatGrinder, groupAmount: 5, conditions: Condition.Hardmode);
            AddBannerGroupToItemRecipe(AnyCrimsonBanner, ItemID.MeatGrinder, groupAmount: 5, conditions: Condition.Hardmode);
        }

        private static void AddBannerToMaterialRecipes()
        {
            AddBannerToItemRecipe(ItemID.AngryBonesBanner, ItemID.Bone, resultAmount: 100, conditions: Condition.DownedSkeletron);
            AddBannerToItemRecipe(ItemID.CursedSkullBanner, ItemID.Bone, resultAmount: 100, conditions: Condition.DownedSkeletron);
            AddBannerToItemRecipe(ItemID.SkeletonMageBanner, ItemID.Bone, resultAmount: 100, conditions: Condition.DownedSkeletron);
            //AddBannerToItemRecipe(ItemID.SkeletonBanner, ItemID.Bone, resultAmount: 100, conditions: Condition.DownedSkeletron);
            AddBannerToItemRecipe(ItemID.DemonEyeBanner, ItemID.BlackLens);
            AddBannerToItemRecipe(ItemID.MeteorHeadBanner, ItemID.Meteorite, resultAmount: 25);
            AddBannerGroupToItemRecipe(AnySlimesBanner, ItemID.Gel, resultAmount: 200);

            AddBannerToItemRecipe(ItemID.WanderingEyeBanner, ItemID.BlackLens, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertGhoulBanner, ItemID.DarkShard, resultAmount: 5, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BloodMummyBanner, ItemID.DarkShard, resultAmount: 5, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DarkMummyBanner, ItemID.DarkShard, resultAmount: 5, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SandsharkCorruptBanner, ItemID.DarkShard, resultAmount: 5, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SandsharkCrimsonBanner, ItemID.DarkShard, resultAmount: 5, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.HarpyBanner, ItemID.GiantHarpyFeather, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertGhoulBanner, ItemID.LightShard, resultAmount: 5, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.LightMummyBanner, ItemID.LightShard, resultAmount: 5, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SandsharkHallowedBanner, ItemID.LightShard, resultAmount: 5, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.PixieBanner, ItemID.PixieDust, resultAmount: 100, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.TortoiseBanner, ItemID.TurtleShell, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.DesertDjinnBanner, ItemID.DjinnLamp, conditions: Condition.Hardmode);
            AddBannerGroupToItemRecipe(AnyArmoredBonesBanner, ItemID.BoneFeather, conditions: Condition.Hardmode);

            AddBannerToItemRecipe(ItemID.MossHornetBanner, ItemID.TatteredBeeWing, conditions: Condition.DownedMechBossAny);
            AddBannerToItemRecipe(ItemID.MothBanner, ItemID.ButterflyDust, conditions: Condition.DownedMechBossAny);
            AddBannerToItemRecipe(ItemID.RedDevilBanner, ItemID.FireFeather, conditions: Condition.DownedMechBossAny);
            AddBannerToItemRecipe(ItemID.VampireBanner, ItemID.BrokenBatWing, conditions: Condition.DownedMechBossAny);

            AddBannerToItemRecipe(ItemID.DungeonSpiritBanner, ItemID.Ectoplasm, resultAmount: 50, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.MothronBanner, ItemID.BrokenHeroSword, conditions: Condition.DownedPlantera);
        }

        private static void AddBannerToMiscItemRecipes()
        {
            AddBannerToItemRecipe(ItemID.DripplerBanner, ItemID.MoneyTrough);
            AddBannerToItemRecipe(ItemID.FlyingFishBanner, ItemID.CarbonGuitar);
            AddBannerToItemRecipe(ItemID.SkeletonMageBanner, ItemID.BoneWand);
            AddBannerToItemRecipe(ItemID.EyeballFlyingFishBanner, ItemID.BloodFishingRod);
            AddBannerToItemRecipe(ItemID.WormBanner, ItemID.WhoopieCushion);
            AddBannerToItemRecipe(ItemID.ZombieMermanBanner, ItemID.BloodFishingRod);

            AddBannerToItemRecipe(ItemID.BloodNautilusBanner, ItemID.BloodMoonMonolith, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.ChaosElementalBanner, ItemID.RodofDiscord, bannerAmount: 4, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.DualHook, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.UnicornBanner, ItemID.UnicornonaStick, conditions: Condition.Hardmode);

            // Biome Keys
            AddBannerGroupToItemRecipe(AnyCorruptBanner, ItemID.CorruptionKey, groupAmount: 10, conditions: Condition.Hardmode);
            AddBannerGroupToItemRecipe(AnyCrimsonBanner, ItemID.CrimsonKey, groupAmount: 10, conditions: Condition.Hardmode);
            AddBannerGroupToItemRecipe(AnyDesertBanner, ItemID.DungeonDesertKey, groupAmount: 10, conditions: Condition.Hardmode);
            AddBannerGroupToItemRecipe(AnyHallowBanner, ItemID.HallowedKey, groupAmount: 10, conditions: Condition.Hardmode);
            AddBannerGroupToItemRecipe(AnyJungleBanner, ItemID.JungleKey, groupAmount: 10, conditions: Condition.Hardmode);
            AddBannerGroupToItemRecipe(AnySnowBanner, ItemID.FrozenKey, groupAmount: 10, conditions: Condition.Hardmode);

            // Kites
            AddBannerToItemRecipe(ItemID.BoneSerpentBanner, ItemID.KiteBoneSerpent);
            AddBannerToItemRecipe(ItemID.BunnyBanner, ItemID.KiteBunny);
            AddBannerToItemRecipe(ItemID.CorruptBunnyBanner, ItemID.KiteBunnyCorrupt);
            AddBannerToItemRecipe(ItemID.CrimsonBunnyBanner, ItemID.KiteBunnyCrimson);
            AddBannerToItemRecipe(ItemID.GoldfishBanner, ItemID.KiteGoldfish);
            AddBannerToItemRecipe(ItemID.JellyfishBanner, ItemID.KiteJellyfishBlue);
            AddBannerToItemRecipe(ItemID.ManEaterBanner, ItemID.KiteManEater);
            AddBannerToItemRecipe(ItemID.PinkJellyfishBanner, ItemID.KiteJellyfishPink);
            AddBannerToItemRecipe(ItemID.RedSlimeBanner, ItemID.KiteRed);
            AddBannerToItemRecipe(ItemID.SharkBanner, ItemID.KiteShark);
            AddBannerToItemRecipe(ItemID.SlimeBanner, ItemID.KiteBlue);
            AddBannerToItemRecipe(ItemID.YellowSlimeBanner, ItemID.KiteYellow);

            AddBannerToItemRecipe(ItemID.AngryTrapperBanner, ItemID.KiteAngryTrapper, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.PigronBanner, ItemID.KitePigron, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SandsharkBanner, ItemID.KiteSandShark, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.UnicornBanner, ItemID.KiteUnicorn, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.WanderingEyeBanner, ItemID.KiteWanderingEye, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.WorldFeederBanner, ItemID.KiteWorldFeeder, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.WyvernBanner, ItemID.KiteWyvern, conditions: Condition.Hardmode);
        }

        private static void AddBannerToMountOrPetRecipes()
        {
            AddBannerToItemRecipe(ItemID.DesertBasiliskBanner, ItemID.AncientHorn, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.UnicornBanner, ItemID.BlessedApple, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.ToySled, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.PigronBanner, ItemID.PigronMinecart, conditions: Condition.Hardmode);

            AddBannerToItemRecipe(ItemID.EyezorBanner, ItemID.EyeSpring, conditions: Condition.DownedMechBossAny);

            AddBannerGroupToItemRecipe(AnyArmoredBonesBanner, ItemID.WispinaBottle, conditions: Condition.DownedPlantera);

            AddBannerToItemRecipe(ItemID.LihzahrdBanner, ItemID.LizardEgg, conditions: Condition.DownedGolem);

            AddBannerToItemRecipe(ItemID.MartianScutlixGunnerBanner, ItemID.BrainScrambler, conditions: Condition.DownedMartians);
            AddBannerToItemRecipe(ItemID.ScutlixBanner, ItemID.BrainScrambler, conditions: Condition.DownedMartians);
        }

        private static void AddBannerToWeaponRecipes()
        {
            AddBannerGroupToItemRecipe(AnyBatBanner, ItemID.BatBat);
            AddBannerToItemRecipe(ItemID.BatBanner, ItemID.ChainKnife, conditions: Condition.NotRemixWorld);
            AddBannerToItemRecipe(ItemID.BloodCrawlerBanner, ItemID.TentacleSpike, bannerAmount: 2);
            AddBannerToItemRecipe(ItemID.CrawdadBanner, ItemID.Rally);
            AddBannerToItemRecipe(ItemID.CrimeraBanner, ItemID.TentacleSpike, bannerAmount: 2);
            AddBannerToItemRecipe(ItemID.DemonBanner, ItemID.DemonScythe);
            AddBannerToItemRecipe(ItemID.EaterofSoulsBanner, ItemID.TentacleSpike, bannerAmount: 2);
            AddBannerToItemRecipe(ItemID.EyeballFlyingFishBanner, ItemID.BloodRainBow);
            AddBannerToItemRecipe(ItemID.EyeballFlyingFishBanner, ItemID.VampireFrogStaff);
            AddBannerToItemRecipe(ItemID.FaceMonsterBanner, ItemID.TentacleSpike, bannerAmount: 2);
            AddBannerToItemRecipe(ItemID.GiantShellyBanner, ItemID.Rally);
            AddBannerToItemRecipe(ItemID.GoblinArcherBanner, ItemID.Harpoon);
            AddBannerToItemRecipe(ItemID.GreekSkeletonBanner, ItemID.Gladius);
            AddBannerToItemRecipe(ItemID.SalamanderBanner, ItemID.Rally);
            AddBannerToItemRecipe(ItemID.SkeletonBanner, ItemID.BoneSword);
            AddBannerToItemRecipe(ItemID.SnowFlinxBanner, ItemID.SnowballLauncher);
            AddBannerToItemRecipe(ItemID.SporeBatBanner, ItemID.Shroomerang);
            AddBannerToItemRecipe(ItemID.UndeadMinerBanner, ItemID.BonePickaxe);
            AddBannerToItemRecipe(ItemID.WalkingAntlionBanner, ItemID.AntlionClaw);
            AddBannerToItemRecipe(ItemID.ZombieBanner, ItemID.ZombieArm);
            AddBannerToItemRecipe(ItemID.ZombieMermanBanner, ItemID.BloodRainBow);
            AddBannerToItemRecipe(ItemID.ZombieMermanBanner, ItemID.VampireFrogStaff);

            AddBannerToItemRecipe(ItemID.LavaSlimeBanner, ItemID.Cascade, conditions: Condition.DownedSkeletron);

            AddBannerGroupToItemRecipe(AnySnowBanner, ItemID.Amarok, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.AngryNimbusBanner, ItemID.NimbusRod, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.AngryTrapperBanner, ItemID.Uzi, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.ArmoredSkeletonBanner, ItemID.BeamSword, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.ArmoredVikingBanner, ItemID.IceSickle, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BlackRecluseBanner, ItemID.PoisonStaff, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.BloodZombieBanner, ItemID.KOCannon, bannerAmount: 4, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.ClownBanner, ItemID.KOCannon, bannerAmount: 4, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.GiantBatBanner, ItemID.ChainKnife, conditions: [Condition.Hardmode, Condition.NotRemixWorld]);
            AddBannerToItemRecipe(ItemID.IcyMermanBanner, ItemID.FrostStaff, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MedusaBanner, ItemID.MedusaHead, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.FlowerofFrost, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.Frostbrand, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.IceBow, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.MimicBanner, ItemID.MagicDagger, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.PigronBanner, ItemID.HamBat, conditions: Condition.Hardmode);
            AddBannerToItemRecipe(ItemID.SkeletonArcherBanner, ItemID.Marrow, conditions: Condition.Hardmode);

            AddBannerGroupToItemRecipe(AnyPirateBanner, ItemID.Cutlass, conditions: Condition.DownedPirates);
            AddBannerGroupToItemRecipe(AnyPirateBanner, ItemID.PirateStaff, conditions: Condition.DownedPirates);
            AddBannerToItemRecipe(ItemID.PirateCaptainBanner, ItemID.CoinGun, conditions: Condition.DownedPirates);

            //AddBannerToItemRecipe(ItemID.EnchantedSwordBanner, ItemID.Smolstar, conditions: Condition.DownedQueenSlime);

            AddBannerGroupToItemRecipe(AnyJungleBanner, ItemID.Yelets, conditions: Condition.DownedMechBossAny);
            AddBannerToItemRecipe(ItemID.LavaBatBanner, ItemID.HelFire, conditions: Condition.DownedMechBossAny);
            AddBannerToItemRecipe(ItemID.RedDevilBanner, ItemID.UnholyTrident, conditions: Condition.DownedMechBossAny);

            AddBannerToItemRecipe(ItemID.ReaperBanner, ItemID.DeathSickle, conditions: Condition.DownedMechBossAll);

            AddBannerGroupToItemRecipe(AnyArmoredBonesBanner, ItemID.Keybrand, conditions: Condition.DownedPlantera);
            AddBannerGroupToItemRecipe(AnyArmoredBonesBanner, ItemID.Kraken, conditions: Condition.DownedPlantera);
            AddBannerGroupToItemRecipe(AnyArmoredBonesBanner, ItemID.MaceWhip, conditions: Condition.DownedPlantera);
            AddBannerGroupToItemRecipe(AnyArmoredBonesBanner, ItemID.MagnetSphere, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.ButcherBanner, ItemID.ButchersChainsaw, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.DeadlySphereBanner, ItemID.DeadlySphereStaff, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.DiablolistBanner, ItemID.InfernoFork, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.DrManFlyBanner, ItemID.ToxicFlask, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.GiantCursedSkullBanner, ItemID.ShadowJoustingLance, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.MothronBanner, ItemID.TheEyeOfCthulhu, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.NailheadBanner, ItemID.NailGun, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.NecromancerBanner, ItemID.ShadowbeamStaff, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.PaladinBanner, ItemID.PaladinsHammer, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.PsychoBanner, ItemID.PsychoKnife, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.RaggedCasterBanner, ItemID.SpectreStaff, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.SkeletonCommandoBanner, ItemID.RocketLauncher, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.SkeletonSniperBanner, ItemID.SniperRifle, conditions: Condition.DownedPlantera);
            AddBannerToItemRecipe(ItemID.TacticalSkeletonBanner, ItemID.TacticalShotgun, conditions: Condition.DownedPlantera);
        }

        private static void AddBannerGroupToItemRecipe(int recipeGroupID, int resultID, int resultAmount = 1, int groupAmount = 1, params Condition[] conditions)
        {
            RecipeHelper.CreateSimpleRecipe(recipeGroupID, resultID, TileID.Solidifier, groupAmount, resultAmount, true, true, conditions);
        }

        private static void AddBannerToItemRecipe(int bannerItemID, int resultID, int bannerAmount = 1, int resultAmount = 1, params Condition[] conditions)
        {
            RecipeHelper.CreateSimpleRecipe(bannerItemID, resultID, TileID.Solidifier, bannerAmount, resultAmount, true, conditions: conditions);
        }

        private static string ItemXOrY(int id1, int id2) => $"{Lang.GetItemName(id1)} {Language.GetTextValue($"Mods.Fargowiltas.RecipeGroups.Or")} {Lang.GetItemName(id2)}";

        private static void AddBannerSetToItemRecipe(bool[] set, int resultID)
        {
            for (int i = 0; i < NPCID.Count; i++)
            {
                if (set[i])
                {
                    int bannerId = Item.NPCtoBanner(i);
                    if (bannerId > 0)
                    {
                        RecipeHelper.CreateSimpleRecipe(Item.BannerToItem(bannerId), resultID, TileID.Solidifier, disableDecraft: true);
                    }
                }
            }
        }
    }
}
