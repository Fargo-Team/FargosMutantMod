using Fargowiltas.Content.Items.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class GoldenSlimeCrown : BaseSummon
    {
        public override int NPCType => NPCID.GoldenSlime;
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 0; // Places it before any other bosses
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PinkSlimeCrown>())
                .AddIngredient(ItemID.GoldDust, 100)
                .AddTile(ModContent.TileType<GoldenDippingVatSheet>())
                .DisableDecraft()
                .Register();
        }
    }
}