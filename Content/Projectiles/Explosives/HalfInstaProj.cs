using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class HalfInstaProj : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Items/Explosives/HalfInstavator";

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
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item14, position);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            //cavern height plus halfway to hell
            int yEndpoint = (int)(Main.rockLayer + (Main.maxTilesY - 200 - Main.rockLayer) / 2);

            // Five across
            for (int x = -2; x <= 2; x++)
            {
                for (int y = (int)(1 + position.Y / 16.0f); y <= yEndpoint; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);

                    if (xPosition < 0 || xPosition >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                        continue;

                    Tile tile = Main.tile[xPosition, y];

                    if (tile == null)
                        continue;

                    if (!FargoGlobalProjectile.OkayToDestroyTileAt(xPosition, y))
                        continue;

                    FargoGlobalTile.ClearEverything(xPosition, y, false);

                    if (x == 0)
                    {
                        WorldGen.PlaceTile(xPosition, y, TileID.Rope);
                    }

                    
                }
            }

            int yStart = (int)(1 + position.Y / 16.0f);
            NetMessage.SendTileSquare(-1, (int)(position.X / 16f) - 2, yStart, 5, yEndpoint + 1 - yStart);
        }
    }
}