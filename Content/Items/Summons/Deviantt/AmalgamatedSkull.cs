using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class AmalgamatedSkull : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<AmalgamatedSkullBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 30)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class AmalgamatedSkullBuff : BaseSpawnBoosterBuff
    {
        public AmalgamatedSkullBuff() : base(() => [NPCID.SkeletonSniper, NPCID.TacticalSkeleton, NPCID.SkeletonCommando], () => Main.LocalPlayer.ZoneDungeon && NPC.downedBoss3 && NPC.downedPlantBoss && Main.hardMode, 0.2f)
        {
        }
    }
}