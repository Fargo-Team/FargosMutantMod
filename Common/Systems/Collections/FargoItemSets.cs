using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.NPCs;
using ReLogic.Reflection;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using static Fargowiltas.Content.Items.FargoGlobalItem;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Common.Systems.Collections
{
    [ReinitializeDuringResizeArrays]
    public static class FargoItemSets
    {
        public static SetFactory ItemFactory = new SetFactory(ItemLoader.ItemCount, "Fargowiltas/ItemID", Search);
        public static IdDictionary Search = IdDictionary.Create<ItemID, int>();

        public static bool[] MechanicalAccessory = ItemFactory.CreateBoolSet(false,
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

        public static bool[] InfoAccessory = ItemFactory.CreateBoolSet(false,
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

        public static int[] ShimmerTransformsFromItem = ItemFactory.CreateIntSet(-1);


        public static DupeType[] DuplicatableItems = ItemFactory.CreateCustomSet<DupeType>(DupeType.NotDupable,
            ItemID.CellPhone, DupeType.Dupable,
            ItemID.Shellphone, DupeType.Dupable,
            ItemID.ShellphoneDummy, DupeType.Dupable,
            ItemID.ShellphoneHell, DupeType.Dupable,
            ItemID.ShellphoneOcean, DupeType.Dupable,
            ItemID.ShellphoneSpawn, DupeType.Dupable,
            ItemID.AnkhShield, DupeType.Dupable,
            ItemID.RodofDiscord, DupeType.Dupable,
            ItemID.TerrasparkBoots, DupeType.Dupable,
            ItemID.TorchGodsFavor, DupeType.Dupable,
            ItemID.HandOfCreation, DupeType.Dupable,
            ItemID.Zenith, DupeType.MaterialsDupable,
            ItemID.AnglerTackleBag, DupeType.Dupable,
            ItemID.LavaproofTackleBag, DupeType.Dupable,
            ItemID.GoldenFishingRod, DupeType.Dupable,
            ItemID.GoldenBugNet, DupeType.Dupable,
            ItemType<Omnistation>(), DupeType.Dupable,
            ItemType<Omnistation2>(), DupeType.Dupable,
            ItemType<CrucibleCosmos>(), DupeType.Dupable,
            ItemType<LuminiteOmniforge>(), DupeType.Dupable,
            ItemType<ElementalAssembler>(), DupeType.Dupable,
            ItemType<MultitaskCenter>(), DupeType.Dupable,
            ItemType<PortableSundial>(), DupeType.Dupable,
            ItemType<BattleCry>(), DupeType.Dupable,
            ItemID.SoulofFright, DupeType.NotDupableFromDupable,
            ItemID.SoulofSight, DupeType.NotDupableFromDupable,
            ItemID.SoulofMight, DupeType.NotDupableFromDupable);

        public static Dictionary<int, List<int>> DuplicatableRecipes = [];

        public static bool[] NonBuffPotion = ItemFactory.CreateBoolSet(false,
            ItemID.RecallPotion,
            ItemID.PotionOfReturn,
            ItemID.WormholePotion,
            ItemID.TeleportationPotion,
            ItemType<BlackHolePotion>());

        //public static bool[] PotionCannotBeInfinite;
        public static int[] BuffStation = ItemFactory.CreateIntSet(-1,
            ItemID.SharpeningStation, BuffID.Sharpened,
            ItemID.AmmoBox, BuffID.AmmoBox,
            ItemID.CrystalBall, BuffID.Clairvoyance,
            ItemID.BewitchingTable, BuffID.Bewitched,
            ItemID.WarTable, BuffID.WarTable);
        public static List<ShopTooltip>[] RegisteredShopTooltips = ItemFactory.CreateCustomSet<List<ShopTooltip>>(null);

        public static int[] SacrificeCountDefault = Squirrel.SetDefaultSacrificeCount(ItemFactory);
        public static int[] SacrificeCount = ItemFactory.CreateIntSet(0);
        public static bool[] HardmodeSacrifice = ItemFactory.CreateBoolSet(false,
            ItemID.DualHook,
            ItemID.MagicDagger,
            ItemID.PhilosophersStone,
            ItemID.TitanGlove,
            ItemID.StarCloak,
            ItemID.CrossNecklace,

            // ice mimic
            ItemID.Frostbrand,
            ItemID.IceBow,
            ItemID.FlowerofFrost,

            // corrupt mimic
            ItemID.ClingerStaff,
            ItemID.DartRifle,
            ItemID.ChainGuillotines,
            ItemID.PutridScent,
            ItemID.WormHook,

            // crimson mimic
            ItemID.SoulDrain,
            ItemID.DartPistol,
            ItemID.FetidBaghnakhs,
            ItemID.FleshKnuckles,
            ItemID.TendonHook,

            // hallowed mimic
            ItemID.DaedalusStormbow,
            ItemID.FlyingKnife,
            ItemID.CrystalVileShard,
            ItemID.IlluminantHook,

            // queenie
            ItemID.CrystalNinjaHelmet,
            ItemID.CrystalNinjaChestplate,
            ItemID.CrystalNinjaLeggings,
            ItemID.Smolstar,
            ItemID.QueenSlimeMountSaddle,
            ItemID.QueenSlimeHook,

            // plantera
            ItemID.GrenadeLauncher,
            ItemID.VenusMagnum,
            ItemID.NettleBurst,
            ItemID.LeafBlower,
            ItemID.FlowerPow,
            ItemID.WaspGun,
            ItemID.Seedler,
            ItemID.PygmyStaff,
            ItemID.ThornHook,

            // golem
            ItemID.Stynger,
            ItemID.PossessedHatchet,
            ItemID.SunStone,
            ItemID.EyeoftheGolem,
            ItemID.HeatRay,
            ItemID.StaffofEarth,
            ItemID.GolemFist,

            // oger
            ItemID.BookStaff, // tome of infinite wisdom
            ItemID.DD2PhoenixBow, // phantom phoenix
            ItemID.DD2SquireDemonSword, // brand of the inferno
            ItemID.MonkStaffT1, // sleepy octopod
            ItemID.MonkStaffT2, // gassy glaive

            // betsy
            ItemID.DD2BetsyBow, // aerial bane
            ItemID.DD2SquireBetsySword, // flying dragon
            ItemID.MonkStaffT3, // sky dragons fury
            ItemID.ApprenticeStaffT3, // betsy's wrath

            // eol
            ItemID.FairyQueenMagicItem, // nightglow
            ItemID.PiercingStarlight,
            ItemID.RainbowWhip,
            ItemID.FairyQueenRangedItem, // eventide

            // duke
            ItemID.Flairon,
            ItemID.BubbleGun,
            ItemID.RazorbladeTyphoon,
            ItemID.TempestStaff,
            ItemID.Tsunami,

            // enemy drops
            ItemID.BeamSword,
            ItemID.Marrow,
            ItemID.Uzi,
            ItemID.UnholyTrident,
            ItemID.IceSickle,
            ItemID.FrostStaff,
            // dungeon
            ItemID.Keybrand,
            ItemID.ShadowbeamStaff,
            ItemID.SpectreStaff,
            ItemID.InfernoFork,
            ItemID.RocketLauncher,
            ItemID.SniperRifle,
            ItemID.ShadowJoustingLance,
            ItemID.TacticalShotgun,
            ItemID.PaladinsHammer,
            ItemID.MagnetSphere,
            ItemID.MaceWhip,

            // materials
            ItemID.TurtleShell,
            ItemID.UnicornHorn);

        public static bool[] TreeTreasureObtainable = ItemFactory.CreateBoolSet(false);
        public static bool[] ChizardHats = ItemFactory.CreateBoolSet(false, ItemID.GreenCap,
            ItemID.PharaohsMask,
            ItemID.CenxsTiara,
            ItemID.DevilHorns,
            ItemID.GiantBow,
            ItemID.NurseHat,
            ItemID.HeartHairpin,
            ItemID.SeashellHairpin,
            ItemID.ReindeerAntlers,
            ItemID.StarHairpin,
            ItemID.Tiara,
            ItemID.AnglerHat,
            ItemID.ArchaeologistsHat,
            ItemID.BeeHat,
            ItemID.BuccaneerBandana,
            ItemID.ClownHat,
            ItemID.CowboyHat,
            ItemID.Fez,
            ItemID.GoldCrown,
            ItemID.LeprechaunHat,
            ItemID.MagicHat,
            ItemID.MrsClauseHat,
            ItemID.PartyHat,
            ItemID.PeddlersHat,
            ItemID.PirateHat,
            ItemID.PlatinumCrown,
            ItemID.PlumbersHat,
            ItemID.PumpkinMask,
            ItemID.RainHat,
            ItemID.RedHat,
            ItemID.RobotHat,
            ItemID.RuneHat,
            ItemID.SantaHat,
            ItemID.ScarecrowHat,
            ItemID.SteampunkHat,
            ItemID.SummerHat,
            ItemID.TopHat,
            ItemID.UmbrellaHat,
            ItemID.TheBrideHat,
            ItemID.WizardHat,
            ItemID.WizardsHat,
            ItemID.GraduationCapBlack,
            ItemID.GraduationCapBlue,
            ItemID.GraduationCapMaroon,
            ItemID.ChefHat,
            ItemID.GolfHat,
            ItemID.ElfHat,
            ItemID.FuneralHat,
            ItemID.HerosHat,
            ItemID.UndertakerHat,
            ItemID.MaidHead,
            ItemID.MaidHead2,
            ItemID.MushroomHat,
            ItemID.PrettyPinkRibbon,
            ItemID.SailorHat,
            ItemID.RoyalTiara,
            ItemID.StarPrincessCrown,
            ItemID.TaxCollectorHat,
            ItemID.VictorianGothHat,
            ItemID.RoninHat,
            ItemID.RabbitOrder,
            ItemID.MushroomCap,
            ItemID.SnowHat,
            ItemID.TamOShanter,
            ItemID.JimsCap,
            ItemID.GangstaHat,
            ItemID.GarlandHat,
            ItemID.Fedora,
            ItemID.Eyebrella,
            ItemID.DizzyHat,
            ItemID.BallaHat,
            ItemID.Beanie,
            ItemID.LeinforsHat,

            //armor
            ItemID.MiningHelmet,
            ItemID.WoodHelmet,
            ItemID.RichMahoganyHelmet,
            ItemID.BorealWoodHelmet,
            ItemID.EbonwoodHelmet,
            ItemID.ShadewoodHelmet,
            ItemID.AshWoodHelmet,
            ItemID.AncientGoldHelmet,
            ItemID.AncientIronHelmet,
            ItemID.AncientNecroHelmet,
            ItemID.PearlwoodHelmet,
            ItemID.MythrilHelmet,
            ItemID.MythrilHat,
            ItemID.HuntressWig,
            ItemID.ApprenticeHat,
            ItemID.HallowedHelmet,
            ItemID.HallowedHeadgear,
            ItemID.HallowedHood,
            ItemID.ChlorophyteHeadgear,
            ItemID.TurtleHelmet
            );
    }
    public class FargoShimmerSetSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            for (int i = 0; i < FargoItemSets.ShimmerTransformsFromItem.Length; i++)
            {
                int shimmerItem = ItemID.Sets.ShimmerTransformToItem[i];
                if (shimmerItem > 0)
                    FargoItemSets.ShimmerTransformsFromItem[shimmerItem] = i;
            }
        }
    }
    public enum DupeType
    {
        Dupable,
        MaterialsDupable,
        NotDupable,
        NotDupableFromDupable //if an item is marked as dupable materials, these items will be excluded
    }

}
