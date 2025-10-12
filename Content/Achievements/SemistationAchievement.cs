using Fargowiltas.Content.Items.Tiles;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements
{
    public class SemistationAchievement : ModAchievement
    {
        public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

        public override int Index => 5;

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

            AddItemCraftCondition(ModContent.ItemType<Semistation>());
        }
        public override Position GetDefaultPosition() => new Before("EYE_ON_YOU");
    }
}
