using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles
{
    public class DeathScythe : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_274";

        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Abominationn Scythe");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.npcProj = true;
            Projectile.penetrate = 50;
            Projectile.scale = 1f;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.ai[0] = -1;
                SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
            }

            Projectile.rotation += 1f;

            const int aislotHomingCooldown = 1;
            const int homingDelay = 30;
            const float desiredFlySpeedInPixelsPerFrame = 70;
            const float amountOfFramesToLerpBy = 10; // Minimum of 1, please keep in full numbers even though it's a float!

            Projectile.ai[aislotHomingCooldown]++;
            if (Projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                Projectile.ai[aislotHomingCooldown] = homingDelay; // Cap this value

                Projectile.ai[0] = HomeOnTarget();
                if (Projectile.ai[0] > -1 && Projectile.ai[0] < 200)
                {
                    NPC n = Main.npc[(int)Projectile.ai[0]];
                    if (n.active && n.CanBeChasedBy())
                    {
                        Vector2 desiredVelocity = Projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                    }
                }
                else
                {
                    Projectile.ai[0] = -1;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 600);
        }

        //public override bool PreDraw(ref Color lightColor)
        //{
        //    Texture2D texture2D13 = Main.ProjectileTexture[Projectile.type];
        //    int num156 = Main.ProjectileTexture[Projectile.type].Height / Main.projFrames[Projectile.type]; // Y-pos of lower right corner of sprite to draw
        //    int y3 = num156 * Projectile.frame; // Y-pos of upper left corner of sprite to draw
        //    Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
        //    Vector2 origin2 = rectangle.Size() / 2f;

        //    Color color26 = lightColor;
        //    color26 = Projectile.GetAlpha(color26);

        //    SpriteEffects effects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        //    for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
        //    {
        //        Color color27 = color26 * 0.5f;
        //        color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
        //        Vector2 value4 = Projectile.oldPos[i];
        //        float num165 = Projectile.oldRot[i];
        //        Main.spriteBatch.Draw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, effects, 0f);
        //    }

        //    Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, effects, 0f);
        //    return false;
        //}

        private int HomeOnTarget()
        {
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 1000;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.CanBeChasedBy(Projectile) && (!n.wet || homingCanAimAtWetEnemies))
                {
                    float distance = Projectile.Distance(n.Center);
                    if (distance <= homingMaximumRangeInPixels && (selectedTarget == -1 || Projectile.Distance(Main.npc[selectedTarget].Center) > distance))
                    {
                        selectedTarget = i;
                    }
                }
            }

            return selectedTarget;
        }
    }
}