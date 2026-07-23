using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class SlimyLockBox : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<SlimyLockBoxBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                  .AddIngredient(ItemID.ChestLock)
                  .AddIngredient(ItemID.Gel, 30)
                  .AddTile(TileID.Solidifier)
                  .Register();
        }
    }
    public class SlimyLockBoxBuff : BaseSpawnBoosterBuff
    {
        public SlimyLockBoxBuff() : base(() => [NPCID.DungeonSlime], () => Main.LocalPlayer.ZoneDungeon && NPC.downedBoss3, 0.33f)
        {
        }
    }
}