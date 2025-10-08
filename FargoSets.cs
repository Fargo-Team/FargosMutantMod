using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.NPCs;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using static Fargowiltas.Content.Items.FargoGlobalItem;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas
{
    public class FargoSets : ModSystem
    {
        public class Items
        {
            public static bool[] MechanicalAccessory;
            public static bool[] InfoAccessory;
            public static bool[] SquirrelSellsDirectly;

            public static bool[] NonBuffPotion;
            public static bool[] PotionCannotBeInfinite;
            public static bool[] BuffStation;
            public static List<ShopTooltip>[] RegisteredShopTooltips;

            public static int[] SacrificeCountDefault;
            public static int[] SacrificeCount;
            public static bool[] HardmodeSacrifice;
        }
        public class Tiles
        {
            public static bool[] InstaCannotDestroy;
            public static bool[] DungeonTile;
            public static bool[] HardmodeOre;
            public static bool[] EvilAltars;
        }
        public class Walls
        {
            public static bool[] InstaCannotDestroy;
            public static bool[] DungeonWall;
        }
        public class NPCs
        {
            public static int[] SwarmHealth;
        }

        public override void PostSetupContent()
        {
            #region Items
            SetFactory itemFactory = ItemID.Sets.Factory;

            Items.MechanicalAccessory = itemFactory.CreateBoolSet(false,
                ItemID.MechanicalLens,
                ItemID.WireKite,
                //ItemID.Ruler,
                ItemID.LaserRuler,
                ItemID.PaintSprayer,
                ItemID.ArchitectGizmoPack,
                ItemID.HandOfCreation,
                ItemID.ActuationAccessory,
                ItemID.EncumberingStone,
                ItemID.DontHurtCrittersBook,
                ItemID.DontHurtComboBook,
                ItemID.DontHurtNatureBook,
                ItemID.LucyTheAxe);

            Items.InfoAccessory = itemFactory.CreateBoolSet(false,
                ItemID.CopperWatch,
                ItemID.TinWatch,
                ItemID.SilverWatch,
                ItemID.TungstenWatch,
                ItemID.GoldWatch,
                ItemID.PlatinumWatch,
                ItemID.Compass,
                ItemID.DepthMeter,
                ItemID.GPS,
                ItemID.PDA,
                ItemID.CellPhone,
                5358,
                5359,
                5360,
                5361,
                ItemID.GoblinTech,
                ItemID.DPSMeter,
                ItemID.MetalDetector,
                ItemID.Stopwatch,
                ItemID.LifeformAnalyzer,
                ItemID.FishermansGuide,
                ItemID.WeatherRadio,
                ItemID.Sextant,
                ItemID.Radar,
                ItemID.TallyCounter,
                ItemID.FishFinder,
                ItemID.REK);

            Items.SquirrelSellsDirectly = itemFactory.CreateBoolSet(false,
                ItemID.CellPhone,
                ItemID.Shellphone,
                ItemID.ShellphoneDummy,
                ItemID.ShellphoneHell,
                ItemID.ShellphoneOcean,
                ItemID.ShellphoneSpawn,
                ItemID.AnkhShield,
                ItemID.RodofDiscord,
                ItemID.TerrasparkBoots,
                ItemID.TorchGodsFavor,
                ItemID.HandOfCreation,
                ItemID.Zenith,
                ItemID.AnglerTackleBag,
                ItemID.LavaproofTackleBag,
                ItemID.GoldenFishingRod,
                ItemID.GoldenBugNet,
                ItemType<Omnistation>(),
                ItemType<Omnistation2>(),
                ItemType<CrucibleCosmos>(),
                ItemType<ElementalAssembler>(),
                ItemType<MultitaskCenter>(),
                ItemType<PortableSundial>(),
                ItemType<BattleCry>());

            Items.NonBuffPotion = itemFactory.CreateBoolSet(false,
                ItemID.RecallPotion,
                ItemID.PotionOfReturn,
                ItemID.WormholePotion,
                ItemID.TeleportationPotion,
                ItemType<BigSuckPotion>());

            Items.PotionCannotBeInfinite = itemFactory.CreateBoolSet(false,
                ItemID.BottledHoney);

            Items.BuffStation = itemFactory.CreateBoolSet(false,
                ItemID.SharpeningStation,
                ItemID.AmmoBox,
                ItemID.CrystalBall,
                ItemID.BewitchingTable,
                ItemID.WarTable);

            Items.RegisteredShopTooltips = itemFactory.CreateCustomSet<List<ShopTooltip>>(null);

            Items.HardmodeSacrifice = itemFactory.CreateBoolSet(false);
            Items.SacrificeCountDefault = Squirrel.SetDefaultSacrificeCount(itemFactory);
            Items.SacrificeCount = itemFactory.CreateIntSet(0);
            #endregion
            #region Tiles
            SetFactory tileFactory = TileID.Sets.Factory;

            Tiles.InstaCannotDestroy = tileFactory.CreateBoolSet(false);

            Tiles.DungeonTile = tileFactory.CreateBoolSet(false,
                TileID.BlueDungeonBrick,
                TileID.GreenDungeonBrick,
                TileID.PinkDungeonBrick);

            Tiles.HardmodeOre = tileFactory.CreateBoolSet(false,
                TileID.Cobalt,
                TileID.Palladium,
                TileID.Mythril,
                TileID.Orichalcum,
                TileID.Adamantite,
                TileID.Titanium);

            Tiles.EvilAltars = tileFactory.CreateBoolSet(false, 
                TileID.DemonAltar);
            #endregion
            #region Walls
            SetFactory wallFactory = WallID.Sets.Factory;

            Walls.InstaCannotDestroy = wallFactory.CreateBoolSet(false);

            Walls.DungeonWall = wallFactory.CreateBoolSet(false,
                WallID.BlueDungeonSlabUnsafe, 
                WallID.BlueDungeonTileUnsafe, 
                WallID.BlueDungeonUnsafe, 
                WallID.GreenDungeonSlabUnsafe, 
                WallID.GreenDungeonTileUnsafe, 
                WallID.GreenDungeonUnsafe, 
                WallID.PinkDungeonSlabUnsafe, 
                WallID.PinkDungeonTileUnsafe, 
                WallID.PinkDungeonUnsafe);
            #endregion
            #region NPCs
            SetFactory npcFactory = NPCID.Sets.Factory;

            NPCs.SwarmHealth = npcFactory.CreateIntSet(0);
            #endregion
        }
    }
}
