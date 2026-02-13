using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class ShadowflameIcon : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<ShadowflameIconBuff>();
    }
    public class ShadowflameIconBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public ShadowflameIconBuff() : base(() => [NPCID.GoblinSummoner], () => Main.invasionType == InvasionID.GoblinArmy, 0.2f)
        {
        }
    }
}