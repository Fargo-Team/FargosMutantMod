using Terraria;
using Terraria.ID;


namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadDestroyer : SwarmSummonBase
    {
        public OverloadDestroyer() : base(NPCID.TheDestroyer, nameof(OverloadDestroyer), 10, ItemID.MechanicalWorm)
        {
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.MechanicalWorm]; // 9
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive && !Main.IsItDay();
        }
    }
}