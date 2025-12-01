using Fargowiltas.Common.Configs;
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
    internal sealed class DoDraw_UpdateCameraPosition_ILEdit : ILEditUtils_Mutant
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
            cursor.EmitDelegate(() => !FargoClientConfig.Instance.DisableAllScopeView);
            cursor.EmitAnd();

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdcI4(Convert.ToInt32(0x4E6)))) // ItemID.SniperRifle : 1254
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdcI4(Convert.ToInt32(0x4E6))");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate((int targetValue) => !FargoClientConfig.Instance.DisableSniperRifleView ? -1 : targetValue);

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>("scope")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdfld<Player>('scope') 2");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => !FargoClientConfig.Instance.DisableRifleScopeAccessoryView);
            cursor.EmitAnd();

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdsfld<Main>("mouseRight")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdsfld<Main>('mouseRight') ");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => !FargoClientConfig.Instance.DisableAllScopeView);
            cursor.EmitAnd();

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdsfld<Main>("mouseRight")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdsfld<Main>('mouseRight') ");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => !FargoClientConfig.Instance.DisableSniperRifleView);
            cursor.EmitAnd();

            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchLdsfld<Main>("mouseRight")))
            {
                Fargowiltas.Instance.Logger.Warn("Rifle scope view edit failure on MatchLdsfld<Main>('mouseRight') ");
                MonoModHooks.DumpIL(ModContent.GetInstance<Fargowiltas>(), context);
                return;
            }
            cursor.EmitDelegate(() => !FargoClientConfig.Instance.DisableRifleScopeAccessoryView);
            cursor.EmitAnd();
        }
    }
}