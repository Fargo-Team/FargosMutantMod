using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles;

public class MechEyeProjectile : ModProjectile
{
    private float HomingCooldown
    {
        get => Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("MechEyeProjectile");
    }

    public override void SetDefaults()
    {
        Projectile.width = 8;
        Projectile.height = 12;
        Projectile.aiStyle = ProjAIStyleID.Arrow;
        Projectile.friendly = true;
        Projectile.npcProj = true;
        Projectile.penetrate = 5;
        Projectile.timeLeft = 600;
        AIType = ProjectileID.Bullet;
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
                if (Projectile.Distance(n.Center) > System.Math.Max(n.width, n.height))
                {
                    Vector2 desiredVelocity = Projectile.DirectionTo(n.Center) * flySpeed;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 1f / lerpFrameAmount);
                }
            }
        }
    }

    public override void OnKill(int timeLeft)
    {
        for (int num468 = 0; num468 < 20; num468++)
        {
            int num469 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.Silver, -Projectile.velocity.X * 0.2f, -Projectile.velocity.Y * 0.2f, 100, default, 1.5f);
            Main.dust[num469].noGravity = true;
            Main.dust[num469].velocity *= 2f;
            num469 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.Silver, -Projectile.velocity.X * 0.2f, -Projectile.velocity.Y * 0.2f, 100, default, .75f);
            Main.dust[num469].velocity *= 2f;
        }
    }

    private int HomeOnTarget()
    {
        const float HOMING_MAXIMUM_RANGE_IN_PIXELS = 500;

        int selectedTarget = -1;
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC n = Main.npc[i];
            if (n.CanBeChasedBy(Projectile) && Collision.CanHitLine(Projectile.Center, 0, 0, n.Center, 0, 0))
            {
                float distance = Projectile.Distance(n.Center);
                if (distance <= HOMING_MAXIMUM_RANGE_IN_PIXELS && (selectedTarget == -1 || Projectile.Distance(Main.npc[selectedTarget].Center) > distance))
                {
                    selectedTarget = i;
                }
            }
        }

        return selectedTarget;
    }
}