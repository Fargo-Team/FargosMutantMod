using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadBee : SwarmSummonBase
{
    public OverloadBee() : base(NPCID.QueenBee, nameof(OverloadBee), 50, ItemID.Abeemination)
    {
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.Abeemination]; // 5
    }

    public override bool CanUseItem(Player player)
    {
        return !Fargowiltas.SwarmActive;
    }
}