using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Buffs;
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
                .AddIngredient(ItemID.DirtBlock, 50)
                .AddRecipeGroup(RecipeGroups.AnyFoodT2, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class WormSnackBuff : BaseSpawnBoosterBuff
    {
        public WormSnackBuff() : base(() => Main.hardMode ? [NPCID.DiggerHead] : [NPCID.GiantWormHead], () => (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight) && !Main.LocalPlayer.ZoneSnow, 0.2f)
        {
        }
    }
}