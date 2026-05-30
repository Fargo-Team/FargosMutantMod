using Fargowiltas.Common.Configs;
using Fargowiltas.Content.NPCs;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas
{
    internal abstract class ILEditUtils_Mutant : ModSystem
    {

    }
    internal sealed class Main_DoDraw_UpdateCameraPosition_ILEdit : ILEditUtils_Mutant
    {
        public override void OnModLoad() => IL_Main.DoDraw_UpdateCameraPosition += DoDraw_UpdateCameraPosition_IL;

        public static void DoDraw_UpdateCameraPosition_IL(ILContext context)
        {
            ILCursor cursor = new(context);
            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchStloc(17)))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchStloc(17)");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>("scope")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdfld<Player>('scope')");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => FargoClientConfig.Instance.DisableAllScopeView == ScopedBinocularViews.AllEnabled);
            cursor.EmitAnd();

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdcI4(Convert.ToInt32(0x4E6)))) // ItemID.SniperRifle : 1254
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdcI4(Convert.ToInt32(0x4E6))");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate((int targetValue) => FargoClientConfig.Instance.DisableAllScopeView is ScopedBinocularViews.AllDisabled or ScopedBinocularViews.SniperRifleScopeDisabled ? -1 : targetValue);

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>("scope")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdfld<Player>('scope') 2");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => FargoClientConfig.Instance.DisableAllScopeView is ScopedBinocularViews.AllEnabled or ScopedBinocularViews.SniperRifleScopeDisabled);
            cursor.EmitAnd();

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdsfld<Main>("mouseRight")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdsfld<Main>('mouseRight') ");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => FargoClientConfig.Instance.DisableAllScopeView == ScopedBinocularViews.AllEnabled);
            cursor.EmitAnd();

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdsfld<Main>("mouseRight")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdsfld<Main>('mouseRight') ");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => FargoClientConfig.Instance.DisableAllScopeView is ScopedBinocularViews.AllEnabled or ScopedBinocularViews.RifleScopeAccessoryDisabled);
            cursor.EmitAnd();

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdsfld<Main>("mouseRight")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdsfld<Main>('mouseRight') ");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => FargoClientConfig.Instance.DisableAllScopeView is ScopedBinocularViews.AllEnabled or ScopedBinocularViews.SniperRifleScopeDisabled);
            cursor.EmitAnd();
        }
    }
    /// <summary>
    /// Prevents Lunar Cultist from summoning Lunar Pillars when undesired.
    /// <br/> This is less sustainable but more optimized in favor of not having to work with garbage net packets and additional workload.
    /// </summary>
    internal sealed class NPC_DoDeathEvents_ILEdit : ModSystem
    {
        public override void OnModLoad() => IL_NPC.DoDeathEvents += DoDeathEvents_IL;
        public static void DoDeathEvents_IL(ILContext context)
        {
            ILCursor cursor = new(context);
            if (!cursor.TryGotoNext(MoveType.Before, i => i.MatchCall<WorldGen>("TriggerLunarApocalypse")))
            {
                Fargowiltas.Instance.Logger.Warn("Prevent Lunar Cultist lunar pillars event on death edit failure on MatchCall<WorldGen>(\"TriggerLunarApocalypse\")");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            ILLabel label = cursor.DefineLabel();

            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate((NPC npc) => !npc.TryGetGlobalNPC(out FargoGlobalNPC global) || global.PillarSpawn);
            cursor.Emit(OpCodes.Brfalse, label);

            cursor.GotoNext(MoveType.After, i => i.MatchCall<WorldGen>("TriggerLunarApocalypse"));
            cursor.MarkLabel(label);
        }
    }
}