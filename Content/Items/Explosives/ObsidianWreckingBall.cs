using Fargowiltas.Content.Projectiles.Explosives;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives;

public class ObsidianWreckingBall : ModItem
{

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 36;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = 60;
        Item.useAnimation = 60;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 10);
        Item.shoot = ModContent.ProjectileType<ObsidianWreckingBallProj>();
        Item.shootSpeed = 0f;
        Item.channel = true;
    }

    public override bool CanUseItem(Player player)
    {
        return !ObsidianWreckingBallProj.PlayerHasActiveBall(player.whoAmI);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        return !ObsidianWreckingBallProj.PlayerHasActiveBall(player.whoAmI);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Obsidian, 100)
            .AddIngredient(ItemID.HellstoneBar, 20)
            .AddIngredient(ItemID.Chain, 10)
            .AddIngredient(ItemID.Bone, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}