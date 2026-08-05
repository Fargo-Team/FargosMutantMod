using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Items.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles
{
    public class MultitaskCenter : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.consumable = true;
            Item.value = Item.buyPrice(gold: 30);
            Item.createTile = ModContent.TileType<MultitaskCenterSheet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(5)
                .AddRecipeGroup(RecipeGroups.IronBar, 5)
                .AddIngredient(ItemID.WorkBench)
                .AddIngredient(ItemID.HeavyWorkBench)
                .AddIngredient(ItemID.Furnace)
                .AddRecipeGroup(FargoRecipeGroups.AnyAnvil)
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.Sawmill)
                .AddIngredient(ItemID.Loom)
                .AddRecipeGroup(FargoRecipeGroups.AnyCookingPot)
                .AddRecipeGroup(FargoRecipeGroups.AnyWoodenSink)
                .AddIngredient(ItemID.Keg)
                .Register();
        }
    }

    public class MultitaskCenterSheet : ModTile
    {
        private Asset<Texture2D> glowTexture;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = false;

            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(200, 200, 200), name);

            //counts as
            int[] countsAs = 
                [
                TileID.WorkBenches,
                TileID.HeavyWorkBench,
                TileID.Furnaces,
                TileID.Anvils,
                TileID.Bottles,
                TileID.Sawmill,
                TileID.Loom,
                TileID.Tables,
                TileID.Chairs,
                TileID.CookingPots,
                TileID.Sinks,
                TileID.Kegs
                ];
            TileID.Sets.CountsAsWaterForCrafting[Type] = true;

            foreach (int item in countsAs)
            {
                Recipe.AddTileCountsAs(Type, item);
            }
            AnimationFrameHeight = 54;

            glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;


        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.8f;
            g = 0.4f;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 10) //replace with duration of frame in ticks
            {
                frameCounter = 0;
                frame++;
                frame %= 4;
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            int num156 = TextureAssets.Tile[Type].Value.Height / 4; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Main.tileFrame[Type]; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(tile.TileFrameX, tile.TileFrameY + y3, 16, 16);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Color color = Lighting.GetColor(i, j);

            Main.spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle?(rectangle), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glowTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

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
