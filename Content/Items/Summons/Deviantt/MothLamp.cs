using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class MothLamp : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<MothLampBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.Vine, 10)
                .AddIngredient(ItemID.Stinger, 10)
                .AddIngredient(ItemID.SoulofFlight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class MothLampBuff : BaseSpawnBoosterBuff
    {
        public MothLampBuff() : base(() => [NPCID.Moth], () => Main.LocalPlayer.ZoneJungle && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight), 0.2f)
        {
        }
    }
}