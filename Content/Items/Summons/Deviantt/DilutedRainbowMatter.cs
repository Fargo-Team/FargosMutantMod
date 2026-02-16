using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class DilutedRainbowMatter : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<DilutedRainbowMatterBuff>();

    }
    public class DilutedRainbowMatterBuff : BaseSpawnBoosterBuff
    {
        public DilutedRainbowMatterBuff() : base(() => [NPCID.RainbowSlime], () => Main.LocalPlayer.ZoneHallow && Main.IsItRaining, 0.2f)
        {
        }
    }
}