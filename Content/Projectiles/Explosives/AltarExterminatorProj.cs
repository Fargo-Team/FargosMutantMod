using Fargowiltas.Common.Systems.Collections;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class AltarExterminatorProj : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Items/Explosives/AltarExterminator";
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
        }

        public override bool? CanDamage() => false;

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath52 with {Pitch = -0.5f}, Projectile.Center);

            if (Main.netMode == NetmodeID.MultiplayerClient || !Main.hardMode)
            {
                return;
            }

            for (int i = -960; i < 960; i += 16)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector2 pos = Main.player[Projectile.owner].Center + (Vector2.UnitX * i) + (Vector2.UnitY * 160);
                    Dust d = Dust.NewDustDirect(pos, 16, 16, DustID.Wraith, Alpha: 100, Scale: Main.rand.NextFloat(2, 3));
                    d.noGravity = true;
                    d.velocity.Y -= Main.rand.NextFloat(1, 12);
                    d.fadeIn = -0.5f;

                    Point p = FindGround(d.position.ToTileCoordinates());
                    d.position = p.ToWorldCoordinates();
                }
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (WorldGen.InWorld(i, j))
                    {
                        Tile tile = Framing.GetTileSafely(i, j);
                        if (FargoTileSets.EvilAltars[tile.TileType])
                        {
                            WorldGen.KillTile(i, j);
                            if (Main.netMode == NetmodeID.Server)
                                NetMessage.SendTileSquare(-1, i, j, 1);
                        }
                    }
                }
            }

            Main.refreshMap = true;
        }

        public static Point FindGround(Point p)
        {
            if (WorldGen.SolidTile(p))
            {
                while (WorldGen.SolidTile(p.X, p.Y + 0) && p.Y >= 1)
                    p.Y--;
            }
            else
            {
                while (!WorldGen.SolidTile(p.X, p.Y - 0) && p.Y < Main.maxTilesY)
                    p.Y++;
            }
            return p;
        }
    }
}