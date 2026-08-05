using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Mutant
{
    public class CultistSummon : BaseSummon
    {
        public override int NPCType => NPCID.CultistBoss;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Zealot's Possession");
            // Tooltip.SetDefault("Summons the Lunatic Cultist\nDoes not spawn the pillars if Lunatic Cultist has been defeated before");

            ItemID.Sets.SortingPriorityMiscImportants[Type] = 18; // Places it right before Celestial Sigil
        }
    }
}