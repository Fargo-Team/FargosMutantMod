using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
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
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.ClayBlock, 50)
                .AddIngredient(RecipeGroupID.IronBar, 10)
                .AddIngredient(ItemID.Ruby, 1)
                .AddTile(TileID.LivingLoom)
                .Register();
        }
    }
    public class GnomeHatBuff : BaseSpawnBoosterBuff
    {
        public GnomeHatBuff() : base(() => [NPCID.Gnome], () => Main.LocalPlayer.ZoneOverworldHeight && Main.LocalPlayer.ZonePurity && !Main.IsItDay(), 0.2f)
        {
        }
    }
}