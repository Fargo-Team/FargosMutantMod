using Fargowiltas.Content.Projectiles.Explosives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class AltarExterminator : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = null;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.shoot = ModContent.ProjectileType<AltarExterminatorProj>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 10)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ItemID.Pwnhammer)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ItemID.Pwnhammer)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}