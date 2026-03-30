using Fargowiltas.Assets.Textures;
using Fargowiltas.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public class UIPotionToggle : UIElement
    {
        public const int CheckboxTextSpace = 4;

        public static DynamicSpriteFont Font => Terraria.GameContent.FontAssets.ItemStack.Value;

        public int ItemID;
        public int BuffID;

        public UIPotionToggle(int itemID, int buffID)
        {
            ItemID = itemID;
            BuffID = buffID;

            Width.Set(18, 0);
            Height.Set(18, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Vector2 position = GetDimensions().Position();
            Player player = Main.LocalPlayer;
            FargoPlayer modPlayer = player.FargoMutant();

            if (IsMouseHovering && Main.mouseLeft && Main.mouseLeftRelease)
            {
                modPlayer.PotionToggler.Toggles[ItemID].ToggleBool = !modPlayer.PotionToggler.Toggles[ItemID].ToggleBool;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    modPlayer.SyncPotionToggle(ItemID);
            }

            bool toggled = Main.LocalPlayer.GetPotionToggleValue(ItemID);

            spriteBatch.Draw(FargoMutantAssets.UI.Toggler.CheckBox.Value, position + new Vector2(0, 0), Color.White);           

            if (toggled)
            {
                spriteBatch.Draw(FargoMutantAssets.UI.Toggler.CheckMark.Value, position + new Vector2(0, -4), Color.White);
                if (IsMouseHovering)
                {
                    spriteBatch.Draw(FargoMutantAssets.UI.Toggler.CheckMarkGlow.Value, position + new Vector2(0, -4), Color.White);
                }           
            }
                

            string GetText()
            {
                string desc = Lang.GetBuffName(BuffID);
                if (ItemID <= 0) return desc;
                string itemIcon = $"[i:{ItemID}]";
                return $"{itemIcon} {desc}";
            }
            string text = GetText();
            position += new Vector2(Width.Pixels * Main.UIScale, 0);
            position += new Vector2(CheckboxTextSpace, 0);
            position += new Vector2(0, Font.MeasureString(text).Y * 0.175f);
            Color color = Color.White;

            Utils.DrawBorderString(spriteBatch, text, position, color);
        }
    }
}
