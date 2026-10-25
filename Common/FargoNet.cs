using Fargowiltas.Content.Items.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Fargowiltas.Fargowiltas;

namespace Fargowiltas;

public static class FargoNet
{
    public static void SendData(int dataType, int dataA, int dataB, string text, int playerID, float dataC, float dataD, float dataE, int clientType)
    {
        NetMessage.SendData(dataType, dataA, dataB, NetworkText.FromLiteral(text), playerID, dataC, dataD, dataE, clientType);
    }
    public static void SendEnchantedTreeFruitPacket(int treeIndex)
    {
        FargoUtils.TryGetTileEntityAs(EnchantedTreeSheet.EnchantedTrees[treeIndex].X, EnchantedTreeSheet.EnchantedTrees[treeIndex].Y, out EnchantedTreeTileEntity tree);
        ModPacket packet = Instance.GetPacket();
        packet.Write((byte)PacketID.SyncTreeFruit);
        packet.Write(treeIndex);
        packet.Write(tree.ItemType);
        packet.Write(tree.Prefix);
        packet.Write(tree.Fruits.Count);
        for (int i = 0; i < tree.Fruits.Count; i++)
        {
            EnchantedTreeTileEntity.Fruit fruit = tree.Fruits[i];
            packet.Write(fruit.type);
            packet.WriteVector2(fruit.center);
            packet.WriteVector2(fruit.targetPosition);
            packet.WriteVector2(fruit.velocity);
            packet.Write(fruit.previousItem);
            packet.Write(fruit.layer);
            packet.Write((byte)fruit.grabCooldown);
            packet.Write((byte)fruit.despawnTimer);
        }
        packet.Send();

    }
    public static void SendEnchantedTreesListPacket()
    {
        ModPacket packet = Instance.GetPacket();
        packet.Write((byte)PacketID.SyncTreeEntities);

        packet.Write(EnchantedTreeSheet.EnchantedTrees.Count);
        foreach (Point16 vec in EnchantedTreeSheet.EnchantedTrees)
        {
            packet.Write((int)vec.X);
            packet.Write((int)vec.Y);
        }
        packet.Send();
    }
    public static void SendChizardRequestChestContents(int chestX, int chestY)
    {
        ModPacket packet = Instance.GetPacket();
        packet.Write((byte)PacketID.SyncChestContents);
        packet.Write(chestX);
        packet.Write(chestY);
        packet.Send();
    }
    public static void SendChizardChestContentsToClient(int client, int chestX, int chestY)
    {
        int c = Chest.FindChest(chestX, chestY);
        if (c >= 0)
        {
            Chest chest = Main.chest[c];
            for (int i = 0; i < chest.item.Length; i++)
            {
                NetMessage.SendData(MessageID.SyncChestItem, client, -1, null, c, i);
            }
        }

    }
    public static void SendChizardTookItem(int itemIndex, int chestX, int chestY)
    {
        int c = Chest.FindChest(chestX, chestY);
        if (c >= 0)
        {
            NetMessage.SendData(MessageID.SyncChestItem, -1, -1, null, c, itemIndex);
        }
    }
    public static void RequestTakeItemOut(int itemIndex, Item expectedItem, int amount, int chestX, int chestY)
    {
        int c = Chest.FindChest(chestX, chestY);
        if (c >= 0)
        {
            ModPacket packet = Instance.GetPacket();
            packet.Write((byte)PacketID.RequestTakeItemFromChest);
            packet.Write(chestX);
            packet.Write(chestY);
            packet.Write(amount);
            packet.Write(itemIndex);
            packet.Write(expectedItem.type);
            packet.Write(expectedItem.stack);
            packet.Write(expectedItem.prefix);
            packet.Send();
        }
    }

    public static void AddPotionToPotionBag(int itemID, int count)
    {
        ModPacket packet = Instance.GetPacket();
        packet.Write((byte)PacketID.AddPotionToBag);
        packet.Write(itemID);
        packet.Write(count);
        packet.Send();
    }
}
