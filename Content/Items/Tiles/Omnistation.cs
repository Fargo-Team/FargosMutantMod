using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Tiles
{
    public abstract class BaseOmnistation : ModItem
    {
        protected int bar;

        public BaseOmnistation(int bar)
        {
            this.bar = bar;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 50);
            Item.createTile = ModContent.TileType<OmnistationSheet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Semistation>())
                .AddIngredient(ItemID.GardenGnome, 3)
                .AddIngredient(ItemID.CatBast, 3)
                .AddIngredient(ItemID.LadyBug, 3)
                .AddIngredient(bar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class Omnistation : BaseOmnistation
    {
        public Omnistation() : base(ItemID.AdamantiteBar) { }
    }

    public class Omnistation2 : BaseOmnistation
    {
        public Omnistation2() : base(ItemID.TitaniumBar) { }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.createTile = ModContent.TileType<OmnistationSheet2>();
        }
    }
}