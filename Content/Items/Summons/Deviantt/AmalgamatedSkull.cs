using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
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
                .AddIngredient<GizmoParts>(2)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class AmalgamatedSkullBuff : BaseSpawnBoosterBuff
    {
        public AmalgamatedSkullBuff() : base(() => [NPCID.SkeletonSniper, NPCID.TacticalSkeleton, NPCID.SkeletonCommando], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}