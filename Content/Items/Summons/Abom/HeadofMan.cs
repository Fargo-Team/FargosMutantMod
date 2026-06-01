using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Abom
{
    public class HeadofMan : BaseSummon
    {
        public override int NPCType => NPCID.HeadlessHorseman;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.PumpkinMoonMedallion]; // 14
        }

        public override bool CanUseItem(Player player) => !Main.IsItDay();
    }
}