using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class BloodUrchin : BaseSummon
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override int NPCType => NPCID.BloodEelHead;
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.BloodMoonStarter]; // 18
		}

        public override bool CanUseItem(Player player)
        {
            return FargoUtils.ActuallyNight && Main.bloodMoon;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.BloodMoonStarter)
                .AddIngredient(ItemID.DeepRedPaint)
                .AddRecipeGroup(RecipeGroups.AnyFoodT3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}