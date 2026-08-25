using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles;

public class WorldTokenProj : ModProjectile
{
    public override string Texture => "Fargowiltas/Content/Items/Misc/ModeToggle_0";

    public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Abominationn Scythe");
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
    }

    public override void SetDefaults()
    {
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.friendly = false;
        Projectile.penetrate = -1;
        Projectile.scale = 1f;
        Projectile.timeLeft = 60;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
    }

    public override void AI()
    {
        if (Projectile.TryGetOwner(out var player))
        {
            Projectile.Center = player.Center - Vector2.UnitY * 100;
        }

        Projectile.scale += 2 / 60f;
        Projectile.Opacity -= 1 / 60f;
        if (Projectile.Opacity <= 0.01f)
            Projectile.Kill();
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = (Texture2D)ModContent.Request<Texture2D>($"Fargowiltas/Content/Items/Misc/ModeToggle_{Main.GameMode}");
        Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);
        Vector2 origin = frame.Size() / 2f;
        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(frame), Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
        return false;
    }
}