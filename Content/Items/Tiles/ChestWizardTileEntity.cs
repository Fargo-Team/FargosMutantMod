using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using System.IO;

namespace Fargowiltas.Content.Items.Tiles
{
    
    public class ChestWizardTileEntity : ModTileEntity
    {
        public float drawTimer;
        public int hatID;
        public int item;
        public override void Update()
        {
            if ((item < 0 || !Main.item[item].active) && item != -1)
            {
                item = -1;
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID);
            }
                       
            base.Update();
        }
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<ChestWizardSheet>();
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            Point16 tileOrigin = new Point16(0, 1);
            int placedEntity = Place(i - tileOrigin.X, j - tileOrigin.Y);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int width = 2;
                int height = 2;
                NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);
                NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
            }
            return placedEntity;
        }
        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write7BitEncodedInt(item);
            base.NetSend(writer);
        }
        public override void NetReceive(BinaryReader reader)
        {
            item = reader.Read7BitEncodedInt();
            base.NetReceive(reader);
        }
    }
}
