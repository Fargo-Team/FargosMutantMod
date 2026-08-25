using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements
{
    public class TreeTreasureAchievements
    {
        public class T1TreeTreasureAchievement : ModAchievement
        {
            public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

            public override int Index => 0;

            public CustomFlagCondition Condition { get; private set; }

            public override void SetStaticDefaults()
            {
                Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

                Condition = AddCondition("LumberjackTreeTreasures");
            }
            public override Position GetDefaultPosition() => new After("OBTAIN_HAMMER");
        }
        public class T2TreeTreasureAchievement : ModAchievement
        {
            public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

            public override int Index => 1;

            public CustomFlagCondition PurityCondition, DesertCondition, SnowCondition, CavernCondition, JungleCondition, UnderworldCondition;
            public CustomFlagCondition CrimsonCondition, CorruptCondition, BeachCondition, HallowCondition, MushroomCondition;

            public override void SetStaticDefaults()
            {
                Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

                PurityCondition = AddCondition("LumberjackTreeTreasuresPurityCondition");
                DesertCondition = AddCondition("LumberjackTreeTreasuresDesertCondition");
                SnowCondition = AddCondition("LumberjackTreeTreasuresSnowCondition");
                CavernCondition = AddCondition("LumberjackTreeTreasuresCavernCondition");
                JungleCondition = AddCondition("LumberjackTreeTreasuresJungleCondition");
                UnderworldCondition = AddCondition("LumberjackTreeTreasuresUnderworldCondition");
                CrimsonCondition = AddCondition("LumberjackTreeTreasuresCrimsonCondition");
                CorruptCondition = AddCondition("LumberjackTreeTreasuresCorruptCondition");
                BeachCondition = AddCondition("LumberjackTreeTreasuresBeachCondition");
                HallowCondition = AddCondition("LumberjackTreeTreasuresHallowCondition");
                MushroomCondition = AddCondition("LumberjackTreeTreasuresMushroomCondition");
            }

            public override Position GetDefaultPosition() => new After("MASTERMIND");
        }
    }

}
