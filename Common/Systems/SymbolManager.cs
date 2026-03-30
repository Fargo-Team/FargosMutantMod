using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Fargowiltas.Common.Systems
{
    public class SymbolSystem : ModSystem
    {
        private readonly string[] tagNames = {"s", "symbol"};

        public override void Load()
        {
            ChatManager.Register<SymbolTagHandler>(tagNames);
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

    public class SymbolTagHandler : ITagHandler
    {
        public class SymbolSnippet : TextSnippet
        {
            private string texturePath;
            private Vector2 frameSize;

            public SymbolSnippet(string texturePath)
            {
                this.texturePath = texturePath;
                this.frameSize = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad).Value.Frame().Size();
                base.Color = Color.White;
            }

            public static bool ShouldDraw = true;

            public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
            {   
                if (!justCheckingString && color != Color.Black)
                {
                    Texture2D value = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad).Value;
                    Rectangle frame = value.Frame();
                    Vector2 origin2 = frame.Size() / 2f;
                    if (ShouldDraw)
                        spriteBatch.Draw(value, position + origin2, frame, Color.White, 0f, origin2, scale, SpriteEffects.None, 0f);                
                }
                size = frameSize;
                return true;
            }

            public override float GetStringLength(DynamicSpriteFont font)
            {
                return frameSize.X;
            }
        }

        public TextSnippet Parse(string text, Color baseColor = default(Color), string options = null)
        {
            string[] args = text.Split('/');

            if (args.Length == 2 && SymbolPathRegistry.ContainsMod(args[0]))
            {
                string filePath = $"{SymbolPathRegistry.GetFilePath(args[0])}/{args[1]}";
                return new SymbolSnippet(filePath)
                {
                    DeleteWhole = true,
                    Text = "[s:" + text + "]"
                };
            }

            return new TextSnippet(text);
        }
    }

    public static class SymbolPathRegistry
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

    public class SymbolTracker
    {
        internal bool SymbolsFinalized = false;

        public SymbolTracker()
        {
            Fargowiltas.symbolTracker = this;
            AddSymbolPath(Fargowiltas.Instance.Name, $"{Fargowiltas.Instance.Name}/Assets/Symbols");
        }

        internal void FinalizeSymbols()
        {
            SymbolsFinalized = true;
        }

        public void AddSymbolPath(string modName, string filePath)
        {
            if (!SymbolsFinalized)
                SymbolPathRegistry.Register(modName, filePath);
        }
    }
}
