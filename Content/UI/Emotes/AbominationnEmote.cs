using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI.Emotes;

public class AbominationnEmote : ModEmoteBubble
{
    public override void SetStaticDefaults()
    {
        AddToCategory(EmoteID.Category.Town);
    }
}
