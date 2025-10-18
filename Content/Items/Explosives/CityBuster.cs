using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class CityBuster : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("City Buster");
            /* Tooltip.SetDefault(@"Destroys a large area
Will not remove background walls
Use at your own risk"); */
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
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
                .AddIngredient(ItemID.Dynamite, 50)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}