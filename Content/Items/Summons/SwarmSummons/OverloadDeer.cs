using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadDeer : SwarmSummonBase
{
    public OverloadDeer() : base(NPCID.Deerclops, nameof(OverloadDeer), 50, ItemID.DeerThing)
    {
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.DeerThing]; // 5
    }
    public override bool CanUseItem(Player player)
    {
        return !Fargowiltas.SwarmActive;
    }
}