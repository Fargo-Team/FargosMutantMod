using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class OmniBridgifierProj : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Items/Explosives/OmniBridgifier";
        protected virtual int TileHeight => 4;
        protected virtual int Placeable => Projectile.ai[0] == 0 ? ModContent.TileType<OmnistationSheet>() : ModContent.TileType<OmnistationSheet2>();
        protected virtual bool Replaceable(int TileType) => TileType == ModContent.TileType<OmnistationSheet>() || TileType == ModContent.TileType<OmnistationSheet2>();

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 36;
            Projectile.aiStyle = -1;
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
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            bool TryPlaceOmni(int xPos, int yPos)
            {
                for (int i = -1; i <= 0; i++)
                {
                    for (int j = -TileHeight; j < 0; j++)
                    {
                        int x = xPos + i;
                        int y = yPos + j;

                        if (WorldGen.InWorld(x, y))
                        {
                            Tile tile = Framing.GetTileSafely(x, y);
                            if (tile == null || tile.TileType != 0)
                                return false;
                        }
                    }
                }

                WorldGen.PlaceTile(xPos, yPos - TileHeight / 2, Placeable);
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, xPos - 1, yPos - TileHeight, 2, TileHeight);

                return true;
            }

            void PlaceInDirection(int direction)
            {
                Vector2 position = Projectile.Center;

                int xPosition = (int)(position.X / 16f);
                int yPosition = (int)(position.Y / 16f);
                const int interval = 128;
                int i = interval / 2;
                bool allowOneGap = true;
                while (WorldGen.InWorld(xPosition, yPosition))
                {
                    Tile platformTile = Framing.GetTileSafely(xPosition, yPosition);
                    if (platformTile != null)
                    {
                        if (platformTile.TileType == TileID.Platforms)
                        {
                            allowOneGap = true;

                            //reset interval if it finds an omni already placed
                            Tile tileAbove = Framing.GetTileSafely(xPosition, yPosition - 1);
                            if (tileAbove != null && Replaceable(tileAbove.TileType))
                            {
                                i = interval;
                            }

                            if (i <= 0 && TryPlaceOmni(xPosition, yPosition))
                            {
                                i = interval;
                            }
                        }
                        else if (platformTile.TileType == 0) //if air, allow one gap
                        {
                            if (allowOneGap)
                                allowOneGap = false;
                            else
                                break;
                        }
                        else
                        {
                            break;
                        }
                    }

                    xPosition += direction;
                    i--;
                }
            }

            PlaceInDirection(1);
            PlaceInDirection(-1);
        }
    }
}