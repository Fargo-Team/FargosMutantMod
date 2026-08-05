using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Abom
{
    public class BatteredClub : BaseSummon
    {
        public override int NPCType => NPCID.DD2OgreT2;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityMiscImportants[Type] = 11; // Places it right after Pirate Map
        }
    }
}