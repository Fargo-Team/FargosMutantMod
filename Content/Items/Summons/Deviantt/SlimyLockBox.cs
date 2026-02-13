using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
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
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public SlimyLockBoxBuff() : base(() => [NPCID.DungeonSlime], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}