using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt;

public class ShadowflameIdol : BaseSpawnBooster
{
    public override int BuffType => ModContent.BuffType<ShadowflameIdolBuff>();

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 5));
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;

    }
}

public class ShadowflameIdolBuff : BaseSpawnBoosterBuff
{
    public ShadowflameIdolBuff() : base(() => [NPCID.GoblinSummoner], () => Main.invasionType == InvasionID.GoblinArmy && Main.hardMode && (Main.LocalPlayer.position.X > Main.invasionX * 16.0 - 3000 && Main.LocalPlayer.position.X < Main.invasionX * 16.0 + 3000), 1f)
    {
    }
}