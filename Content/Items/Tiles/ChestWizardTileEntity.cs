using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Fargowiltas.Content.Items.Tiles
{
    
    public class ChestWizardTileEntity : ModTileEntity
    {
        public float drawTimer;
        public int hatID;
        public override void Update()
        {
                       
            base.Update();
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(hatID);
        }
        public override void NetReceive(BinaryReader reader)
        {
            hatID = reader.ReadInt32();
        }
        public override void SaveData(TagCompound tag)
        {
            tag["hat"] = hatID;
        }
        public override void LoadData(TagCompound tag)
        {
            hatID = tag.GetInt("hat");
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
                hatID = Main.rand.Next([ItemID.WizardHat, ItemID.WizardsHat, ItemID.RuneHat, ItemID.MagicHat]);
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }
    }
}
