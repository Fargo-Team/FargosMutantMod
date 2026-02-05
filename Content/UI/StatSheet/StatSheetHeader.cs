using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace Fargowiltas.Content.UI.StatSheet
{
    public class StatSheetHeader : UIPanel
    {
        string key;
        string headerString;
        UIText text;
        Asset<Texture2D> line;

        public StatSheetHeader(string key, string headerString)
        {
            this.headerString = headerString;
            this.key = key;
            BackgroundColor = Color.White;

            line = TextureAssets.Extra[ExtrasID.FairyQueenLance];

            text = new UIText($"[c/{Color.Pink.Hex3()}:{headerString}]");
            text.Left.Set(0, 0f);
            text.Top.Set(0, 0f);
            text.Height.Set(36, 0);
            text.HAlign = 0.5f;

            Append(text);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (key != "PermaUpgrade")
            {
                Rectangle lineFrame = line.Value.Frame();
                Vector2 lineOrigin = lineFrame.Size() / 2;
                Vector2 scale = new Vector2(0.2f, 1f);
                float textWidth = 0.4f * text.GetOuterDimensions().Width;
                spriteBatch.Draw(line.Value, GetOuterDimensions().Center() + 0.1f * GetOuterDimensions().Width * Vector2.UnitX + textWidth * Vector2.UnitX, lineFrame, Color.Pink, 0f, lineOrigin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(line.Value, GetOuterDimensions().Center() - 0.1f * GetOuterDimensions().Width * Vector2.UnitX - textWidth * Vector2.UnitX, lineFrame, Color.Pink, MathHelper.Pi, lineOrigin, scale, SpriteEffects.None, 0);
            }
            else
            {
                Rectangle lineFrame = line.Value.Frame();
                Vector2 lineOrigin = lineFrame.Size() / 2;
                Vector2 scale = new Vector2(0.05f, 100f);
                float textHeight = 0.4f * text.GetOuterDimensions().Height;
                spriteBatch.Draw(line.Value, text.GetOuterDimensions().Center(), lineFrame, Color.Pink * 0.5f, 3 * MathHelper.PiOver2, lineOrigin, scale, SpriteEffects.None, 1);
                spriteBatch.Draw(line.Value, text.GetOuterDimensions().Center() + lineFrame.Width * scale.X * Vector2.UnitY, lineFrame, Color.Pink * 0.2f, MathHelper.PiOver2, lineOrigin, scale, SpriteEffects.None, 1);

                spriteBatch.DrawString(FontAssets.MouseText.Value, headerString, text.GetOuterDimensions().Position() + 2f * textHeight * Vector2.UnitY, Color.Pink * 0.1f, 0f, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 1);
            }
        }
    }
}
