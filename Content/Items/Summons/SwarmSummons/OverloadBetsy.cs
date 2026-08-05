using Fargowiltas.Content.Items.Summons.Abom;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadBetsy : SwarmSummonBase
    {
        public OverloadBetsy() : base(NPCID.DD2Betsy, nameof(OverloadBetsy), 25, ModContent.ItemType<BetsyEgg>())
        {
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMiscImportants[Type] = 17; // Puts it right after Lihzahrd Power Cell and Solar Tablet
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }
    }
}