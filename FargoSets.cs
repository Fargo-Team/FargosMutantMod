using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.NPCs;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
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
            public static int[] ShimmerTransformsFromItem;
            public enum DupeType
            {
                Dupable,
                MaterialsDupable,
                NotDupable,
                NotDupableFromDupable //if an item is marked as dupable materials, these items will be excluded
            }
            public static DupeType[] DuplicatableItems;
            public static Dictionary<int, List<int>> DuplicatableRecipes = [];

            public static bool[] NonBuffPotion;
            //public static bool[] PotionCannotBeInfinite;
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

            Items.ShimmerTransformsFromItem = itemFactory.CreateIntSet(-1);
            for (int i = 0; i < Items.ShimmerTransformsFromItem.Length; i++)
            {
                int shimmerItem = ItemID.Sets.ShimmerTransformToItem[i];
                if (shimmerItem > 0)
                    Items.ShimmerTransformsFromItem[shimmerItem] = i;
            }

            Items.DuplicatableItems = itemFactory.CreateCustomSet<Items.DupeType>(Items.DupeType.NotDupable,
                ItemID.CellPhone, Items.DupeType.Dupable,
                ItemID.Shellphone, Items.DupeType.Dupable,
                ItemID.ShellphoneDummy, Items.DupeType.Dupable,
                ItemID.ShellphoneHell, Items.DupeType.Dupable,
                ItemID.ShellphoneOcean, Items.DupeType.Dupable,
                ItemID.ShellphoneSpawn, Items.DupeType.Dupable,
                ItemID.AnkhShield, Items.DupeType.Dupable,
                ItemID.RodofDiscord, Items.DupeType.Dupable,
                ItemID.TerrasparkBoots, Items.DupeType.Dupable,
                ItemID.TorchGodsFavor, Items.DupeType.Dupable,
                ItemID.HandOfCreation, Items.DupeType.Dupable,
                ItemID.Zenith, Items.DupeType.MaterialsDupable,
                ItemID.AnglerTackleBag, Items.DupeType.Dupable,
                ItemID.LavaproofTackleBag, Items.DupeType.Dupable,
                ItemID.GoldenFishingRod, Items.DupeType.Dupable,
                ItemID.GoldenBugNet, Items.DupeType.Dupable,
                ItemType<Omnistation>(), Items.DupeType.Dupable,
                ItemType<Omnistation2>(), Items.DupeType.Dupable,
                ItemType<CrucibleCosmos>(), Items.DupeType.Dupable,
                ItemType<LuminiteOmniforge>, Items.DupeType.Dupable,
                ItemType<ElementalAssembler>(), Items.DupeType.Dupable,
                ItemType<MultitaskCenter>(), Items.DupeType.Dupable,
                ItemType<PortableSundial>(), Items.DupeType.Dupable,
                ItemType<BattleCry>(), Items.DupeType.Dupable,
                ItemID.SoulofFright, Items.DupeType.NotDupableFromDupable,
                ItemID.SoulofSight, Items.DupeType.NotDupableFromDupable,
                ItemID.SoulofMight, Items.DupeType.NotDupableFromDupable);

            if (ModLoader.HasMod("FargowiltasSouls"))
            {
                TryFind<ModItem>("FargowiltasSouls/BionomicCluster", out ModItem biocluster);
                TryFind<ModItem>("FargowiltasSouls/HeartoftheMasochist", out ModItem masoheart);
                TryFind<ModItem>("FargowiltasSouls/ChaliceoftheMoon", out ModItem moonchalice);
                TryFind<ModItem>("FargowiltasSouls/DubiousCircuitry", out ModItem dubiouscirc);
                TryFind<ModItem>("FargowiltasSouls/PureHeart", out ModItem pureheart);
                TryFind<ModItem>("FargowiltasSouls/SupremeDeathbringerFairy", out ModItem deathfairy);
                TryFind<ModItem>("FargowiltasSouls/LithosphericCluster", out ModItem lithocluster);
                TryFind<ModItem>("FargowiltasSouls/MasochistSoul", out ModItem masosoul);
                TryFind<ModItem>("FargowiltasSouls/AeolusBoots", out ModItem aeolus);
                TryFind<ModItem>("FargowiltasSouls/ZephyrBoots", out ModItem zephyr);
                TryFind<ModItem>("FargowiltasSouls/DeviatingEnergy", out ModItem devienergy);
                TryFind<ModItem>("FargowiltasSouls/AbomEnergy", out ModItem abomenergy);
                TryFind<ModItem>("FargowiltasSouls/EternalEnergy", out ModItem mutantenergy);

                Items.DuplicatableItems.SetValue(Items.DupeType.Dupable, biocluster.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.Dupable, masoheart.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.Dupable, moonchalice.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.Dupable, dubiouscirc.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.Dupable, pureheart.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.Dupable, deathfairy.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.Dupable, lithocluster.Type);

                Items.DuplicatableItems.SetValue(Items.DupeType.MaterialsDupable, masosoul.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.MaterialsDupable, aeolus.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.MaterialsDupable, zephyr.Type);

                Items.DuplicatableItems.SetValue(Items.DupeType.NotDupableFromDupable, devienergy.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.NotDupableFromDupable, abomenergy.Type);
                Items.DuplicatableItems.SetValue(Items.DupeType.NotDupableFromDupable, mutantenergy.Type);
            }

            Items.NonBuffPotion = itemFactory.CreateBoolSet(false,
                ItemID.RecallPotion,
                ItemID.PotionOfReturn,
                ItemID.WormholePotion,
                ItemID.TeleportationPotion,
                ItemType<BigSuckPotion>());

            //Items.PotionCannotBeInfinite = itemFactory.CreateBoolSet(false,
            //    ItemID.BottledHoney);

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
