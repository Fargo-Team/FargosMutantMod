using Fargowiltas.Content.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt;

public class GoblinScrap : BaseSpawnBooster
{
    public override int BuffType => ModContent.BuffType<GoblinScrapBuff>();

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Hay, 20) //do not fucking askkkkkkkk
            .AddIngredient(ItemID.ClayBlock, 20)
            .AddTile(TileID.Furnaces)
            .Register();
    }
}
public class GoblinScrapBuff : BaseSpawnBoosterBuff
{
    public GoblinScrapBuff() : base(() => [NPCID.GoblinScout], () => Main.LocalPlayer.ZoneOverworldHeight && !Main.LocalPlayer.ZoneGraveyard && ((Math.Abs(Main.LocalPlayer.Center.X / 16f - Main.spawnTileX) > Main.maxTilesX / 3) || Main.remixWorld), 0.2f)
    {
    }
}