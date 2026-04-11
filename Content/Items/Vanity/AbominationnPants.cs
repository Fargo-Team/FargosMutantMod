using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    public class AbominationnPants : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.vanity = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TreeTrunks)
                .AddIngredient(ItemID.FallenTuxedoPants)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
