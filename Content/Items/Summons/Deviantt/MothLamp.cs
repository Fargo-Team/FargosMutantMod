using Fargowiltas.Content.Buffs;
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
                .AddIngredient(ItemID.Torch, 3)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.Vine, 6)
                .AddIngredient(ItemID.SoulofFlight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class MothLampBuff : BaseSpawnBoosterBuff
    {
        public MothLampBuff() : base(() => [NPCID.Moth], () => Main.LocalPlayer.ZoneJungle && !Main.LocalPlayer.ZoneLihzhardTemple && !Main.LocalPlayer.ZoneCrimson && !Main.LocalPlayer.ZoneCorrupt && Main.hardMode && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight), 0.08f)
        {
        }
    }
}