using Fargowiltas.Content.Buffs.SpawnBoosters;
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
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public WormSnackBuff() : base(() => Main.hardMode ? [NPCID.DiggerHead] : [NPCID.GiantWormHead], () => Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight, 0.2f)
        {
        }
    }
}