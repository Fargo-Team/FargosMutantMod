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
        }
    }
}
