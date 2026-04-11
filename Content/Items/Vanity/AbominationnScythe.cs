using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.HandsOff)]
    public class AbominationnScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.vanity = true;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
        }
    }
}
