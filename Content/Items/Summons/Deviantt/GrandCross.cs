using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class GrandCross : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<GrandCrossBuff>();
    }
    public class GrandCrossBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public GrandCrossBuff() : base(() => [NPCID.Paladin], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}