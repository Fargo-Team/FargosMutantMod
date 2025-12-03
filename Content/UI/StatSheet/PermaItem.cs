using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace Fargowiltas.Content.UI.StatSheet
{
    public class PermaItem : UIElement
    {
        int timer;
        Item item;
        Asset<Texture2D> line;

        public PermaItem(int type)
        {
            timer = 0;
            item = new Item(type).Clone();
            line = TextureAssets.Extra[178];
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (item != null)
            {


                if (ContainsPoint(Main.MouseScreen))
                {
                    Main.hoverItemName = item.Name;
                    Main.HoverItem = item.Clone();
                }

                Rectangle lineFrame = line.Value.Frame();
                Vector2 lineOrigin = lineFrame.Size() / 2;
                Vector2 scale = new Vector2(0.02f, 20f);
                float height = 0.2f * GetOuterDimensions().Height;
                spriteBatch.Draw(line.Value, GetOuterDimensions().Center() + height * Vector2.UnitY, lineFrame, Color.Pink * 0.25f, 3 * MathHelper.PiOver2, lineOrigin, scale, SpriteEffects.None, 1);
                spriteBatch.Draw(line.Value, GetOuterDimensions().Center() + (height + lineFrame.Width * scale.X) * Vector2.UnitY, lineFrame, Color.Pink * 0.1f, MathHelper.PiOver2, lineOrigin, scale, SpriteEffects.None, 1);

                Vector2 offset = new Vector2(0, 0.7f * MathF.Sin(MathHelper.TwoPi * timer++ / 300f) - 2);
                for (int n = 0; n < 5; n++)
                {
                    Vector2 glowOffset = offset + new Vector2((float)Math.Sin(timer / 40f + n * 2f) * 3, (float)Math.Cos(timer / 40f + n * 3f) * 3);
                    ItemSlot.DrawItemIcon(item, ItemSlot.Context.InventoryItem, spriteBatch, GetOuterDimensions().Center() + glowOffset, 1f, GetOuterDimensions().Width, Color.White * 0.3f);
                }

                ItemSlot.DrawItemIcon(item, ItemSlot.Context.InventoryItem, spriteBatch, GetOuterDimensions().Center() + offset, 1f, GetOuterDimensions().Width, Color.White);

            }
        }
    }
}
