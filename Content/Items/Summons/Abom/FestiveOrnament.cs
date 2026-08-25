using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Abom;

public class FestiveOrnament : BaseSummon
{
    public override int NPCType => NPCID.Everscream;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.NaughtyPresent]; // 15
    }

    public override bool CanUseItem(Player player) => !Main.IsItDay();
}