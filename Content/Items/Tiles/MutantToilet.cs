using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Tiles
{
    public class MutantToilet : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<MutantToiletSheet>());
            Item.maxStack = 9999;
            Item.width = 16;
            Item.height = 24;
            Item.value = Item.sellPrice(1, 50);
            Item.rare = ItemRarityID.Purple;
        }

        public override void AddRecipes()
        {
            if (ModContent.TryFind("Fargowiltas/Mutant", out ModItem modItem))
            {
                CreateRecipe()
                  .AddIngredient(ItemID.LunarBar, 6)
                  .AddIngredient(modItem.Type)
                  .AddTile(ModContent.TileType<CrucibleCosmosSheet>())
                  .Register();
            }
        }
    }
}