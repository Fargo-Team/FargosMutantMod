using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.Projectiles;
using Fargowiltas.Content.Projectiles.Explosives;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
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
                .AddRecipeGroup(RecipeGroups.IronBar, 6)
                .AddRecipeGroup("Fargowiltas:AnyWoodenPlatform", 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void Load()
        {
            On_Player.ItemCheck_UseMiningTools_ActuallyUseMiningTool += ItemCheck_UseMiningTools_ActuallyUseMiningTool_Detour;
        }
        public static void ItemCheck_UseMiningTools_ActuallyUseMiningTool_Detour(On_Player.orig_ItemCheck_UseMiningTools_ActuallyUseMiningTool orig, Player self, Item sItem, out bool canHitWalls, int x, int y)
        {
            if (sItem.type == ModContent.ItemType<BridgeAtomizer>())
            {
                canHitWalls = true;
                if (WorldGen.InWorld(x, y) && Main.tile[x, y].HasTile)
                {
                    int type = Main.tile[x, y].TileType;
                    if (TileID.Sets.Platforms[type])
                    {
                        orig(self, sItem, out canHitWalls, x, y);
                        if (!Main.tile[x, y].HasTile) // killed successfully; propagate platform kill
                        {
                            Point tileCenter = new(x, y);
                            int left = 0;
                            int right = 0;
                            for (int dir = -1; dir <= 1; dir += 2)
                            {
                                for (int i = 1; i < Main.maxTilesX; i++)
                                {
                                    if (dir == -1)
                                        left++;
                                    else
                                        right++;

                                    Point pos = new(tileCenter.X + dir * i, tileCenter.Y);
                                    if (pos.X < 0 || pos.X >= Main.maxTilesX || pos.Y < 0 || pos.Y >= Main.maxTilesY)
                                        break;

                                    if (Main.tile[pos].HasTile && Main.tile[pos].TileType > TileID.Dirt && TileID.Sets.Platforms[Main.tile[pos].TileType] && FargoGlobalProjectile.OkayToDestroyTileAt(pos.X, pos.Y))
                                    {
                                        WorldGen.KillTile(pos.X, pos.Y, noItem: true);
                                        continue;
                                    }
                                    break;
                                }
                            }

                            if (Main.netMode != NetmodeID.SinglePlayer)
                                NetMessage.SendTileSquare(-1, tileCenter.X - left, tileCenter.Y, left + right, 1, TileChangeType.None);
                        }
                        
                    }
                }
                
                return;
            }
            orig(self, sItem, out canHitWalls, x, y);
        }
    }
}