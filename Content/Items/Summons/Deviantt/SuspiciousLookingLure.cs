using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class SuspiciousLookingLure : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<SuspiciousLookingLureBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.JourneymanBait)
                .AddIngredient(ItemID.DeepRedPaint)
                .AddIngredient(ItemID.Lens, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class SuspiciousLookingLureBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public SuspiciousLookingLureBuff() : base(() => [NPCID.EyeballFlyingFish, NPCID.ZombieMerman], () => Main.bloodMoon && Main.LocalPlayer.ZoneBeach, 0.2f)
        {
        }
    }
}