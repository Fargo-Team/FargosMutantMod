using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles
{
    public class FakeHeartMarkDeviantt : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Projectiles/FakeHeartDeviantt";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.timeLeft = 2;
            Projectile.aiStyle = -1;
            Projectile.hide = true;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = -3; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -Projectile.velocity.RotatedBy(Math.PI / 7 * i), ModContent.ProjectileType<FakeHeart2Deviantt>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -1, 120 + 20 * i);
                }
            }

            Projectile.Kill();
        }

        public override bool? CanDamage()
        {
            return false;
        }
    }
}