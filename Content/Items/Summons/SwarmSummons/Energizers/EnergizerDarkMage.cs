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
        Item.maxStack = 9999;
        Item.rare = ItemRarityID.Blue;
        Item.value = 1000000;
    }
}