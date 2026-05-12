using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class PinkSlimeCrown : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<PinkSlimeCrownBuff>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.sellPrice(0, 0, 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.Gel, 20)
                .AddIngredient(ItemID.PinkDye)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
    public class PinkSlimeCrownBuff : BaseSpawnBoosterBuff
    {
        public PinkSlimeCrownBuff() : base(() => [NPCID.Pinky], () => Main.LocalPlayer.ZoneForest || Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight, 0.2f)
        {
        }
    }
}