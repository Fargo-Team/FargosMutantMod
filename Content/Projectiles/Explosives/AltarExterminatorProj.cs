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
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (WorldGen.InWorld(i, j))
                    {
                        Tile tile = Framing.GetTileSafely(i, j);
                        if (FargoSets.Tiles.EvilAltars[tile.TileType])
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
    }
}