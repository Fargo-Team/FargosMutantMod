using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class ForbiddenForbiddenFragment : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<ForbiddenForbiddenFragmentBuff>();

    }
    public class ForbiddenForbiddenFragmentBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public ForbiddenForbiddenFragmentBuff() : base(() => [NPCID.SandElemental], () => Main.LocalPlayer.ZoneSandstorm, 0.2f)
        {
        }
    }
}