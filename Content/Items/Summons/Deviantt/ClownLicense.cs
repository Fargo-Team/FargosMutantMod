using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
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
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public ClownLicenseBuff() : base(() => [NPCID.Clown], () => Main.bloodMoon, 0.2f)
        {
        }
    }
}