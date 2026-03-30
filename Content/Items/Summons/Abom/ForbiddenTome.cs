using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Abom
{
    public class ForbiddenTome : BaseSummon
    {
        public override int NPCType => NPCID.DD2DarkMageT1;
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 4; // Places it after the two evil boss summons
		}
    }
}