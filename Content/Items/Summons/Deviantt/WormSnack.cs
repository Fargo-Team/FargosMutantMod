using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class WormSnack : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<WormSnackBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddRecipeGroup(RecipeGroups.AnyFoodT2, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class WormSnackBuff : BaseSpawnBoosterBuff
    {
        public WormSnackBuff() : base(() => Main.hardMode ? [NPCID.DiggerHead] : [NPCID.GiantWormHead], () => Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight, 0.2f)
        {
        }
    }
}