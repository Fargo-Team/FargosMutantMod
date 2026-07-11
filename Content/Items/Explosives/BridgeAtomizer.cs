using System.Collections.Generic;
using System.Linq;
using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.Projectiles;
using Fargowiltas.Content.Projectiles.Explosives;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class BridgeAtomizer : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return FargoServerConfig.Instance.InstantItems;
        }

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 46;
            Item.pick = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.shootSpeed = 24;
            Item.shoot = ModContent.ProjectileType<BridgeAtomizerProj>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine pickPower = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "PickPower");
            pickPower?.Hide();
        }

        public override bool CanUseItem(Player player)
        {
            /*if (player.HasBuff(BuffID.NoBuilding))
                return false;
            Point pos = Main.MouseWorld.ToTileCoordinates();
            if (pos.X < 0 || pos.X >= Main.maxTilesX || pos.Y < 0 || pos.Y >= Main.maxTilesY)
                return false;
            if (Main.tile[pos].HasTile)
                return base.CanUseItem(player);
            return false;*/
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(1)
                .AddIngredient(ItemID.Chain, 15)
                .AddRecipeGroup(RecipeGroupID.IronBar, 6)
                .AddRecipeGroup("Fargowiltas:AnyWoodenPlatform", 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class AtomizerGlobalTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && !TileID.Sets.Platforms[type] && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<BridgeAtomizer>() && Main.LocalPlayer.channel)
                return false;
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && TileID.Sets.Platforms[type] && !fail && !noItem && 
                Main.LocalPlayer.HeldItem.type == ModContent.ItemType<BridgeAtomizer>() && Main.LocalPlayer.channel)
            {
                noItem = true;
                Vector2 position = new Vector2(i, j).ToWorldCoordinates();
                SoundEngine.PlaySound(SoundID.Item14, position);

                Point tileCenter = new(i, j);
                int left = 0;
                int right = 0;
                for (int dir = -1; dir <= 1; dir += 2)
                {
                    for (int x = 0; x < Main.maxTilesX; x++)
                    {
                        if (x != 0)
                        {
                            if (dir == -1)
                                left++;
                            else
                                right++;
                        }
                        else if (dir == 1)
                            continue;
                        Point pos = new(tileCenter.X + dir * x, tileCenter.Y);
                        if (pos.X < 0 || pos.X >= Main.maxTilesX || pos.Y < 0 || pos.Y >= Main.maxTilesY)
                            break;
                        if (Main.tile[pos].HasTile && Main.tile[pos].TileType > TileID.Dirt && TileID.Sets.Platforms[Main.tile[pos].TileType] && FargoGlobalProjectile.OkayToDestroyTileAt(pos.X, pos.Y))
                        {
                            FargoGlobalTile.ClearEverything(pos.X, pos.Y, false);
                            continue;
                        }
                        break;
                    }
                }

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, tileCenter.X - left, tileCenter.Y, left + right, 1, TileChangeType.None);
            }
        }
    }
}