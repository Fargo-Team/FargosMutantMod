using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles
{
    public class Explosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Explosion");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = false;
            Projectile.light = 0.75f;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.Center = Projectile.position;

            for (int i = 0; i < 30; i++)
            {
                int num616 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Alpha: 100, Scale: 1.5f);
                Main.dust[num616].velocity *= 1.4f;
            }

            //for (int i = 0; i < 20; i++)
            //{
            //    int num618 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Fire, Alpha: 100, Scale: 3.5f);
            //    Main.dust[num618].noGravity = true;
            //    Main.dust[num618].velocity *= 7f;

            //    num618 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Fire, Alpha: 100, Scale: 1.5f);
            //    Main.dust[num618].velocity *= 3f;
            //}

            for (int i = 0; i < 2; i++)
            {
                float scaleFactor = 0.4f;
                if (i == 1)
                {
                    scaleFactor = 0.8f;
                }

                int num620 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, default, Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor;

                Gore gore97 = Main.gore[num620];
                gore97.velocity.X += 1f;

                Gore gore98 = Main.gore[num620];
                gore98.velocity.Y += 1f;

                num620 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, default, Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor;

                Gore gore99 = Main.gore[num620];
                gore99.velocity.X -= 1f;

                Gore gore100 = Main.gore[num620];
                gore100.velocity.Y += 1f;

                num620 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, default, Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor;

                Gore gore101 = Main.gore[num620];
                gore101.velocity.X += 1f;

                Gore gore102 = Main.gore[num620];
                gore102.velocity.Y -= 1f;

                num620 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, default, Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor;

                Gore gore103 = Main.gore[num620];
                gore103.velocity.X -= 1f;

                Gore gore104 = Main.gore[num620];
                gore104.velocity.Y -= 1f;
            }
        }
    }
}