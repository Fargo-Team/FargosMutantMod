using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;

namespace Fargowiltas.Common.Systems.Shaders
{
    public class FargoFilter
    {
        public Ref<Effect> Effect;

        public Dictionary<string, object> Parameters;

        public FargoFilter(Ref<Effect> effect)
        {
            Effect = effect;
            Parameters = new Dictionary<string, object>();
            IsActive = false;
        }

        public FargoFilter()
        {
            Effect = null;
            Parameters = new Dictionary<string, object>();
            IsActive = false;
        }

        /// <summary>
        /// Whether the given filter is active
        /// </summary>
        public bool IsActive { get; private set; }


        public float Opacity { get; private set; }

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

        public void Activate() => IsActive = (Effect != null);

        public void Deactivate() => IsActive = false;

        public void Update()
        {
            if (Effect == null)
            {
                Deactivate();
                Opacity = 0;
                return;
            }

            if (IsActive)
            {
                Opacity = Math.Min(Opacity + 0.1f, 1f);
            }
            else
            {
                Opacity = Math.Max(Opacity - 0.1f, 0f);
            }
        }

        public void Apply(string passName = null)
        {
            Effect?.Value.CurrentTechnique.Passes[passName ?? ShaderSystem.DefaultPassName].Apply();
        }
    }
}
