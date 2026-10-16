using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Ammo;

public abstract class CoinBag : ModItem
{
    public abstract int AmmunitionItem { get; }

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(AmmunitionItem);
        Item.width = 26;
        Item.height = 26;
        Item.consumable = false;
        Item.maxStack = 1;
        Item.value *= 100; // plat value is 5M, limit is 2.147B, cant do 1000 etc or else overflow
        Item.rare += 1;
        Item.notAmmo = false;
        Item.useStyle = ItemUseStyleID.None;
        Item.useTime = 0;
        Item.useAnimation = 0;
        Item.createTile = -1;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(AmmunitionItem, 3996)
            .AddTile(TileID.CrystalBall)
            .Register();
    }
}

public class CopperCoinBag : CoinBag
{
    public override int AmmunitionItem => ItemID.CopperCoin;
}

public class SilverCoinBag : CoinBag
{
    public override int AmmunitionItem => ItemID.SilverCoin;
}

public class GoldCoinBag : CoinBag
{
    public override int AmmunitionItem => ItemID.GoldCoin;
}

public class PlatinumCoinBag : CoinBag
{
    public override int AmmunitionItem => ItemID.PlatinumCoin;
}
