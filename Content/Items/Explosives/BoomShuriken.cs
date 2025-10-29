using Fargowiltas.Content.Projectiles.Explosives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class BoomShuriken : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Boom Shuriken");
            // Tooltip.SetDefault("Rapid firing explosives\nUses your pickaxe's mining power\n'The fastest way to dig through anything is always to blow it up!'");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 999;
        }

        public override void SetDefaults()
        {
            Item.width = 11;
            Item.height = 11;
            Item.damage = 10;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.scale = 0.75f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<ShurikenProj>();
            Item.shootSpeed = 11f;
        }
        public override float UseSpeedMultiplier(Player player)
        {
            if (player.GetBestPickaxe() == null)
                return base.UseSpeedMultiplier(player);
            float pickSpeed = player.GetBestPickaxe().useTime;
            float playerSpeed = player.pickSpeed;
            float itemTime = pickSpeed * playerSpeed;
            if (itemTime <= 0)
                itemTime = 1;
            float mult = 7f / itemTime;
            return mult;
        }
        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ItemID.Shuriken, 10)
                .AddIngredient(ItemID.Dynamite, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}