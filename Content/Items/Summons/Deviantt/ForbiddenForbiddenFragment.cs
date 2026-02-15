using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class ForbiddenForbiddenFragment : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<ForbiddenForbiddenFragmentBuff>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 5));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

        }
    }
    public class ForbiddenForbiddenFragmentBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public ForbiddenForbiddenFragmentBuff() : base(() => [NPCID.SandElemental], () => Main.LocalPlayer.ZoneSandstorm, 0.2f)
        {
        }
    }
}