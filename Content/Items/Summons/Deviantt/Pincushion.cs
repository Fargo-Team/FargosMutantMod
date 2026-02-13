using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class Pincushion : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<PincushionBuff>();
    }
    public class PincushionBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public PincushionBuff() : base(() => [NPCID.Nailhead], () => Main.eclipse, 0.2f)
        {
        }
    }
}