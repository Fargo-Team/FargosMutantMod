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
    public class FargoRecipeGroups : ModSystem
    {
        public static string ItemXOrY(int id1, int id2) => $"{Lang.GetItemName(id1)} {Language.GetTextValue($"Mods.Fargowiltas.RecipeGroups.Or")} {Lang.GetItemName(id2)}";
        public static RecipeGroup AnyGoldBar, AnyEvilBar;
        public static RecipeGroup AnyDemonAltar, AnyAnvil, AnyHMAnvil, AnyForge, AnyBookcase, AnyCookingPot, AnyTombstone, AnyWoodenTable, AnyWoodenChair, AnyWoodenSink, AnyDecayChamber, AnyWoodenPlatform;
        public static RecipeGroup AnyButterfly, /*AnySquirrel,*/ AnyCommonFish, AnyDragonfly, AnyBird, AnyDuck;
        public static RecipeGroup AnyFoodT2, AnyFoodT3, AnyGemRobe;
        public static RecipeGroup AnyWoodCrate, AnyIronCrate, AnyGoldCrate, AnyJungleCrate, AnySkyCrate, AnyCorruptCrate, AnyCrimsonCrate, AnyHallowedCrate, AnyDungeonCrate, AnyFrozenCrate, AnySandCrate, AnyLavaCrate, AnyOceanCrate;

        public override void Unload()
        {
            AnyGoldBar = AnyEvilBar = null;
            AnyDemonAltar = AnyAnvil = AnyHMAnvil = AnyForge = AnyBookcase = AnyCookingPot = AnyTombstone = AnyWoodenTable = AnyWoodenChair = AnyWoodenSink = AnyDecayChamber = AnyWoodenPlatform = null;

            AnyButterfly = AnyCommonFish = AnyDragonfly = AnyBird = AnyDuck = null;
            AnyFoodT2 = AnyFoodT3 = AnyGemRobe = null;
            AnyWoodCrate = AnyIronCrate = AnyGoldCrate = AnyJungleCrate = AnySkyCrate = AnyCorruptCrate = AnyCrimsonCrate = AnyHallowedCrate = AnyDungeonCrate = AnyFrozenCrate = AnySandCrate = AnyLavaCrate = AnyOceanCrate = null;
        }

        public override void AddRecipeGroups()
        {
            //copper bar
            AnyGoldBar = RecipeGroup.Register("Fargowiltas:AnyCopperBar", () => ItemXOrY(ItemID.CopperBar, ItemID.TinBar), ItemID.CopperBar, ItemID.TinBar);
           

            //gold ore
            AnyGoldBar = RecipeGroup.Register("Fargowiltas:AnyGoldOre", () => ItemXOrY(ItemID.GoldOre, ItemID.PlatinumOre), ItemID.GoldOre, ItemID.PlatinumOre);

            //gold bar
            AnyGoldBar = RecipeGroup.Register("Fargowiltas:AnyGoldBar", () => ItemXOrY(ItemID.GoldBar, ItemID.PlatinumBar), ItemID.GoldBar, ItemID.PlatinumBar);

            //demonite bar
            AnyEvilBar = RecipeGroup.Register("Fargowiltas:AnyEvilBar", () => ItemXOrY(ItemID.DemoniteBar, ItemID.CrimtaneBar), ItemID.DemoniteBar, ItemID.CrimtaneBar);

            //demon altar
            List<int> demonaltars = new() { ModContent.ItemType<DemonAltar>(), ModContent.ItemType<CrimsonAltar>() };
            if (ModLoader.HasMod("ImproveGame"))
                demonaltars.AddRange([ModLoader.GetMod("ImproveGame").Find<ModItem>("DemonAltarItem").Type, ModLoader.GetMod("ImproveGame").Find<ModItem>("CrimsonAltarItem").Type]);
            if (ModLoader.HasMod("CalValEX"))
                demonaltars.AddRange([ModLoader.GetMod("CalValEX").Find<ModItem>("MoulderingAltarItem").Type, ModLoader.GetMod("CalValEX").Find<ModItem>("VisceralAltarItem").Type]);
            AnyDemonAltar = RecipeGroup.Register("Fargowiltas:AnyDemonAltar", () => RecipeHelper.GenerateAnyItemRecipeGroupText(ModContent.ItemType<DemonAltar>()), demonaltars.ToArray());

            //iron anvil
            AnyAnvil = RecipeGroup.Register("Fargowiltas:AnyAnvil", () => ItemXOrY(ItemID.IronAnvil, ItemID.LeadAnvil), ItemID.IronAnvil, ItemID.LeadAnvil);

            //anvil HM
            AnyHMAnvil = RecipeGroup.Register("Fargowiltas:AnyHMAnvil", () => ItemXOrY(ItemID.MythrilAnvil, ItemID.OrichalcumAnvil), ItemID.MythrilAnvil, ItemID.OrichalcumAnvil);

            //forge HM
            AnyForge = RecipeGroup.Register("Fargowiltas:AnyForge", () => ItemXOrY(ItemID.AdamantiteForge, ItemID.TitaniumForge), ItemID.AdamantiteForge, ItemID.TitaniumForge);

            //book cases
            //todo: add new bookcases added by 1.4.5
            AnyBookcase = RecipeGroup.Register("Fargowiltas:AnyBookcase", () => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Bookcase),
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
            group = RecipeGroup.Register(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Bookcase),
                ContentSamples.ItemsByType.Keys.Where(i => (ContentSamples.ItemsByType[i].Name.Contains("Bookcase"))).Cast<int>().ToArray()
            );
            */

            AnyCookingPot = RecipeGroup.Register("Fargowiltas:AnyCookingPot", () => ItemXOrY(ItemID.CookingPot, ItemID.Cauldron), ItemID.CookingPot, ItemID.Cauldron);

            AnyButterfly = RecipeGroup.Register("Fargowiltas:AnyButterfly", () => RecipeHelper.GenerateAnyItemRecipeGroupText("LegacyMisc.87", true),
                ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly, ItemID.RedAdmiralButterfly,
                ItemID.SulphurButterfly, ItemID.TreeNymphButterfly, ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly,
                ItemID.HellButterfly
            );

            /* //vanilla squirrels
            group = RecipeGroup.Register(() => ItemXOrY(ItemID.Squirrel, ItemID.SquirrelRed),
                ItemID.Squirrel,
                ItemID.SquirrelRed
            );
            AnySquirrel = RecipeGroup.RegisterGroup("Fargowiltas:AnySquirrel", group); */

            //vanilla fishes
            AnyCommonFish = RecipeGroup.Register("Fargowiltas:AnyCommonFish", () => RecipeHelper.GenerateAnyItemRecipeGroupText("CommonFish"),
                ItemID.AtlanticCod,
                ItemID.Bass,
                ItemID.Trout,
                ItemID.RedSnapper,
                ItemID.Salmon,
                ItemID.Tuna
            //ItemID.GoldenCarp
            );

            //vanilla dragonfly
            AnyDragonfly = RecipeGroup.Register("Fargowiltas:AnyDragonfly", () => RecipeHelper.GenerateAnyItemRecipeGroupText("LegacyMisc.105", true),
                //ItemID.GoldDragonfly,
                ItemID.BlackDragonfly,
                ItemID.BlueDragonfly,
                ItemID.GreenDragonfly,
                ItemID.OrangeDragonfly,
                ItemID.RedDragonfly,
                ItemID.YellowDragonfly
            );

            //vanilla birds
            AnyBird = RecipeGroup.Register("Fargowiltas:AnyBird", () => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Bird),
                ItemID.Bird,
                //ItemID.GoldBird,
                ItemID.BlueJay,
                ItemID.Cardinal,
                ItemID.Duck,
                ItemID.MallardDuck,
                ItemID.Grebe,
                ItemID.Seagull
            );

            //vanilla ducks
            AnyDuck = RecipeGroup.Register("Fargowiltas:AnyDuck", () => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Duck),
                ItemID.Duck,
                ItemID.MallardDuck,
                ItemID.Grebe
            );

            //tombstones
            AnyTombstone = RecipeGroup.Register("Fargowiltas:AnyTombstone", () => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.Tombstone),
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

            //wooden tables
            AnyWoodenTable = RecipeGroup.Register("Fargowiltas:AnyWoodenTable", () => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.WoodenTable),
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

            //wooden chairs
            AnyWoodenChair = RecipeGroup.Register("Fargowiltas:AnyWoodenChair", () => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.WoodenChair),
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

            //wooden sinks
            AnyWoodenSink = RecipeGroup.Register("Fargowiltas:AnyWoodenSink", () => RecipeHelper.GenerateAnyItemRecipeGroupText(ItemID.WoodenSink),
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

            AnyDecayChamber = RecipeGroup.Register("Fargowiltas:AnyDecayChamber", () => ItemXOrY(ItemID.LesionStation, ItemID.FleshCloningVaat), ItemID.LesionStation, ItemID.FleshCloningVaat);

            //t2 foods
            AnyFoodT2 = RecipeGroup.Register("Fargowiltas:AnyFoodT2", () => RecipeHelper.GenerateAnyItemRecipeGroupText("FoodT2"),
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

            //t3 foods
            AnyFoodT3 = RecipeGroup.Register("Fargowiltas:AnyFoodT3", () => RecipeHelper.GenerateAnyItemRecipeGroupText("FoodT3"),
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

            //gem robes
            AnyGemRobe = RecipeGroup.Register("Fargowiltas:AnyGemRobe", () => RecipeHelper.GenerateAnyItemRecipeGroupText("GemRobe"),
                ItemID.AmberRobe,
                ItemID.AmethystRobe,
                ItemID.DiamondRobe,
                ItemID.EmeraldRobe,
                ItemID.RubyRobe,
                ItemID.SapphireRobe,
                ItemID.TopazRobe
            );

            //any wood platforms
            AnyWoodenPlatform = RecipeGroup.Register("Fargowiltas:AnyWoodenPlatform", () => RecipeHelper.GenerateAnyItemRecipeGroupText("WoodenPlatform"),
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

            //wooden crates
            AnyWoodCrate = RecipeGroup.Register("Fargowiltas:AnyWoodCrate", () => ItemXOrY(ItemID.WoodenCrate, ItemID.WoodenCrateHard), ItemID.WoodenCrate, ItemID.WoodenCrateHard);

            //iron crates
            AnyIronCrate = RecipeGroup.Register("Fargowiltas:AnyIronCrate", () => ItemXOrY(ItemID.IronCrate, ItemID.IronCrateHard), ItemID.IronCrate, ItemID.IronCrateHard);

            //gold crates
            AnyGoldCrate = RecipeGroup.Register("Fargowiltas:AnyGoldCrate", () => ItemXOrY(ItemID.GoldenCrate, ItemID.GoldenCrateHard), ItemID.GoldenCrate, ItemID.GoldenCrateHard);

            //jungle crates
            AnyJungleCrate = RecipeGroup.Register("Fargowiltas:AnyJungleCrate", () => ItemXOrY(ItemID.JungleFishingCrate, ItemID.JungleFishingCrateHard), ItemID.JungleFishingCrate, ItemID.JungleFishingCrateHard);

            //sky crates
            AnySkyCrate = RecipeGroup.Register("Fargowiltas:AnySkyCrate", () => ItemXOrY(ItemID.FloatingIslandFishingCrate, ItemID.FloatingIslandFishingCrateHard), ItemID.FloatingIslandFishingCrate, ItemID.FloatingIslandFishingCrateHard);

            //corrupt crates
            AnyCorruptCrate = RecipeGroup.Register("Fargowiltas:AnyCorruptCrate", () => ItemXOrY(ItemID.CorruptFishingCrate, ItemID.CorruptFishingCrateHard), ItemID.CorruptFishingCrate, ItemID.CorruptFishingCrateHard);

            //crimson crates
            AnyCrimsonCrate = RecipeGroup.Register("Fargowiltas:AnyCrimsonCrate", () => ItemXOrY(ItemID.CrimsonFishingCrate, ItemID.CrimsonFishingCrateHard), ItemID.CrimsonFishingCrate, ItemID.CrimsonFishingCrateHard);

            //hallowed crates
            AnyHallowedCrate = RecipeGroup.Register("Fargowiltas:AnyHallowedCrate", () => ItemXOrY(ItemID.HallowedFishingCrate, ItemID.HallowedFishingCrateHard), ItemID.HallowedFishingCrate, ItemID.HallowedFishingCrateHard);

            //dungeon crates
            AnyDungeonCrate = RecipeGroup.Register("Fargowiltas:AnyDungeonCrate", () => ItemXOrY(ItemID.DungeonFishingCrate, ItemID.DungeonFishingCrateHard), ItemID.DungeonFishingCrate, ItemID.DungeonFishingCrateHard);

            //frozen crates
            AnyFrozenCrate = RecipeGroup.Register("Fargowiltas:AnyFrozenCrate", () => ItemXOrY(ItemID.FrozenCrate, ItemID.FrozenCrateHard), ItemID.FrozenCrate, ItemID.FrozenCrateHard);

            //oasis crates
            AnySandCrate = RecipeGroup.Register("Fargowiltas:AnySandCrate", () => ItemXOrY(ItemID.OasisCrate, ItemID.OasisCrateHard), ItemID.OasisCrate, ItemID.OasisCrateHard);

            //lava crates
            AnyLavaCrate = RecipeGroup.Register("Fargowiltas:AnyLavaCrate", () => ItemXOrY(ItemID.LavaCrate, ItemID.LavaCrateHard), ItemID.LavaCrate, ItemID.LavaCrateHard);

            //ocean crates
            AnyOceanCrate = RecipeGroup.Register("Fargowiltas:AnyOceanCrate", () => ItemXOrY(ItemID.OceanCrate, ItemID.OceanCrateHard), ItemID.OceanCrate, ItemID.OceanCrateHard);
        }
    }
}