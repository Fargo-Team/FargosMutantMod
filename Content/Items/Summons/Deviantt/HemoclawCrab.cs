using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class HemoclawCrab : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<HemoclawCrabBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SeafoodDinner)
                .AddIngredient(ItemID.DeepRedPaint)
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class HemoclawCrabBuff : BaseSpawnBoosterBuff
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public HemoclawCrabBuff() : base(() => [NPCID.BloodEelHead, NPCID.GoblinShark], () => Main.bloodMoon && Main.LocalPlayer.ZoneBeach, 0.2f)
        {
        }
    }
}