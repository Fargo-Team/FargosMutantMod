using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.Items.Tiles
{
    public class OmnistationSheet : ModTile
    {
        public virtual Color color => new Color(221, 85, 125);

        public override void SetStaticDefaults()
        {            
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16];
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(color, name);

            AnimationFrameHeight = 72;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 6)
            {
                frameCounter = 0;

                if (++frame >= 42)
                    frame = 0;
            }
        }

        public override bool CanDrop(int i, int j) => false;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                    Main.LocalPlayer.AddBuff(BuffType<Buffs.Omnistation>(), 30);
            }
        }

        //public override void MouseOver(int i, int j)
        //{
        //    Player player = Main.LocalPlayer;
        //    player.noThrow = 2;
        //    player.cursorItemIconEnabled = true;
        //    player.cursorItemIconID = ItemType<Omnistation>();
        //}

        //public override bool RightClick(int i, int j)
        //{
        //    Item item = Main.LocalPlayer.HeldItem;
        //    if (item.CountsAsClass(DamageClass.Melee))
        //    {
        //        Main.LocalPlayer.AddBuff(BuffID.Sharpened, 60 * 60 * 10);
        //    }

        //    if (item.CountsAsClass(DamageClass.Ranged))
        //    {
        //        Main.LocalPlayer.AddBuff(BuffID.AmmoBox, 60 * 60 * 10);
        //    }

        //    if (item.CountsAsClass(DamageClass.Magic))
        //    {
        //        Main.LocalPlayer.AddBuff(BuffID.Clairvoyance, 60 * 60 * 10);
        //    }

        //    if (item.CountsAsClass(DamageClass.Summon))
        //    {
        //        Main.LocalPlayer.AddBuff(BuffID.Bewitched, 60 * 60 * 10);
        //    }

        //    SoundEngine.PlaySound(SoundID.Item44, new Vector2(i * 16 + 8, j * 16 + 8));

        //    return true;
        //}

        /*public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.TileFrameY == 36 ? 18 : 16;




            Main.spriteBatch.Draw(Request<Texture2D>(Texture + "_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }*/

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Tile[Type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Tile[Type].Value.Height / 42; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Main.tileFrame[Type] ; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(tile.TileFrameX, tile.TileFrameY + y3, 16, 16);
            Vector2 origin2 = rectangle.Size() / 2f;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            //Rectangle frame = new(tile.TileFrameX, tile.TileFrameY + Main.tileFrame[Type], texture2D13.Width, texture2D13.Height);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Main.spriteBatch.Draw(Request<Texture2D>(Texture + "_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}