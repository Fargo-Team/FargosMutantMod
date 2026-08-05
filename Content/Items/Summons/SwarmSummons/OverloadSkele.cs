using Fargowiltas.Content.Items.Summons.Mutant;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadSkele : SwarmSummonBase
    {
        public OverloadSkele() : base(NPCID.SkeletronHead, nameof(OverloadSkele), 40, ModContent.ItemType<SuspiciousSkull>())
        {
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMiscImportants[Type] = 5; // Puts it right after Deer Thing and Abeemination
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }
    }
}