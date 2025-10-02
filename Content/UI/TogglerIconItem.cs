using Fargowiltas.Assets.Textures;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI
{
    // Exists to be displayed as an item icon in the Toggler UI when inflicted with Mutant's Presence.
    public class TogglerIconItem : ModItem
    {
        public override string Texture => FargoMutantAssets.GetAssetString("UI", "SoulTogglerToggle");
    }
}
