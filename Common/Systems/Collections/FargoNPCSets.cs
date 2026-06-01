using ReLogic.Reflection;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Collections
{
    [ReinitializeDuringResizeArrays]
    public static class FargoNPCSets
    {
        public static SetFactory NPCFactory = new SetFactory(NPCLoader.NPCCount, "Fargowiltas/NPCID", Search);
        public static IdDictionary Search = IdDictionary.Create<NPCID, int>();
        public static int[] SwarmHealth = NPCFactory.CreateIntSet(0);
        public static bool[] ShouldGrantBossZen = NPCFactory.CreateBoolSet(false, NPCID.EaterofWorldsHead);
    }
}
