using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class DevianttBody : ModItem
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
                .AddIngredient(ItemID.Robe)
                .AddIngredient(ItemID.PinkGel)
                .AddIngredient(ItemID.AncientBattleArmorMaterial)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
