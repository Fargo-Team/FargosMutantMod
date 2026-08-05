using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Abom
{
    public class SpookyBranch : BaseSummon
    {
        public override int NPCType => NPCID.MourningWood;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityMiscImportants[Type] = ItemID.Sets.SortingPriorityMiscImportants[ItemID.PumpkinMoonMedallion]; // 14
        }

        public override bool CanUseItem(Player player) => !Main.IsItDay();
    }
}