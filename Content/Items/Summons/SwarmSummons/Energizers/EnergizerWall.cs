using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers
{
    public class EnergizerWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wally Energizer");
            // Tooltip.SetDefault("Formed after using 10 Bundle of Dolls\n'You feel claustrophobic'");
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