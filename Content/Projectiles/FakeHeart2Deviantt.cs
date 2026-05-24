using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles
{
    public class FakeHeart2Deviantt : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Projectiles/FakeHeartDeviantt";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = 20;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            float rand = Main.rand.Next(90, 111) * 0.01f * (Main.essScale * 0.5f);
            Lighting.AddLight(Projectile.Center, 0.5f * rand, 0.1f * rand, 0.1f * rand);

            if (++Projectile.localAI[0] == 30)
            {
                Projectile.localAI[1] = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }

            if (--Projectile.ai[1] == 0)
            {
                Projectile.velocity = Projectile.localAI[1].ToRotationVector2() * -12.5f;
            }
            else if (Projectile.ai[1] < 0)
            {
                if (Projectile.ai[0] >= 0 && Projectile.ai[0] < 200)
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

                        Projectile.velocity = Projectile.velocity.RotatedBy(num4 * (Projectile.Distance(Main.npc[ai0].Center) > 100 ? 0.6f : 0.2f));
                    }
                    else
                    {
                        Projectile.ai[0] = -1f;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (++Projectile.localAI[1] > 12f)
                    {
                        Projectile.localAI[1] = 0f;
                        float maxDistance = 700f;
                        int possibleTarget = -1;
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.CanBeChasedBy())
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
            }

            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }

            Projectile.rotation -= (float)Math.PI / 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
            target.AddBuff(BuffID.Lovestruck, 600);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, lightColor.G, lightColor.B, lightColor.A);
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

        //    for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
        //    {
        //        Color color27 = color26;
        //        color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
        //        Vector2 value4 = Projectile.oldPos[i];
        //        float num165 = Projectile.oldRot[i];
        //        Main.spriteBatch.Draw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, SpriteEffects.None, 0f);
        //    }

        //    Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0f);
        //    return false;
        //}
    }
}