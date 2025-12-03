using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Fargowiltas.Content.UI.StatSheet
{
    public class CategoryButton : UIPanel
    {
        StatCategory category;
        public Action<StatCategory> onPress;
        Asset<Texture2D> texture;
        public bool selected;
        Point drawFrame;

        public CategoryButton(StatCategory category, bool selected)
        {
            this.category = category;
            texture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Tabs_A", AssetRequestMode.ImmediateLoad);
            this.selected = selected;
            drawFrame = new Point(2, 1);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //base.DrawSelf(spriteBatch);

            Color c = category.Name == "PermaUpgrade" ? Color.Lerp(Color.Purple, Color.White, 0.4f) : Color.White;
            if (ContainsPoint(Main.MouseScreen))
            {
                drawFrame.X = 2;
                UICommon.TooltipMouseText($"[c/{c.Hex3()}:{Language.GetTextValue(category.HeaderLocalPath)}]");
            }
            else
            {
                drawFrame.X = 3;
            }
            drawFrame.Y = 0;

            if (selected)
            {
                drawFrame.Y += 1;
                drawFrame.X -= 2;
            }

            Point frameOffset = new Point(drawFrame.X == 1 ? -1 : 0, drawFrame.Y == 0 ? -1 : 0); // i hate this game.
            Rectangle frame = texture.Value.Frame(4, 2, drawFrame.X, drawFrame.Y, frameOffset.X, frameOffset.Y);
            Vector2 origin2 = frame.Size() / 2;
            spriteBatch.Draw(texture.Value, GetOuterDimensions().Center() + 9.8f * Vector2.UnitY, frame, c, 0f, origin2, 1f, SpriteEffects.None, 0);

            if (category.iconPath != null)
            {
                Asset<Texture2D> icon = ModContent.Request<Texture2D>(category.iconPath, AssetRequestMode.ImmediateLoad);
                Rectangle iconFrame = icon.Frame();
                spriteBatch.Draw(icon.Value, GetOuterDimensions().Center() + new Vector2(-1, 0.15f * iconFrame.Height), iconFrame, Color.White * 0.8f, 0f, iconFrame.Size() / 2, 1f, SpriteEffects.None, 0);
            }


        }

        public override void LeftClick(UIMouseEvent evt)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            onPress.Invoke(category);
            base.LeftClick(evt);
        }
    }
}
