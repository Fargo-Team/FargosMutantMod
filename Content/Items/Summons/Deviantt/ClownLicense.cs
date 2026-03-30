using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class ClownLicense : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<ClownLicenseBuff>();

    }
    public class ClownLicenseBuff : BaseSpawnBoosterBuff
    {
        public ClownLicenseBuff() : base(() => [NPCID.Clown], () => Main.bloodMoon, 0.2f)
        {
        }
    }
}