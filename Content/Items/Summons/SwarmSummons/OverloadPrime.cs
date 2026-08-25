using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadPrime : SwarmSummonBase
{
    public OverloadPrime() : base(NPCID.SkeletronPrime, nameof(OverloadPrime), 25, ItemID.MechanicalSkull)
    {
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.MechanicalSkull]; // 10
    }

    public override bool CanUseItem(Player player)
    {
        return !Fargowiltas.SwarmActive && !Main.IsItDay();
    }
}