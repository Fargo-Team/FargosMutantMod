using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class HolyGrail : BaseSummon
    {
        public override int NPCType => NPCID.Tim;
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			// DisplayName.SetDefault("Holy Grail");
			/* Tooltip.SetDefault("Summons Tim" +
                               "\nOnly usable at night or underground"); */

			ItemID.Sets.SortingPriorityBossSpawns[Type] = 0; // Places it before any other bosses
		}

        public override bool CanUseItem(Player player)
        {
            return FargoUtils.ActuallyNight || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight;
        }

        public override void AddRecipes()
        {
			CreateRecipe()
					.AddRecipeGroup("Fargowiltas:AnyGoldBar", 4)
					.AddIngredient(ItemID.ManaPotion, 6)
					.AddIngredient(ItemID.Ruby)
					.AddTile(TileID.DemonAltar)
					.Register();
        }
    }
}