using Microsoft.Extensions.Primitives;
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
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Fargowiltas.Common.Systems
{
    public class GlyphManager : ModSystem
    {
        public override void Load()
        {
            On_GlyphTagHandler.Terraria_UI_Chat_ITagHandler_Parse += ParseGlyph;
        }

        public override void Unload()
        {
            On_GlyphTagHandler.Terraria_UI_Chat_ITagHandler_Parse -= ParseGlyph;
        }

        private TextSnippet ParseGlyph(On_GlyphTagHandler.orig_Terraria_UI_Chat_ITagHandler_Parse orig, GlyphTagHandler self, string text, Color baseColor, string options)
        {
            // vanilla glyph
            if (int.TryParse(text, out var result) && result < 26)
                return orig(self, text, baseColor, options);

            // custom glyph
            string[] s = text.Split('/');
            if (s.Length == 2 && GlyphPathRegistry.ContainsMod(s[0]))
            {
                return new CustomGlyphSnippet(s[0], s[1])
                {
                    DeleteWhole = true,
                    Text = "[g:" + text + "]"
                };
            }

            // not recognized, fallback to vanilla
            return orig(self, text, baseColor, options);
        }
    }

    public static class GlyphPathRegistry
    {
        private static Dictionary<string, string> registry = new Dictionary<string, string>();

        public static void Register(string modName, string filePath)
        {
            if (registry.ContainsKey(modName))
                return;

            registry[modName] = filePath;
        }

        public static bool ContainsMod(string modName) => registry.ContainsKey(modName);

        public static string GetFilePath(string modName)
        {
            if (!registry.TryGetValue(modName, out string value))
                return null;

            return value;
        }
    }

    internal class CustomGlyphSnippet : TextSnippet
    {
        private readonly string texturePath;
        private readonly Vector2 textureSize;

        public CustomGlyphSnippet(string modName, string value)
        {
            Color = Color.White;

            string fileName = $"{GlyphPathRegistry.GetFilePath(modName)}/{value}";
            Texture2D tex = ModContent.Request<Texture2D>(fileName, AssetRequestMode.ImmediateLoad).Value;
            texturePath = fileName;
            textureSize = tex.Frame().Size();
        }

        public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch,
            Vector2 position = default, Color color = default, float scale = 1f)
        {
            if (!justCheckingString)
            {
                Texture2D tex = ModContent.Request<Texture2D>(texturePath).Value;
                Rectangle frame = tex.Frame();
                Vector2 origin2 = frame.Size() / 2;
                spriteBatch.Draw(tex, position + origin2, frame, color, 0f, origin2, scale, SpriteEffects.None, 0f);
            }


            size = textureSize * scale * GlyphScale;
            return true;
        }

        public readonly float GlyphScale = 0.85f;

        public override float GetStringLength(DynamicSpriteFont font)
        {
            return 26f * GlyphScale;
        }
    }
}
