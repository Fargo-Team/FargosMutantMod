using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Items.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles
{
    public class LuminiteOmniforge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.rare = ItemRarityID.Red;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<LuminiteOmniforgeTile>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Mod souls = Fargowiltas.SoulsMod;
            if (souls != null)
            {
                tooltips.Insert(4, new TooltipLine(Mod, "SoulsCrafts", Language.GetTextValue("Mods.Fargowiltas.Items.LuminiteOmniforge.SoulsTooltip")));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(5)
                .AddIngredient(ItemID.LunarBar, 25)
                .AddRecipeGroup(RecipeGroups.AnyForge)
                .AddRecipeGroup(RecipeGroups.AnyHMAnvil)
                .AddIngredient(ItemID.CrystalBall)
                .AddIngredient(ItemID.Autohammer)
                .AddIngredient(ItemID.LunarCraftingStation)
                .Register();
        }
    }

    public class LuminiteOmniforgeTile : ModTile
    {
        private Asset<Texture2D> glowTexture;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new(200, 200, 200), name);

            #region Counts as
            AdjTiles =
                [
                TileID.Furnaces,
                TileID.Anvils,
                TileID.Hellforge,
                TileID.MythrilAnvil,
                TileID.AdamantiteForge,
                TileID.CrystalBall,
                TileID.Autohammer,
                TileID.LunarCraftingStation
                ];
            #endregion

            AnimationFrameHeight = 54;

            glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            b = 0.8f;
            g = 0.5f;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 5) //replace with duration of frame in ticks
            {
                frameCounter = 0;
                frame++;
                frame %= 20;
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            int num156 = TextureAssets.Tile[Type].Value.Height / 20; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Main.tileFrame[Type]; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new(tile.TileFrameX, tile.TileFrameY + y3, 16, 16);
            Color color = Lighting.GetColor(i, j);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

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
