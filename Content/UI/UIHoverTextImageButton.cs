using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace Fargowiltas.Content.UI
{
    public class UIHoverTextImageButton : UIImageButton
    {
        public readonly string Text;

        public UIHoverTextImageButton(Asset<Texture2D> texture, string text) : base(texture) => Text = text;

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;

                Main.instance.MouseText(Text, 0, 0);
            }
        }
    }
}
