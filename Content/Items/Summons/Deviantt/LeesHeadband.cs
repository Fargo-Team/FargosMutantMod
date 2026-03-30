using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class LeesHeadband : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<LeesHeadbandBuff>();
    }
    public class LeesHeadbandBuff : BaseSpawnBoosterBuff
    {
        public LeesHeadbandBuff() : base(() => [NPCID.BoneLee], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}