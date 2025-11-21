using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Common.Systems.Recipes;
using Terraria.GameContent;

namespace Fargowiltas.Content.Items.Tiles
{
    public class GoldenDippingVat : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Golden Dipping Vat");
            // Tooltip.SetDefault("Used to craft Gold Critters");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 10);
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<GoldenDippingVatSheet>();
        }

        public override void AddRecipes()
        {
            AddCritter(ItemID.Bird, ItemID.GoldBird);
            AddCritter(ItemID.Bunny, ItemID.GoldBunny);
            AddCritter(ItemID.Frog, ItemID.GoldFrog);
            AddCritter(ItemID.Goldfish, ItemID.GoldGoldfish);
            AddCritter(ItemID.Grasshopper, ItemID.GoldGrasshopper);
            AddCritter(ItemID.LadyBug, ItemID.GoldLadyBug);
            AddCritter(ItemID.Mouse, ItemID.GoldMouse);
            AddCritter(ItemID.Seahorse, ItemID.GoldSeahorse);
            AddCritter(ItemID.WaterStrider, ItemID.GoldWaterStrider);
            AddCritter(ItemID.Worm, ItemID.GoldWorm);

            AddCritterFromGroup(RecipeGroupID.Squirrels, ItemID.SquirrelGold);
            AddCritterFromGroup(RecipeGroups.AnyButterfly, ItemID.GoldButterfly);
            AddCritterFromGroup(RecipeGroups.AnyCommonFish, ItemID.GoldenCarp);
            AddCritterFromGroup(RecipeGroups.AnyDragonfly, ItemID.GoldDragonfly);
        }

        private static void AddCritter(int critterID, int goldCritterID)
        {
            Recipe.Create(goldCritterID)
                .AddIngredient(critterID)
                .AddIngredient(ItemID.GoldDust, 100)
                .AddTile(ModContent.TileType<GoldenDippingVatSheet>())
                .DisableDecraft()
                .Register();
        }

        private static void AddCritterFromGroup(int critterGroup, int goldCritterID)
        {
            Recipe.Create(goldCritterID)
                .AddRecipeGroup(critterGroup)
                .AddIngredient(ItemID.GoldDust, 100)
                .AddTile(ModContent.TileType<GoldenDippingVatSheet>())
                .DisableDecraft()
                .Register();
        }
    }
}