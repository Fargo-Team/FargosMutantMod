using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Summons.Mutant;
using Fargowiltas.Content.Items.Summons.VanillaCopy;
using Fargowiltas.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes
{
    public class ConversionRecipeSystem : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return FargoServerConfig.Instance.MiscRecipes;
        }
        public override void AddRecipes()
        {
            AddSummonConversions();
            AddEvilConversions();
            AddMetalConversions();
        }

        private static void AddSummonConversions()
        {
            RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<FleshyDoll>(), ItemID.GuideVoodooDoll, TileID.WorkBenches);
            RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<LihzahrdPowerCell2>(), ItemID.LihzahrdPowerCell, TileID.WorkBenches);
            RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<TruffleWorm2>(), ItemID.TruffleWorm, TileID.WorkBenches);
            RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<PrismaticPrimrose>(), ItemID.EmpressButterfly, TileID.WorkBenches);
        }

        private static void AddEvilConversions()
        {
            int evilBar = RecipeGroups.AnyEvilBar;
            AddConvertRecipe(ItemID.Vertebrae, ItemID.RottenChunk);
            AddConvertRecipe(ItemID.ShadowScale, ItemID.TissueSample);
            AddConvertRecipe(ItemID.PurpleSolution, ItemID.RedSolution);
            AddConvertRecipe(ItemID.Ichor, ItemID.CursedFlame);
            AddConvertRecipe(ItemID.PutridScent, ItemID.FleshKnuckles, evilBar);
            AddConvertRecipe(ItemID.DartRifle, ItemID.DartPistol, evilBar);
            AddConvertRecipe(ItemID.WormHook, ItemID.TendonHook, evilBar);
            AddConvertRecipe(ItemID.ChainGuillotines, ItemID.FetidBaghnakhs, evilBar);
            AddConvertRecipe(ItemID.ClingerStaff, ItemID.SoulDrain, evilBar);
            AddConvertRecipe(ItemID.ShadowOrb, ItemID.CrimsonHeart);
            AddConvertRecipe(ItemID.Musket, ItemID.TheUndertaker, evilBar);
            //AddConvertRecipe(ItemID.PanicNecklace, ItemID.BandofStarpower);
            AddConvertRecipe(ItemID.BallOHurt, ItemID.TheRottedFork, evilBar);
            AddConvertRecipe(ItemID.CrimsonRod, ItemID.Vilethorn, evilBar);
            AddConvertRecipe(ItemID.CrimstoneBlock, ItemID.EbonstoneBlock);
            AddConvertRecipe(ItemID.Shadewood, ItemID.Ebonwood);
            AddConvertRecipe(ItemID.VileMushroom, ItemID.ViciousMushroom);
            AddConvertRecipe(ItemID.Bladetongue, ItemID.Toxikarp, evilBar);
            AddConvertRecipe(ItemID.VampireKnives, ItemID.ScourgeoftheCorruptor, evilBar);
            AddConvertRecipe(ItemID.Ebonkoi, ItemID.CrimsonTigerfish);
            AddConvertRecipe(ItemID.Hemopiranha, ItemID.Ebonkoi);
            AddConvertRecipe(ItemID.BoneRattle, ItemID.EatersBone);
            AddConvertRecipe(ItemID.CrimsonSeeds, ItemID.CorruptSeeds);
            AddConvertRecipe(ItemID.DeadlandComesAlive, ItemID.LightlessChasms);
            AddConvertRecipe(ItemID.BlackCurrant, ItemID.BloodOrange);
        }

        private static void AddMetalConversions()
        {
            AddConvertRecipe(ItemID.CopperOre, ItemID.TinOre);
            AddConvertRecipe(ItemID.CopperBar, ItemID.TinBar);
            AddConvertRecipe(ItemID.IronOre, ItemID.LeadOre);
            AddConvertRecipe(ItemID.IronBar, ItemID.LeadBar);
            AddConvertRecipe(ItemID.SilverOre, ItemID.TungstenOre);
            AddConvertRecipe(ItemID.SilverBar, ItemID.TungstenBar);
            AddConvertRecipe(ItemID.GoldOre, ItemID.PlatinumOre);
            AddConvertRecipe(ItemID.GoldBar, ItemID.PlatinumBar);
            AddConvertRecipe(ItemID.CobaltOre, ItemID.PalladiumOre);
            AddConvertRecipe(ItemID.CobaltBar, ItemID.PalladiumBar);
            AddConvertRecipe(ItemID.MythrilOre, ItemID.OrichalcumOre);
            AddConvertRecipe(ItemID.MythrilBar, ItemID.OrichalcumBar);
            AddConvertRecipe(ItemID.AdamantiteOre, ItemID.TitaniumOre);
            AddConvertRecipe(ItemID.AdamantiteBar, ItemID.TitaniumBar);
            AddConvertRecipe(ItemID.DemoniteOre, ItemID.CrimtaneOre);
            AddConvertRecipe(ItemID.DemoniteBar, ItemID.CrimtaneBar);
        }

        private static void AddConvertRecipe(int itemID, int otherItemID, int extraGroup = -1)
        {
            if (extraGroup >= 0)
            {
                Recipe.Create(otherItemID).AddIngredient(itemID).AddRecipeGroup(extraGroup, 5).AddTile(TileID.DemonAltar).DisableDecraft().Register();
                Recipe.Create(itemID).AddIngredient(otherItemID).AddRecipeGroup(extraGroup, 5).AddTile(TileID.DemonAltar).DisableDecraft().Register();
            }
            else
            {
                RecipeHelper.CreateSimpleRecipe(itemID, otherItemID, TileID.DemonAltar, disableDecraft: true);
                RecipeHelper.CreateSimpleRecipe(otherItemID, itemID, TileID.DemonAltar, disableDecraft: true);
            }
                
        }
    }
}
