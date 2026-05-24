using Fargowiltas.Assets.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI
{
    public class UIFargoPanel : UIPanel
    {
        private static Asset<Texture2D> borderTexture = ModContent.Request<Texture2D>(FargoMutantAssets.GetAssetString("UI", "FargoPanelBorder"));
        private static Asset<Texture2D> backgroundTexture = ModContent.Request<Texture2D>(FargoMutantAssets.GetAssetString("UI", "FargoPanelBackground"));

        public UIFargoPanel() : base(backgroundTexture, borderTexture)
        {
            BackgroundColor = Color.White;
            BorderColor = Color.White;
        }
    }
}
