using Fargowiltas.Common.Systems.Recipes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles
{
    public class CrucibleCosmos : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.Mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.OverrideColor = new Color?(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
                }
            }
        }

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
            Item.value = Item.buyPrice(2);
            Item.createTile = ModContent.TileType<CrucibleCosmosSheet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MultitaskCenter>())
                .AddIngredient(ModContent.ItemType<ElementalAssembler>())
                .AddIngredient(ModContent.ItemType<LuminiteOmniforge>())
				.AddIngredient(ModContent.ItemType<GoldenDippingVat>())
                .AddRecipeGroup(RecipeGroups.AnyBookcase)
                .AddIngredient(ItemID.BlendOMatic)
                .AddIngredient(ItemID.MeatGrinder)
                .AddIngredient(ItemID.SteampunkBoiler)
                .AddRecipeGroup(RecipeGroups.AnyDecayChamber)
                .AddIngredient(ItemID.LihzahrdFurnace)
                .Register();

            if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage))
            {
                CreateRecipe()
                    .AddIngredient(magicStorage.Find<ModItem>("CombinedStations4Item").Type)
                    .AddIngredient(ItemID.LunarBar, 25)
                    .Register();
            }
        }
    }

    public class CrucibleCosmosSheet : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 4;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(200, 200, 200), name);
            TileID.Sets.DisableSmartCursor[Type] = true;

            #region Counts as
            AdjTiles =
                [TileID.WorkBenches,
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
                TileID.Kegs,
                TileID.Hellforge,
                TileID.AlchemyTable,
                TileID.TinkerersWorkbench,
                TileID.ImbuingStation,
                TileID.DyeVat,
                TileID.LivingLoom,
                TileID.GlassKiln,
                TileID.IceMachine,
                TileID.HoneyDispenser,
                TileID.SkyMill,
                TileID.Solidifier,
                TileID.BoneWelder,
                TileID.MythrilAnvil,
                TileID.AdamantiteForge,
                TileID.DemonAltar,
                TileID.Bookcases,
                TileID.CrystalBall,
                TileID.Autohammer,
                TileID.LunarCraftingStation,
                TileID.LesionStation,
                TileID.FleshCloningVat,
                TileID.LihzahrdFurnace,
                TileID.SteampunkBoiler,
                TileID.Blendomatic,
                TileID.MeatGrinder,
                TileID.Tombstones,
                ModContent.TileType<GoldenDippingVatSheet>(),
                ModContent.TileType<LuminiteOmniforgeTile>()
                ];

            TileID.Sets.CountsAsHoneySource[Type] = true;
            TileID.Sets.CountsAsLavaSource[Type] = true;
            TileID.Sets.CountsAsWaterSource[Type] = true;
            #endregion

            AnimationFrameHeight = 54;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            b = 1.2f;
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
                frame %= 16;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.LocalPlayer.Distance(new Vector2(i * 16 + 8, j * 16 + 8)) < 16 * 5)
            {
                Main.LocalPlayer.GetModPlayer<FargoPlayer>().ElementalAssemblerNearby = 6;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Tile[Type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Tile[Type].Value.Height / 16; //ypos of lower right corner of sprite to draw
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