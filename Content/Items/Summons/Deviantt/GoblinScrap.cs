using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class GoblinScrap : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<GoblinScrapBuff>();
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
		}
    }
    public class GoblinScrapBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public GoblinScrapBuff() : base(() => [NPCID.GoblinScout], () => Main.LocalPlayer.ZonePurity && Main.LocalPlayer.ZoneOverworldHeight && ((Main.LocalPlayer.Center.X / 16f - Main.spawnTileX) > Main.maxTilesX / 3), 0.2f) // condition is close enough
        {
        }
    }
}