using Fargowiltas.Assets.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public class UICloseButton : UIElement
    {

        public UICloseButton()
        {
            Width.Set(20, 0);
            Height.Set(20, 0);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            CalculatedStyle style = GetDimensions();
            // Logic
            if (IsMouseHovering)
            {
                Vector2 textPosition = style.Position() + new Vector2(0, style.Height + 8);
                Utils.DrawBorderString(spriteBatch, Language.GetTextValue("Mods.Fargowiltas.UI.Close"), textPosition, Color.White);
            }

            // Drawing
            Texture2D outlineTexture = FargoMutantAssets.UI.Toggler.Cross.Value;
            Vector2 position = style.Position();
            spriteBatch.Draw(outlineTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
