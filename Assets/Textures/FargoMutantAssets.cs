using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

// Base namespace for convinience
namespace Fargowiltas.Assets.Textures
{
    public class FargoMutantAssets
    {
        public static string Filepath => "Fargowiltas/Assets/Textures/";

        /// <summary>
        /// Retrieves the asset string associated with a texture
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static string GetAssetString(string path, string name) => Filepath + path + "/" + name;

        /// <summary>
        /// Shorthand for for grabbing a texture through Modcontent.Request.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Asset<Texture2D> GetTexture2D(string path, string name) => ModContent.Request<Texture2D>(GetAssetString(path, name), AssetRequestMode.ImmediateLoad);

        public class UI
        {
            public static Asset<Texture2D> SoulTogglerButtonTexture => ModContent.Request<Texture2D>(Filepath + "UI/SoulTogglerToggle", AssetRequestMode.ImmediateLoad);
            public static Asset<Texture2D> SoulTogglerButton_MouseOverTexture => ModContent.Request<Texture2D>(Filepath + "UI/SoulTogglerToggle_MouseOver", AssetRequestMode.ImmediateLoad);

            public class Toggler
            {
                public static Asset<Texture2D> SoulTogglerButtonTexture => ModContent.Request<Texture2D>(Filepath + "UI/SoulTogglerToggle", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> SoulTogglerButton_MouseOverTexture => ModContent.Request<Texture2D>(Filepath + "UI/SoulTogglerToggle_MouseOver", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> CheckBox => ModContent.Request<Texture2D>(Filepath + "UI/CheckBox", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> CheckMark => ModContent.Request<Texture2D>(Filepath + "UI/CheckMark", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> CheckMarkGlow => ModContent.Request<Texture2D>(Filepath + "UI/CheckMarkGlow", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> Cross => ModContent.Request<Texture2D>(Filepath + "UI/Cross", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> CrossGlow => ModContent.Request<Texture2D>(Filepath + "UI/CrossGlow", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> DisplayAllButton => ModContent.Request<Texture2D>(Filepath + "UI/DisplayAllButton", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> PresetCustom => ModContent.Request<Texture2D>(Filepath + "UI/PresetCustom", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> PresetMinimal => ModContent.Request<Texture2D>(Filepath + "UI/PresetMinimal", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> PresetOff => ModContent.Request<Texture2D>(Filepath + "UI/PresetOff", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> PresetOn => ModContent.Request<Texture2D>(Filepath + "UI/PresetOn", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> PresetOutline => ModContent.Request<Texture2D>(Filepath + "UI/PresetOutline", AssetRequestMode.ImmediateLoad);
                public static Asset<Texture2D> ReloadButton => ModContent.Request<Texture2D>(Filepath + "UI/ReloadButton", AssetRequestMode.ImmediateLoad);
            }
        }
    }
}
