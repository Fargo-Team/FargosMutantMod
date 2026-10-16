using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerPlant : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Leafy Energizer");
        // Tooltip.SetDefault("Formed after using 10 Heart of the Jungles\n'Being a leaf sounds like a good time'");
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