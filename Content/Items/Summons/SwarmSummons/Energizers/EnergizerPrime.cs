using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerPrime : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Prime Energizer");
        // Tooltip.SetDefault("Formed after using 10 Primal Control Chips\n'You feel like a new you'");
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