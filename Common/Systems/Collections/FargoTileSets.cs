using ReLogic.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Collections;

[ReinitializeDuringResizeArrays]
public static class FargoTileSets
{
    public static SetFactory TileFactory = new SetFactory(TileLoader.TileCount, "Fargowiltas/TileID", Search);
    public static IdDictionary Search = IdDictionary.Create<TileID, int>();

    public static bool[] InstaCannotDestroy = TileFactory.CreateBoolSet(false);

    public static bool[] DungeonTile = TileFactory.CreateBoolSet(false,
        TileID.BlueDungeonBrick,
        TileID.GreenDungeonBrick,
        TileID.PinkDungeonBrick);

    public static bool[] HardmodeOre = TileFactory.CreateBoolSet(false,
        TileID.Cobalt,
        TileID.Palladium,
        TileID.Mythril,
        TileID.Orichalcum,
        TileID.Adamantite,
        TileID.Titanium);

    public static bool[] EvilAltars = TileFactory.CreateBoolSet(false,
        TileID.DemonAltar);

    public static int[] BuffStationTileToItem = TileFactory.CreateIntSet(-1,
        TileID.SharpeningStation, ItemID.SharpeningStation,
        TileID.AmmoBox, ItemID.AmmoBox,
        TileID.CrystalBall, ItemID.CrystalBall,
        TileID.BewitchingTable, ItemID.BewitchingTable,
        TileID.WarTable, ItemID.WarTable);
}
public class FargoTileToItemSetSystem : ModSystem
{
    /// <summary>
    /// Populates <see cref="FargoTileSets.BuffStationTileToItem"/> with any new entries from <see cref="FargoItemSets.BuffStation"/>.
    /// </summary>
    public override void PostSetupContent()
    {
        // Return early if BuffStation and BuffStationTileToItem are synced already
        int[] buffStation = FargoItemSets.BuffStation;
        int[] bSTileToItem = FargoTileSets.BuffStationTileToItem;
        if (buffStation.Length - buffStation.Count(-1) == bSTileToItem.Length - bSTileToItem.Count(-1))
            return;
        int[] ItemToTile = FargoItemSets.ItemFactory.CreateIntSet(-1);
        for (int i = 0; i < bSTileToItem.Length; i++)
        {
            if (bSTileToItem[i] != -1)
            {
                ItemToTile.SetValue(i, bSTileToItem[i]);
            }
        }
        foreach (Item item in ContentSamples.ItemsByType.Values)
        {
            if (ItemToTile[item.type] == -1 && buffStation[item.type] != -1 && item.createTile != -1)
            {
                FargoTileSets.BuffStationTileToItem[item.createTile] = item.type;
            }
        }
    }
}
