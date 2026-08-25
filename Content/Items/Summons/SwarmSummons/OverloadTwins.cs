using Terraria;
using Terraria.ID;


namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadTwins : SwarmSummonBase
{
    public OverloadTwins() : base(NPCID.Retinazer, nameof(OverloadTwins), 25, ItemID.MechanicalEye)
    {
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.MechanicalEye]; // 8
    }
    public override bool CanUseItem(Player player)
    {
        return !Fargowiltas.SwarmActive && !Main.IsItDay();
    }
}