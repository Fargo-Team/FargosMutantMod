using Fargowiltas.Assets.Textures;
using Fargowiltas.Common;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public class CombinedUIButton : FargoUI
    {
        public override int InterfaceIndex(List<GameInterfaceLayer> layers, int vanillaInventoryIndex) => vanillaInventoryIndex;
        public override string InterfaceLayerName => "Fargos: Combined UI Button";
        public UIImage Icon;
        public UIHoverTextImageButton IconHighlight;
        public UIImage IconFlash;
        public override void OnLoad()
        {
            FargoUIManager.Open<CombinedUIButton>();
        }
        public override void UpdateUI()
        {
            if (!Main.playerInventory || Main.LocalPlayer.chest != -1 || Main.LocalPlayer.talkNPC != -1)
                FargoUIManager.Close<CombinedUIButton>();
            else
                FargoUIManager.Open<CombinedUIButton>();
        }
        public const int x = 570;
        public const int y = 278;
        public override void OnActivate()
        {
            IconFlash = new UIImage(FargoMutantAssets.UI.SoulTogglerButton_MouseOverTexture);
            IconFlash.Left.Set(x, 0);
            IconFlash.Top.Set(y, 0);
            Append(IconFlash);

            Icon = new UIImage(FargoMutantAssets.UI.SoulTogglerButtonTexture);
            Icon.Left.Set(x, 0); //26
            Icon.Top.Set(y, 0); //300
            Append(Icon);

            IconHighlight = new UIHoverTextImageButton(FargoMutantAssets.UI.SoulTogglerButton_MouseOverTexture, Language.GetTextValue("Mods.Fargowiltas.UI.CombinedUIButton"));
            IconHighlight.Left.Set(0, 0);
            IconHighlight.Top.Set(0, 0);
            IconHighlight.SetVisibility(1f, 0);
            IconHighlight.OnMouseOver += IconHighlight_MouseOver;
            IconHighlight.OnLeftClick += IconHighlight_OnClick;
            Icon.Append(IconHighlight);

            base.OnActivate();
        }
        private void IconHighlight_MouseOver(UIMouseEvent evt, UIElement listeningElement)
        {

        }
        private void IconHighlight_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!Main.playerInventory)
            {
                return;

            }

            FargoUIManager.Toggle<CombinedUI>();
            Main.LocalPlayer.FargoMutant().HasClickedWrench = true;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.playerInventory)
            {
                //base.Draw(spriteBatch);

                Icon.Draw(spriteBatch);
                IconHighlight.Draw(spriteBatch);
                if (!Main.LocalPlayer.FargoMutant().HasClickedWrench && Main.GlobalTimeWrappedHourly % 1f < 0.5f)
                {
                    IconFlash.Draw(spriteBatch);
                }
            }

        }
    }
}
