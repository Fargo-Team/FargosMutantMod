using Terraria;
using Terraria.ID;


namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadGolem : SwarmSummonBase
    {
        public OverloadGolem() : base(NPCID.Golem, nameof(OverloadGolem), 25, ItemID.LihzahrdPowerCell)
        {
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMiscImportants[Type] = ItemID.Sets.SortingPriorityMiscImportants[ItemID.LihzahrdPowerCell]; // 16
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive && NPC.downedPlantBoss;
        }
    }
}