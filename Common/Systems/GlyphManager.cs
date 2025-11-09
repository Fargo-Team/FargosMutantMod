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
            if (GlyphRegistry.ContainsKey(text))
            {
                return new CustomGlyphSnippet(text)
                {
                    DeleteWhole = true,
                    Text = "[g:" + text + "]"
                };
            }

            // not recognized, fallback to vanilla
            return orig(self, text, baseColor, options);
        }
    }

    public static class GlyphRegistry
    {
        private static Dictionary<string, string> registry = new Dictionary<string, string>();

        public static void Register(string key, string filename)
        {
            if (registry.ContainsKey(key))
                return;

            registry[key] = filename;
        }

        public static bool ContainsKey(string key) => registry.ContainsKey(key);

        public static string GetFileName(string key)
        {
            if (!registry.TryGetValue(key, out string value))
                return null;

            return value;
        }
    }

    internal class CustomGlyphSnippet : TextSnippet
    {
        private readonly string key;
        private readonly Vector2 textureSize;

        public CustomGlyphSnippet(string key)
        {
            this.key = key;
            Color = Color.White;

            string fileName = GlyphRegistry.GetFileName(key);
            Texture2D tex = ModContent.Request<Texture2D>(fileName, AssetRequestMode.ImmediateLoad).Value;
            textureSize = tex.Frame().Size();
        }

        public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch,
            Vector2 position = default, Color color = default, float scale = 1f)
        {
            if (!justCheckingString)
            {
                string fileName = GlyphRegistry.GetFileName(key);
                Texture2D tex = ModContent.Request<Texture2D>(fileName).Value;
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
