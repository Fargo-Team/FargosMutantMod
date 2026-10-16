using ReLogic.Reflection;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Collections;

[ReinitializeDuringResizeArrays]
public static class FargoBuffSets
{
    public static SetFactory BuffFactory = new SetFactory(BuffLoader.BuffCount, "Fargowiltas/BuffID", Search);
    public static IdDictionary Search = IdDictionary.Create<BuffID, int>();
    public static bool[] BuffDisplayBlacklist = BuffFactory.CreateBoolSet(false,
        BuffID.Campfire,
        BuffID.HeartLamp,
        BuffID.Sunflower,
        BuffID.PeaceCandle,
        BuffID.StarInBottle,
        BuffID.Tipsy,
        BuffID.MonsterBanner,
        BuffID.Werewolf,
        BuffID.Merfolk,
        BuffID.CatBast,
        BuffID.BrainOfConfusionBuff,
        BuffID.NeutralHunger,
        BuffID.WaterCandle,
        BuffID.ShadowCandle,
        BuffID.WindPushed,
        BuffID.Shimmer,
        BuffID.NoBuilding,
        BuffID.Horrified
        );
}
