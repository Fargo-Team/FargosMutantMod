using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class HeartChocolate : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<HeartChocolateBuff>();


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.LifeCrystal)
                .AddRecipeGroup(RecipeGroups.AnyFoodT2)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
    public class HeartChocolateBuff : BaseSpawnBoosterBuff
    {
        public HeartChocolateBuff() : base(() => [NPCID.Nymph], () => Main.LocalPlayer.ZoneRockLayerHeight, 0.2f)
        {
        }
    }
}