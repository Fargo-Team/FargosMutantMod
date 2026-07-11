using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class AthenianIdol : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<AthenianIdolBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Fargowiltas:AnyGoldBar", 10)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class AthenianIdolBuff : BaseSpawnBoosterBuff
    {
        public AthenianIdolBuff() : base(() => [NPCID.Medusa], () => Main.LocalPlayer.ZoneMarble, 0.2f)
        {
        }
    }
}