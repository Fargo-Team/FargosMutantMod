using Fargowiltas.Assets.Textures;
using Fargowiltas.Content.NPCs.SquirrelNPC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Content.UI.SquirrelUI
{
    public class SquirrelIcon : UIElement
    {
        Item item;
        int swapTimer;
        int timer;
        int mode;

        Asset<Texture2D> speechBubble => ModContent.Request<Texture2D>(FargoMutantAssets.GetAssetString("UI", "SpeechBubble"));
        public SquirrelIcon()
        {
            mode = -1;
            timer = 0;
        }

        public void NewBubble(int mode, Item item)
        {
            timer = 0;
            this.mode = mode;
            this.item = item.Clone();
        }

        public void Kill()
        {
            timer = 0;
            mode = -1;
            item.TurnToAir(true);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (mode == -1 || item.IsAir)
                return;

            timer++;

            SpriteEffects flip = mode == SquirrelUI.PotionMode ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle frame = speechBubble.Value.Frame(2, frameX: timer < 10 ? 0 : 1);
            Vector2 offset = new Vector2((mode == SquirrelUI.PotionMode ? 1 : -1) * 70, 0);
            spriteBatch.Draw(speechBubble.Value, GetOuterDimensions().Center() + offset, frame, Color.White, 0f, frame.Size() / 2, 2f, flip, 0f);

            if (timer > 20)
            {
                Asset<Texture2D> texture = Squirrel.CanSacrifice(item) ? FargoMutantAssets.UI.Toggler.CheckMark : FargoMutantAssets.UI.Toggler.Cross;
                Rectangle tFrame = texture.Value.Frame();
                spriteBatch.Draw(texture.Value, GetOuterDimensions().Center() + offset, tFrame, Color.White, 0f, tFrame.Size() * new Vector2(0.5f, 0.6f), 1.5f, SpriteEffects.None, 0f);
            }
        }
    }
}
