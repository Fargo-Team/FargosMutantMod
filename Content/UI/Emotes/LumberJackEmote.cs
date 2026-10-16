using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI.Emotes;

public class LumberJackEmote : ModEmoteBubble
{
    public override void SetStaticDefaults()
    {
        AddToCategory(EmoteID.Category.Town);
    }
}
