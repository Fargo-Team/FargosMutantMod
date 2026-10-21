using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.NPCs.DevianttNPC.Vanity;

[AutoloadEquip(EquipType.Body)]
[LegacyName("DevianttBody")]
public class Bloodress : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.vanity = true;
        Item.rare = ItemRarityID.Blue;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 7)
            .AddIngredient(ItemID.ChumBucket, 3)
            .AddTile(TileID.Loom)
            .Register();
    }
}
