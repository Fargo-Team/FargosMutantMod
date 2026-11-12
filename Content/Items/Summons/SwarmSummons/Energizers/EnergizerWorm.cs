using Fargowiltas.Content.Items.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerWorm : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = Terraria.Item.CommonMaxStack;
        Item.rare = ItemRarityID.Blue;
        Item.value = 1000000;
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.EaterofWorldsTrophy)
            .AddIngredient(ItemID.RottenChunk, 60)
            .AddIngredient(ItemID.LunarOre, 50)
            .AddTile<LuminiteOmniforgeTile>()
            .Register();
    }
}