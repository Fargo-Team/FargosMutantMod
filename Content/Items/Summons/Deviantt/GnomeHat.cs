using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class GnomeHat : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<GnomeHatBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ClayBlock, 50)
                .AddRecipeGroup("Fargowiltas:AnyCopperBar", 5)
                .AddTile(TileID.LivingLoom)
                .Register();
        }
    }
    public class GnomeHatBuff : BaseSpawnBoosterBuff
    {
        public static int livingWoodTileCount; //living tree has no biome definition and these only spawn on living wood walls, close enough
        public GnomeHatBuff() : base(() => [NPCID.Gnome], () => Main.LocalPlayer.ZoneOverworldHeight && livingWoodTileCount >= 200, 0.4f)
        {
        }
    }
}