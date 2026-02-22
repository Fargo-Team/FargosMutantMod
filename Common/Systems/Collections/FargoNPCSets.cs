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
    public static class FargoNPCSets
    {
        public static SetFactory NPCFactory = new SetFactory(NPCLoader.NPCCount, "Fargowiltas/NPCID", Search);
        public static IdDictionary Search = IdDictionary.Create<NPCID, int>();
        public static int[] SwarmHealth = NPCFactory.CreateIntSet(0);
    }
}
