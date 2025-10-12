using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

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

            public CustomIntCondition IntCondition { get; private set; }

            public override void SetStaticDefaults()
            {
                Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

                IntCondition = AddIntCondition(25);
            }

            public override Position GetDefaultPosition() => new After("MASTERMIND");
        }
    }
    
}
