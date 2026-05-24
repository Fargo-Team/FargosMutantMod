using Fargowiltas.Content.Items.Explosives;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class InstaHouseVisual : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 96;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 mouse = Main.MouseWorld;

            if (player.position.X > mouse.X)
            {
                Projectile.position.X = mouse.X - Projectile.width + 4;
                Projectile.position.Y = mouse.Y - Projectile.height + 8 + 16;
            }
            else
            {
                Projectile.position.X = mouse.X - 4;
                Projectile.position.Y = mouse.Y - Projectile.height + 8 + 16;
            }

            Projectile.timeLeft++;

            if (player.HeldItem.type != ModContent.ItemType<AutoHouse>())
            {
                Projectile.Kill();
            }

            Projectile.hide = Projectile.owner != Main.myPlayer;
        }
    }
}