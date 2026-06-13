using Fargowiltas.Common;
using Fargowiltas.Common.Systems.Collections;
using Fargowiltas.Content.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles
{
    public class ChestWizardSheet : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;


            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.AvoidedByNPCs[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = ModContent.GetInstance<ChestWizardTileEntity>().Generic_HookPostPlaceMyPlayer;// new PlacementHook(ModContent.GetInstance<EnchantedTreeTileEntity>().Hook_AfterPlacement, -3, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(Color.DarkGray, name);

            DustType = DustID.Stone;

            AnimationFrameHeight = 38;
        }
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            FargoUtils.TryGetTileEntityAs(i, j, out ChestWizardTileEntity TE);
            bool interacted = false;
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                if (Main.player[p].active && Main.player[p].FargoMutant().LastInteractedChizard == FargoUtils.GetTopLeftTileInMultitile(i, j).ToVector2())
                {
                    interacted = true;
                    break;
                }
            }
            if (interacted)
            {
                frameYOffset = 38;
            }
            else
            {
                frameYOffset = 0;
            }
            base.AnimateIndividualTile(type, i, j, ref frameXOffset, ref frameYOffset);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (FargoUtils.GetTopLeftTileInMultitile(i, j) == new Point16(i, j))
            {
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, Terraria.GameContent.Drawing.TileDrawing.TileCounterType.CustomNonSolid);
            }
            base.DrawEffects(i, j, spriteBatch, ref drawData);
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (!FargoUtils.TryGetTileEntityAs(i, j, out ChestWizardTileEntity TE))
            {
                return;
            }

            Asset<Texture2D> eye = ModContent.Request<Texture2D>("Fargowiltas/Content/Items/Tiles/ChestWizardEyeAssembly");
            Rectangle ball = new(2, 6, 18, 18);
            Rectangle pupil = new(26, 10, 8, 8);
            Rectangle beard = new(38, 12, 22, 18);

            TE.drawTimer += 0.1f;
            if (TE.drawTimer >= MathHelper.Pi * 20)
            {
                TE.drawTimer = 0;
            }
            if (TE.hatID == 0)
            {
                TE.hatID = Main.rand.Next([ItemID.WizardHat, ItemID.WizardsHat, ItemID.RuneHat, ItemID.MagicHat]);
            }
            int armor = Array.IndexOf(Item.headType, TE.hatID);
            Asset<Texture2D> hat = TextureAssets.ArmorHead[armor];
            Main.instance.LoadArmorHead(armor);
            SpriteEffects hatSide = Main.LocalPlayer.Center.X > TE.Position.ToWorldCoordinates().X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Rectangle headSource = new(0, 0, hat.Width(), hat.Height() / 20);
            Vector2 pos = new Vector2(i, j).ToWorldCoordinates() - Main.screenPosition + new Vector2(8 , MathF.Sin(TE.drawTimer) - 24);
            spriteBatch.Draw(eye.Value, pos, ball, Lighting.GetColor(new Point(i, j)), 0, ball.Size() / 2, 1, SpriteEffects.None, 1);

            float angle = (pos + Main.screenPosition).AngleTo(Main.LocalPlayer.Center);
            //Main.NewText(TE.item);
            spriteBatch.Draw(eye.Value, pos + new Vector2(3, 0).RotatedBy(angle), pupil, Lighting.GetColor(new Point(i, j)), 0, pupil.Size() / 2, 1, SpriteEffects.None, 1);
            spriteBatch.Draw(eye.Value, pos + new Vector2(0, 10), beard, Lighting.GetColor(new Point(i, j)), 0, beard.Size() / 2, 1, SpriteEffects.None, 1);
            spriteBatch.Draw(hat.Value, new Vector2(i, j).ToWorldCoordinates() - Main.screenPosition + new Vector2(8, MathF.Sin(TE.drawTimer) * 2 - 25), headSource, Lighting.GetColor(new Point(i, j)), 0, headSource.Size() / 2, 1, hatSide, 1);
            base.SpecialDraw(i, j, spriteBatch);
        }
        public override bool RightClick(int i, int j)
        {

            FargoUtils.TryGetTileEntityAs(i, j, out ChestWizardTileEntity TE);
            int type = Main.LocalPlayer.HeldItem.type;
            if (Main.LocalPlayer.HeldItem.headSlot != -1 && TE.hatID != type)
            {
                TE.hatID = type;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = Fargowiltas.Instance.GetPacket();
                    packet.Write((byte)Fargowiltas.PacketID.ChangeChizardHat);
                    packet.Write(TE.ID);
                    packet.Write(type);
                    packet.Send();
                }
                SoundEngine.PlaySound(SoundID.Grab);
                return true;
            }
            Main.LocalPlayer.FargoMutant().LastInteractedChizard = FargoUtils.GetTopLeftTileInMultitile(i, j).ToVector2();
            FargoUI ui = FargoUIManager.Get<ChizardSearchBar>();
            if (FargoUIManager.IsOpen(ui))
            {
                FargoUIManager.Close(ui);
                SoundEngine.PlaySound(SoundID.MenuClose);
                Main.LocalPlayer.FargoMutant().LastInteractedChizard = Vector2.Zero;
            }
            else
            {
                Vector2 pos = Main.LocalPlayer.FargoMutant().LastInteractedChizard.ToWorldCoordinates();

                FargoUIManager.Open(ui);
                if (!Main.playerInventory)
                    Main.LocalPlayer.ToggleInv();
                SoundEngine.PlaySound(SoundID.MenuOpen);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    for (int c = 0; c < Main.chest.Length; c++)
                    {
                        if (Main.chest[c] != null && new Vector2(Main.chest[c].x * 16, Main.chest[c].y * 16).Distance(Main.LocalPlayer.Center) < 1000)
                        {
                            FargoNet.SendChizardRequestChestContents(Main.chest[c].x, Main.chest[c].y);
                        }
                    }
                }
            }
            return true;
        }
    }
}
