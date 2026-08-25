using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerDG : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Extra Boney Energizer");
        // Tooltip.SetDefault("Formed after using 10 Skull Chain Necklaces\n'Reminds you of a mean cow'");
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