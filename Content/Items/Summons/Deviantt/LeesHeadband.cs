using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt;

public class LeesHeadband : BaseSpawnBooster
{
    public override int BuffType => ModContent.BuffType<LeesHeadbandBuff>();

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Blindfold)
            .AddIngredient(ItemID.Ectoplasm, 5)
            .AddIngredient(ItemID.SoulofSight, 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
public class LeesHeadbandBuff : BaseSpawnBoosterBuff
{
    public LeesHeadbandBuff() : base(() => [NPCID.BoneLee], () => Main.LocalPlayer.ZoneDungeon && NPC.downedBoss3 && NPC.downedPlantBoss && Main.hardMode, 0.4f)
    {
    }
}