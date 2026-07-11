using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class DilutedRainbowMatter : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<DilutedRainbowMatterBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Gel, 100)
                .AddIngredient(ItemID.RainbowMoss, 20) //this is the only consistently obtainable rainbow item premechs
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

    }
    public class DilutedRainbowMatterBuff : BaseSpawnBoosterBuff
    {
        public DilutedRainbowMatterBuff() : base(() => [NPCID.RainbowSlime], () => Main.LocalPlayer.ZoneHallow && Main.IsItRaining, 0.2f)
        {
        }
    }
}