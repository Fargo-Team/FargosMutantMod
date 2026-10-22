using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements;

public class TreeTreasureAchievements
{
    public class T1TreeTreasureAchievement : ModAchievement
    {
        public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

        public override int Index => 0;

        public CustomFlagCondition Condition { get; private set; } = new(string.Empty);

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);

            Condition = AddCondition("LumberjackTreeTreasures");
        }
        public override Position GetDefaultPosition() => new After("OBTAIN_HAMMER");
    }
    public class T2TreeTreasureAchievement : ModAchievement
    {
        public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

        public override int Index => 1;

        public CustomFlagCondition PurityCondition = new(string.Empty), DesertCondition = new(string.Empty),
            SnowCondition = new(string.Empty), CavernCondition = new(string.Empty),
            JungleCondition = new(string.Empty), UnderworldCondition = new(string.Empty);
        public CustomFlagCondition CrimsonCondition = new(string.Empty), CorruptCondition = new(string.Empty),
            BeachCondition = new(string.Empty), HallowCondition, MushroomCondition = new(string.Empty);

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);

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
