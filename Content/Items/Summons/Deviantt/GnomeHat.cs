using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class GnomeHat : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<GnomeHatBuff>();
    }
    public class GnomeHatBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public GnomeHatBuff() : base(() => [NPCID.Gnome], () => Main.LocalPlayer.ZoneOverworldHeight && Main.LocalPlayer.ZonePurity && !Main.IsItDay(), 0.2f)
        {
        }
    }
}