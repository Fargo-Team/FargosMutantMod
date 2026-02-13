using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class MothronEgg : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<MothronEggBuff>();
    }
    public class MothronEggBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public MothronEggBuff() : base(() => [NPCID.Mothron], () => Main.eclipse, 0.2f)
        {
        }
    }
}