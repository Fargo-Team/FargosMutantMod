using Fargowiltas.Content.Items.Tiles;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements
{
    public class MultitaskCenterAchievement : ModAchievement
    {
        public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

        public override int Index => 7;

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

            AddItemCraftCondition(ModContent.ItemType<MultitaskCenter>());
        }
    }
}
