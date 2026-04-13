using Fargowiltas.Assets.Textures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI
{
    // Exists to be displayed as an item icon in the Toggler UI
    public class TogglerIconItem : ModItem
    {
        public override string Texture => FargoMutantAssets.GetAssetString("UI", "SoulTogglerToggle");

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.useAnimation = 4;
            Item.useTime = 4;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
            FargoUIManager.Toggle<CombinedUI>();
            return true;
        }
    }
}
