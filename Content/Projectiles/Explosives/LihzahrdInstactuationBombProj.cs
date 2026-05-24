using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Projectiles.Explosives
{
    public class LihzahrdInstactuationBombProj : ModProjectile
    {
        public override string Texture => "Fargowiltas/Content/Items/Explosives/LihzahrdInstactuationBomb";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 36;
            Projectile.aiStyle = ProjAIStyleID.Explosive;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            int xPos = (int)Projectile.Center.X / 16;
            int yPos = (int)Projectile.Center.Y / 16;

            bool WipeColumn(int i)
            {
                for (int j = 0; j >= -60; j--)
                {
                    int tileX = xPos + i;
                    int tileY = yPos + j;

                    if (tileX < 0 || tileX > Main.maxTilesX || tileY <= 0 || tileY > Main.maxTilesY)
                    {
                        if (j == 0)
                            return false;
                        continue;
                    }

                    Tile tile = Framing.GetTileSafely(tileX, tileY);

                    if (tile.TileType == TileID.LihzahrdAltar)
                        continue;

                    if (tile.WallType != WallID.LihzahrdBrickUnsafe)
                    {
                        if (j == 0)
                            return false;
                        continue;
                    }

                    //check for chest above this block
                    Tile tileAbove = Framing.GetTileSafely(tileX, tileY - 1);
                    if (TileID.Sets.BasicChest[tileAbove.TileType])
                    {
                        TileObjectData data = TileObjectData.GetTileData(tileAbove.TileType, 0);
                        int x = tileX - tile.TileFrameX / 18 % data.Width;
                        int y = tileY - 1 - tile.TileFrameY / 18 % data.Height; //get top left of chest

                        WorldGen.KillTile(x, y);
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, x, y, 3);

                        //if couldnt destroy chest, ignore this block
                        if (TileID.Sets.BasicChest[tileAbove.TileType])
                            continue;
                    }

                    if (TileID.Sets.BasicChest[tile.TileType])
                    {
                        TileObjectData data = TileObjectData.GetTileData(tile.TileType, 0);
                        int x = tileX - tile.TileFrameX / 18 % data.Width;
                        int y = tileY - tile.TileFrameY / 18 % data.Height; //get top left of chest

                        WorldGen.KillTile(x, y); //try to kill chest
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, x, y, 3);

                        continue;
                    }

                    if (tile.TileType == TileID.LihzahrdBrick)
                    {
                        tile.IsActuated = true; //actuate it
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, tileX, tileY, 1);

                        continue;
                    }

                    WorldGen.KillTile(tileX, tileY);
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendTileSquare(-1, tileX, tileY, 1);
                }

                return true;
            }

            int leftMax = 60;
            int rightMax = 60;

            int leftTry = 0;
            for (; leftTry >= -leftMax; leftTry--) //try clearing left side
            {
                if (!WipeColumn(leftTry)) //if went OOB or exited temple before reaching normal left limit, give up
                {
                    rightMax += leftMax - Math.Abs(leftTry); //try to extend right side by this much
                    //Main.NewText($"Extended right max to {rightMax}");
                    break;
                }
            }

            for (int rightTry = 0; rightTry <= rightMax; rightTry++) //try clearing right side
            {
                if (!WipeColumn(rightTry)) //if went OOB or exited temple before reaching normal right limit, give up
                {
                    leftMax += rightMax - rightTry; //try to extend left side by this much
                    //Main.NewText($"Extended left max to {leftMax}");
                    for (; leftTry >= -leftMax; leftTry--) //try left one more time with the new extended limit
                    {
                        if (!WipeColumn(leftTry))
                            break;
                    }
                    break;
                }
            }
        }
    }
}