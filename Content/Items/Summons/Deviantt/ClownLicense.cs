using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class ClownLicense : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<ClownLicenseBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.BloodMoonStarter)
                .AddIngredient(ItemID.DeepRedPaint)
                .AddIngredient(ItemID.Bomb, 20)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class ClownLicenseBuff : BaseSpawnBoosterBuff
    {
        public ClownLicenseBuff() : base(() => [NPCID.Clown], () => Main.bloodMoon, 0.2f)
        {
        }
    }
}