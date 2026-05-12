using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
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

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.IceBlock, 100)
                .AddIngredient(ItemID.FlinxFur, 6)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CoreoftheFrostCoreBuff : BaseSpawnBoosterBuff
    {
        public CoreoftheFrostCoreBuff() : base(() => [NPCID.IceGolem], () => Main.LocalPlayer.ZoneSnow && Main.IsItStorming, 0.2f)
        {
        }
    }
}