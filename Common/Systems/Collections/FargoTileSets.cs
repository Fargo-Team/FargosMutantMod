using ReLogic.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Collections
{
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
    }
}
