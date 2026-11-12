using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Items.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerTwins : ModItem
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
            .AddRecipeGroup(RecipeGroups.AnyTwinsTrophy)
            .AddIngredient(ItemID.SoulofLight, 60)
            .AddIngredient(ItemID.LunarOre, 50)
            .AddTile<LuminiteOmniforgeTile>()
            .Register();
    }
}