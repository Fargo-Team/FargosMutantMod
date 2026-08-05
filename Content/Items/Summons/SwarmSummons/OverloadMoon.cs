using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadMoon : SwarmSummonBase
    {
        public OverloadMoon() : base(NPCID.MoonLordCore, nameof(OverloadMoon), 20, ItemID.CelestialSigil)
        {
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMiscImportants[Type] = ItemID.Sets.SortingPriorityMiscImportants[ItemID.CelestialSigil];
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }
    }
}