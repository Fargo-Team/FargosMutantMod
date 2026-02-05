using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Tiles
{
    public class Semistation : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.createTile = ModContent.TileType<SemistationSheet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sunflower, 5)
                .AddIngredient(ItemID.Campfire, 5)
                .AddIngredient(ItemID.HeartLantern, 5)
                .AddIngredient(ItemID.StarinaBottle, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}