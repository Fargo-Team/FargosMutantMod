using Fargowiltas.Content.Items.Tiles;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Achievements;

public class ElementalAssemblerAchievement : ModAchievement
{
    public override string TextureName => "Fargowiltas/Content/Achievements/MutantAchievements";

    public override int Index => 8;

    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);

        AddItemCraftCondition(ModContent.ItemType<ElementalAssembler>());
    }

    public override Position GetDefaultPosition() => new After("BONED");
}
