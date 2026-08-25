using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt;

public class PirateFlag : BaseSpawnBooster
{
    public override int BuffType => ModContent.BuffType<PirateFlagBuff>();
}
public class PirateFlagBuff : BaseSpawnBoosterBuff
{
    public PirateFlagBuff() : base(() => [NPCID.PirateCaptain], () => Main.invasionType == InvasionID.PirateInvasion && (Main.LocalPlayer.position.X > Main.invasionX * 16.0 - 3000 && Main.LocalPlayer.position.X < Main.invasionX * 16.0 + 3000 || Main.invasionProgressNearInvasion), 1f)
    {
    }
}