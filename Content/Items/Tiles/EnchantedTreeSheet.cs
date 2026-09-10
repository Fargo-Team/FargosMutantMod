using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace Fargowiltas.Content.Items.Tiles;

public class EnchantedTreeSheet : ModTile
{
    public static List<Point16> EnchantedTrees = [];
    public override void SetStaticDefaults()
    {
        Main.tileShine[Type] = 400;
        Main.tileLighted[Type] = true;
        Main.tileSolid[Type] = false;
        Main.tileMergeDirt[Type] = false;
        Main.tileBlockLight[Type] = false;
        Main.tileNoAttach[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
        TileObjectData.newTile.Width = 3;
        TileObjectData.newTile.Height = 4;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 };
        TileObjectData.newTile.HookPostPlaceMyPlayer = ModContent.GetInstance<EnchantedTreeTileEntity>().Generic_HookPostPlaceMyPlayer;// new PlacementHook(ModContent.GetInstance<EnchantedTreeTileEntity>().Hook_AfterPlacement, -3, 0, false);
        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.Origin = new Point16(1, 3);
        TileObjectData.addTile(Type);

        LocalizedText name = CreateMapEntryName();
        AddMapEntry(Color.DarkGray, name);

        DustType = DustID.TintableDust;
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        r = 0.4f;
        g = 0.3f;
        b = 0.5f;
    }

    public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
    {

    }

    public override void PlaceInWorld(int i, int j, Item item)
    {
        EnchantedTrees.Add(FargoUtils.GetTopLeftTileInMultitile(i, j));
        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            FargoNet.SendEnchantedTreesListPacket();
        }
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        //drop item currently inside it
        if (FargoUtils.TryGetTileEntityAs<EnchantedTreeTileEntity>(i, j, out EnchantedTreeTileEntity entity) == true && entity.ItemType >= 0 && !Main.dedServ)
        {
            int item = Item.NewItem(Item.GetSource_NaturalSpawn(), new Rectangle(i * 16 + 50, j * 16 + 50, 1, 1), entity.ItemType, 1, prefixGiven: entity.Prefix);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.SyncItem, Main.myPlayer, number: item, number2: -1);
        }
        EnchantedTrees.Remove(FargoUtils.GetTopLeftTileInMultitile(i, j));
        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            FargoNet.SendEnchantedTreesListPacket();
        }
        ModContent.GetInstance<EnchantedTreeTileEntity>().Kill(i, j);
    }

    public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
    {
        //allow drawing in front of the tile
        if (new Point16(i, j) == FargoUtils.GetTopLeftTileInMultitile(i, j))
        {
            Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
        }
    }

    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        if (Main.gameMenu || !FargoUtils.TryGetTileEntityAs<EnchantedTreeTileEntity>(i, j, out EnchantedTreeTileEntity tree))
            return;

        tree.drawTimer += 0.1f;

        //arbitrary number change to a multiple of 2 pi if theres a noticeable cut in the rotation of the backglow but make sure you change float x to be correct if you do
        if (tree.drawTimer >= 20)
            tree.drawTimer = 0;

        //lerp values, lerper is a bell curve
        float x = tree.drawTimer / 20f;
        float lerper = (float)Math.Sin(6 * x - 1.5) / 2 + 0.5f;
        if (tree == null)
            return;
        if (tree.ItemType >= 1 && tree.Fruits.Count == 0 && tree.ItemType < ItemLoader.ItemCount)
            DrawItem(tree.ItemType, new Vector2(i, j).ToWorldCoordinates() + new Vector2(16, -12 + MathHelper.Lerp(-10, 10, lerper)));


        void DrawItem(int type, Vector2 position, float opacity = 1)
        {
            Main.GetItemDrawFrame(type, out Texture2D _, out Rectangle frame);
            Asset<Texture2D> item = TextureAssets.Item[type];
            position += new Vector2(190, 190);
            //disco backglow
            for (int n = 0; n < 5; n++)
            {
                Main.EntitySpriteDraw(item.Value, position - Main.screenPosition + new Vector2((float)Math.Sin(tree.drawTimer + n * 2f) * 3, (float)Math.Cos(tree.drawTimer + n * 3f) * 3), frame, Main.DiscoColor * 0.5f * opacity, 0, new Vector2(frame.Width, frame.Height) / 2, 1, SpriteEffects.None, 0);
            }
            //real item
            Main.EntitySpriteDraw(item.Value, position - Main.screenPosition, frame, Color.White * opacity, 0, new Vector2(frame.Width, frame.Height) / 2, 1, SpriteEffects.None, 0);
        }
    }

    public override bool RightClick(int i, int j)
    {
        Player player = Main.LocalPlayer;
        if (player != null && player.active && !player.dead && Main.netMode != NetmodeID.Server)
        {
            FargoUtils.TryGetTileEntityAs<EnchantedTreeTileEntity>(i, j, out EnchantedTreeTileEntity tree);
            //set tile entity's item to held item of the player and reduce stack of player's held item!
            if (tree != null && player.HeldItem != null && player.HeldItem.type >= ItemID.None && tree.ItemType == -1 && player.HeldItem.stack > 0 && tree.Fruits.Count == 0 && !player.HeldItem.favorited)
            {
                tree.ItemType = player.HeldItem.type;
                tree.Prefix = player.HeldItem.prefix;
                if (EnchantedTreeTileEntity.IsItemDupable(tree.ItemType))
                {
                    tree.Fruits.Add(new(tree.ItemType, tree.Position.ToWorldCoordinates() + new Vector2(16, -12), tree.Position.ToWorldCoordinates() + new Vector2(16, -80), Vector2.Zero));
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        FargoNet.SendEnchantedTreeFruitPacket(EnchantedTrees.IndexOf(FargoUtils.GetTopLeftTileInMultitile(i, j)));
                    }
                }
                player.HeldItem.stack -= 1;
                tree.CursorInsertCooldown = 10;
                return true;
            }
            //if the entity has item already drop it and set it to null
            else if (tree != null && tree.ItemType >= 0 && tree.CursorInsertCooldown <= 0)
            {
                int it = Item.NewItem(player.GetSource_TileInteraction(i, j), new Vector2(i, j).ToWorldCoordinates(), tree.ItemType, 1, prefixGiven: tree.Prefix);
                NetMessage.SendData(MessageID.SyncItem, player.whoAmI, number: it, number2: 1f);
                tree.ItemType = -1;
                for (int f = 0; f < tree.Fruits.Count; f++)
                {
                    EnchantedTreeTileEntity.Fruit fruit = tree.Fruits[f];
                    if (fruit.despawnTimer == 0)
                    {
                        fruit.despawnTimer = 1;

                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    FargoNet.SendEnchantedTreeFruitPacket(EnchantedTrees.IndexOf(FargoUtils.GetTopLeftTileInMultitile(i, j)));
                }
                return true;

            }
        }
        return base.RightClick(i, j);
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (!closer)
        {
            //Debug.WriteLine(EnchantedTreeTileEntity.EnchantedTrees[0]);
            if (!EnchantedTrees.Contains(FargoUtils.GetTopLeftTileInMultitile(i, j)))
            {
                //EnchantedTreeTileEntity.EnchantedTrees = [];
                EnchantedTrees.Add(FargoUtils.GetTopLeftTileInMultitile(i, j));
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    FargoNet.SendEnchantedTreesListPacket();
                }
            }
        }
    }
}
