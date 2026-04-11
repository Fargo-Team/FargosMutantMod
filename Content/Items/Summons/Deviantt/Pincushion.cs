using Fargowiltas.Content.Buffs;
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
        public PincushionBuff() : base(() => [NPCID.Nailhead], () => Main.eclipse, 0.2f)
        {
        }
    }
}