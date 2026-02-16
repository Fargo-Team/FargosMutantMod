using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class HallowChest : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<HallowChestBuff>();
    }
    public class HallowChestBuff : BaseSpawnBoosterBuff
    {
        public HallowChestBuff() : base(() => [NPCID.BigMimicHallow], () => Main.LocalPlayer.ZoneHallow && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight), 0.2f)
        {
        }
    }
}