using Fargowiltas.Common.Configs;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace Fargowiltas;

public class InputManager : ModPlayer
{
    public int latestXDirPressed = 0;
    public int latestXDirReleased = 0;
    private bool LeftLastPressed = false;
    private bool RightLastPressed = false;
    int lastSetBonusTimer = 0;
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        int setbonusDir = Main.ReversedUpDownArmorSetBonuses ? 1 : 0;
        if (Fargowiltas.SetBonusKey.JustPressed)
        {
            Main.LocalPlayer.KeyDoubleTap(setbonusDir);
        }
        if (Fargowiltas.SetBonusKey.Current)
        {
            if (Main.LocalPlayer.holdDownCardinalTimer[setbonusDir] != lastSetBonusTimer + 1)//don't double dip when holding normal set bonus key
            {
                Main.LocalPlayer.holdDownCardinalTimer[setbonusDir]++;
            }
            Main.LocalPlayer.KeyHoldDown(setbonusDir, Main.LocalPlayer.holdDownCardinalTimer[setbonusDir]);
            lastSetBonusTimer = Main.LocalPlayer.holdDownCardinalTimer[setbonusDir];
        }
        else
        {
            if (FargoClientConfig.Instance.DoubleTapSetBonusDisabled)
                Main.LocalPlayer.doubleTapCardinalTimer[0] = Main.LocalPlayer.doubleTapCardinalTimer[1] = 0;
        }
        if (Main.LocalPlayer.controlLeft && !LeftLastPressed)
        {
            latestXDirPressed = -1;
        }
        if (Main.LocalPlayer.controlRight && !RightLastPressed)
        {
            latestXDirPressed = 1;
        }
        if (!Main.LocalPlayer.controlLeft && !Main.LocalPlayer.releaseLeft)
        {
            latestXDirReleased = -1;
        }
        if (!Main.LocalPlayer.controlRight && !Main.LocalPlayer.releaseRight)
        {
            latestXDirReleased = 1;
        }
        LeftLastPressed = Main.LocalPlayer.controlLeft;
        RightLastPressed = Main.LocalPlayer.controlRight;
    }
}
