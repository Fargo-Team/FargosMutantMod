using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class CloudSnack : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<CloudSnackBuff>();
    }
    public class CloudSnackBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public CloudSnackBuff() : base(() => [NPCID.WyvernHead], () => Main.LocalPlayer.ZoneSkyHeight, 0.2f)
        {
        }
    }
}