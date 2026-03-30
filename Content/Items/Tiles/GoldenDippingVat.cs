using Fargowiltas.Common.Systems.Recipes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles
{
    public class GoldenDippingVat : ModItem
    {
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

    public class GoldenDippingVatSheet : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(255, 215, 0), name);

            AnimationFrameHeight = 54;
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 10) //replace with duration of frame in ticks
            {
                frameCounter = 0;
                frame++;
                frame %= 10;
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            int num156 = TextureAssets.Tile[Type].Value.Height / 10; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Main.tileFrame[Type]; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(tile.TileFrameX, tile.TileFrameY + y3, 16, 16);
            Color color = Lighting.GetColor(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            Main.spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle?(rectangle), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            Color highlightColor;
            if (TileID.Sets.HasOutlines[Type] && Main.InSmartCursorHighlightArea(i, j, out var actuallySelected) && !true)
            {
                int avgBrightness = (color.R + color.G + color.B) / 3;
                if (avgBrightness > 10)
                {
                    highlightColor = Colors.GetSelectionGlowColor(actuallySelected, avgBrightness);
                    Main.spriteBatch.Draw(TextureAssets.HighlightMask[Type].Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle?(rectangle), highlightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}