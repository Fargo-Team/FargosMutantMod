using Fargowiltas.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes
{
    public class ContainerRecipeSystem : ModSystem
    {
        public override void AddRecipes()
        {
            AddPreHMTreasureBagRecipes();
            AddHMTreasureBagRecipes();
            AddEventTreasureBagRecipes();
            AddGrabBagRecipes();
            AddCrateRecipes();
            AddBiomeKeyRecipes();

            // Treasure magnet I HATE TREASURE MAGNET TEAR OFF ALL OF YOUR FLESH
            CreateTreasureGroupRecipe(ItemID.TreasureMagnet,
                ItemID.DarkLance,
                ItemID.HellwingBow,
                ItemID.Flamelash,
                ItemID.Sunfury
            );
            RecipeHelper.CreateSimpleRecipe(ItemID.TreasureMagnet, ItemID.FlowerofFire, TileID.Solidifier, disableDecraft: true, conditions: Condition.NotRemixWorld);
        }

        private static void AddPreHMTreasureBagRecipes()
        {
            CreateTreasureGroupRecipe(ItemID.KingSlimeTrophy, ItemID.SlimeStaff);
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
            RecipeHelper.CreateSimpleRecipe(ItemID.CrimsonKey, ItemID.VampireKnives, TileID.MythrilAnvil, disableDecraft: true, conditions: Condition.DownedPlantera);
            RecipeHelper.CreateSimpleRecipe(ItemID.CorruptionKey, ItemID.ScourgeoftheCorruptor, TileID.MythrilAnvil, disableDecraft: true, conditions: Condition.DownedPlantera);
            RecipeHelper.CreateSimpleRecipe(ItemID.JungleKey, ItemID.PiranhaGun, TileID.MythrilAnvil, disableDecraft: true, conditions: Condition.DownedPlantera);
            RecipeHelper.CreateSimpleRecipe(ItemID.FrozenKey, ItemID.StaffoftheFrostHydra, TileID.MythrilAnvil, disableDecraft: true, conditions: Condition.DownedPlantera);
            RecipeHelper.CreateSimpleRecipe(ItemID.HallowedKey, ItemID.RainbowGun, TileID.MythrilAnvil, disableDecraft: true, conditions: Condition.DownedPlantera);
            RecipeHelper.CreateSimpleRecipe(ItemID.DungeonDesertKey, ItemID.StormTigerStaff, TileID.MythrilAnvil, disableDecraft: true, conditions: Condition.DownedPlantera);
        }

        private static void AddGrabBagRecipes()
        {
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.DogWhistle, TileID.WorkBenches, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.Toolbox, TileID.WorkBenches, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.HandWarmer, TileID.WorkBenches, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.RedRyder, TileID.WorkBenches, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.CandyCaneSword, TileID.WorkBenches, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.CandyCaneHook, TileID.WorkBenches, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Present, ItemID.FruitcakeChakram, TileID.WorkBenches, ingredientAmount: 10, disableDecraft: true);
            
            RecipeHelper.CreateSimpleRecipe(ItemID.GoodieBag, ItemID.UnluckyYarn, TileID.WorkBenches, ingredientAmount: 10, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.GoodieBag, ItemID.BatHook, TileID.WorkBenches, ingredientAmount: 25, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.GoodieBag, ItemID.RottenEgg, TileID.WorkBenches, ingredientAmount: 2, resultAmount: 25, disableDecraft: true);
            
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Daybloom, TileID.WorkBenches, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Moonglow, TileID.WorkBenches, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Blinkroot, TileID.WorkBenches, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Waterleaf, TileID.WorkBenches, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Deathweed, TileID.WorkBenches, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Fireblossom, TileID.WorkBenches, resultAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.HerbBag, ItemID.Shiverthorn, TileID.WorkBenches, resultAmount: 5, disableDecraft: true);
        }

        private static void AddCrateRecipes()
        {
            //wooden
            CreateCrateRecipe(ItemID.SailfishBoots, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.TsunamiInABottle, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Extractinator, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Aglet, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.CordageGuide, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Umbrella, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.ClimbingClaws, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Radar, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.WoodenBoomerang, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.WandofSparking, RecipeGroups.AnyWoodCrate, 5, conditions: Condition.NotRemixWorld);
            CreateCrateRecipe(ItemID.Spear, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Blowpipe, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.PortableStool, RecipeGroups.AnyWoodCrate, 5);
            //CreateCrateRecipe(ItemID.BabyBirdStaff, ItemID.WoodenCrate, 5, ItemID.WoodenCrateHard);
            CreateCrateRecipe(ItemID.SunflowerMinecart, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.LadybugMinecart, RecipeGroups.AnyWoodCrate, 5);
            CreateCrateRecipe(ItemID.Anchor, -1, 5, ItemID.WoodenCrateHard);

            //iron
            CreateCrateRecipe(ItemID.FalconBlade, RecipeGroups.AnyIronCrate, 5);
            CreateCrateRecipe(ItemID.TartarSauce, RecipeGroups.AnyIronCrate, 5);
            CreateCrateRecipe(ItemID.GingerBeard, RecipeGroups.AnyIronCrate, 5);
            CreateCrateRecipe(ItemID.CloudinaBottle, RecipeGroups.AnyIronCrate, 3);


            //gold
            CreateCrateRecipe(ItemID.BandofRegeneration, RecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.MagicMirror, RecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.FlareGun, RecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.HermesBoots, RecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.ShoeSpikes, RecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.Mace, RecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.LifeCrystal, RecipeGroups.AnyGoldCrate, 2);
            CreateCrateRecipe(ItemID.HardySaddle, -1, 2, ItemID.GoldenCrateHard);
            CreateCrateRecipe(ItemID.EnchantedSword, RecipeGroups.AnyGoldCrate, 2);

            CreateCrateRecipe(ItemID.Sundial, RecipeGroups.AnyGoldCrate, 2); //actually should be hm but fuck it

            //jungle
            CreateCrateRecipe(ItemID.AnkletoftheWind, RecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.Boomstick, RecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.FeralClaws, RecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.StaffofRegrowth, RecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.FiberglassFishingPole, RecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.BeeMinecart, RecipeGroups.AnyJungleCrate, 3);
            CreateCrateRecipe(ItemID.Seaweed, RecipeGroups.AnyJungleCrate, 5);
            CreateCrateRecipe(ItemID.FlowerBoots, RecipeGroups.AnyJungleCrate, 5);
            CreateCrateRecipe(ItemID.HoneyDispenser, RecipeGroups.AnyJungleCrate, 5);

            //sky
            CreateCrateRecipe(ItemID.ShinyRedBalloon, RecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.Starfury, RecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.CreativeWings, RecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.SkyMill, RecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.LuckyHorseshoe, RecipeGroups.AnySkyCrate, 3);
            CreateCrateRecipe(ItemID.CelestialMagnet, RecipeGroups.AnySkyCrate, 3);

            //corrupt
            CreateCrateRecipe(ItemID.BallOHurt, RecipeGroups.AnyCorruptCrate, 3);
            CreateCrateRecipe(ItemID.BandofStarpower, RecipeGroups.AnyCorruptCrate, 3);
            CreateCrateRecipe(ItemID.ShadowOrb, RecipeGroups.AnyCorruptCrate, 3);
            CreateCrateRecipe(ItemID.Musket, RecipeGroups.AnyCorruptCrate, 3);
            CreateCrateRecipe(ItemID.Vilethorn, RecipeGroups.AnyCorruptCrate, 3);

            //crimson
            CreateCrateRecipe(ItemID.TheUndertaker, RecipeGroups.AnyCrimsonCrate, 5);
            CreateCrateRecipe(ItemID.TheRottedFork, RecipeGroups.AnyCrimsonCrate, 5);
            CreateCrateRecipe(ItemID.CrimsonRod, RecipeGroups.AnyCrimsonCrate, 5);
            CreateCrateRecipe(ItemID.PanicNecklace, RecipeGroups.AnyCrimsonCrate, 5);
            CreateCrateRecipe(ItemID.CrimsonHeart, RecipeGroups.AnyCrimsonCrate, 5);

            //hallow

            //dungeon
            CreateCrateRecipe(ItemID.WaterBolt, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.Muramasa, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.CobaltShield, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.MagicMissile, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.AquaScepter, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey, conditions: Condition.NotRemixWorld);
            CreateCrateRecipe(ItemID.Valor, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.Handgun, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.ShadowKey, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.BlueMoon, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.BoneWelder, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.AlchemyTable, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);
            CreateCrateRecipe(ItemID.BewitchingTable, RecipeGroups.AnyDungeonCrate, 3, -1, ItemID.GoldenKey);

            //frozen crate
            CreateCrateRecipe(ItemID.SnowballCannon, RecipeGroups.AnyFrozenCrate, 3, conditions: Condition.NotRemixWorld);
            CreateCrateRecipe(ItemID.BlizzardinaBottle, RecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceBlade, RecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceSkates, RecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceMirror, RecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.FlurryBoots, RecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceBoomerang, RecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.IceMachine, RecipeGroups.AnyFrozenCrate, 3);
            CreateCrateRecipe(ItemID.Fish, RecipeGroups.AnyFrozenCrate, 5);

            //oasis crate
            CreateCrateRecipe(ItemID.SandBoots, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.AncientChisel, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.ThunderSpear, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.ScarabFishingRod, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.ThunderStaff, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.CatBast, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.MagicConch, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.MysticCoilSnake, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.DesertMinecart, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.EncumberingStone, RecipeGroups.AnySandCrate, 3);
            CreateCrateRecipe(ItemID.FlyingCarpet, RecipeGroups.AnySandCrate, 5);
            CreateCrateRecipe(ItemID.SandstorminaBottle, RecipeGroups.AnySandCrate, 5);

            //obsidian
            CreateCrateRecipe(ItemID.DarkLance, RecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);
            CreateCrateRecipe(ItemID.HellwingBow, RecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);
            CreateCrateRecipe(ItemID.Flamelash, RecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);
            CreateCrateRecipe(ItemID.FlowerofFire, RecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey, conditions: Condition.NotRemixWorld);
            CreateCrateRecipe(ItemID.Sunfury, RecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);
            CreateCrateRecipe(ItemID.TreasureMagnet, RecipeGroups.AnyLavaCrate, 3, -1, ItemID.ShadowKey);

            CreateCrateRecipe(ItemID.LavaCharm, RecipeGroups.AnyLavaCrate, 5);
            CreateCrateRecipe(ItemID.HellCake, RecipeGroups.AnyLavaCrate, 5);
            CreateCrateRecipe(ItemID.OrnateShadowKey, RecipeGroups.AnyLavaCrate, 5);
            CreateCrateRecipe(ItemID.SuperheatedBlood, RecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.FlameWakerBoots, RecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.LavaFishingHook, RecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.HellMinecart, RecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.WetBomb, RecipeGroups.AnyLavaCrate, 3);
            CreateCrateRecipe(ItemID.DemonConch, RecipeGroups.AnyLavaCrate, 3);

            // ocean crate
            CreateCrateRecipe(ItemID.Trident, RecipeGroups.AnyOceanCrate, 3);
            CreateCrateRecipe(ItemID.BreathingReed, RecipeGroups.AnyOceanCrate, 3);
            CreateCrateRecipe(ItemID.Flipper, RecipeGroups.AnyOceanCrate, 3);
            CreateCrateRecipe(ItemID.FloatingTube, RecipeGroups.AnyOceanCrate, 3);
            CreateCrateRecipe(ItemID.WaterWalkingBoots, RecipeGroups.AnyOceanCrate, 5);
            CreateCrateRecipe(ItemID.SharkBait, RecipeGroups.AnyOceanCrate, 5);
        }

        private static void CreateCrateRecipe(int result, int crate, int crateAmount, int hardmodeCrate = -1, int extraItem = -1, params Condition[] conditions)
        {
            if (crate != -1)
            {
                var recipe = Recipe.Create(result);
                recipe.AddRecipeGroup(crate, crateAmount);
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
            foreach (int output in outputs)
            {
                RecipeHelper.CreateSimpleRecipe(input, output, TileID.Solidifier, disableDecraft: true);
            }
        }
    }
}
