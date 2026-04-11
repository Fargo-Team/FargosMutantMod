using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class AttractiveOre : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<AttractiveOreBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                /*.AddIngredient(ItemID.DeadMansChest)
                .AddIngredient(ItemID.MiningHelmet)
                .AddIngredient(ItemID.SpelunkerPotion)
                .AddTile(TileID.HeavyWorkBench)*/
                .AddRecipeGroup("Fargowiltas:AnyGoldOre", 8)
                .AddIngredient(ItemID.SpelunkerPotion)
                .AddIngredient(ItemID.SilverDye)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
    public class AttractiveOreBuff : BaseSpawnBoosterBuff
    {
        public AttractiveOreBuff() : base(() => [NPCID.UndeadMiner], () => Main.LocalPlayer.ZoneRockLayerHeight, 0.2f)
        {
        }
    }
}