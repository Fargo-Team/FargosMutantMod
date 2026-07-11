using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class GrandCross : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<GrandCrossBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddIngredient(ItemID.SoulofMight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class GrandCrossBuff : BaseSpawnBoosterBuff
    {
        public GrandCrossBuff() : base(() => [NPCID.Paladin], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}