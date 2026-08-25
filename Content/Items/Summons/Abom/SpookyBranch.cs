using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Abom;

public class SpookyBranch : BaseSummon
{
    public override int NPCType => NPCID.MourningWood;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.PumpkinMoonMedallion]; // 14
    }

    public override bool CanUseItem(Player player) => !Main.IsItDay();
}