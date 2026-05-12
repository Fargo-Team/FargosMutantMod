using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class LeesHeadband : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<LeesHeadbandBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddIngredient(ItemID.SoulofSight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class LeesHeadbandBuff : BaseSpawnBoosterBuff
    {
        public LeesHeadbandBuff() : base(() => [NPCID.BoneLee], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}