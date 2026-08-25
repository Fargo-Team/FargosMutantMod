using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadMoon : SwarmSummonBase
{
    public OverloadMoon() : base(NPCID.MoonLordCore, nameof(OverloadMoon), 20, ItemID.CelestialSigil)
    {
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.CelestialSigil];
    }

    public override bool CanUseItem(Player player)
    {
        return !Fargowiltas.SwarmActive;
    }
}