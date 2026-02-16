using Fargowiltas.Content.Buffs;
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
        public GrandCrossBuff() : base(() => [NPCID.Paladin], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}