using Fargowiltas.Content.Items.Summons.Mutant;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadWall : SwarmSummonBase
    {
        public OverloadWall() : base(NPCID.WallofFlesh, nameof(OverloadWall), 10, ModContent.ItemType<FleshyDoll>())
        {
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMiscImportants[Type] = 5; // Puts it right after Deer Thing and Abeemination, and before Gelatin Crystal
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive && player.ZoneUnderworldHeight;
        }
    }
}