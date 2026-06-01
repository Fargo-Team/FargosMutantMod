using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class RuneOrb : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<RuneOrbBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                  .AddIngredient<GizmoParts>(2)
                  .AddIngredient(ItemID.LargeAmber)
                  .AddIngredient(ItemID.SoulofLight, 8)
                  .AddIngredient(ItemID.SoulofNight, 8)
                  .AddTile(TileID.CrystalBall)
                  .Register();
        }
    }
    public class RuneOrbBuff : BaseSpawnBoosterBuff
    {
        public RuneOrbBuff() : base(() => [NPCID.RuneWizard], () => Main.LocalPlayer.ZoneRockLayerHeight && (double)Main.LocalPlayer.Center.Y / 16 > (Main.rockLayer + Main.maxTilesY) / 2.0, 0.2f)
        {
        }
    }
}