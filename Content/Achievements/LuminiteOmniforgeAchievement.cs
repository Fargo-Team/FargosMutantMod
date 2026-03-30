using Fargowiltas.Content.Items.Tiles;
using System.Collections.Generic;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements
{
    public class LuminiteOmniforgeAchievement : ModAchievement
    {
        public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

        public override int Index => 11;

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

            AddItemCraftCondition(ModContent.ItemType<LuminiteOmniforge>());
        }

        public override Position GetDefaultPosition() => new After("CHAMPION_OF_TERRARIA");

        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new Before(ModContent.GetInstance<CrucibleofTheCosmosAchievement>());
        }
    }
}
