using Fargowiltas.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles
{
    public class CoolCrab : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
            Main.projPet[Type] = true;

            ProjectileID.Sets.CharacterPreviewAnimations[Type] =
                ProjectileID.Sets.SimpleLoop(0, 8, 5)
                .WithOffset(new(-2, -2))
                .WithCode(CustomPreviewAnimation);
        }

        private void CustomPreviewAnimation(Projectile proj, bool walking)
        {
            if (walking)
            {
                if (proj.frame > 6)
                    proj.frame = 0;
            }
            else
            {
                proj.frame = 0;
            }
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Turtle);
            AIType = ProjectileID.Turtle;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.turtle = false; // Relic from AIType
            return true;
        }
        public int FrameCounter = 0;
        public int Frame = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            var modPlayer = player.GetModPlayer<FargoPlayer>();
            if (player.dead)
            {
                modPlayer.CoolCrab = false;
            }
            if (modPlayer.CoolCrab)
            {
                Projectile.timeLeft = 2;
            }
            int startFrame;
            int endFrame;
            int animFrames = 5;

            if (Projectile.ai[0] == 1)
            {
                startFrame = endFrame = Main.projFrames[Type] - 1;
            }
            else
            {
                startFrame = 0;
                endFrame = Main.projFrames[Type] - 2;
                if (Projectile.velocity.X == 0)
                    endFrame = 0;
            }
            if (Frame < startFrame || Frame > endFrame)
            {
                Frame = startFrame;
            }
            if (++FrameCounter > animFrames)
            {
                FrameCounter = 0;
                if (++Frame > endFrame)
                    Frame = startFrame;
            }

            Projectile.direction = Projectile.spriteDirection = MathF.Sign(player.Center.X - Projectile.Center.X);

        }
        public override bool PreDraw(Player player, ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            int num156 = texture2D13.Height / 8; //ypos of lower right corner of sprite to draw
            int y3 = num156 * (Projectile.isAPreviewDummy ? Projectile.frame : Frame); //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Vector2 drawOffset = Vector2.UnitY * 0 * Projectile.scale;

            SpriteEffects effects = Projectile.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture2D13, Projectile.Center + drawOffset - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, effects, 0);
            return false;
        }
    }
}