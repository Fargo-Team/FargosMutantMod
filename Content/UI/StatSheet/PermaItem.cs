using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Fargowiltas.Content.UI.StatSheet
{
    public class PermaItem : UIElement
    {
        int timer;
        Func<bool> activeFunc;
        Item item;
        Asset<Texture2D> line;

        public PermaItem(int type, Func<bool> activeFunc)
        {
            timer = 0;
            item = new Item(type).Clone();
            line = TextureAssets.Extra[178];
            this.activeFunc = activeFunc;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            bool active = activeFunc.Invoke();

            if (item != null)
            {


                if (ContainsPoint(Main.MouseScreen))
                {
                    if (active)
                    {
                        Main.hoverItemName = item.Name;
                        Main.HoverItem = item.Clone();
                    }
                    else
                    {
                        UICommon.TooltipMouseText($"[c/{ItemRarity.GetColor(item.rare).Hex3()}:???]");
                    }
                }

                Rectangle lineFrame = line.Value.Frame();
                Vector2 lineOrigin = lineFrame.Size() / 2;
                Vector2 scale = new Vector2(0.02f, 20f);
                float height = 0.2f * GetOuterDimensions().Height;
                spriteBatch.Draw(line.Value, GetOuterDimensions().Center() + height * Vector2.UnitY, lineFrame, Color.Pink * 0.25f, 3 * MathHelper.PiOver2, lineOrigin, scale, SpriteEffects.None, 1);
                spriteBatch.Draw(line.Value, GetOuterDimensions().Center() + (height + lineFrame.Width * scale.X) * Vector2.UnitY, lineFrame, Color.Pink * 0.1f, MathHelper.PiOver2, lineOrigin, scale, SpriteEffects.None, 1);

                Color drawColor = active ? Color.White : Color.Black;
                Vector2 offset = new Vector2(0, 0.7f * MathF.Sin(MathHelper.TwoPi * timer++ / 300f) - 2);
                if (active)
                {
                    for (int n = 0; n < 5; n++)
                    {
                        Vector2 glowOffset = offset + new Vector2((float)Math.Sin(timer / 40f + n * 2f) * 3, (float)Math.Cos(timer / 40f + n * 3f) * 3);
                        ItemSlot.DrawItemIcon(item, ItemSlot.Context.InventoryItem, spriteBatch, GetOuterDimensions().Center() + glowOffset, 1f, GetOuterDimensions().Width, Color.White * 0.3f);
                    }
                }

                ItemSlot.DrawItemIcon(item, ItemSlot.Context.InventoryItem, spriteBatch, GetOuterDimensions().Center() + offset, 1f, GetOuterDimensions().Width, drawColor);

            }
        }
    }
}
