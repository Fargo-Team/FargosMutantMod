using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Security.Principal;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class ShurikenProj : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Items/Explosives/BoomShuriken";
        public override void SetDefaults()
        {
            Projectile.width = 11;
            Projectile.height = 11;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = 5;
            Projectile.aiStyle = 2;
            Projectile.timeLeft = 600;
            AIType = 48;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.timeLeft = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.timeLeft = 0;
            return false;
        }

        bool tryExplode;

        public override bool CanHitPlayer(Player target)
        {
            return false;
            //if (target.whoAmI != Projectile.owner)
            //    return false;
            //return base.CanHitPlayer(target);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            //modifiers.IncomingDamageMultiplier *= 1.5f;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.hostile = true;

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.Center = Projectile.position;

            if (!tryExplode)
            {
                tryExplode = true;
                Projectile.Damage();
            }

            for (int i = 0; i < 30; i++)
            {
                int num616 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Alpha: 100, Scale: 1.5f);
                Main.dust[num616].velocity *= 1.4f;
            }

            for (int i = 0; i < 2; i++)
            {
                float scaleFactor = 0.4f;
                if (i == 1)
                {
                    scaleFactor = 0.8f;
                }

                int num620 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, default, Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor;

                Gore gore97 = Main.gore[num620];
                gore97.velocity.X += 1f;

                Gore gore98 = Main.gore[num620];
                gore98.velocity.Y += 1f;

                num620 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, default, Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor;

                Gore gore99 = Main.gore[num620];
                gore99.velocity.X -= 1f;

                Gore gore100 = Main.gore[num620];
                gore100.velocity.Y += 1f;

                num620 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, default, Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor;

                Gore gore101 = Main.gore[num620];
                gore101.velocity.X += 1f;

                Gore gore102 = Main.gore[num620];
                gore102.velocity.Y -= 1f;

                num620 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, default, Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor;

                Gore gore103 = Main.gore[num620];
                gore103.velocity.X -= 1f;

                Gore gore104 = Main.gore[num620];
                gore104.velocity.Y -= 1f;
            }

            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item14, position);
            int radius = 16;     // bigger = boomer

            Player player = Main.player[Projectile.owner];

            NetMessage.SendData(MessageID.KillProjectile, -1, -1, null, Projectile.identity, Projectile.owner);

            AchievementsHelper.CurrentlyMining = true;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);

                    if (xPosition < 0 || xPosition >= Main.maxTilesX || yPosition < 0 || yPosition >= Main.maxTilesY)
                        continue;

                    Tile tile = Main.tile[xPosition, yPosition];


                    // Circle
                    if (x * x + y * y <= radius)
                    {
                        if (tile.IsActuated || FargoGlobalProjectile.TileIsLiterallyAir(tile) || FargoGlobalProjectile.TileBelongsToMagicStorage(tile))
                            continue;

                        if (player.HasEnoughPickPowerToHurtTile(xPosition, yPosition) && WorldGen.CanKillTile(xPosition, yPosition))
                        {
                            WorldGen.KillTile(xPosition, yPosition);
                            /*
                            if (Main.netMode != NetmodeID.SinglePlayer)
                            {
                                NetMessage.SendTileSquare(-1 , ,,,,)
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 20, xPosition, yPosition);
                            }
                            */
                        }

                        Dust.NewDust(position, 22, 22, DustID.Smoke, 0.0f, 0.0f, 120);
                    }
                }
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                Point center = position.ToTileCoordinates();
                NetMessage.SendTileSquare(-1, center.X - radius * 2, center.Y - radius * 2, 4 * radius);
            }
            AchievementsHelper.CurrentlyMining = false;
        }
    }
}