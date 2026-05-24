using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.UI.StatSheet
{
    public class StatTracker
    {
        public bool statsInitialized;

        public StatTracker()
        {
            statsInitialized = false;
            AddFargoStats();
        }

        public void FinalizeStats()
        {
            statsInitialized = true;
            StatRegistry.FinalizeRegistry();
        }

        private static string StatSheetLocal(string key, object arg) => Language.GetTextValue($"Mods.Fargowiltas.UI.StatSheet.{key}", arg);
        private static string StatSheetLocal(string key) => Language.GetTextValue($"Mods.Fargowiltas.UI.StatSheet.{key}");

        private static string IconPath(string key) => $"Fargowiltas/Assets/Textures/UI/{key}";

        private StatCategory FargoCreate(string key, Func<bool> condition = null) => StatCategory.Create(key, $"Mods.Fargowiltas.UI.StatSheet.{key}", IconPath($"{key}_Icon"), condition);
        

        internal void AddFargoStats()
        {
            if (statsInitialized)
                return;

            // Only used for ui logic (DO NOT ADD TO THIS)
            FargoCreate("PermaUpgrade").RegisterCategory();

            // Combat
            FargoCreate("Combat")
                .FargoStat("Life", () => Main.LocalPlayer.statLifeMax2)
                .FargoStat("LifeRegen", () => Main.LocalPlayer.lifeRegen / 2)
                .FargoStat("Defense", () => Main.LocalPlayer.statDefense)
                .FargoStat("DamageReduction", () => DamageReduction())
                .FargoStat("KnockbackImmunity", Main.LocalPlayer.noKnockback.ToString)
                .FargoStat("Aggro", () => Main.LocalPlayer.aggro)
                .FargoStat("ArmorPenetration", () => Main.LocalPlayer.GetArmorPenetration(DamageClass.Generic))
                .RegisterCategory();

            // Movement
            FargoCreate("Movement")
                .FargoStat("MovementSpeed", () => Math.Round(Main.LocalPlayer.moveSpeed * 100))
                .FargoStat("MaxSpeed", () => MaxSpeed())
                .FargoStat("Acceleration", () => Math.Round((1f + Main.LocalPlayer.runAcceleration) * 100f))
                .FargoStat("Deceleration", () => Math.Round((1f + Main.LocalPlayer.runSlowdown) * 100f))
                .FargoStat("WingTime", WingTime, condition: HasWings)
                .FargoStat("WingMaxSpeed", () => Math.Round(Main.LocalPlayer.FargoMutant().StatSheetWingSpeed * 32 / 6.25), condition: HasWings)
                .FargoStat("WingAscentModifier", () => Math.Round(Main.LocalPlayer.FargoMutant().StatSheetMaxAscentMultiplier * 100), condition: HasWings)
                .RegisterCategory();

            // Utility
            FargoCreate("Utility")
                .FargoStat("FishingQuests", () => Main.LocalPlayer.anglerQuestsFinished)
                .FargoStat("MiningSpeed", () => Math.Round(Math.Min(170, 200 - Main.LocalPlayer.pickSpeed * 100)))
                .FargoStat("Luck", () => Math.Round(Main.LocalPlayer.luck, 2))
                .FargoStat("PlacementSpeed", () => Math.Round((1f / Main.LocalPlayer.tileSpeed - 0.25f) * 100))
                .FargoStat("ExtraPlacementRange", () => Main.LocalPlayer.blockRange)
                .RegisterCategory();

            // Melee
            FargoCreate("Melee")
                .FargoStat("MeleeDamage", () => Damage(DamageClass.Melee))
                .FargoStat("MeleeCritical", () => Crit(DamageClass.Melee))
                .FargoStat("MeleeSpeed", () => (int)Math.Round(Main.LocalPlayer.GetAttackSpeed(DamageClass.Melee) * 100))
                .FargoStat("MeleeSize", () => Math.Round(Main.LocalPlayer.GetAdjustedItemScale(Main.LocalPlayer.HeldItem ?? ContentSamples.ItemsByType[ItemID.CopperBroadsword]) * 100f))
                .RegisterCategory();

            // Ranged
            FargoCreate("Ranged")
                .FargoStat("RangedDamage", () => Damage(DamageClass.Ranged))
                .FargoStat("RangedCritical", () => Crit(DamageClass.Ranged))
                .RegisterCategory();

            // Magic
            FargoCreate("Magic")
                .FargoStat("MagicDamage", () => Damage(DamageClass.Magic))
                .FargoStat("MagicCritical", () => Crit(DamageClass.Magic))
                .FargoStat("Mana", () => Main.LocalPlayer.statManaMax2)
                .FargoStat("ManaRegen", () => Main.LocalPlayer.manaRegen / 2)
                .FargoStat("ManaCostReduction", () => Math.Round((1.0 - Main.LocalPlayer.manaCost) * 100))
                .RegisterCategory();

            // Summon
            FargoCreate("Summon")
                .FargoStat("SummonDamage", () => Damage(DamageClass.Summon))
                .FargoStat("MaxMinions", () => Main.LocalPlayer.maxMinions)
                .FargoStat("MaxSentries", () => Main.LocalPlayer.maxTurrets)
                .FargoStat("WhipSpeed", () => Math.Round(Main.LocalPlayer.GetAttackSpeed<SummonMeleeSpeedDamageClass>() * 100f))
                .FargoStat("WhipLength", () => Math.Round(Main.LocalPlayer.whipRangeMultiplier * 100f))
                .RegisterCategory();
        }

        internal void AddSoulsStats()
        {
            Mod souls = Fargowiltas.SoulsMod;
            if (souls != null)
            {
                StatRegistry.TryAddStatToCategory("Summon", "SummonCritical", () => (int)souls.Call("GetSummonCrit"), () => StatSheetLocal("SummonCritical"), 1 + float.Epsilon, modName: "FargowiltasSouls");
                StatRegistry.TryAddStatToCategory("Combat", "AttackSpeed", () => (int)Math.Round(MathF.Max((float)souls.Call("GetCachedAttackSpeed"), (float)souls.Call("GetAttackSpeed")) * 100), () => StatSheetLocal("AttackSpeed"), modName: "FargowiltasSouls");

            }
        }

        private static double Damage(DamageClass damageClass) => Math.Round(Main.LocalPlayer.GetTotalDamage(damageClass).Additive * Main.LocalPlayer.GetTotalDamage(damageClass).Multiplicative * 100 - 100);
        private static int Crit(DamageClass damageClass) => (int)Main.LocalPlayer.GetTotalCritChance(damageClass);

        private static bool HasWings() => Main.LocalPlayer.wingTimeMax > 0;

        private static int MaxSpeed() => (int)((Main.LocalPlayer.accRunSpeed + Main.LocalPlayer.maxRunSpeed) / 2f * Main.LocalPlayer.moveSpeed * 3);

        private static string WingTime()
        {
            Player player = Main.LocalPlayer;
            if (player.wingTimeMax > 60 * 60 || (player.empressBrooch && Fargowiltas.CalamityMod == null))
                return StatSheetLocal("WingTimeMoreThan60Sec");
            return StatSheetLocal("WingTimeActual", Math.Round(player.wingTimeMax / 60.0, 2));
        }

        private static int DamageReduction()
        {
            float endurance = Main.LocalPlayer.endurance;
            if (FargoWorld.EternityMode)
            {
                float r = 0.15f;
                if (endurance >= r)
                    endurance = 1 - MathF.Pow(1 - r, endurance / r);
            }
            return (int)Math.Round(endurance * 100);
        }
    }
}
