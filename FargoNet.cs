using System;
using System.Collections.Generic;
using System.IO;
using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Fargowiltas.Fargowiltas;

namespace Fargowiltas
{
    public static class FargoNet
    {
        public const byte SummonNPCFromClient = 0;
        private const bool Debug = false;

        public static void SendData(int dataType, int dataA, int dataB, string text, int playerID, float dataC, float dataD, float dataE, int clientType)
        {
            NetMessage.SendData(dataType, dataA, dataB, NetworkText.FromLiteral(text), playerID, dataC, dataD, dataE, clientType);
        }

        public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
        {
            packet.Write(msg);
            for (int m = 0; m < param.Length; m++)
            {
                object obj = param[m];
                switch (obj)
                {
                    case byte[] _:
                        {
                            byte[] array = (byte[])obj;
                            foreach (byte b in array)
                            {
                                packet.Write(b);
                            }

                            break;
                        }

                    case bool _:
                        packet.Write((bool)obj);
                        break;

                    case byte _:
                        packet.Write((byte)obj);
                        break;

                    case short _:
                        packet.Write((short)obj);
                        break;

                    case int _:
                        packet.Write((int)obj);
                        break;

                    case float _:
                        packet.Write((float)obj);
                        break;

                    case string _:
                        packet.Write((string)obj);
                        break;
                }
            }

            return packet;
        }

        public static void SyncAI(Entity codable, float[] ai, int aitype)
        {
            int entType = codable is NPC ? 0 : codable is Projectile ? 1 : -1;
            if (entType == -1)
            {
                return;
            }

            int id = codable is NPC ? ((NPC)codable).whoAmI : ((Projectile)codable).identity;
            SyncAI(entType, id, ai, aitype);
        }

        /*
         * Used to sync custom ai float arrays. (the npc or projectile requires a method called 'public void SetAI(float[] ai, int type)' that sets the ai for this to work)
         */
        public static void SyncAI(int entType, int id, float[] ai, int aitype)
        {
            object[] ai2 = new object[ai.Length + 4];
            ai2[0] = (byte)entType;
            ai2[1] = (short)id;
            ai2[2] = (byte)aitype;
            ai2[3] = (byte)ai.Length;
            for (int m = 4; m < ai2.Length; m++)
            {
                ai2[m] = ai[m - 4];
            }

            SendFargoNetMessage(1, ai2);
        }

        /*
         * Writes a vector2 array to an obj[] array that can be sent via netmessaging.
         */
        public static object[] WriteVector2Array(Vector2[] array)
        {
            List<object> list = new List<object>
            {
                array.Length,
            };

            foreach (Vector2 vec in array)
            {
                list.Add(vec.X);
                list.Add(vec.Y);
            }

            return list.ToArray();
        }

        /*
         * Writes a vector2 array to a binary writer.
         */
        public static void WriteVector2Array(Vector2[] array, BinaryWriter writer)
        {
            writer.Write(array.Length);
            foreach (Vector2 vec in array)
            {
                writer.Write(vec.X);
                writer.Write(vec.Y);
            }
        }

        /*
         * Reads a vector2 array from a binary reader.
         */
        public static Vector2[] ReadVector2Array(BinaryReader reader)
        {
            int arrayLength = reader.ReadInt32();
            Vector2[] array = new Vector2[arrayLength];
            for (int m = 0; m < arrayLength; m++)
            {
                array[m] = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            }

            return array;
        }

        public static void SendFargoNetMessage(int msg, params object[] param)
        {
            // Nothing to sync in SP
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                return;
            }

            WriteToPacket(ModContent.GetInstance<Fargowiltas>().GetPacket(), (byte)msg, param).Send();
        }

        public static void HandlePacket(BinaryReader bb, byte msg)
        {
            if (Debug)
            {
                ModContent.GetInstance<Fargowiltas>().Logger.Error((Main.netMode == NetmodeID.Server ? "--SERVER-- " : "--CLIENT-- ") + "HANDLING MESSAGE: " + msg);
            }

            try
            {
                if (msg == SummonNPCFromClient)
                {
                    int playerID = bb.ReadByte();
                    int bossType = bb.ReadInt16();
                    bool spawnMessage = bb.ReadBoolean();
                    int npcCenterX = bb.ReadInt32();
                    int npcCenterY = bb.ReadInt32();
                    string overrideDisplayName = bb.ReadString();
                    bool namePlural = bb.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        Fargowiltas.SpawnBoss(Main.player[playerID], bossType, spawnMessage, new Vector2(npcCenterX, npcCenterY), overrideDisplayName, namePlural);
                    }
                }
            }
            catch (Exception e)
            {
                ModContent.GetInstance<Fargowiltas>().Logger.Error((Main.netMode == NetmodeID.Server ? "--SERVER-- " : "--CLIENT-- ") + "ERROR HANDLING MSG: " + msg.ToString() + ": " + e.Message);
                ModContent.GetInstance<Fargowiltas>().Logger.Error(e.StackTrace);
                ModContent.GetInstance<Fargowiltas>().Logger.Error("-------");
            }
        }

        public static void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (Debug)
            {
                ModContent.GetInstance<Fargowiltas>().Logger.Error((Main.netMode == NetmodeID.Server ? "--SERVER-- " : "--CLIENT-- ") + "SYNC PLAYER CALLED! NEWPLAYER: " + newPlayer + ". TOWHO: " + toWho + ". FROMWHO:" + fromWho);
            }

            if (Main.netMode == NetmodeID.Server && (toWho > -1 || fromWho > -1))
            {
                PlayerConnected();
            }
        }

        public static void PlayerConnected()
        {
            if (Debug)
            {
                ModContent.GetInstance<Fargowiltas>().Logger.Info("--SERVER-- PLAYER JOINED!");
            }
        }

        public static void SendNetMessage(int msg, params object[] param)
        {
            SendNetMessageClient(msg, -1, param);
        }

        public static void SendNetMessageClient(int msg, int client, params object[] param)
        {
            try
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    return;
                }

                WriteToPacket(ModContent.GetInstance<Fargowiltas>().GetPacket(), (byte)msg, param).Send(client);
            }
            catch (Exception e)
            {
                ModContent.GetInstance<Fargowiltas>().Logger.Error((Main.netMode == NetmodeID.Server ? "--SERVER-- " : "--CLIENT-- ") + "ERROR SENDING MSG: " + msg.ToString() + ": " + e.Message);
                ModContent.GetInstance<Fargowiltas>().Logger.Error(e.StackTrace);
                ModContent.GetInstance<Fargowiltas>().Logger.Error("-------");
                string param2 = string.Empty;
                for (int m = 0; m < param.Length; m++)
                {
                    param2 += param[m];
                }

                ModContent.GetInstance<Fargowiltas>().Logger.Error("PARAMS: " + param2);
                ModContent.GetInstance<Fargowiltas>().Logger.Error("-------");
            }
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
                packet.Write(fruit.grabCooldown);
                packet.Write(fruit.despawnTimer);
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
    }
}
