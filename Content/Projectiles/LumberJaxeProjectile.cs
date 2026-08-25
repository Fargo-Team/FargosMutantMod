using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles;

public class LumberJaxeProjectile : ModProjectile
{
    public override string Texture => "Fargowiltas/Content/Items/Weapons/LumberJaxe";
    public override void SetDefaults()
    {
        Projectile.width = 42;
        Projectile.height = 40;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.penetrate = 1;
        Projectile.aiStyle = 0;
        Projectile.timeLeft = 150;
        AIType = ProjectileID.CrystalBullet;
    }

    public override void AI()
    {
        Projectile.rotation += 0.3f;
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (target.type == NPCID.MourningWood || target.type == NPCID.Everscream || target.type == NPCID.Splinterling)
        {
            modifiers.FinalDamage *= 10;
        }
        base.ModifyHitNPC(target, ref modifiers);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Projectile.Kill();
    }

    public override void OnKill(int timeLeft)
    {
        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0, Projectile.velocity.Y * 0, ModContent.ProjectileType<Explosion>(), (int)(Projectile.damage * 1f), Projectile.knockBack, Projectile.owner);
    }
}