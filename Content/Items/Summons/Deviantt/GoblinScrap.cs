using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class GoblinScrap : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<GoblinScrapBuff>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.Silk, 20)
                .AddIngredient(RecipeGroupID.IronBar, 10)
                .AddIngredient(ItemID.Diamond, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class GoblinScrapBuff : BaseSpawnBoosterBuff
    {
        public GoblinScrapBuff() : base(() => [NPCID.GoblinScout], () => Main.LocalPlayer.ZonePurity && Main.LocalPlayer.ZoneOverworldHeight && ((Main.LocalPlayer.Center.X / 16f - Main.spawnTileX) > Main.maxTilesX / 3), 0.2f) // condition is close enough
        {
        }
    }
}