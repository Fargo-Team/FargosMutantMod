using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Fargowiltas.Content.UI.StatSheet;

public class StatLine : UIText
{
    private readonly string ModName;
    private readonly string Name;

    public StatLine(string displayText, string ModName, string Name) : base(displayText)
    {
        this.ModName = ModName;
        this.Name = Name;
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        if (Main.keyState.IsKeyDown(Keys.LeftAlt) || Main.keyState.IsKeyDown(Keys.RightAlt))
        {
            bool? result = Main.LocalPlayer.GetModPlayer<FargoPlayer>().TogglePinnedStat(ModName, Name);
            SoundEngine.PlaySound(result switch { true => SoundID.MenuTick, false => SoundID.MenuClose, null => SoundID.MenuClose with { Pitch = -0.6f }, });
            return;
        }
        base.LeftClick(evt);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        // Dynamically change text color based on pin state before drawing
        bool pinned = Main.LocalPlayer.GetModPlayer<FargoPlayer>().IsStatPinned(ModName, Name);

        // Uses the exact light blue color defined in PinnedStatDisplay
        TextColor = pinned ? PinnedStatDisplay.PinnedStatColor : Color.White;

        base.DrawSelf(spriteBatch);

        if (ContainsPoint(Main.MouseScreen))
        {
            UICommon.TooltipMouseText(Language.GetTextValue(pinned ? "Mods.Fargowiltas.UI.StatSheet.UnpinTooltip" : "Mods.Fargowiltas.UI.StatSheet.PinTooltip"));
        }
    }
}