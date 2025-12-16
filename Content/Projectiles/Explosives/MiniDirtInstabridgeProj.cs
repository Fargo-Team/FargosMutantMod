using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class MiniDirtInstabridgeProj : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Items/Explosives/MiniDirtInstaBridge";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 36;
            Projectile.aiStyle = 16;
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

            // All the way across
            const int length = 150;
            bool goLeft = Projectile.Center.X < Main.player[Projectile.owner].Center.X;
            int min = goLeft ? -length : 0;
            int max = goLeft ? 0 : length;

            int[] deletableTiles = [ 
                TileID.Cactus,
                TileID.Trees,
                TileID.CorruptThorns,
                TileID.CrimsonThorns,
                TileID.JungleThorns
            ];

            for (int x = min; x < max; x++)
            {
                int xPosition = (int)(x + position.X / 16.0f);
                int yPosition = (int)(position.Y / 16.0f);

                if (xPosition < 0 || xPosition >= Main.maxTilesX || yPosition < 0 || yPosition >= Main.maxTilesY)
                    continue;

                Tile tile = Main.tile[xPosition, yPosition];

                if (tile == null)
                {
                    continue;
                }

                if (deletableTiles.Contains(tile.TileType))
                {
                    FargoGlobalTile.ClearEverything(xPosition, yPosition);
                }

                // Spawn platforms
                WorldGen.PlaceTile(xPosition, yPosition, TileID.Dirt);

                NetMessage.SendTileSquare(-1, xPosition, yPosition, 1);
            }
        }
    }
}