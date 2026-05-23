using Fargowiltas.Common.Configs;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Buffs
{
    public class FargoGlobalBuff : GlobalBuff
    {
        public override void Load()
        {
            On_Main.DrawInterface_Resources_Buffs += InterfaceResourcesCheck;
        }
        public override void Unload()
        {
            On_Main.DrawInterface_Resources_Buffs -= InterfaceResourcesCheck;
        }
        public static void InterfaceResourcesCheck(On_Main.orig_DrawInterface_Resources_Buffs orig, Main self)
        {
            // Store actual current buff types and times in temporary arrays, to be restored after draw call.
            // Doing the same operations on buff time is needed for proper buff display.
            int[] buffTypes = (int[])Main.LocalPlayer.buffType.Clone();
            int[] buffTimes = ((int[])Main.LocalPlayer.buffTime.Clone());
            // Remove all hidden buffs. They'll be readded at the end of the method.
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                if (FargoClientConfig.Instance.HideUnlimitedBuffs && Main.LocalPlayer.buffType[i] > 0 && BuffCanBeHidden(Main.LocalPlayer, i))
                {
                    Main.LocalPlayer.buffType[i] = 0;
                    Main.LocalPlayer.buffTime[i] = 0;
                }
            }
            // Reshuffle array order to have non-hidden buffs first.
            int[] sortedTimes = Main.LocalPlayer.buffTime.Where(x => x != 0).ToArray();
            Array.Resize(ref sortedTimes, Main.LocalPlayer.buffTime.Length);
            Main.LocalPlayer.buffTime = (int[])sortedTimes.Clone();

            int[] sortedTypes = Main.LocalPlayer.buffType.Where(x => x != 0).ToArray();
            Array.Resize(ref sortedTypes, Main.LocalPlayer.buffType.Length);
            Main.LocalPlayer.buffType = (int[])sortedTypes.Clone();

            orig(self);
            // Store types that were removed from the array during the orig call. These were manually removed (usually by being right clicked, usually just one, but this covers all possible cases).
            var removedTypes = sortedTypes.Except(Main.LocalPlayer.buffType);

            // Restore the arrays.
            Main.LocalPlayer.buffType = buffTypes;
            Main.LocalPlayer.buffTime = buffTimes;
            // Remove manually-removed buffs.
            foreach (var type in removedTypes)
                Main.LocalPlayer.ClearBuff(type);
        }
        static bool BuffCanBeHidden(Player player, int buffIndex)
        {
            int buffTime = player.buffTime[buffIndex];
            int buffType = player.buffType[buffIndex];
            return buffTime <= 2
                && (!Main.debuff[buffType] || buffType == BuffID.Tipsy)
                && !Main.buffNoTimeDisplay[buffType]
                && !BuffID.Sets.TimeLeftDoesNotDecrease[buffType];
        }

        public override void Update(int type, Player player, ref int buffIndex)
        {
            
        }
    }
}