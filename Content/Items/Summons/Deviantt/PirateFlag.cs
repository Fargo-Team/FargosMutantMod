using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class PirateFlag : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<PirateFlagBuff>();
    }
    public class PirateFlagBuff : BaseSpawnBoosterBuff
    {
        public PirateFlagBuff() : base(() => [NPCID.PirateCaptain], () => Main.invasionType == InvasionID.PirateInvasion, 0.2f)
        {
        }
    }
}