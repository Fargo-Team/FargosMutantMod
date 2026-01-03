using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class AttractiveOre : BaseSummon
    {
        public override int NPCType => NPCID.UndeadMiner;
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			// DisplayName.SetDefault("Attractive Ore");
			/* Tooltip.SetDefault("Summons Undead Miner" +
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
                /*.AddIngredient(ItemID.DeadMansChest)
                .AddIngredient(ItemID.MiningHelmet)
                .AddIngredient(ItemID.SpelunkerPotion)
                .AddTile(TileID.HeavyWorkBench)*/
                .AddRecipeGroup("Fargowiltas:AnyGoldOre", 8)
                .AddIngredient(ItemID.SpelunkerPotion)
                .AddIngredient(ItemID.SilverDye)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
}