using Fargowiltas.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Tiles;

// TODO: Delete these items and tiles when 1.4.5 TML is real.
public class DemonAltar : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 14;
        Item.rare = ItemRarityID.Green;
        Item.maxStack = 9999;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;
        Item.createTile = ModContent.TileType<DemonAltarSheet>();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<GizmoParts>(3)
            .AddIngredient(ItemID.DemoniteBar, 10)
            .AddIngredient(ItemID.ShadowScale, 5)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

public class CrimsonAltar : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 14;
        Item.rare = ItemRarityID.Green;
        Item.maxStack = 9999;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;
        Item.createTile = ModContent.TileType<CrimsonAltarSheet>();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<GizmoParts>(3)
            .AddIngredient(ItemID.CrimtaneBar, 10)
            .AddIngredient(ItemID.TissueSample, 5)
            .AddTile(TileID.Anvils)
            .Register();
    }
}