using Microsoft.CodeAnalysis.Operations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            switch (CurrentState)
            {
                case 0: drawParams.Color = Color.DarkGray; break;
                case 1: drawParams.Color = Color.Gray; break;
                case 2: drawParams.Color = Color.White; break;
            }
            return true;
        }
    }
}
