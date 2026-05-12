using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class HallowChest : BaseSummon
    {
        public override int NPCType => NPCID.BigMimicHallow;

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
                .AddIngredient(ItemID.LightKey, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}