using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles
{
    public class FakeHeartDeviantt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fake Heart");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.npcProj = true;
            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            float rand = Main.rand.Next(90, 111) * 0.01f * (Main.essScale * 0.5f);
            Lighting.AddLight(Projectile.Center, 0.5f * rand, 0.1f * rand, 0.1f * rand);

            /*Projectile.ai[0]--;
            if (Projectile.ai[0] > 0)
            {
                Projectile.rotation = -Projectile.velocity.ToRotation();
            }
            else if (Projectile.ai[0] == 0)
                Projectile.velocity = Vector2.Zero;
            else
            {
                Projectile.ai[1]--;
                if (Projectile.ai[1] == 0)
                {
                    Projectile.velocity = Projectile.DirectionTo(Main.player[Player.FindClosest(Projectile.Center, 0, 0)].Center) * 20;
                    Projectile.netUpdate = true;
                }
                if (Projectile.ai[1] <= 0)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
            }

            Projectile.rotation -= (float)Math.PI / 2;*/

            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.ai[0] = -1;
            }

            if (Projectile.ai[0] >= 0 && Projectile.ai[0] < Main.maxNPCs)
            {
                int ai0 = (int)Projectile.ai[0];
                if (Main.npc[ai0].CanBeChasedBy())
                {
                    double num4 = (Main.npc[ai0].Center - Projectile.Center).ToRotation() - Projectile.velocity.ToRotation();
                    if (num4 > Math.PI)
                    {
                        num4 -= 2.0 * Math.PI;
                    }

                    if (num4 < -1.0 * Math.PI)
                    {
                        num4 += 2.0 * Math.PI;
                    }

                    Projectile.velocity = Projectile.velocity.RotatedBy(num4 * (Projectile.Distance(Main.npc[ai0].Center) > 100 ? 0.4f : 0.1f));
                }
                else
                {
                    Projectile.ai[0] = -1f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                if (++Projectile.localAI[1] > 6f)
                {
                    Projectile.localAI[1] = 0f;
                    float maxDistance = 700f;
                    int possibleTarget = -1;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy() && Collision.CanHitLine(Projectile.Center, 0, 0, npc.Center, 0, 0))
                        {
                            float npcDistance = Projectile.Distance(npc.Center);
                            if (npcDistance < maxDistance)
                            {
                                maxDistance = npcDistance;
                                possibleTarget = i;
                            }
                        }
                    }

                    Projectile.ai[0] = possibleTarget;
                    Projectile.netUpdate = true;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - (float)Math.PI / 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Lovestruck, 600);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, lightColor.G, lightColor.B, lightColor.A) * Projectile.Opacity;
        }

        //public override bool PreDraw(ref Color lightColor)
        //{
        //    Texture2D texture2D13 = Main.ProjectileTexture[Projectile.type];
        //    int num156 = Main.ProjectileTexture[Projectile.type].Height / Main.projFrames[Projectile.type]; // ypos of lower right corner of sprite to draw
        //    int y3 = num156 * Projectile.frame; // ypos of upper left corner of sprite to draw
        //    Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
        //    Vector2 origin2 = rectangle.Size() / 2f;
        //    Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0f);
        //    return false;
        //}
    }
}