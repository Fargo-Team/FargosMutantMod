using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Mutant
{
    public class SuspiciousSkull : BaseSummon
    {
        public override int NPCType => (!Main.IsItDay() && !(Main.remixWorld && !(Main.LocalPlayer.Center.Y > Main.worldSurface * 16))) ? NPCID.SkeletronHead : NPCID.DungeonGuardian;

        public override bool ResetTimeWhenUsed => !Main.IsItDay() && !NPC.downedBoss3;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 5; // Places it right after Deer Thing and Abeemination
        }

        public override bool CanUseItem(Player player) => true;

        public override void AddRecipes()
        {
            CreateRecipe()
              .AddIngredient(ItemID.ClothierVoodooDoll)
              .AddTile(TileID.WorkBenches)
              .Register();
        }
    }
}