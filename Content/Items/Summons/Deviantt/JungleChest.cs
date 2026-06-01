using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class JungleChest : BaseSummon
    {
        public override int NPCType => NPCID.BigMimicJungle;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 6; // Places it right after Gelatin Crystal
        }
    }
}