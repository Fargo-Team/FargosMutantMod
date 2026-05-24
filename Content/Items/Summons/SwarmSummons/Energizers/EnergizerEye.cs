using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers
{
    public class EnergizerEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Optical Energizer");
            // Tooltip.SetDefault("Formed after using 10 Eyemalgamations\n'It feels like it's watching you'");
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