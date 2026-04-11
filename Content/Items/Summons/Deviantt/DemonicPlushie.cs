using Fargowiltas.Content.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class DemonicPlushie : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<DemonicPlushieBuff>();
    }
    public class DemonicPlushieBuff : BaseSpawnBoosterBuff
    {
        public DemonicPlushieBuff() : base(() => [NPCID.RedDevil], () => Main.LocalPlayer.ZoneUnderworldHeight && MathF.Abs(Main.LocalPlayer.Center.X / 16f - Main.spawnTileX) > Main.maxTilesX / 3, 0.2f)
        {
        }
    }
}