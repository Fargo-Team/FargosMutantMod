using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Abom
{
    public class MartianMemoryStick : BaseSummon
    {
        public override int NPCType => NPCID.MartianSaucerCore;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityMiscImportants[Type] = 17; // Places it right after Lihzahrd Power Cell and Solar Tablet
        }
    }
}