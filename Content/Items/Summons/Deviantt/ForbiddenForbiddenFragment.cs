using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
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

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sandstone, 100)
                .AddIngredient(ItemID.AntlionMandible, 6)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class ForbiddenForbiddenFragmentBuff : BaseSpawnBoosterBuff
    {
        public ForbiddenForbiddenFragmentBuff() : base(() => [NPCID.SandElemental], () => Main.LocalPlayer.ZoneSandstorm, 0.2f)
        {
        }
    }
}