using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class SlimyLockBox : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<SlimyLockBoxBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                  .AddIngredient<GizmoParts>(2)
                  .AddIngredient(ItemID.Bone, 50)
                  .AddIngredient(ItemID.Gel, 50)
                  .AddRecipeGroup("Fargowiltas:AnyGoldBar", 10)
                  .AddTile(TileID.Solidifier)
                  .Register();
        }
    }
    public class SlimyLockBoxBuff : BaseSpawnBoosterBuff
    {
        public SlimyLockBoxBuff() : base(() => [NPCID.DungeonSlime], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}