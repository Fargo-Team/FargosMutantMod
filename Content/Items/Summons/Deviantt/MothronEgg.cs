using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
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
        public MothronEggBuff() : base(() => [NPCID.Mothron], () => Main.eclipse, 0.2f)
        {
        }
    }
}