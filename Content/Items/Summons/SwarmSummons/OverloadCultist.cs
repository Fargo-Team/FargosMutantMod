using Fargowiltas.Content.Items.Summons.Mutant;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadCultist : SwarmSummonBase
{
    public OverloadCultist() : base(NPCID.CultistBoss, nameof(OverloadCultist), 25, ModContent.ItemType<CultistSummon>())
    {
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = 18; // Puts it right before Celestial Sigil
    }

    public override bool CanUseItem(Player player)
    {
        return !Fargowiltas.SwarmActive;
    }
}