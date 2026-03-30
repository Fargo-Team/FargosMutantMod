using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Fargowiltas.Content.Items.Misc
{
    public class EnchantedAcorn : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 2);
            Item.width = Item.height = 20;
            Item.maxStack = 9999;
            base.SetDefaults();
        }
    }
}
