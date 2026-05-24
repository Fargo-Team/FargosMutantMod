using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
    public class GizmoParts : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 10);
            Item.width = Item.height = 20;
            Item.maxStack = 9999;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 10)
            .AddIngredient(ItemID.Chain, 30)
            .AddIngredient(ItemID.Glass, 4)
            .AddTile(TileID.Anvils)
            .DisableDecraft()
            .Register();
        }
    }
}
