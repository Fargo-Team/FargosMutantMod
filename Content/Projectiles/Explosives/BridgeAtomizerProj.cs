using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class BridgeAtomizerProj : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Items/Explosives/InstaBridge";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 36;
            Projectile.aiStyle = ProjAIStyleID.Explosive;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item14, position);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            Point tileCenter = position.ToTileCoordinates();
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