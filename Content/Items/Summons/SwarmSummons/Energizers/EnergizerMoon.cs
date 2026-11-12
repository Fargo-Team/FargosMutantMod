using Fargowiltas.Content.Items.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerMoon : ModItem
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
            .AddIngredient(ItemID.MoonLordTrophy)
            .AddIngredient(ItemID.FragmentSolar, 20)
            .AddIngredient(ItemID.FragmentVortex, 20)
            .AddIngredient(ItemID.FragmentNebula, 20)
            .AddIngredient(ItemID.FragmentStardust, 20)
            .AddIngredient(ItemID.LunarOre, 100)
            .AddTile<LuminiteOmniforgeTile>()
            .Register();
    }
}