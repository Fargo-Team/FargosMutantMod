//using Fargowiltas.Content.Items.Ammos.Bullets;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes
{
    public class RecipeGroups : ModSystem
    {
        public static string ItemXOrY(int id1, int id2) => $"{Lang.GetItemName(id1)} {Language.GetTextValue($"Mods.Fargowiltas.RecipeGroups.Or")} {Lang.GetItemName(id2)}";
        internal static int AnyGoldBar, AnyEvilBar;
        internal static int AnyDemonAltar, AnyAnvil, AnyHMAnvil, AnyForge, AnyBookcase, AnyCookingPot, AnyTombstone, AnyWoodenTable, AnyWoodenChair, AnyWoodenSink, AnyDecayChamber, AnyWoodenPlatform;
        internal static int AnyButterfly, /*AnySquirrel,*/ AnyCommonFish, AnyDragonfly, AnyBird, AnyDuck;
        internal static int AnyFoodT2, AnyFoodT3, AnyGemRobe;
        internal static int AnyWoodCrate, AnyIronCrate, AnyGoldCrate, AnyJungleCrate, AnySkyCrate, AnyCorruptCrate, AnyCrimsonCrate, AnyHallowedCrate, AnyDungeonCrate, AnyFrozenCrate, AnySandCrate, AnyLavaCrate, AnyOceanCrate;

        public override void AddRecipeGroups()
        {
            //copper bar
            var group = new RecipeGroup(() => ItemXOrY(ItemID.CopperBar, ItemID.TinBar), ItemID.CopperBar, ItemID.TinBar);
            AnyGoldBar = RecipeGroup.RegisterGroup("Fargowiltas:AnyCopperBar", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //gold ore
            group = new RecipeGroup(() => ItemXOrY(ItemID.GoldOre, ItemID.PlatinumOre), ItemID.GoldOre, ItemID.PlatinumOre);
            AnyGoldBar = RecipeGroup.RegisterGroup("Fargowiltas:AnyGoldOre", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //gold bar
            group = new RecipeGroup(() => ItemXOrY(ItemID.GoldBar, ItemID.PlatinumBar), ItemID.GoldBar, ItemID.PlatinumBar);
            AnyGoldBar = RecipeGroup.RegisterGroup("Fargowiltas:AnyGoldBar", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //demonite bar
            group = new RecipeGroup(() => ItemXOrY(ItemID.DemoniteBar, ItemID.CrimtaneBar), ItemID.DemoniteBar, ItemID.CrimtaneBar);
            AnyEvilBar = RecipeGroup.RegisterGroup("Fargowiltas:AnyEvilBar", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //demon altar
            List<int> demonaltars = new() { ModContent.ItemType<DemonAltar>(), ModContent.ItemType<CrimsonAltar>() };
            if (ModLoader.HasMod("ImproveGame"))
                demonaltars.AddRange([ModLoader.GetMod("ImproveGame").Find<ModItem>("DemonAltarItem").Type, ModLoader.GetMod("ImproveGame").Find<ModItem>("CrimsonAltarItem").Type]);
            if (ModLoader.HasMod("CalValEX"))
                demonaltars.AddRange([ModLoader.GetMod("CalValEX").Find<ModItem>("MoulderingAltarItem").Type, ModLoader.GetMod("CalValEX").Find<ModItem>("VisceralAltarItem").Type]);
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ModContent.ItemType<DemonAltar>()), demonaltars.ToArray());
            AnyDemonAltar = RecipeGroup.RegisterGroup("Fargowiltas:AnyDemonAltar", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //iron anvil
            group = new RecipeGroup(() => ItemXOrY(ItemID.IronAnvil, ItemID.LeadAnvil), ItemID.IronAnvil, ItemID.LeadAnvil);
            AnyAnvil = RecipeGroup.RegisterGroup("Fargowiltas:AnyAnvil", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //anvil HM
            group = new RecipeGroup(() => ItemXOrY(ItemID.MythrilAnvil, ItemID.OrichalcumAnvil), ItemID.MythrilAnvil, ItemID.OrichalcumAnvil);
            AnyHMAnvil = RecipeGroup.RegisterGroup("Fargowiltas:AnyHMAnvil", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //forge HM
            group = new RecipeGroup(() => ItemXOrY(ItemID.AdamantiteForge, ItemID.TitaniumForge), ItemID.AdamantiteForge, ItemID.TitaniumForge);
            AnyForge = RecipeGroup.RegisterGroup("Fargowiltas:AnyForge", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //book cases
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Bookcase),
                ItemID.Bookcase, ItemID.BlueDungeonBookcase, ItemID.BoneBookcase, ItemID.BorealWoodBookcase,
                ItemID.CactusBookcase, ItemID.CrystalBookCase, ItemID.DynastyBookcase, ItemID.EbonwoodBookcase,
                ItemID.FleshBookcase, ItemID.FrozenBookcase, ItemID.GlassBookcase, ItemID.GoldenBookcase,
                ItemID.GothicBookcase, ItemID.GraniteBookcase, ItemID.GreenDungeonBookcase, ItemID.HoneyBookcase,
                ItemID.LivingWoodBookcase, ItemID.MarbleBookcase, ItemID.MeteoriteBookcase, ItemID.MushroomBookcase,
                ItemID.ObsidianBookcase, ItemID.PalmWoodBookcase, ItemID.PearlwoodBookcase, ItemID.PinkDungeonBookcase,
                ItemID.PumpkinBookcase, ItemID.RichMahoganyBookcase, ItemID.ShadewoodBookcase, ItemID.SkywareBookcase,
                ItemID.SlimeBookcase, ItemID.SpookyBookcase, ItemID.SteampunkBookcase, ItemID.AshWoodBookcase
            );
            //book cases
            /*
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Bookcase),
                ContentSamples.ItemsByType.Keys.Where(i => (ContentSamples.ItemsByType[i].Name.Contains("Bookcase"))).Cast<int>().ToArray()
            );
            */
            AnyBookcase = RecipeGroup.RegisterGroup("Fargowiltas:AnyBookcase", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            group = new RecipeGroup(() => ItemXOrY(ItemID.CookingPot, ItemID.Cauldron), ItemID.CookingPot, ItemID.Cauldron);
            AnyCookingPot = RecipeGroup.RegisterGroup("Fargowiltas:AnyCookingPot", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("LegacyMisc.87", true),
                ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly, ItemID.RedAdmiralButterfly,
                ItemID.SulphurButterfly, ItemID.TreeNymphButterfly, ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly,
                ItemID.HellButterfly
            );
            AnyButterfly = RecipeGroup.RegisterGroup("Fargowiltas:AnyButterfly", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            /* //vanilla squirrels
            group = new RecipeGroup(() => ItemXOrY(ItemID.Squirrel, ItemID.SquirrelRed),
                ItemID.Squirrel,
                ItemID.SquirrelRed
            );
            AnySquirrel = RecipeGroup.RegisterGroup("Fargowiltas:AnySquirrel", group); */

            //vanilla fishes
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("CommonFish"),
                ItemID.AtlanticCod,
                ItemID.Bass,
                ItemID.Trout,
                ItemID.RedSnapper,
                ItemID.Salmon,
                ItemID.Tuna
            //ItemID.GoldenCarp
            );
            AnyCommonFish = RecipeGroup.RegisterGroup("Fargowiltas:AnyCommonFish", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //vanilla dragonfly
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("LegacyMisc.105", true),
                //ItemID.GoldDragonfly,
                ItemID.BlackDragonfly,
                ItemID.BlueDragonfly,
                ItemID.GreenDragonfly,
                ItemID.OrangeDragonfly,
                ItemID.RedDragonfly,
                ItemID.YellowDragonfly
            );
            AnyDragonfly = RecipeGroup.RegisterGroup("Fargowiltas:AnyDragonfly", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //vanilla birds
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Bird),
                ItemID.Bird,
                //ItemID.GoldBird,
                ItemID.BlueJay,
                ItemID.Cardinal,
                ItemID.Duck,
                ItemID.MallardDuck,
                ItemID.Grebe,
                ItemID.Seagull
            );
            AnyBird = RecipeGroup.RegisterGroup("Fargowiltas:AnyBird", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //vanilla ducks
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Duck),
                ItemID.Duck,
                ItemID.MallardDuck,
                ItemID.Grebe
            );
            AnyDuck = RecipeGroup.RegisterGroup("Fargowiltas:AnyDuck", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //tombstones
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Tombstone),
                ItemID.Tombstone,
                ItemID.CrossGraveMarker,
                ItemID.Headstone,
                ItemID.GraveMarker,
                ItemID.Gravestone,
                ItemID.Obelisk,
                ItemID.RichGravestone1,
                ItemID.RichGravestone2,
                ItemID.RichGravestone3,
                ItemID.RichGravestone4,
                ItemID.RichGravestone5
            );
            AnyTombstone = RecipeGroup.RegisterGroup("Fargowiltas:AnyTombstone", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //wooden tables
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.WoodenTable),
                ItemID.WoodenTable,
                ItemID.BorealWoodTable,
                ItemID.AshWoodTable,
                ItemID.RichMahoganyTable,
                ItemID.LivingWoodTable,
                ItemID.PearlwoodTable,
                ItemID.SpookyTable,
                ItemID.EbonwoodTable,
                ItemID.ShadewoodTable,
                ItemID.PalmWoodTable,
                ItemID.DynastyTable,
                ItemID.BambooTable
            );
            AnyWoodenTable = RecipeGroup.RegisterGroup("Fargowiltas:AnyWoodenTable", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //wooden chairs
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.WoodenChair),
                ItemID.WoodenChair,
                ItemID.BorealWoodChair,
                ItemID.AshWoodChair,
                ItemID.RichMahoganyChair,
                ItemID.LivingWoodChair,
                ItemID.PearlwoodChair,
                ItemID.SpookyChair,
                ItemID.EbonwoodChair,
                ItemID.ShadewoodChair,
                ItemID.PalmWoodChair,
                ItemID.DynastyChair,
                ItemID.BambooChair
            );
            AnyWoodenChair = RecipeGroup.RegisterGroup("Fargowiltas:AnyWoodenChair", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //wooden sinks
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.WoodenSink),
                ItemID.WoodenSink,
                ItemID.BorealWoodSink,
                ItemID.AshWoodSink,
                ItemID.RichMahoganySink,
                ItemID.LivingWoodSink,
                ItemID.PearlwoodSink,
                ItemID.SpookySink,
                ItemID.EbonwoodSink,
                ItemID.ShadewoodSink,
                ItemID.PalmWoodSink,
                ItemID.DynastySink,
                ItemID.BambooSink
            );
            AnyWoodenSink = RecipeGroup.RegisterGroup("Fargowiltas:AnyWoodenSink", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            group = new RecipeGroup(() => ItemXOrY(ItemID.LesionStation, ItemID.FleshCloningVaat), ItemID.LesionStation, ItemID.FleshCloningVaat);
            AnyDecayChamber = RecipeGroup.RegisterGroup("Fargowiltas:AnyDecayChamber", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //t2 foods
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("FoodT2"),
                ItemID.BowlofSoup,
                ItemID.CookedShrimp,
                ItemID.PumpkinPie,
                ItemID.Sashimi,
                ItemID.Escargot,
                ItemID.FroggleBunwich,
                ItemID.GrubSoup,
                ItemID.LobsterTail,
                ItemID.MonsterLasagna,
                ItemID.PrismaticPunch,
                ItemID.RoastedDuck,
                ItemID.SeafoodDinner,
                ItemID.BananaSplit,
                ItemID.ChickenNugget,
                ItemID.ChocolateChipCookie,
                ItemID.CreamSoda,
                ItemID.FriedEgg,
                ItemID.Fries,
                ItemID.IceCream,
                ItemID.Nachos,
                ItemID.ShrimpPoBoy,
                ItemID.CoffeeCup
            );
            AnyFoodT2 = RecipeGroup.RegisterGroup("Fargowiltas:AnyFoodT2", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //t3 foods
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("FoodT3"),
                ItemID.GoldenDelight,
                ItemID.GrapeJuice,
                ItemID.Milkshake,
                ItemID.Pizza,
                ItemID.Spaghetti,
                ItemID.Steak,
                ItemID.Hotdog,
                ItemID.ApplePie,
                ItemID.Bacon,
                ItemID.GingerbreadCookie,
                ItemID.BBQRibs,
                ItemID.SugarCookie,
                ItemID.ChristmasPudding
            );
            AnyFoodT3 = RecipeGroup.RegisterGroup("Fargowiltas:AnyFoodT3", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //gem robes
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("GemRobe"),
                ItemID.AmberRobe,
                ItemID.AmethystRobe,
                ItemID.DiamondRobe,
                ItemID.EmeraldRobe,
                ItemID.RubyRobe,
                ItemID.SapphireRobe,
                ItemID.TopazRobe
            );
            AnyGemRobe = RecipeGroup.RegisterGroup("Fargowiltas:AnyGemRobe", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //any wood platforms
            group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("WoodenPlatform"),
                ItemID.WoodPlatform,
                ItemID.BorealWoodPlatform,
                ItemID.AshWoodPlatform,
                ItemID.RichMahoganyPlatform,
                ItemID.LivingWoodPlatform,
                ItemID.PearlwoodPlatform,
                ItemID.SpookyPlatform,
                ItemID.EbonwoodPlatform,
                ItemID.ShadewoodPlatform,
                ItemID.PalmWoodPlatform,
                ItemID.DynastyPlatform,
                ItemID.BambooPlatform
            );
            AnyWoodenPlatform = RecipeGroup.RegisterGroup("Fargowiltas:AnyWoodenPlatform", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //wooden crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.WoodenCrate, ItemID.WoodenCrateHard), ItemID.WoodenCrate, ItemID.WoodenCrateHard);
            AnyWoodCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyWoodCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //iron crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.IronCrate, ItemID.IronCrateHard), ItemID.IronCrate, ItemID.IronCrateHard);
            AnyIronCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyIronCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //gold crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.GoldenCrate, ItemID.GoldenCrateHard), ItemID.GoldenCrate, ItemID.GoldenCrateHard);
            AnyGoldCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyGoldCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //jungle crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.JungleFishingCrate, ItemID.JungleFishingCrateHard), ItemID.JungleFishingCrate, ItemID.JungleFishingCrateHard);
            AnyJungleCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyJunglCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //sky crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.FloatingIslandFishingCrate, ItemID.FloatingIslandFishingCrateHard), ItemID.FloatingIslandFishingCrate, ItemID.FloatingIslandFishingCrateHard);
            AnySkyCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnySkyCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //corrupt crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.CorruptFishingCrate, ItemID.CorruptFishingCrateHard), ItemID.CorruptFishingCrate, ItemID.CorruptFishingCrateHard);
            AnyCorruptCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyCorruptCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //crimson crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.CrimsonFishingCrate, ItemID.CrimsonFishingCrateHard), ItemID.CrimsonFishingCrate, ItemID.CrimsonFishingCrateHard);
            AnyCrimsonCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyCrimsonCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //hallowed crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.HallowedFishingCrate, ItemID.HallowedFishingCrateHard), ItemID.HallowedFishingCrate, ItemID.HallowedFishingCrateHard);
            AnyHallowedCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyHallowedCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //dungeon crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.DungeonFishingCrate, ItemID.DungeonFishingCrateHard), ItemID.DungeonFishingCrate, ItemID.DungeonFishingCrateHard);
            AnyDungeonCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyDungeonCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //frozen crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.FrozenCrate, ItemID.FrozenCrateHard), ItemID.FrozenCrate, ItemID.FrozenCrateHard);
            AnyFrozenCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyFrozenCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //oasis crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.OasisCrate, ItemID.OasisCrateHard), ItemID.OasisCrate, ItemID.OasisCrateHard);
            AnySandCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnySandCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //lava crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.LavaCrate, ItemID.LavaCrateHard), ItemID.LavaCrate, ItemID.LavaCrateHard);
            AnyLavaCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyLavaCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;

            //ocean crates
            group = new RecipeGroup(() => ItemXOrY(ItemID.OceanCrate, ItemID.OceanCrateHard), ItemID.OceanCrate, ItemID.OceanCrateHard);
            AnyOceanCrate = RecipeGroup.RegisterGroup("Fargowiltas:AnyOceanCrate", group)/* tModPorter Note: Removed. Replace this and "new RecipeGroup()" with RecipeGroup.Register */;
        }
    }
}