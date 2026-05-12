using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.Projectiles.Explosives;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class OmniBridgifier : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return FargoServerConfig.Instance.InstantItems;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<OmniBridgifierProj>();
            Item.shootSpeed = 5f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Bottom;
            position.Y += 8;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai0 = -1;
            if (player.inventory.Any(i => !i.IsAir && i.type == ModContent.ItemType<Omnistation>()))
                ai0 = 0;
            if (player.inventory.Any(i => !i.IsAir && i.type == ModContent.ItemType<Omnistation2>()))
                ai0 = ai0 == 0 ? Main.rand.Next(2) : 1; //if have both omnis, pick one randomly
            if (ai0 == -1)
                ai0 = Main.rand.Next(2); //if have neither omni, pick one randomly
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<InstaBridge>())
                .AddTile(ModContent.TileType<OmnistationSheet>())
                .Register();

            
        }
    }

    public class OmniBridgifier2 : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return FargoServerConfig.Instance.InstantItems;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<OmniBridgifierProj>();
            Item.shootSpeed = 5f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Bottom;
            position.Y += 8;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai0 = -1;
            if (player.inventory.Any(i => !i.IsAir && i.type == ModContent.ItemType<Omnistation>()))
                ai0 = 0;
            if (player.inventory.Any(i => !i.IsAir && i.type == ModContent.ItemType<Omnistation2>()))
                ai0 = ai0 == 0 ? Main.rand.Next(2) : 1; //if have both omnis, pick one randomly
            if (ai0 == -1)
                ai0 = Main.rand.Next(2); //if have neither omni, pick one randomly
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ModContent.ItemType<InstaBridge>())
                .AddTile(ModContent.TileType<OmnistationSheet2>())
                .Register();
        }
    }
}