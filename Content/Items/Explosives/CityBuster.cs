using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class CityBuster : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return FargoServerConfig.Instance.InstantItems;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 1);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Explosives.CityBuster>();
            Item.shootSpeed = 5f;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(BuffID.NoBuilding))
                return false;
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(10)
                .AddIngredient(ItemID.Dynamite, 50)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}