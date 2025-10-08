using Fargowiltas.Assets.Textures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI
{
    public class StatSheetItem : ModItem
    {
        public override string Texture => FargoMutantAssets.GetAssetString("UI", "StatSheetItem");

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.useAnimation = 4;
            Item.useTime = 4;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
            CombinedUI.ToggleUI<StatSheetUI>();
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(RecipeGroupID.IronBar).AddTile(TileID.Anvils).Register();
        }
    }
}
