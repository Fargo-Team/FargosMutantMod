using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Projectiles.Explosives;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class LihzahrdInstactuationBomb : ModItem
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
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<LihzahrdInstactuationBombProj>();
        }

        private static Vector2 NearbyAltar(Player player)
        {
            Vector2 startPos = player.Bottom;
            startPos.Y -= 8;
            List<Vector2> positions = new();
            for (int i = 0; i <= 2; i++) //check up to this many blocks away
            {
                for (int j = -1; j <= 1; j += 2) //check on both sides
                {
                    Vector2 pos = startPos;
                    pos.X += 16 * i * j;
                    Tile tile = Framing.GetTileSafely(pos);
                    if (tile.TileType == TileID.LihzahrdAltar && tile.WallType == WallID.LihzahrdBrickUnsafe && Collision.CanHitLine(player.Center, 0, 0, pos, 0, 0))
                        positions.Add(pos);
                    if (positions.Count == 3) // when exactly 3 positions are found (one altar), return mean of positions (center)
                    {
                        Vector2 center = Vector2.Zero;
                        foreach (Vector2 p in positions)
                            center += p;
                        center /= positions.Count;
                        return center;
                    }
                }
            }
            return default;
        }

        public override bool CanUseItem(Player player)
            => NPC.downedPlantBoss && NearbyAltar(player) != default;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 altarPos = NearbyAltar(player);
            if (altarPos != default)
                Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), altarPos, Vector2.Zero, type, 0, 0, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Actuator, 500)
                .AddIngredient(ItemID.Dynamite, 25)
                .AddIngredient(ItemID.LunarTabletFragment, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}