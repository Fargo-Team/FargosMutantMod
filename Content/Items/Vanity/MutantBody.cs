using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Vanity;

[AutoloadEquip(EquipType.Body)]
public class MutantBody : ModItem
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
            .AddIngredient(ItemID.SkeletronMask)
            .AddIngredient(ItemID.DestroyerMask)
            .AddIngredient(ItemID.SkeletronPrimeMask)
            .AddIngredient(ItemID.TwinMask)
            .AddIngredient(ItemID.GolemMask)
            .AddIngredient(ItemID.DukeFishronMask)
            .AddIngredient(ItemID.FairyQueenMask)
            .AddIngredient(ItemID.BossMaskMoonlord)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}
