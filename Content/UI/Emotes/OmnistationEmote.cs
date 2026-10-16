using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI.Emotes;

public class OmnistationEmote : ModEmoteBubble
{
    public override void SetStaticDefaults()
    {
        AddToCategory(EmoteID.Category.Items);
    }
}
