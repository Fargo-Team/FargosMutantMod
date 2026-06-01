using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class HolyGrail : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<HolyGrailBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddRecipeGroup("Fargowiltas:AnyGoldBar", 4)
                .AddIngredient(ItemID.ManaPotion, 6)
                .AddIngredient(ItemID.Ruby)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
    public class HolyGrailBuff : BaseSpawnBoosterBuff
    {
        public HolyGrailBuff() : base(() => [NPCID.Tim], () => Main.LocalPlayer.ZoneRockLayerHeight && (double)Main.LocalPlayer.Center.Y / 16 > (Main.rockLayer + Main.maxTilesY) / 2.0, 0.1f)
        {
        }
    }
}