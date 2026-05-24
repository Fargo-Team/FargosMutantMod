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
            if (!FargoClientConfig.Instance.HideUnlimitedBuffs)
            {
                orig(self);
                return;
            }
            Player player = Main.LocalPlayer;
            // Store actual current buff types and times in temporary arrays, to be restored after draw call.
            // Doing the same operations on buff time is needed for proper buff display.
            int[] buffTypes = (int[])player.buffType.Clone();
            int[] buffTimes = ((int[])player.buffTime.Clone());
            // Remove all hidden buffs. They'll be readded at the end of the method.
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                if (player.buffType[i] > 0 && BuffCanBeHidden(player, i))
                {
                    player.buffType[i] = 0;
                    player.buffTime[i] = 0;
                }
            }
            // Reshuffle array order to have non-hidden buffs first.
            int[] sortedTimes = player.buffTime.Where(x => x != 0).ToArray();
            Array.Resize(ref sortedTimes, player.buffTime.Length);
            player.buffTime = (int[])sortedTimes.Clone();

            int[] sortedTypes = player.buffType.Where(x => x != 0).ToArray();
            Array.Resize(ref sortedTypes, player.buffType.Length);
            player.buffType = (int[])sortedTypes.Clone();

            orig(self);
            // Store types that were removed from the array during the orig call. These were manually removed (usually by being right clicked, usually just one, but this covers all possible cases).
            var removedTypes = sortedTypes.Except(player.buffType);

            // Restore the arrays.
            player.buffType = buffTypes;
            player.buffTime = buffTimes;
            // Remove manually-removed buffs.
            foreach (var type in removedTypes)
                player.ClearBuff(type);
        }
        static bool BuffCanBeHidden(Player player, int buffIndex)
        {
            int buffTime = player.buffTime[buffIndex];
            int buffType = player.buffType[buffIndex];
            // Might wait for some toggler tweaks for optimization Idk
            return /*player.FargoMutant().PotionToggler.Toggles.Any(t => t.Value.BuffID == buffType && t.Value.ToggleBool) ||*/
                (buffTime <= 2
                && (!Main.debuff[buffType] || buffType == BuffID.Tipsy)
                && !Main.buffNoTimeDisplay[buffType]
                && !BuffID.Sets.TimeLeftDoesNotDecrease[buffType]);
        }

        public override void Update(int type, Player player, ref int buffIndex)
        {

        }
    }
}