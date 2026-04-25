using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;

namespace Fargowiltas.Common.Systems.Shaders
{
    public class FargoShader
    {
        public Ref<Effect> Effect;

        public Dictionary<string, object> Parameters;

        public FargoShader(Ref<Effect> effect)
        {
            Effect = effect;
            Parameters = new Dictionary<string, object>();
        }

        public FargoShader()
        {
            Effect = null;
            Parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Safely attempts to set the parameter of the given name.
        /// </summary>
        public void TrySetParameter(string name, object value)
        {
            if (Effect == null)
                return;
            EffectParameter param = Effect.Value.Parameters[name];
            if (param == null)
                return;

            Parameters[name] = value;

            switch (value)
            {
                case bool x:
                    param.SetValue(x);
                    break;
                case bool[] x:
                    param.SetValue(x);
                    break;
                case int x:
                    param.SetValue(x);
                    break;
                case int[] x:
                    param.SetValue(x);
                    break;
                case float x:
                    param.SetValue(x);
                    break;
                case float[] x:
                    param.SetValue(x);
                    break;
                case Vector2 x:
                    param.SetValue(x);
                    break;
                case Vector2[] x:
                    param.SetValue(x);
                    break;
                case Vector3 x:
                    param.SetValue(x);
                    break;
                case Vector3[] x:
                    param.SetValue(x);
                    break;
                case Vector4 x:
                    param.SetValue(x);
                    break;
                case Vector4[] x:
                    param.SetValue(x);
                    break;
            }
        }

        /// <summary>
        /// Draws the shader with the Magic Pixel texture.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawMagicPixel(SpriteBatch spriteBatch)
        {
            if (Effect == null)
                return;

            spriteBatch.End(out var state);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, Effect.Value, state.TransformMatrix);
            Rectangle rekt = new(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth, Main.screenHeight);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, rekt, null, default, 0f, TextureAssets.MagicPixel.Value.Size() * 0.5f, 0, 0f);
            spriteBatch.End();
            spriteBatch.Begin(state);
        }
    }
}
