using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class CloudSnack : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<CloudSnackBuff>();
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cloud, 50)
                .AddRecipeGroup(RecipeGroups.AnyFoodT3)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CloudSnackBuff : BaseSpawnBoosterBuff
    {
        public CloudSnackBuff() : base(() => [NPCID.WyvernHead], () => Main.LocalPlayer.ZoneSkyHeight, 0.2f)
        {
        }
    }
}