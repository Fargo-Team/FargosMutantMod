using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives;

public class ObsidianWreckingBall : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 36;

        // placeholder
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = false;
        Item.noUseGraphic = true;

        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 10);

        // todo projectile
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Obsidian, 100)
            .AddIngredient(ItemID.HellstoneBar, 20)
            .AddIngredient(ItemID.Bone, 10)
            .AddIngredient(ItemID.Rope, 50)
            .AddTile(TileID.Anvils)
            .Register();
    }
}