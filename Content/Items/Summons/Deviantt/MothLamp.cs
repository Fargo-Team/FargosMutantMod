using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class MothLamp : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<MothLampBuff>();
    }
    public class MothLampBuff : BaseSpawnBoosterBuff
    {
        public MothLampBuff() : base(() => [NPCID.Moth], () => Main.LocalPlayer.ZoneJungle && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight), 0.2f)
        {
        }
    }
}