using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Shaders
{
    public static class DrawExtensions
    {
        public static SpriteBatchState GetSpriteBatchState(this SpriteBatch spriteBatch) => new SpriteBatchState(spriteBatch);

        public static void Begin(this SpriteBatch spriteBatch, SpriteBatchState state)
            => spriteBatch.Begin(state.SortMode, state.BlendState, state.SamplerState, state.DepthStencilState, state.RasterizerState, state.Effect, state.TransformMatrix);

        public static void End(this SpriteBatch spriteBatch, out SpriteBatchState oldState)
        {
            oldState = spriteBatch.GetSpriteBatchState();
            spriteBatch.End();
        }
        
        /// <summary>
        /// Sets the texture buffer at the given channel index. <para/>
        /// Note: Channel 0 will be overriden by any texture passed to any <see cref="SpriteBatch.Draw"/> call.
        /// </summary>
        public static void SetTextureBuffer(this SpriteBatch spriteBatch, int channel, Texture2D texture) => spriteBatch.GraphicsDevice.Textures[channel] = texture;
    }

    /// <summary>
    /// A struct representing the state of a spriteBatch
    /// </summary>
    public struct SpriteBatchState
    {
        private static Type type = typeof(SpriteBatch);
        public readonly SpriteSortMode SortMode;
        public readonly BlendState BlendState;
        public readonly SamplerState SamplerState;
        public readonly DepthStencilState DepthStencilState;
        public readonly RasterizerState RasterizerState;
        public readonly Effect Effect;
        public readonly Matrix TransformMatrix;

        public SpriteBatchState(SpriteBatch spriteBatch)
        {
            SortMode = (SpriteSortMode)GetField(spriteBatch, "sortMode");
            BlendState = (BlendState)GetField(spriteBatch, "blendState");
            SamplerState = (SamplerState)GetField(spriteBatch, "samplerState");
            DepthStencilState = (DepthStencilState)GetField(spriteBatch, "depthStencilState");
            RasterizerState = (RasterizerState)GetField(spriteBatch, "rasterizerState");
            Effect = (Effect)GetField(spriteBatch, "customEffect");
            TransformMatrix = (Matrix)GetField(spriteBatch, "transformMatrix");
        }

        private object GetField(SpriteBatch spriteBatch, string name) => type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(spriteBatch);
    }

    public class ShaderSystem : ModSystem
    {
        private static bool _hasLoaded;
        private static Dictionary<string, FargoShader> shaders;
        public static IReadOnlyDictionary<string, FargoShader> Shaders => shaders;

        private static Dictionary<string, FargoFilter> filters;
        public static IReadOnlyDictionary<string, FargoFilter> Filters => filters;

        public static string DefaultPassName => "AutoloadPass";

        /// <summary>
        /// Safely attempts to retrieve a shader of the given shader name. <para/>
        /// Shader names should be of the format "ModName:FileName"
        /// </summary>
        /// <param name="shaderName"></param>
        public static FargoShader TryGetShader(string shaderName)
        {
            if (shaders.TryGetValue(shaderName, out FargoShader value))
            {
                return value;
            }
            return new();
        }

        /// <summary>
        /// Safely attempts to retrieve a filter of the given filter name. <para/>
        /// Filter names should be of the format "ModName:FileName"
        /// </summary>
        /// <param name="shaderName"></param>
        public static FargoFilter TryGetFilter(string filterName)
        {
            if (filters.TryGetValue(filterName, out FargoFilter value))
            {
                return value;
            }
            return new();
        }

        public override void OnModLoad()
        {
            if (!Main.dedServ)
            {
                Main.QueueMainThreadAction(() =>
                {
                    PrimaryTarget = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                    SecondaryTarget = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                });

                shaders = new Dictionary<string, FargoShader>();
                filters = new Dictionary<string, FargoFilter>();
            }
        }

        public override void PostUpdateEverything()
        {
            base.PostUpdateEverything();

            foreach (var filter in filters.Values)
            {
                filter.Update();
                filter.Deactivate();
            }
        }

        public override void PostSetupContent()
        {
            base.PostSetupContent();

            _hasLoaded = false;
            foreach (Mod mod in ModLoader.Mods)
            {
                LoadShaders(mod);
                LoadFilters(mod);
                ShaderCompileManager.LoadWatchers(mod);
            }
            _hasLoaded = true;
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                shaders = null;
                filters = null;
            }
        }

        public static void SetShader(string name, FargoShader shader)
        {
            shaders[name] = shader;
        }

        public static void SetFilter(string name, FargoFilter filter)
        {
            filters[name] = filter;
        }

        /// <summary>
        /// Load all of the shaders of the given mod.
        /// </summary>
        private static void LoadShaders(Mod mod)
        {
            if (Main.dedServ || _hasLoaded)
                return;

            List<string> fileNames = mod.GetFileNames();
            if (fileNames == null)
                return;

            IEnumerable<string> shaderFiles = fileNames.Where(p => p.Contains("AutoloadedEffects/Shaders") && !p.Contains("FXC/") && p.Contains(".fxc"));

            foreach (var file in shaderFiles)
            {
                string rawName = Path.GetFileNameWithoutExtension(file);
                string displayText = $"{mod.Name}:{rawName}";
                string effectName = Path.Combine(Path.GetDirectoryName(file), rawName);
                Ref<Effect> shader = new Ref<Effect>(mod.Assets.Request<Effect>(effectName, AssetRequestMode.ImmediateLoad).Value);
                SetShader(displayText, new(shader));
            }
        }

        /// <summary>
        /// Load all of the filters of the given mod.
        /// </summary>
        private static void LoadFilters(Mod mod)
        {
            if (Main.dedServ || _hasLoaded)
                return;

            List<string> fileNames = mod.GetFileNames();
            if (fileNames == null)
                return;

            IEnumerable<string> filterFiles = fileNames.Where(p => p.Contains("AutoloadedEffects/Filters") && !p.Contains("FXC/") && p.Contains(".fxc"));
            foreach (var file in filterFiles)
            {
                string rawName = Path.GetFileNameWithoutExtension(file);
                string displayText = $"{mod.Name}:{rawName}";
                string effectName = Path.Combine(Path.GetDirectoryName(file), rawName);
                Ref<Effect> filter = new Ref<Effect>(mod.Assets.Request<Effect>(effectName, AssetRequestMode.ImmediateLoad).Value);
                SetFilter(displayText, new(filter));
            }
        }

        private static RenderTarget2D PrimaryTarget;
        private static RenderTarget2D SecondaryTarget;
        /// <summary>
        /// Applies all active screen filters.
        /// </summary>
        /// <param name="finalTexture"></param>
        /// <param name="screentarget1"></param>
        /// <param name="screentarget2"></param>
        /// <param name="clearColor"></param>
        public static void ApplyFilters(RenderTarget2D finalTexture, RenderTarget2D screentarget1, RenderTarget2D screentarget2, Color clearColor)
        {
            // Vanilla code
            // TODO: Review if this properly works or needs to be adjusted

            RenderTarget2D target1 = null;
            RenderTarget2D target2 = screentarget1;
            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;

            if (Main.LocalPlayer.gravDir == -1f)
            {
                target1 = SecondaryTarget;
                graphicsDevice.SetRenderTarget(target1);
                graphicsDevice.Clear(clearColor);
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Invert(Main.GameViewMatrix.EffectMatrix));
                Main.spriteBatch.Draw(target2, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
                target2 = SecondaryTarget;
            }

            List<FargoFilter> activeFilters = [.. filters.Values.Where(f => f.IsActive)];
            foreach (FargoFilter filter in activeFilters)
            {
                if (filter != null)
                {
                    target1 = ((target2 != target1) ? PrimaryTarget : SecondaryTarget);
                    graphicsDevice.SetRenderTarget(target1);
                    graphicsDevice.Clear(clearColor);
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    filter.Apply();
                    Main.spriteBatch.Draw(target2, Vector2.Zero, Main.ColorOfTheSkies);
                    Main.spriteBatch.End();
                    target2 = ((target2 != target1) ? target1 : target2);
                }
            }
            graphicsDevice.SetRenderTarget(finalTexture);
            graphicsDevice.Clear(clearColor);

            if (target1 != null)
            {
                Main.instance.GraphicsDevice.SetRenderTarget(screentarget1);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(target1, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }
        }
    }
}
