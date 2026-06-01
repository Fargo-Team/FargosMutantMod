using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class DilutedRainbowMatter : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<DilutedRainbowMatterBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.Gel, 100)
                .AddIngredient(ItemID.RainbowDye, 1)
                .AddIngredient(ItemID.SoulofLight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

    }
    public class DilutedRainbowMatterBuff : BaseSpawnBoosterBuff
    {
        public DilutedRainbowMatterBuff() : base(() => [NPCID.RainbowSlime], () => Main.LocalPlayer.ZoneHallow && Main.IsItRaining, 0.2f)
        {
        }
    }
}