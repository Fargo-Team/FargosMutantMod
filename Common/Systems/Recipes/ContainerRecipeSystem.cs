using Fargowiltas.Common.Configs;
using Fargowiltas.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes
{
    public class ContainerRecipeSystem : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return FargoServerConfig.Instance.ContainerRecipes;
        }
        public override void AddRecipes()
        {
            AddPreHMTreasureBagRecipes();
            AddHMTreasureBagRecipes();
            AddEventTreasureBagRecipes();
            AddGrabBagRecipes();
            AddCrateRecipes();
            AddBiomeKeyRecipes();

            // Treasure magnet I HATE TREASURE MAGNET TEAR OFF ALL OF YOUR FLESH
            /*CreateTreasureGroupRecipe(ItemID.TreasureMagnet,
                ItemID.DarkLance,
                ItemID.HellwingBow,
                ItemID.Flamelash,
                ItemID.Sunfury
            );
            RecipeHelper.CreateSimpleRecipe(ItemID.TreasureMagnet, ItemID.FlowerofFire, TileID.Solidifier, ingredientAmount: 2, disableDecraft: true, conditions: Condition.NotRemixWorld);*/
        }

        private static void AddPreHMTreasureBagRecipes()
        {
            //CreateTreasureGroupRecipe(ItemID.KingSlimeTrophy, ItemID.SlimeStaff);
            CreateTreasureGroupRecipe(ItemID.EyeofCthulhuTrophy, ItemID.Binoculars);
            CreateTreasureGroupRecipe(ItemID.EaterofWorldsTrophy, ItemID.EatersBone);
            CreateTreasureGroupRecipe(ItemID.BrainofCthulhuTrophy, ItemID.BoneRattle);
            CreateTreasureGroupRecipe(ItemID.SkeletronTrophy, ItemID.BookofSkulls);

            // Queen Bee
            CreateTreasureGroupRecipe(ItemID.QueenBeeBossBag,
                ItemID.BeesKnees,
                ItemID.BeeGun,
                ItemID.BeeKeeper,
                ItemID.HoneyComb
            );
            CreateTreasureGroupRecipe(ItemID.QueenBeeTrophy,
                ItemID.HoneyedGoggles,
                ItemID.Nectar
            );

            // Deerclops
            CreateTreasureGroupRecipe(ItemID.DeerclopsBossBag,
                ItemID.PewMaticHorn,
                ItemID.WeatherPain,
                ItemID.HoundiusShootius,
                ItemID.LucyTheAxe
            );
            CreateTreasureGroupRecipe(ItemID.DeerclopsTrophy,
                ItemID.ChesterPetItem,
                ItemID.Eyebrella,
                ItemID.DontStarveShaderItem
            );

            // Wall of Flesh
            CreateTreasureGroupRecipe(ItemID.WallOfFleshBossBag,
                ItemID.ClockworkAssaultRifle,
                ItemID.BreakerBlade,
                ItemID.LaserRifle,
                ItemID.FireWhip
            );
        }

        private static void AddHMTreasureBagRecipes()
        {
            // Queen Slime
            CreateTreasureGroupRecipe(ItemID.QueenSlimeBossBag,
                ItemID.Smolstar,
                ItemID.QueenSlimeMountSaddle,
                ItemID.QueenSlimeHook
            );

            // Plantera
            CreateTreasureGroupRecipe(ItemID.PlanteraBossBag,
                ItemID.GrenadeLauncher,
                ItemID.PygmyStaff,
                ItemID.VenusMagnum,
                ItemID.NettleBurst,
                ItemID.LeafBlower,
                ItemID.Seedler,
                ItemID.FlowerPow,
                ItemID.WaspGun
            );
            CreateTreasureGroupRecipe(ItemID.PlanteraTrophy,
                ItemID.TheAxe,
                ItemID.Seedling
            );

            // Golem
            CreateTreasureGroupRecipe(ItemID.GolemBossBag,
                ItemID.Stynger,
                ItemID.PossessedHatchet,
                ItemID.SunStone,
                ItemID.EyeoftheGolem,
                ItemID.Picksaw,
                ItemID.HeatRay,
                ItemID.StaffofEarth,
                ItemID.GolemFist
            );

            // Duke Fishron
            CreateTreasureGroupRecipe(ItemID.FishronBossBag,
                ItemID.Flairon,
                ItemID.Tsunami,
                ItemID.RazorbladeTyphoon,
                ItemID.TempestStaff,
                ItemID.BubbleGun
            );
            CreateTreasureGroupRecipe(ItemID.DukeFishronTrophy, ItemID.FishronWings);

            // Empress of Light
            CreateTreasureGroupRecipe(ItemID.FairyQueenBossBag,
                ItemID.PiercingStarlight,
                ItemID.RainbowWhip,
                ItemID.FairyQueenMagicItem,
                ItemID.FairyQueenRangedItem,
                ItemID.HallowBossDye
            );
            CreateTreasureGroupRecipe(ItemID.FairyQueenTrophy,
                ItemID.SparkleGuitar,
                ItemID.RainbowCursor,
                ItemID.RainbowWings
            );

            // Moon Lord
            CreateTreasureGroupRecipe(ItemID.MoonLordBossBag,
                ItemID.Meowmere,
                ItemID.Terrarian,
                ItemID.StarWrath,
                ItemID.SDMG,
                ItemID.Celeb2,
                ItemID.LastPrism,
                ItemID.LunarFlareBook,
                ItemID.RainbowCrystalStaff,
                ItemID.MoonlordTurretStaff
            );
            CreateTreasureGroupRecipe(ItemID.MoonLordTrophy, ItemID.MeowmereMinecart);
        }

        private static void AddEventTreasureBagRecipes()
        {
            // Dark Mage
            CreateTreasureGroupRecipe(ItemID.BossTrophyDarkmage,
                ItemID.DD2PetDragon,
                ItemID.DD2PetGato,
                ItemID.SquireShield,
                ItemID.ApprenticeScarf
            );

            // Ogre
            CreateTreasureGroupRecipe(ItemID.BossTrophyOgre,
                ItemID.HuntressBuckler,
                ItemID.MonkBelt,
                ItemID.DD2PhoenixBow,
                ItemID.BookStaff,
                ItemID.DD2SquireDemonSword,
                ItemID.MonkStaffT1,
                ItemID.MonkStaffT2,
                ItemID.DD2PetGhost
            );

            // Betsy
            CreateTreasureGroupRecipe(ItemID.BossBagBetsy,
                ItemID.BetsyWings,
                ItemID.DD2SquireBetsySword,
                ItemID.ApprenticeStaffT3,
                ItemID.MonkStaffT3,
                ItemID.DD2BetsyBow
            );

            // Mourning Wood
            CreateTreasureGroupRecipe(ItemID.MourningWoodTrophy,
                ItemID.SpookyHook,
                ItemID.SpookyTwig,
                ItemID.StakeLauncher,
                ItemID.CursedSapling,
                ItemID.NecromanticScroll
            );

            // Pumpking
            CreateTreasureGroupRecipe(ItemID.PumpkingTrophy,
                ItemID.TheHorsemansBlade,
                ItemID.BatScepter,
                ItemID.RavenStaff,
                ItemID.CandyCornRifle,
                ItemID.JackOLanternLauncher,
                ItemID.BlackFairyDust,
                ItemID.ScytheWhip
            );

            // Everscream
            CreateTreasureGroupRecipe(ItemID.EverscreamTrophy,
                ItemID.ChristmasTreeSword,
                ItemID.ChristmasHook,
                ItemID.Razorpine,
                ItemID.FestiveWings
            );

            // Santa NK1
            CreateTreasureGroupRecipe(ItemID.SantaNK1Trophy,
                ItemID.ElfMelter,
                ItemID.ChainGun
            );

            // Ice Queen
            CreateTreasureGroupRecipe(ItemID.IceQueenTrophy,
                ItemID.BlizzardStaff,
                ItemID.SnowmanCannon,
                ItemID.NorthPole,
                ItemID.BabyGrinchMischiefWhistle,
                ItemID.ReindeerBells
            );

            // Saucer
            CreateTreasureGroupRecipe(ItemID.MartianSaucerTrophy,
                ItemID.Xenopopper,
                ItemID.XenoStaff,
                ItemID.LaserMachinegun,
                ItemID.ElectrosphereLauncher,
                ItemID.InfluxWaver,
                ItemID.CosmicCarKey,
                ItemID.AntiGravityHook,
                ItemID.ChargedBlasterCannon,
                ItemID.LaserDrill
            );

            // Flying Dutchman
            CreateTreasureGroupRecipe(ItemID.FlyingDutchmanTrophy,
                ItemID.LuckyCoin,
                ItemID.DiscountCard,
                ItemID.CoinGun,
                ItemID.PirateStaff,
                ItemID.GoldRing,
                ItemID.Cutlass,
                ItemID.PirateMinecart
            );
        }

        private static void AddBiomeKeyRecipes()
        {
            CreateBiomeKeyRecipe(ItemID.CrimsonKey, ItemID.VampireKnives, TileID.MythrilAnvil, disableDecraft: true);
            CreateBiomeKeyRecipe(ItemID.CorruptionKey, ItemID.ScourgeoftheCorruptor, TileID.MythrilAnvil, disableDecraft: true);
            CreateBiomeKeyRecipe(ItemID.JungleKey, ItemID.PiranhaGun, TileID.MythrilAnvil, disableDecraft: true);
            CreateBiomeKeyRecipe(ItemID.FrozenKey, ItemID.StaffoftheFrostHydra, TileID.MythrilAnvil, disableDecraft: true);
            CreateBiomeKeyRecipe(ItemID.HallowedKey, ItemID.RainbowGun, TileID.MythrilAnvil, disableDecraft: true);
            CreateBiomeKeyRecipe(ItemID.DungeonDesertKey, ItemID.StormTigerStaff, TileID.MythrilAnvil, disableDecraft: true);
        }

        private static void AddGrabBagRecipes()
        {
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.DogWhistle, TileID.Solidifier, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.Toolbox, TileID.Solidifier, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.HandWarmer, TileID.Solidifier, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.RedRyder, TileID.Solidifier, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.CandyCaneSword, TileID.Solidifier, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.CandyCaneHook, TileID.Solidifier, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.FruitcakeChakram, TileID.Solidifier, ingredientAmount: 10, disableDecraft: true);

            RecipeHelper.CreateSimpleRecipe(ItemID.GoodieBag, ItemID.UnluckyYarn, TileID.Solidifier, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.GoodieBag, ItemID.BatHook, TileID.Solidifier, ingredientAmount: 25, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.GoodieBag, ItemID.RottenEgg, TileID.Solidifier, ingredientAmount: 2, resultAmount: 25, disableDecraft: true);

            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Daybloom, TileID.Solidifier, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Moonglow, TileID.Solidifier, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Blinkroot, TileID.Solidifier, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Waterleaf, TileID.Solidifier, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Deathweed, TileID.Solidifier, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Fireblossom, TileID.Solidifier, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Shiverthorn, TileID.Solidifier, resultAmount: 5, disableDecraft: true);
        }

        private static void AddCrateRecipes()
        {
            //wooden
            CreateCrateRecipe(ItemID.SailfishBoots, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.TsunamiInABottle, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Extractinator, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Aglet, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.CordageGuide, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Umbrella, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.ClimbingClaws, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Radar, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.WoodenBoomerang, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.WandofSparking, FargoRecipeGroups.AnyWoodCrate, 5, conditions: Condition.NotRemixWorld);
            CreateCrateRecipe(ItemID.Spear, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Blowpipe, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.PortableStool, FargoRecipeGroups.AnyWoodCrate, 5);
            //CreateCrateRecipe(ItemID.BabyBirdStaff, ItemID.WoodenCrate, 5, ItemID.WoodenCrateHard);
            CreateCrateRecipe(ItemID.SunflowerMinecart, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.LadybugMinecart, FargoRecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Anchor, null, 5, ItemID.WoodenCrateHard);

            //iron
            CreateCrateRecipe(ItemID.FalconBlade, FargoRecipeGroups.AnyIronCrate, 5);
            CreateCrateRecipe(ItemID.TartarSauce, FargoRecipeGroups.AnyIronCrate, 5);
            CreateCrateRecipe(ItemID.GingerBeard, FargoRecipeGroups.AnyIronCrate, 5);

            //gold
            CreateCrateRecipe(ItemID.BandofRegeneration, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.MagicMirror, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.FlareGun, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.HermesBoots, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.ShoeSpikes, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.Mace, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.CloudinaBottle, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.LifeCrystal, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.HardySaddle, FargoRecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.EnchantedSword, FargoRecipeGroups.AnyGoldCrate, 2);

            CreateCrateRecipe(ItemID.Sundial, FargoRecipeGroups.AnyGoldCrate, 2); //actually should be hm but fuck it

            //jungle
            CreateCrateRecipe(ItemID.AnkletoftheWind, FargoRecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.Boomstick, FargoRecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.FeralClaws, FargoRecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.StaffofRegrowth, FargoRecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.FiberglassFishingPole, FargoRecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.BeeMinecart, FargoRecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.Seaweed, FargoRecipeGroups.AnyJungleCrate, 5);
            CreateCrateRecipe(ItemID.FlowerBoots, FargoRecipeGroups.AnyJungleCrate, 5);
            CreateCrateRecipe(ItemID.HoneyDispenser, FargoRecipeGroups.AnyJungleCrate, 5);

            //sky
            CreateCrateRecipe(ItemID.ShinyRedBalloon, FargoRecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.Starfury, FargoRecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.CreativeWings, FargoRecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.SkyMill, FargoRecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.LuckyHorseshoe, FargoRecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.CelestialMagnet, FargoRecipeGroups.AnySkyCrate, 3);

            //corrupt
            CreateCrateRecipe(ItemID.BallOHurt, FargoRecipeGroups.AnyCorruptCrate, 3);
            CreateCrateRecipe(ItemID.BandofStarpower, FargoRecipeGroups.AnyCorruptCrate, 3);
            CreateCrateRecipe(ItemID.ShadowOrb, FargoRecipeGroups.AnyCorruptCrate, 3);
            CreateCrateRecipe(ItemID.Musket, FargoRecipeGroups.AnyCorruptCrate, 3);
            CreateCrateRecipe(ItemID.Vilethorn, FargoRecipeGroups.AnyCorruptCrate, 3);

            //crimson
            CreateCrateRecipe(ItemID.TheUndertaker, FargoRecipeGroups.AnyCrimsonCrate, 3);
            CreateCrateRecipe(ItemID.TheRottedFork, FargoRecipeGroups.AnyCrimsonCrate, 3);
            CreateCrateRecipe(ItemID.CrimsonRod, FargoRecipeGroups.AnyCrimsonCrate, 3);
            CreateCrateRecipe(ItemID.PanicNecklace, FargoRecipeGroups.AnyCrimsonCrate, 3);
            CreateCrateRecipe(ItemID.CrimsonHeart, FargoRecipeGroups.AnyCrimsonCrate, 3);

            //hallow

            //dungeon
            CreateCrateRecipe(ItemID.WaterBolt, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.Muramasa, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.CobaltShield, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.MagicMissile, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.AquaScepter, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey, conditions: Condition.NotRemixWorld);
            CreateCrateRecipe(ItemID.Valor, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.Handgun, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.ShadowKey, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.BlueMoon, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.BoneWelder, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.AlchemyTable, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.BewitchingTable, FargoRecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);

            //frozen crate
            CreateCrateRecipe(ItemID.SnowballCannon, FargoRecipeGroups.AnyFrozenCrate, 3, conditions: Condition.NotRemixWorld);
            CreateCrateRecipe(ItemID.BlizzardinaBottle, FargoRecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceBlade, FargoRecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceSkates, FargoRecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceMirror, FargoRecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.FlurryBoots, FargoRecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceBoomerang, FargoRecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceMachine, FargoRecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.Fish, FargoRecipeGroups.AnyFrozenCrate, 5);

            //oasis crate
            CreateCrateRecipe(ItemID.SandBoots, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.AncientChisel, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.ThunderSpear, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.ScarabFishingRod, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.ThunderStaff, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.CatBast, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.MagicConch, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.MysticCoilSnake, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.DesertMinecart, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.EncumberingStone, FargoRecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.FlyingCarpet, FargoRecipeGroups.AnySandCrate, 5);
            CreateCrateRecipe(ItemID.SandstorminaBottle, FargoRecipeGroups.AnySandCrate, 5);

            //obsidian
            CreateCrateRecipe(ItemID.DarkLance, FargoRecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);
            CreateCrateRecipe(ItemID.HellwingBow, FargoRecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);
            CreateCrateRecipe(ItemID.Flamelash, FargoRecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);
            CreateCrateRecipe(ItemID.FlowerofFire, FargoRecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey, conditions: Condition.NotRemixWorld);
            CreateCrateRecipe(ItemID.Sunfury, FargoRecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);
            CreateCrateRecipe(ItemID.TreasureMagnet, FargoRecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);

            CreateCrateRecipe(ItemID.LavaCharm, FargoRecipeGroups.AnyLavaCrate, 5);
            CreateCrateRecipe(ItemID.HellCake, FargoRecipeGroups.AnyLavaCrate, 5);
            CreateCrateRecipe(ItemID.OrnateShadowKey, FargoRecipeGroups.AnyLavaCrate, 5);
            CreateCrateRecipe(ItemID.SuperheatedBlood, FargoRecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.FlameWakerBoots, FargoRecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.LavaFishingHook, FargoRecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.HellMinecart, FargoRecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.WetBomb, FargoRecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.DemonConch, FargoRecipeGroups.AnyLavaCrate, 3);

            // ocean crate
            CreateCrateRecipe(ItemID.Trident, FargoRecipeGroups.AnyOceanCrate, 3);
            CreateCrateRecipe(ItemID.BreathingReed, FargoRecipeGroups.AnyOceanCrate, 3);
            CreateCrateRecipe(ItemID.Flipper, FargoRecipeGroups.AnyOceanCrate, 3);
            CreateCrateRecipe(ItemID.FloatingTube, FargoRecipeGroups.AnyOceanCrate, 3);
            CreateCrateRecipe(ItemID.WaterWalkingBoots, FargoRecipeGroups.AnyOceanCrate, 5);
            CreateCrateRecipe(ItemID.SharkBait, FargoRecipeGroups.AnyOceanCrate, 5);
        }

        private static void CreateCrateRecipe(int result, RecipeGroup crateGroup, int crateAmount, int hardmodeCrate = -1, int extraItem = -1, params Condition[] conditions)
        {
            if (crateGroup != null)
            {
                var recipe = Recipe.Create(result);
                recipe.AddRecipeGroup(crateGroup, crateAmount);
                if (extraItem != -1)
                {
                    recipe.AddIngredient(extraItem);
                }
                recipe.AddTile(TileID.WorkBenches);
                foreach (Condition condition in conditions)
                {
                    recipe.AddCondition(condition);
                }
                recipe.DisableDecraft();
                recipe.Register();
            }

            if (hardmodeCrate != -1)
            {
                var recipe = Recipe.Create(result);
                recipe.AddIngredient(hardmodeCrate, crateAmount);
                if (extraItem != -1)
                {
                    recipe.AddIngredient(extraItem);
                }
                recipe.AddTile(TileID.WorkBenches);
                foreach (Condition condition in conditions)
                {
                    recipe.AddCondition(condition);
                }
                recipe.DisableDecraft();
                recipe.Register();
            }
        }

        private static void CreateTreasureGroupRecipe(int input, params int[] outputs)
        {
            int amount = (ItemID.Sets.BossBag[input] || input == ItemID.TreasureMagnet) ? 2 : 1;
            foreach (int output in outputs)
            {
                RecipeHelper.CreateSimpleRecipe(input, output, TileID.Solidifier, ingredientAmount: amount, disableDecraft: true);
            }
        }

        public static void CreateBiomeKeyRecipe(int ingredientID, int resultID, int tileID, int ingredientAmount = 1, int resultAmount = 1, bool disableDecraft = false, bool usesRecipeGroup = false, params Condition[] conditions)
        {
            var recipe = Recipe.Create(resultID, resultAmount);
            if (usesRecipeGroup)
            {
                recipe.AddRecipeGroup(ingredientID, ingredientAmount);
            }
            else
            {
                recipe.AddIngredient(ingredientID, ingredientAmount);
            }
            recipe.AddIngredient(ItemID.Ectoplasm, 20);
            recipe.AddTile(tileID);

            foreach (Condition condition in conditions)
            {
                recipe.AddCondition(condition);
            }

            if (disableDecraft)
            {
                recipe.DisableDecraft();
            }

            recipe.Register();
        }
    }
}
