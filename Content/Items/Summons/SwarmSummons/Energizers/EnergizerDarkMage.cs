using Fargowiltas.Content.Items.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerDarkMage : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Mysterious Energizer");
        // Tooltip.SetDefault("Formed after using 10 Really Forbidden Tomes\n'You feel like you've waited one thousand years'");
    }

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
            .AddIngredient(ItemID.BossTrophyDarkmage)
            .AddIngredient(ItemID.DefenderMedal, 50)
            .AddIngredient(ItemID.LunarOre, 50)
            .AddTile<LuminiteOmniforgeTile>()
            .Register();
    }
}