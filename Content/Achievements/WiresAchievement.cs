using Fargowiltas.Content.Items.Tiles;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements;

public class WiresAchievement : ModAchievement
{
    public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

    public override int Index => 10;

    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

        AddItemPickupCondition(ModContent.ItemType<WiresPainting>());
    }

    public override Position GetDefaultPosition() => new After("GET_CELL_PHONE");
}
