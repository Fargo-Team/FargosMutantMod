using Terraria;
using Terraria.ID;


namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadWorm : SwarmSummonBase
{
    public OverloadWorm() : base(NPCID.EaterofWorldsHead, nameof(OverloadWorm), 25, ItemID.WormFood)
    {
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.WormFood]; // 3
    }

    public override bool CanUseItem(Player player)
    {
        return !Fargowiltas.SwarmActive;
    }
}