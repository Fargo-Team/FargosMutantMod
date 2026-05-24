using Fargowiltas.Content.Items.Misc;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class CrimsonChest : BaseSummon
    {
        public override int NPCType => NPCID.BigMimicCrimson;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 6; // Places it right after Gelatin Crystal
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.Chest, 1)
                .AddIngredient(ItemID.NightKey, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}