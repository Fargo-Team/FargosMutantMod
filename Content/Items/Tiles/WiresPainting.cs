using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Fargowiltas.Content.Items.Tiles;

public class WiresPainting : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 9999;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(0, 0, 10);
        Item.createTile = ModContent.TileType<WiresPaintingSheet>();
    }
}