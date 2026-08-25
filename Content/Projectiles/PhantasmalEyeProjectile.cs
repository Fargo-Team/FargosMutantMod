using Fargowiltas.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles;

public class PhantasmalEyeProjectile : ModProjectile
{
    private float HomingCooldown
    {
        get => Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("PhantasmalEyeProjectile");
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
    }

    public override void SetDefaults()
    {
        Projectile.width = 9;
        Projectile.height = 16;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.npcProj = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = 50;
        Projectile.timeLeft = 600;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Ichor, 240);
        target.AddBuff(BuffID.BetsysCurse, 240);
    }

    public override void AI()
    {
        const int homingDelay = 10;
        const float flySpeed = 60; // fly speed in pixels per frame
        const int lerpFrameAmount = 20; // minimum of 1

        HomingCooldown++;
        if (HomingCooldown > homingDelay)
        {
            HomingCooldown = homingDelay; // cap this value

            int foundTarget = HomeOnTarget();
            if (foundTarget != -1)
            {
                NPC n = Main.npc[foundTarget];
                if (Projectile.Distance(n.Center) > Math.Max(n.width, n.height))
                {
                    Vector2 desiredVelocity = Projectile.DirectionTo(n.Center) * flySpeed;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 1f / lerpFrameAmount);

                    double num4 = (n.Center - Projectile.Center).ToRotation() - Projectile.velocity.ToRotation();
                    if (num4 > Math.PI)
                    {
                        num4 -= 2.0 * Math.PI;
                    }

                    if (num4 < -1.0 * Math.PI)
                    {
                        num4 += 2.0 * Math.PI;
                    }

                    Projectile.velocity = Projectile.velocity.RotatedBy(num4 * (Projectile.Distance(n.Center) > 100 ? 0.4f : 0.1f));
                }
            }
        }

        if (Projectile.velocity.Length() < 2)
            Projectile.velocity *= 1.03f;

        Projectile.rotation = Projectile.velocity.ToRotation() + 1.570796f;

        Projectile.localAI[0] += 0.1f;
        if (Projectile.localAI[0] > ProjectileID.Sets.TrailCacheLength[Projectile.type])
            Projectile.localAI[0] = ProjectileID.Sets.TrailCacheLength[Projectile.type];

        Projectile.localAI[1] += 0.25f;
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Zombie_103"), Projectile.Center);

        Projectile.position = Projectile.Center;
        Projectile.width = Projectile.height = 144;
        Projectile.Center = Projectile.position;

        for (int i = 0; i < 2; i++)
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0.0f, 0.0f, 100, new Color(), 1.5f);

        for (int i = 0; i < 20; i++)
        {
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, 0.0f, 0.0f, 0, new Color(), 2.5f);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity *= 3f;

            d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, 0.0f, 0.0f, 100, new Color(), 1.5f);
            Main.dust[d].velocity *= 2f;
            Main.dust[d].noGravity = true;
        }
    }

    private int HomeOnTarget()
    {
        const float HOMING_MAXIMUM_RANGE_IN_PIXELS = 1000;

        int selectedTarget = -1;
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC n = Main.npc[i];
            if (n.CanBeChasedBy(Projectile))
            {
                float distance = Projectile.Distance(n.Center);
                if (distance <= HOMING_MAXIMUM_RANGE_IN_PIXELS && (selectedTarget == -1 || Projectile.Distance(Main.npc[selectedTarget].Center) > distance))
                {
                    selectedTarget = i;
                }
            }
        }

        if (selectedTarget == -1)
            selectedTarget = NPC.FindFirstNPC(ModContent.NPCType<Mutant>());

        return selectedTarget;
    }

    public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
        int rect1 = glow.Height / Main.projFrames[Projectile.type];
        int rect2 = rect1 * Projectile.frame;
        Rectangle glowrectangle = new Rectangle(0, rect2, glow.Width, rect1);
        Vector2 gloworigin2 = glowrectangle.Size() / 2f;
        Color glowcolor = Color.Lerp(new Color(31, 187, 192, 0), Color.Transparent, 0.74f);
        Vector2 drawCenter = Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.UnitX) * 14;

        for (int i = 0; i < 3; i++) //create multiple transparent trail textures ahead of the projectile
        {
            Vector2 drawCenter2 = drawCenter + (Projectile.velocity.SafeNormalize(Vector2.UnitX) * 8).RotatedBy(MathHelper.Pi / 5 - i * MathHelper.Pi / 5); //use a normalized version of the Projectile's velocity to offset it at different angles
            drawCenter2 -= Projectile.velocity.SafeNormalize(Vector2.UnitX) * 8; //then move it backwards
            float scale = Projectile.scale;
            scale += (float)Math.Sin(Projectile.localAI[1]) / 10;
            Main.spriteBatch.Draw(glow, drawCenter2 - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glowrectangle),
                glowcolor, Projectile.velocity.ToRotation() + MathHelper.PiOver2, gloworigin2, scale, SpriteEffects.None, 0f);
        }

        for (float i = Projectile.localAI[0] - 1; i > 0; i -= Projectile.localAI[0] / 5) //trail grows in length as Projectile travels
        {

            float lerpamount = 0.2f;
            if (i > 5 && i < 10)
                lerpamount = 0.4f;
            if (i >= 10)
                lerpamount = 0.6f;

            Color color27 = Color.Lerp(glowcolor, Color.Transparent, 0.1f + lerpamount);

            color27 *= (int)((Projectile.localAI[0] - i) / Projectile.localAI[0]) ^ 2;
            float scale = Projectile.scale * (float)(Projectile.localAI[0] - i) / Projectile.localAI[0];
            scale += (float)Math.Sin(Projectile.localAI[1]) / 10;
            Vector2 value4 = Projectile.oldPos[(int)i] - Projectile.velocity.SafeNormalize(Vector2.UnitX) * 14;
            Main.spriteBatch.Draw(glow, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(glowrectangle), color27,
                Projectile.velocity.ToRotation() + MathHelper.PiOver2, gloworigin2, scale * 0.8f, SpriteEffects.None, 0f);
        }

        return false;
    }

    public override void PostDraw(Color lightColor)
    {
        Texture2D texture2D13 = ModContent.Request<Texture2D>(Texture).Value;
        int num156 = texture2D13.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
        int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
        Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
        Vector2 origin2 = rectangle.Size() / 2f;
        Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0f);
    }
}