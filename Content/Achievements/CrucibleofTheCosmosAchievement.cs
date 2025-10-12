using Fargowiltas.Content.Items.Tiles;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements
{
    public class CrucibleofTheCosmosAchievement : ModAchievement
    {
        public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

        public override int Index => 9;

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);

            AddItemCraftCondition(ModContent.ItemType<CrucibleCosmos>());
        }
        public override Position GetDefaultPosition() => new After("CHAMPION_OF_TERRARIA");
    }
}
