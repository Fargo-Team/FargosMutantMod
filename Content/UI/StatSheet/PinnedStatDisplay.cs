using Fargowiltas.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI.StatSheet;

public class PinnedStatDisplay : InfoDisplay
{
    public static readonly Color PinnedStatColor = new(135, 206, 250);
    private readonly int slotIndex;

    public PinnedStatDisplay(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    public (string ModName, string Name)? Pin
    {
        get
        {
            if (Main.gameMenu || Main.LocalPlayer == null || !Main.LocalPlayer.active)
                return null;

            var modPlayer = Main.LocalPlayer.GetModPlayer<FargoPlayer>();
            var pins = modPlayer?.PinnedStats;

            if (pins == null)
                return null;

            int count = pins.Count;

            return (slotIndex >= 0 && slotIndex < count) ? pins[slotIndex] : null;
        }
    }

    public override string Texture
    {
        get
        {
            if (Pin is not { } pin)
                return "Fargowiltas/Assets/Symbols/InfoDisplay/StatSheet";

            return $"{SymbolPathRegistry.GetFilePath(pin.ModName)}/InfoDisplay/{pin.Name}";
        }
    }

    public override string Name => $"PinnedStatDisplay{slotIndex}";

    public override bool Active() => Pin is { } pin && StatRegistry.FindStat(pin.ModName, pin.Name) != null;

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        displayColor = PinnedStatColor;

        if (Pin is not { } pin)
            return string.Empty;

        Stat stat = StatRegistry.FindStat(pin.ModName, pin.Name);
        if (stat == null)
            return string.Empty;

        // Fetch raw localized template string (e.g., "Fishing Quests: {0}")
        string rawText = stat.TextFunction?.Invoke() ?? string.Empty;

        // Evaluate actual value to fill {0}
        object val = stat.Value?.Invoke();
        return val != null ? string.Format(rawText, val) : rawText;
    }
}