using Fargowiltas.Content.Items.Misc;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Tiles;

public class EnchantedTree : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.maxStack = 9999;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.rare = ItemRarityID.Green;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;
        Item.createTile = ModContent.TileType<EnchantedTreeSheet>();
        Item.value = Terraria.Item.buyPrice(gold: 1);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<GizmoParts>(3)
            .AddRecipeGroup(RecipeGroupID.Wood, 50)
            .AddIngredient(ItemID.Mushroom, 3)
            .AddIngredient(ItemID.GlowingMushroom, 3)
            .AddIngredient(ItemID.FallenStar, 3)
            .AddTile(TileID.DemonAltar)
            .DisableDecraft()
            .Register();
    }
}
