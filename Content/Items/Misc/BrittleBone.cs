using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
    public class BrittleBone : ModItem
    {
        public override string Texture => "Terraria/Images/Item_154";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Bone);
            /*Item.shoot = ProjectileID.None;
            Item.useAnimation = 0;
            Item.useTime = 0;
            Item.useStyle = ItemUseStyleID.None;*/
            Item.notAmmo = false;
        }
    }
}
