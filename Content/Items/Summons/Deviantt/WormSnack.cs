using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class WormSnack : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<WormSnackBuff>();
    }
    public class WormSnackBuff : BaseSpawnBoosterBuff
    {
        public WormSnackBuff() : base(() => Main.hardMode ? [NPCID.DiggerHead] : [NPCID.GiantWormHead], () => Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight, 0.2f)
        {
        }
    }
}