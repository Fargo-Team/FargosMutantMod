using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Projectiles.Explosives;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives;

public class InstaTrack : ModItem
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
        Item.width = 10;
        Item.height = 32;
        Item.maxStack = 9999;
        Item.consumable = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;
        Item.useAnimation = 20;
        Item.useTime = 20;
        Item.value = Item.buyPrice(0, 0, 3);
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.shoot = ModContent.ProjectileType<InstatrackProj>();
    }

    public override void HoldItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            Vector2 mouse = Main.MouseWorld;
            int side = Math.Sign(mouse.X - player.Center.X);
            InstaVisual.DrawOrigin origin = side > 0 ? InstaVisual.DrawOrigin.Left : InstaVisual.DrawOrigin.Right;
            InstaVisual.DrawInstaVisual(player, mouse, new(2000, 1), origin);
        }
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Vector2 mouse = Main.MouseWorld;

        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), mouse, Vector2.Zero, type, 0, 0, player.whoAmI);

        return false;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<GizmoParts>(10)
            .AddIngredient(ItemID.Dynamite, 20)
            .AddIngredient(ItemID.MinecartTrack, 2000)
            .AddIngredient(ItemID.FallenStar, 4)
            .AddTile(TileID.Anvils)
            .Register();
    }
}