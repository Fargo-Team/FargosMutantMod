using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class CoreoftheFrostCore : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<CoreoftheFrostCoreBuff>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

		}
    }
    public class CoreoftheFrostCoreBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public CoreoftheFrostCoreBuff() : base(() => [NPCID.IceGolem], () => Main.LocalPlayer.ZoneSnow && Main.IsItStorming, 0.2f)
        {
        }
    }
}