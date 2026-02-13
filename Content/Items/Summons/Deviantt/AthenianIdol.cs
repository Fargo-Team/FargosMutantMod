using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class AthenianIdol : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<AthenianIdolBuff>();
    }
    public class AthenianIdolBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public AthenianIdolBuff() : base(() => [NPCID.Medusa], () => Main.LocalPlayer.ZoneMarble, 0.2f)
        {
        }
    }
}