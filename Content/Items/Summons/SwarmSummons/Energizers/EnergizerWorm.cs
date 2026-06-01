using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers
{
    public class EnergizerWorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wormy Energizer");
            // Tooltip.SetDefault("Formed after using 10 Worm Chickens\n'It's a little squishy'");
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
}