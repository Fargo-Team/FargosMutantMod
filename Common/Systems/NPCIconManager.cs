using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Fargowiltas.Common.Systems
{
    public class NPCIconSystem : ModSystem
    {
        private readonly string[] tagNames = { "h", "head" };

        public override void Load()
        {
            ChatManager.Register<NPCIconTagHandler>(tagNames);
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

    public class NPCIconTagHandler : ITagHandler
    {
        public class NPCIconSnippet : TextSnippet
        {
            private int id;
            private Vector2 frameSize;

            public NPCIconSnippet(int npcID)
            {
                this.id = npcID;
                this.frameSize = new Vector2(TextureAssets.NpcHead[npcID].Size().X, 26);
            }

            public static bool ShouldDraw = true;

            public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
            {
                if (!justCheckingString && color != Color.Black)
                {
                    Texture2D value = TextureAssets.NpcHead[id].Value;
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
            int npcID = int.Parse(text);
                       
            if (!text.AsSpan().IsWhiteSpace() && npcID != -1)
            {
                return new NPCIconSnippet(npcID)
                {
                    DeleteWhole = true,
                    Text = "[h:" + text + "]"
                };
            }

            return new TextSnippet("");
        }
    }
}
