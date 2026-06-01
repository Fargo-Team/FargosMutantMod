using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.BuilderToggles
{
    public class PlacementReachBuilderToggle : BuilderToggle
    {
        public static LocalizedText FullViewText { get; private set; }
        public static LocalizedText OutlineViewText { get; private set; }
        public static LocalizedText DisabledText { get; private set; }
        public static LocalizedText InverseOpacityText { get; private set; }
        public static LocalizedText NormalOpacityText { get; private set; }

        public override string HoverTexture => Texture;
        public override int NumberOfStates => 3;

        public static int RightClickState = 0;
        public override Position OrderPosition => new After(RulerLine);
        public override bool Active() => true;

        public override void OnRightClick()
        {
            SoundEngine.PlaySound(SoundID.Unlock);

            RightClickState += 1;

            if (RightClickState > 1)
                RightClickState = 0;
        }

        public override string DisplayValue()
        {
            string text = "";
            string rightclicktext = "";

            switch (CurrentState)
            {
                case 0: text = DisabledText.Value; break;
                case 1: text = OutlineViewText.Value; break;
                case 2: text = FullViewText.Value; break;
                default: text = "How did you get here?"; break;
            }
            switch (RightClickState)
            {
                case 0: rightclicktext = NormalOpacityText.Value; break;
                case 1: rightclicktext = InverseOpacityText.Value; break;
                default: rightclicktext = "How did you get here?"; break;
            }
            return text + "\n" + rightclicktext;
        }

        public override void SetStaticDefaults()
        {
            FullViewText = this.GetLocalization(nameof(FullViewText));
            OutlineViewText = this.GetLocalization(nameof(OutlineViewText));
            DisabledText = this.GetLocalization(nameof(DisabledText));

            NormalOpacityText = this.GetLocalization(nameof(NormalOpacityText));
            InverseOpacityText = this.GetLocalization(nameof(InverseOpacityText));
        }


        public override bool Draw(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
        {
            int frame = 0;
            drawParams.Position.X += 1;
            switch (CurrentState)
            {
                case 0: frame = 2; break;
                case 1: frame = 1; break;
                case 2: frame = 0; break;
            }
            drawParams.Frame = new(24 * frame, 0, 24, 22);

            if (RightClickState == 1)
                drawParams.Color *= 0.5f;
            return true;
        }

        public override bool DrawHover(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
        {
            int frame = 0;
            drawParams.Position.X += 1;
            switch (CurrentState)
            {
                case 0: frame = 5; break;
                case 1: frame = 4; break;
                case 2: frame = 3; break;
            }
            drawParams.Frame = new(24 * frame, 0, 24, 22);
            return true;
        }
    }
}
