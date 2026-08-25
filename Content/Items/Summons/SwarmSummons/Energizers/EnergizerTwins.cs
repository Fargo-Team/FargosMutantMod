using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerTwins : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Sibling Energizer");
        // Tooltip.SetDefault("Formed after using 10 Omnifocal Lens\n'You wish you had more'");
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