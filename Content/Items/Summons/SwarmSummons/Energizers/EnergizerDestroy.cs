using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons.Energizers
{
    public class EnergizerDestroy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Destruction Energizer");
            // Tooltip.SetDefault("Formed after using 10 Seismic Actuator\n'It makes you want to break some kids'");
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