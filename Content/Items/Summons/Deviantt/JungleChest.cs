using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class JungleChest : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<JungleChestBuff>();
        /*
        public override void AddRecipes()
        {
            if (ModContent.TryFind("Fargowiltas/Deviantt", out ModItem modItem))
            {
                CreateRecipe()
                  .AddIngredient(ItemID.SoulofLight, 7)
                  .AddIngredient(ItemID.SoulofNight, 7)
                  .AddIngredient(ItemID.GoldCoin, 30)
                  .AddIngredient(modItem.Type)
                  .AddTile(TileID.MythrilAnvil)
                  .Register();
            }
        }
        */
    }
    public class JungleChestBuff : BaseSpawnBoosterBuff
    {
        public JungleChestBuff() : base(() => [NPCID.BigMimicJungle], () => Main.LocalPlayer.ZoneJungle && (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight), 0.2f)
        {
        }
    }
}