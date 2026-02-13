using Fargowiltas.Content.Buffs.SpawnBoosters;
using Fargowiltas.Content.Items.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class GoldenSlimeCrown : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<GoldenSlimeCrownBuff>();
   

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PinkSlimeCrown>())
                .AddIngredient(ItemID.GoldDust, 100)
                .AddTile(ModContent.TileType<GoldenDippingVatSheet>())
                .DisableDecraft()
                .Register();
        }
    }

    public class GoldenSlimeCrownBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public GoldenSlimeCrownBuff() : base(() => [NPCID.GoldenSlime], () => Main.LocalPlayer.ZoneForest || Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight, 0.05f)
        {
        }
    }
}