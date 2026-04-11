using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class SlimyLockBox : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<SlimyLockBoxBuff>();
    }
    public class SlimyLockBoxBuff : BaseSpawnBoosterBuff
    {
        public SlimyLockBoxBuff() : base(() => [NPCID.DungeonSlime], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}