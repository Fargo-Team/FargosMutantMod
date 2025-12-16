using Fargowiltas.Assets.Textures;
using Fargowiltas.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Fargowiltas.FargoSets;

namespace Fargowiltas.Content.UI.SquirrelUI
{
    public class SquirrelInnerPanel : UIPanel
    {
        SquirrelFeedSlot FeedSlot;
        SquirrelPotionSlot PotionSlot;

        public int mode;
        int prevMode;
        public float timer;
        public float swapTimer;

        public bool InFeedMode() => mode == SquirrelUI.FeedMode;

        public SquirrelInnerPanel()
        {

        }

        public void ResetPanel()
        {
            mode = SquirrelUI.FeedMode;
            timer = 0;
            swapTimer = 0;
            FeedSlot?.ReturnItemToPlayer();
            PotionSlot?.ReturnItemToPlayer();
        }

        void OnItemSwap(Item newItem)
        {
            swapTimer = 0;
            
        }

        void OnModeSwap()
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            swapTimer = 0;
        }

        public override void OnInitialize()
        {
            ResetPanel();

            base.OnInitialize();

            FeedSlot = new SquirrelFeedSlot(this);
            FeedSlot.Width.Set(50, 0);
            FeedSlot.Height.Set(50, 0);
            FeedSlot.Top.Set(0.75f * GetOuterDimensions().Height, 0);
            FeedSlot.Left.Set(0.75f * GetInnerDimensions().Width, 0);
            FeedSlot.OnSwap = OnItemSwap;
            Append(FeedSlot);

            PotionSlot = new SquirrelPotionSlot(this);
            PotionSlot.Width.Set(50, 0);
            PotionSlot.Height.Set(50, 0);
            PotionSlot.Top.Set(0.75f * GetOuterDimensions().Height, 0);
            PotionSlot.Left.Set(0.25f * GetInnerDimensions().Width - PotionSlot.Width.Pixels, 0);
            PotionSlot.OnSwap = OnItemSwap;
            Append(PotionSlot);
        }

        Asset<Texture2D> speechBubble => ModContent.Request<Texture2D>(FargoMutantAssets.GetAssetString("UI", "SpeechBubble"));
        Asset<Texture2D> squirrelTexture => ModContent.Request<Texture2D>("Fargowiltas/Content/NPCs/Squirrel");
        private Asset<Texture2D> squirrelEyes => ModContent.Request<Texture2D>("Fargowiltas/Content/NPCs/Squirrel_Eyes");
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            swapTimer++;
            timer++;

            base.DrawSelf(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                Main.UIScaleMatrix);

            Vector2 squirrelPos = GetOuterDimensions().Center();
            SpriteEffects flip = InFeedMode() ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle squirrelFrame = squirrelTexture.Value.Frame(1, 5);

            float swapScale = MathHelper.Clamp(swapTimer / 60f, 0, 1);

            SquirrelItemSlot selectedSlot = InFeedMode() ? FeedSlot : PotionSlot;
            bool? valid = selectedSlot.HasValidItem();

            // underglow
            if (valid == false)
            {
                Color glowColor = Color.Red with { A = 0 };
                for (int j = 0; j < 12; j++)
                {
                    Vector2 afterimageOffset = (MathHelper.TwoPi * j / 12f).ToRotationVector2() * 4f;
                    spriteBatch.Draw(squirrelTexture.Value, squirrelPos + afterimageOffset, squirrelFrame, glowColor * swapScale, 0f, squirrelFrame.Size() / 2, (1 + swapScale * 0.1f) * 2f, flip, 0);
                }
            }
            // squirrel
            spriteBatch.Draw(squirrelTexture.Value, squirrelPos, squirrelFrame, Color.White, 0f, squirrelFrame.Size() / 2, 2f, flip, 0);
            // red eyes
            if (valid == false)
            {
                spriteBatch.Draw(squirrelEyes.Value, squirrelPos, squirrelFrame, Color.White * swapScale, 0f, squirrelFrame.Size() / 2f, 2f, flip, 0f);
            }

            // speech bubble
            if (valid.HasValue)
            {
                SpriteEffects bubbleFlip = !InFeedMode() ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Rectangle frame = speechBubble.Value.Frame(2, frameX: swapTimer < 5 ? 0 : 1);
                Vector2 offset = new Vector2((InFeedMode() ? 1 : -1) * 70, -squirrelFrame.Height);
                spriteBatch.Draw(speechBubble.Value, squirrelPos + offset, frame, Color.White, 0f, frame.Size() / 2, 2f, bubbleFlip, 0f);

                if (swapTimer > 10)
                {
                    Asset<Texture2D> texture = valid.Value ? FargoMutantAssets.UI.Toggler.CheckMark : FargoMutantAssets.UI.Toggler.Cross;
                    Rectangle tFrame = texture.Value.Frame();
                    spriteBatch.Draw(texture.Value, squirrelPos + offset, tFrame, Color.White, 0f, tFrame.Size() * new Vector2(0.5f, 0.6f), 1.5f, SpriteEffects.None, 0f);
                }
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);

            int n = Main.MouseScreen.X > GetOuterDimensions().X + 0.5f * GetOuterDimensions().Width ? SquirrelUI.FeedMode : SquirrelUI.PotionMode;
            if (mode != n)
                OnModeSwap();
            mode = n;
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                Main.UIScaleMatrix);
        }
    }
}
