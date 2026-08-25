using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles;

public class DemonAltarSheet : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        Main.tileNoAttach[Type] = true;
        TileObjectData.newTile.CoordinateHeights = [16, 16];
        TileObjectData.addTile(Type);
        LocalizedText name = CreateMapEntryName();
        // name.SetDefault("Demon Altar");
        AddMapEntry(new Color(200, 200, 200), name);
        TileID.Sets.DisableSmartCursor[Type] = true;
        //counts as
        AdjTiles = [TileID.DemonAltar];
    }

    //public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    //{
    //    r = 0.93f;
    //    g = 0.11f;
    //    b = 0.9f;
    //}

    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = fail ? 1 : 3;
    }
    /*
    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<DemonAltar>());
    }
    */
}

public class CrimsonAltarSheet : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        Main.tileNoAttach[Type] = true;
        TileObjectData.newTile.CoordinateHeights = [16, 16];
        TileObjectData.addTile(Type);
        LocalizedText name = CreateMapEntryName();
        // name.SetDefault("Crimson Altar");
        AddMapEntry(new Color(200, 200, 200), name);
        TileID.Sets.DisableSmartCursor[Type] = true;
        //counts as
        AdjTiles = [TileID.DemonAltar];
    }

    //public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    //{
    //    r = 0.93f;
    //    g = 0.11f;
    //    b = 0.9f;
    //}

    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = fail ? 1 : 3;
    }
    /*
    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<CrimsonAltar>());
    }
    */
}