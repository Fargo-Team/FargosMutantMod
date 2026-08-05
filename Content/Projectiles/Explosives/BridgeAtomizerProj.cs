using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class BridgeAtomizerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 40;
            Projectile.tileCollide = false;
            Projectile.aiStyle = ProjAIStyleID.Drill;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.hide = true;
        }

        public override void AI()
        {
            if (Projectile.frameCounter++ >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }
        }

        public override bool PreDraw(Player player, ref Color lightColor)
        {
            Texture2D t = TextureAssets.Projectile[Type].Value;
            int sizeY = t.Height / Main.projFrames[Type];
            int frameY = Projectile.frame * sizeY;
            Rectangle rectangle = new(0, frameY, t.Width, sizeY);
            Vector2 origin = rectangle.Size() / 2f;
            Vector2 pos = Projectile.Center - Main.screenPosition + new Vector2(0f, 4 + Projectile.gfxOffY);
            float rot = Projectile.rotation + MathHelper.PiOver2;
            if (Projectile.spriteDirection > 0)
                rot += MathHelper.Pi;
            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(t, pos, rectangle, Projectile.GetAlpha(lightColor),
                    rot, origin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}