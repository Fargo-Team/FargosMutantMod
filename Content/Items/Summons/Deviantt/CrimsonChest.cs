using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class CrimsonChest : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<CrimsonChestBuff>();
    }
    public class CrimsonChestBuff : BaseSpawnBoosterBuff
    {
        public CrimsonChestBuff() : base(() => [NPCID.BigMimicCrimson], () => Main.LocalPlayer.ZoneCrimson && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight), 0.2f)
        {
        }
    }
}