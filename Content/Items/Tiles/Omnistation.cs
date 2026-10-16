using Fargowiltas.Content.Items.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles;

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
            .AddIngredient<GizmoParts>(5)
            .AddIngredient(bar, 10)
            .AddIngredient(ModContent.ItemType<Semistation>())
            .AddIngredient(ItemID.GardenGnome, 3)
            .AddIngredient(ItemID.CatBast, 3)
            .AddIngredient(ItemID.LadyBug, 3)
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

public class OmnistationSheet : ModTile
{
    public virtual Color color => new Color(221, 85, 125);

    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
        TileObjectData.newTile.Height = 4;
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16];
        TileObjectData.addTile(Type);

        LocalizedText name = CreateMapEntryName();
        AddMapEntry(color, name);

        AnimationFrameHeight = 72;
    }

    public override void AnimateTile(ref int frame, ref int frameCounter)
    {
        if (++frameCounter >= 6)
        {
            frameCounter = 0;

            if (++frame >= 42)
                frame = 0;
        }
    }

    public override bool CanDrop(int i, int j) => false;

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        r = 1f;
        g = 1f;
        b = 1f;
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (!closer)
        {
            if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.Omnistation>(), 30);
        }
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];
        Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Tile[Type].Value;
        int num156 = Terraria.GameContent.TextureAssets.Tile[Type].Value.Height / 42; //ypos of lower right corner of sprite to draw
        int y3 = num156 * Main.tileFrame[Type]; //ypos of upper left corner of sprite to draw
        Rectangle rectangle = new(tile.TileFrameX, tile.TileFrameY + y3, 16, 16);
        Vector2 origin2 = rectangle.Size() / 2f;
        Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
        //Rectangle frame = new(tile.TileFrameX, tile.TileFrameY + Main.tileFrame[Type], texture2D13.Width, texture2D13.Height);
        if (Main.drawToScreen)
        {
            zero = Vector2.Zero;
        }
        Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
}

public class OmnistationSheet2 : OmnistationSheet
{
    public override Color color => new Color(102, 116, 130);

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];
        Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Tile[Type].Value;
        int num156 = Terraria.GameContent.TextureAssets.Tile[Type].Value.Height / 42; //ypos of lower right corner of sprite to draw
        int y3 = num156 * Main.tileFrame[Type]; //ypos of upper left corner of sprite to draw
        Rectangle rectangle = new(tile.TileFrameX, tile.TileFrameY + y3, 16, 16);
        Vector2 origin2 = rectangle.Size() / 2f;
        Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
        //Rectangle frame = new(tile.TileFrameX, tile.TileFrameY + Main.tileFrame[Type], texture2D13.Width, texture2D13.Height);
        if (Main.drawToScreen)
        {
            zero = Vector2.Zero;
        }
        Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
}