using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers;

public class EnergizerEmpress : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Fairy Energizer");
        // Tooltip.SetDefault("Formed after using 10 Jars of Lacewings\n'Wear eye protection when looking'");
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