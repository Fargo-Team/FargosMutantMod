using Fargowiltas.Content.Items.Summons.Mutant;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadEmpress : SwarmSummonBase
    {
        public OverloadEmpress() : base(NPCID.HallowBoss, nameof(OverloadEmpress), 25, ModContent.ItemType<PrismaticPrimrose>())
        {
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMiscImportants[Type] = 12; // Puts it right after Truffle Worm
        }
        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }
    }
}