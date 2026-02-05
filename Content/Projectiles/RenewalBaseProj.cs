using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles
{
    public class RenewalBaseProj : ModProjectile
    {
        private readonly string name;
        private readonly int projType;
        private readonly int convertType;
        private readonly bool supreme;

        protected RenewalBaseProj(string name, int projType, int convertType, bool supreme)
        {
            this.name = name;
            this.projType = projType;
            this.convertType = convertType;
            this.supreme = supreme;
        }

        public override string Texture => "Fargowiltas/Content/Items/Renewals/" + name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault(name);
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 170;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);

            int radius = 150;
            float[] speedX = [0, 0, 5, 5, 5, -5, -5, -5];
            float[] speedY = [5, -5, 0, 5, -5, 0, 5, -5];

            //because these projs may apparently delete blocks if spawned in unloaded chunks far away from players in mp
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                for (int i = 0; i < 8; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speedX[i], speedY[i], projType, 0, 0, Main.myPlayer);
                }
            }
            if (supreme)
            {
                for (int x = -Main.maxTilesX; x < Main.maxTilesX; x++)
                {
                    for (int y = -Main.maxTilesY; y < Main.maxTilesY; y++)
                    {
                        int xPosition = (int)(x + Projectile.Center.X / 16.0f);
                        int yPosition = (int)(y + Projectile.Center.Y / 16.0f);

                        WorldGen.Convert(xPosition, yPosition, convertType, 1); 
                    }
                }
            }
            else
            {
                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        int xPosition = (int)(x + Projectile.Center.X / 16.0f);
                        int yPosition = (int)(y + Projectile.Center.Y / 16.0f);

                        // Circle
                        if (Math.Sqrt(x * x + y * y) <= radius + 0.5)
                        {
                            WorldGen.Convert(xPosition, yPosition, convertType, 1);
                        }
                    }
                }
            }
        }
    }
}
