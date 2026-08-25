using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Collections.Concurrent;
using System.Reflection;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI.Chat;

namespace Fargowiltas.Common.Systems;

public class KeywordSystem : ModSystem
{
    private readonly string[] tagNames = { "key", "keyword" };

    public override void Load()
    {
        ChatManager.Register<KeywordTagHandler>(tagNames);
    }

    public override void Unload()
    {
        var handlers = typeof(ChatManager).GetField("_handlers", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as ConcurrentDictionary<string, ITagHandler>;
        foreach (var tag in tagNames)
        {
            handlers.TryRemove(tag, out _);
        }
    }
}

public class KeywordTagHandler : ITagHandler
{
    public class KeywordSnippet : TextSnippet
    {
        string Word;
        string Description;

        public KeywordSnippet(string key)
        {
            this.Word = Language.GetTextValue($"{key}.Keyword");
            this.Description = Language.GetTextValue($"{key}.Description");
            this.Text = Word;
            base.Color = Color.White;
        }

        public Color ShadowColor = Color.Navy;

        public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
        {
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            TextSnippet[] snippet = ChatManager.ParseMessage(Text, Color.White).ToArray();
            if (!justCheckingString && color is { R: > 0, G: > 0, B: > 0 })
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, snippet, position, 0f, color, ShadowColor, Vector2.Zero, Vector2.One, out _, -1);
                ShadowColor = Color.Navy;
            }
            size = ChatManager.GetStringSize(font, snippet, Vector2.One);
            return true;
        }

        public override float GetStringLength(DynamicSpriteFont font)
        {
            return base.GetStringLength(font);
        }

        public override void OnHover()
        {
            ShadowColor = Color.Purple;
            UICommon.TooltipMouseText(Description);
        }
    }

    public TextSnippet Parse(string text, Color baseColor = default(Color), string options = null)
    {
        string[] args = text.Split('/');

        if (args.Length == 2)
        {
            string localPath = $"Mods.{args[0]}.Keywords.{args[1]}";
            if (Language.Exists($"{localPath}.Keyword"))
            {
                return new KeywordSnippet(localPath)
                {
                    DeleteWhole = true
                };
            }
        }

        return new TextSnippet(text);
    }
}
