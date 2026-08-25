using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class InstaProj : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Items/Explosives/Instavator";

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

            Player player = Main.player[Projectile.owner];
            var pickaxe = player.GetBestPickaxe() ?? ContentSamples.ItemsByType[ItemID.CopperPickaxe];

            // Seven across
            int yStart = (int)(1 + position.Y / 16.0f);
            
            int yEnd = Main.UnderworldLayer - 10;
            for (int x = -3; x <= 3; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);

                    if (xPosition < 0 || xPosition >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                        continue;

                    Tile tile = Main.tile[xPosition, y];

                    if (tile == null)
                        continue;

                    if (!FargoGlobalProjectile.InstaDestructionCheck(xPosition, y, player, pickaxe))
                        continue;

                    FargoGlobalTile.ClearEverything(xPosition, y, false);

                    // Spawn structure
                    WorldGen.PlaceWall(xPosition, y, WallID.Stone);

                    if (x == -3 || x == 3)
                    {
                        WorldGen.PlaceTile(xPosition, y, TileID.GrayBrick);
                    }
                    else if ((x == -2 || x == 2) && y % 10 == 0)
                    {
                        WorldGen.PlaceTile(xPosition, y, TileID.Torches);
                    }
                    else if (x == 0)
                    {
                        WorldGen.PlaceTile(xPosition, y, TileID.Rope);
                    }
                }
            }

            NetMessage.SendTileSquare(-1, (int)(position.X / 16.0f) - 3, yStart, 7, yEnd - yStart + 1);
        }
    }
}