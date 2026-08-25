using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt;

public class ShadowflameIcon : BaseSpawnBooster
{
    public override int BuffType => ModContent.BuffType<ShadowflameIconBuff>();
}
public class ShadowflameIconBuff : BaseSpawnBoosterBuff
{
    public ShadowflameIconBuff() : base(() => [NPCID.GoblinSummoner], () => Main.invasionType == InvasionID.GoblinArmy && Main.hardMode && (Main.LocalPlayer.position.X > Main.invasionX * 16.0 - 3000 && Main.LocalPlayer.position.X < Main.invasionX * 16.0 + 3000), 1f)
    {
    }
}