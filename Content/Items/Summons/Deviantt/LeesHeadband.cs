using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
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
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public LeesHeadbandBuff() : base(() => [NPCID.BoneLee], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}