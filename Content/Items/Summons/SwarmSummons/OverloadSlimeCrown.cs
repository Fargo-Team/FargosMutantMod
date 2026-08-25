using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadSlimeCrown : SwarmSummonBase
{
    public OverloadSlimeCrown() : base(NPCID.KingSlime, nameof(OverloadSlimeCrown), 50, ItemID.SlimeCrown)
    {
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.SlimeCrown]; // 2
    }

    public override bool CanUseItem(Player player)
    {
        return !Fargowiltas.SwarmActive;
    }
}