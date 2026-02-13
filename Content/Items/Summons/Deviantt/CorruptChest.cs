using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class CorruptChest : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<CorruptChestBuff>();

    }
    public class CorruptChestBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public CorruptChestBuff() : base(() => [NPCID.BigMimicCorruption], () => Main.LocalPlayer.ZoneCorrupt && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight), 0.2f)
        {
        }
    }
}