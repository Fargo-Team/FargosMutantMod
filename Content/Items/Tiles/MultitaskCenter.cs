using Fargowiltas.Common.Systems.Recipes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles
{
    public class MultitaskCenter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
                .AddIngredient(ItemID.WorkBench)
                .AddIngredient(ItemID.HeavyWorkBench)
                .AddIngredient(ItemID.Furnace)
                .AddRecipeGroup(RecipeGroups.AnyAnvil)
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.Sawmill)
                .AddIngredient(ItemID.Loom)
                .AddRecipeGroup(RecipeGroups.AnyWoodenTable)
                .AddRecipeGroup(RecipeGroups.AnyWoodenChair)
                .AddRecipeGroup(RecipeGroups.AnyCookingPot)
                .AddRecipeGroup(RecipeGroups.AnyWoodenSink)
                .AddIngredient(ItemID.Keg)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .Register();
        }
    }

    public class MultitaskCenterSheet : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 4;
            Main.tileNoAttach[Type] = false;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(200, 200, 200), name);
            TileID.Sets.DisableSmartCursor[Type] = true;

            //counts as
            AdjTiles = 
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
            TileID.Sets.CountsAsWaterSource[Type] = true;
            AnimationFrameHeight = 54;
        }

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

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Tile[Type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Tile[Type].Value.Height / 4; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Main.tileFrame[Type]; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(tile.TileFrameX, tile.TileFrameY + y3, 16, 16);
            Vector2 origin2 = rectangle.Size() / 2f;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
