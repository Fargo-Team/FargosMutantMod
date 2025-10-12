using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements
{
    public class NPCSacrificeAchievement : ModAchievement
    {
        public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

        public override int Index => 4;

        public CustomFlagCondition Condition { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

            Condition = AddCondition("NPCSacrificeAchievementCondition");
        }
        public override Position GetDefaultPosition() => new After("NO_HOBO");
    }
}
