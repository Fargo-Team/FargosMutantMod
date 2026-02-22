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
    public static class FargoWallSets
    {
        public static SetFactory WallFactory = new SetFactory(WallLoader.WallCount, "Fargowiltas/WallID", Search);
        public static IdDictionary Search = IdDictionary.Create<WallID, int>();
        public static bool[] InstaCannotDestroy = WallFactory.CreateBoolSet(false);
        public static bool[] DungeonWall = WallFactory.CreateBoolSet(false,
            WallID.BlueDungeonSlabUnsafe,
            WallID.BlueDungeonTileUnsafe,
            WallID.BlueDungeonUnsafe,
            WallID.GreenDungeonSlabUnsafe,
            WallID.GreenDungeonTileUnsafe,
            WallID.GreenDungeonUnsafe,
            WallID.PinkDungeonSlabUnsafe,
            WallID.PinkDungeonTileUnsafe,
            WallID.PinkDungeonUnsafe);
    }
}
