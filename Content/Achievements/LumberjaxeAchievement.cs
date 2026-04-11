using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements
{
    public class LumberjaxeAchievement : ModAchievement
    {
        public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

        public override int Index => 3;

        public CustomFlagCondition Condition { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

            Condition = AddCondition("LumberjaxeAchievementCondition");
        }
        public override Position GetDefaultPosition() => new After("NO_HOBO");
    }
}
