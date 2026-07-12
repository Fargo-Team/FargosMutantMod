using Fargowiltas.Content.Buffs;
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
                .AddIngredient(ItemID.BloodMoonStarter)
                .AddIngredient(ItemID.Bomb, 20)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class ClownLicenseBuff : BaseSpawnBoosterBuff
    {
        public ClownLicenseBuff() : base(() => [NPCID.Clown], () => Main.bloodMoon && Main.LocalPlayer.ZoneOverworldHeight && Main.hardMode, 0.2f)
        {
        }
    }
}