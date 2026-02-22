
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fargowiltas.Common.Systems.Collections;
using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Items.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Fargowiltas.Content.Items.Tiles
{
    public class EnchantedTreeTileEntity : ModTileEntity
    {
        public List<Fruit> Fruits = [];
        //timer for drawing disco backglow
        public float drawTimer = 0;
        public class Fruit
        {
            public Vector2 center;
            public Vector2 velocity;
            public Vector2 targetPosition; 
            public int type; //item sprite to draw
            public bool grabbed; //if the current client is dragging it around
            public int grabCooldown; //dont allow grabbing (on new fruit & right after duping)
            public int previousItem; //index to draw lines to
            public int layer; //how many branches deep it is, use to despawn other branches of the same depth
            public float despawnTimer; //fade out to despawn
            public Fruit(int type, Vector2 position, Vector2 targetPosition, Vector2 velocity, int previousItem = 0, int layer = 0)
            {
                
                this.type = type;
                this.center = position;
                this.velocity = velocity;
                this.targetPosition = targetPosition;
                grabbed = false;
                grabCooldown = 20;
                this.previousItem = previousItem;
                this.layer = layer;
                despawnTimer = 0;
            }
        }
        public override void Update()
        {
            for (int i = 0; i < Fruits.Count; i++)
            {
                Fruit fruit = Fruits[i];
                
            }
        }
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<EnchantedTreeSheet>();
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            Point16 tileOrigin = new Point16(0, 3);
            int placedEntity = Place(i - tileOrigin.X, j - tileOrigin.Y);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Main.NewText("yo");
                int width = 3;
                int height = 4;
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
        public int ItemType = -1;
        public int Prefix = 0;
        public override void SaveData(TagCompound tag)
        {
            //dont save item if its null because ItemIO.Save will fuck up
            tag["ItemID"] = ItemType;
            tag["Prefix"] = Prefix;
        }
        public override void LoadData(TagCompound tag)
        {
            //only load if the bool sent indicated item is not null
            ItemType = tag.GetAsInt("ItemID");
            Prefix = tag.GetAsInt("Prefix");
        }
        //same concept as save and load
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(ItemType);
            writer.Write(Prefix);

            writer.Write(Fruits.Count);
            for (int i = 0; i < Fruits.Count; i++)
            {
                Fruit fruit = Fruits[i];
                writer.Write(fruit.type);
                writer.WriteVector2(fruit.center);
                writer.WriteVector2(fruit.targetPosition);
                writer.WriteVector2(fruit.velocity);
                writer.Write(fruit.previousItem);
                writer.Write(fruit.layer);
                writer.Write(fruit.grabCooldown);
                writer.Write(fruit.despawnTimer);
            }
        }
        public override void NetReceive(BinaryReader reader)
        {
            ItemType = reader.ReadInt32();
            Prefix = reader.ReadInt32();

            int count = reader.ReadInt32();
            //Main.NewText(count);
            Fruits = [];
            for (int i = 0; i < count; i++)
            {
                Fruit fruit = new(reader.ReadInt32(), reader.ReadVector2(), reader.ReadVector2(), reader.ReadVector2(), reader.ReadInt32(), reader.ReadInt32());
                fruit.grabCooldown = reader.ReadInt32();
                fruit.despawnTimer = reader.ReadSingle();
                Fruits.Add(fruit);
            }
        }
       
        

        public static bool IsItemDupable(int type)
        {
            if (FargoItemSets.DuplicatableItems[type] != DupeType.NotDupable && FargoItemSets.DuplicatableItems[type] != DupeType.NotDupableFromDupable) return true;
            ModItem moditem = ContentSamples.ItemsByType[type].ModItem;
            //is an enchant force or soul
            if (moditem != null)
            {
                string modName = moditem.Mod.Name;

                return (moditem.Name.EndsWith("Enchant") || moditem.Name.EndsWith("Force") || moditem.Name.EndsWith("Soul")) && Fargowiltas.SoulsMods.Contains(modName);
            }
            return false;
        }
        public static void UpdateEnchantedTrees()
        {
            if (!Main.dedServ)
            {

                for (int t = 0; t < EnchantedTreeSheet.EnchantedTrees.Count; t++)
                {
                    if (!FargoUtils.TryGetTileEntityAs(EnchantedTreeSheet.EnchantedTrees[t].X, EnchantedTreeSheet.EnchantedTrees[t].Y, out EnchantedTreeTileEntity tree))
                    {
                        //Main.NewText(EnchantedTreeTileEntity.EnchantedTrees.Count);
                        return;
                    }

                    for (int i = 0; i < tree.Fruits.Count; i++)
                    {

                        EnchantedTreeTileEntity.Fruit fruit = tree.Fruits[i];
                        //radius for a dist check so width / 2
                        float size = 30;
                        //enchanted acorns cost 2 gold. divide value by 10000 to get cost in gold coins, then by 2 for cost in acorns.
                        int cost = ContentSamples.ItemsByType[fruit.type].value / 10000 / 2 * 2;
                        cost = Math.Clamp(cost, 1, 9999);
                        if (!Main.LocalPlayer.controlUseItem)
                        {
                            fruit.grabbed = false;
                        }
                        if (Main.MouseWorld.Distance(fruit.center) <= size)
                        {
                            Main.instance.MouseText(Lang.GetItemNameValue(fruit.type) + "\n[i:Fargowiltas/EnchantedAcorn] [c/3BFFEB:" + cost + "]", ContentSamples.ItemsByType[fruit.type].rare);
                        }
                        if (Main.MouseWorld.Distance(fruit.center) <= size && Main.LocalPlayer.controlUseItem && fruit.grabCooldown == 0 &&
                            (Main.LocalPlayer.FargoMutant().grabbedFruit == null || Main.LocalPlayer.FargoMutant().grabbedFruit == fruit))
                        {
                            fruit.grabbed = true;
                            Main.LocalPlayer.FargoMutant().grabbedFruit = fruit;
                        }

                        fruit.center += fruit.velocity;
                        if (fruit.grabCooldown > 0)
                        {
                            fruit.grabCooldown--;
                        }
                        //fruit.velocity = Vector2.Lerp(fruit.velocity, fruit.center.AngleTo(fruit.targetPosition).ToRotationVector2() * fruit.center.Distance(fruit.targetPosition), 0.05f);
                        fruit.center = Vector2.Lerp(fruit.center, fruit.targetPosition, 0.05f);

                        if (fruit.grabbed)
                        {
                            bool expand = true;
                            bool netsync = false;
                            for (int k = 0; k < tree.Fruits.Count; k++)
                            {
                                Fruit fruit2 = tree.Fruits[k];
                                if (fruit2.previousItem == i && k != i) expand = false;
                                else if (fruit2.layer > fruit.layer && fruit2.despawnTimer == 0)
                                {
                                    netsync = true;
                                    fruit2.despawnTimer = 1;
                                }
                            }
                            if (expand && FargoItemSets.DuplicatableRecipes.ContainsKey(fruit.type) && FargoItemSets.DuplicatableRecipes[fruit.type].Count > 0)
                            {
                                SoundEngine.PlaySound(SoundID.Item130, fruit.center);
                                netsync = true;
                                for (int d = 0; d < FargoItemSets.DuplicatableRecipes[fruit.type].Count; d++)
                                {
                                    float side = (d % 2 == 0 ? -1 : 1);
                                    Vector2 position = fruit.targetPosition + new Vector2(0, 250) - new Vector2(0, 500).RotatedBy(MathHelper.ToRadians((d) * 7 - FargoItemSets.DuplicatableRecipes[fruit.type].Count * 7 / 2 + 3f));
                                    tree.Fruits.Add(new Fruit(FargoItemSets.DuplicatableRecipes[fruit.type][d], fruit.center, position, Vector2.Zero, i, fruit.layer + 1));
                                }
                            }
                            if (netsync && Main.netMode == NetmodeID.MultiplayerClient)
                            {

                                FargoNet.SendEnchantedTreeFruitPacket(t);
                                //NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, tree.ID, tree.Position.X, tree.Position.Y);
                            }
                            //fruit.velocity = Vector2.Lerp(fruit.velocity, fruit.center.AngleTo(Main.MouseWorld).ToRotationVector2() * fruit.center.Distance(Main.MouseWorld), 0.07f);
                            fruit.center = Vector2.Lerp(fruit.center, Main.MouseWorld, 0.07f);
                            if (fruit.center.Distance(fruit.targetPosition) > 100)
                            {
                                fruit.grabbed = false;
                                Main.LocalPlayer.FargoMutant().grabbedFruit = null;
                                fruit.grabCooldown = 30;

                                if (Main.LocalPlayer.CountItem(ModContent.ItemType<EnchantedAcorn>()) >= cost)
                                {
                                    if (Main.netMode != NetmodeID.Server)
                                    {
                                        int item = Item.NewItem(Main.LocalPlayer.GetSource_TileInteraction(tree.Position.X, tree.Position.Y, "EnchantedTreeDuplication"), fruit.center, fruit.type, prefixGiven: tree.Prefix);

                                        Main.item[item].GetGlobalItem<FargoGlobalItem>().Grabbed = Main.myPlayer;
                                        Main.item[item].GetGlobalItem<FargoGlobalItem>().FromEnchantedTree = true;
                                        //FargoNet.SyncItemFromFruitPacket(Main.item[item].type, Main.myPlayer, tree.Position.ToVector2(), fruit.center, item);
                                        NetMessage.SendData(MessageID.SyncItem, Main.myPlayer, number: item, number2: 1f);

                                    }
                                    else if (Main.netMode == NetmodeID.MultiplayerClient)
                                    {
                                        //FargoNet.SyncItemFromFruitPacket(fruit.type, Main.myPlayer, tree.Position.ToVector2(), fruit.center);
                                    }
                                    SoundEngine.PlaySound(SoundID.Item176 with { Pitch = 0.2f }, fruit.center);
                                    for (int a = 0; a < cost; a++)
                                    {
                                        Main.LocalPlayer.ConsumeItem(ModContent.ItemType<EnchantedAcorn>(), includeVoidBag: true);
                                    }
                                }
                                else
                                {
                                    SoundEngine.PlaySound(SoundID.Item150 with { Pitch = 0f }, fruit.center);
                                }

                            }
                        }
                        if (fruit.despawnTimer > 0)
                        {
                            fruit.despawnTimer++;
                            if (fruit.despawnTimer > 20)
                            {
                                tree.Fruits.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
        public static void DrawEnchantedTrees()
        {
            for (int i = 0; i < EnchantedTreeSheet.EnchantedTrees.Count; i++)
            {
                if (FargoUtils.TryGetTileEntityAs<EnchantedTreeTileEntity>(EnchantedTreeSheet.EnchantedTrees[i].X, EnchantedTreeSheet.EnchantedTrees[i].Y, out EnchantedTreeTileEntity tree))
                {
                    Main.spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.Camera.Sampler, DepthStencilState.None, Main.Camera.Rasterizer,null, transformMatrix: Main.Camera.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> line = TextureAssets.Extra[ExtrasID.FairyQueenLance];
                    //draw lines
                    for (int f = 0; f < tree.Fruits.Count; f++)
                    {
                        EnchantedTreeTileEntity.Fruit fruit = tree.Fruits[f];
                        if (fruit.previousItem >= 0 && tree.Fruits.Count > fruit.previousItem && Main.LocalPlayer.Distance(fruit.center) < 1200)
                        {
                            Vector2 pos = tree.Fruits[fruit.previousItem].center;
                            Main.EntitySpriteDraw(line.Value, pos - Main.screenPosition, null, Color.White * (1 - fruit.despawnTimer / 20f), pos.AngleTo(fruit.center), new Vector2(0, line.Height()) / 2, new Vector2(pos.Distance(fruit.center) / line.Width(), 1), SpriteEffects.None);
                        }
                    }

                    //if (tree.Fruits.Count == 0) tree.Fruits.Add(new Fruit(ItemID.IronBrick, tree.Position.ToWorldCoordinates(), tree.Position.ToWorldCoordinates(), Vector2.Zero));
                    //draw item after for on top
                    for (int f = 0; f < tree.Fruits.Count; f++)
                    {

                        EnchantedTreeTileEntity.Fruit fruit = tree.Fruits[f];
                        if (Main.LocalPlayer.Distance(fruit.center) < 1200)
                            DrawItem(fruit.type, fruit.center, 1 - fruit.despawnTimer / 20f);

                    }
                    void DrawItem(int type, Vector2 position, float opacity = 1)
                    {

                        //needed for animated item sprites
                        //Main.instance.LoadItem(type);
                        Rectangle frame;
                        Main.GetItemDrawFrame(type, out Texture2D useless, out frame);
                        Asset<Texture2D> item = TextureAssets.Item[type];
                        //position += new Vector2(190, 190);
                        //disco backglow
                        for (int n = 0; n < 5; n++)
                        {
                            Main.EntitySpriteDraw(item.Value, position - Main.screenPosition + new Vector2((float)Math.Sin(tree.drawTimer + n * 2f) * 3, (float)Math.Cos(tree.drawTimer + n * 3f) * 3), frame, Color.White * 0.5f * opacity, 0, new Vector2(frame.Width, frame.Height) / 2, 1, SpriteEffects.None, 0);
                        }
                        //real item
                        Main.EntitySpriteDraw(item.Value, position - Main.screenPosition, frame, Color.White * opacity, 0, new Vector2(frame.Width, frame.Height) / 2, 1, SpriteEffects.None, 0);
                    }
                    Main.spriteBatch.End();
                }
            }
        }
    }
}
