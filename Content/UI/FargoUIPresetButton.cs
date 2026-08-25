using Fargowiltas.Assets.Textures;
using Fargowiltas.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;

namespace Fargowiltas.Content.UI;

public class FargoUIPresetButton : UIElement
{
    public Texture2D Texture;
    public Action<ToggleBackend> ApplyPreset;
    public Action<ToggleBackend> SavePreset;
    public Func<string> Text;  //Needs to be a Func<string> to make it work with localization. Language.GetTextValue does not work correctly on initialize.
    public Func<ToggleBackend> Toggler; // Also needs to be a Func to not be called on initialization, where lookups aren't built yet

    public FargoUIPresetButton(Texture2D tex, Action<ToggleBackend> preset, Func<string> text, Func<ToggleBackend> toggler)
    {
        Texture = tex;
        ApplyPreset = preset;
        SavePreset = null;
        Text = text;
        Toggler = toggler;

        Width.Set(20, 0);
        Height.Set(20, 0);
    }

    public FargoUIPresetButton(Texture2D tex, Action<ToggleBackend> preset, Action<ToggleBackend> save, Func<string> text, Func<ToggleBackend> toggler)
    {
        Texture = tex;
        ApplyPreset = preset;
        SavePreset = save;
        Text = text;
        Toggler = toggler;

        Width.Set(20, 0);
        Height.Set(20, 0);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        CalculatedStyle style = GetDimensions();
        bool hovered = false;
        // Logic
        if (IsMouseHovering)
        {
            Vector2 textPosition = style.Position() + new Vector2(0, style.Height + 8);
            Utils.DrawBorderString(spriteBatch, Text.Invoke(), textPosition, Color.White);

            hovered = true;

            if (Main.mouseLeft && Main.mouseLeftRelease)
            {
                ApplyPreset(Toggler.Invoke());
            }
            if (SavePreset != null && Main.mouseRight && Main.mouseRightRelease)
            {
                SavePreset(Toggler.Invoke());
            }
        }

        // Drawing
        Texture2D outlineTexture = FargoMutantAssets.UI.Toggler.PresetOutline.Value;
        Vector2 position = style.Position();
        spriteBatch.Draw(outlineTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

        Rectangle frame = new(0, 0, 20, 20);
        if (hovered)
            frame.X += 20;
        spriteBatch.Draw(Texture, position, frame, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
    }
}
