using Fargowiltas.Content.Items.Misc;
using Steamworks;
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
                .FargoStat("Life", ItemID.LifeCrystal, () => Main.LocalPlayer.statLifeMax2)
                .FargoStat("LifeRegen", ItemID.BandofRegeneration, () => Main.LocalPlayer.lifeRegen / 2)
                .FargoStat("Defense", ItemID.Shackle, () => Main.LocalPlayer.statDefense)
                .FargoStat("DamageReduction", ItemID.WormScarf, () => DamageReduction())
                .FargoStat("KnockbackImmunity", ItemID.CobaltShield, Main.LocalPlayer.noKnockback.ToString)
                .FargoStat("Aggro", ItemID.FleshKnuckles, () => Main.LocalPlayer.aggro)
                .FargoStat("ArmorPenetration", ItemID.SharkToothNecklace, () => Main.LocalPlayer.GetArmorPenetration(DamageClass.Generic))
                .RegisterCategory();

            // Movement
            FargoCreate("Movement")
                .FargoStat("MovementSpeed", ItemID.SwiftnessPotion, () => Math.Round(Main.LocalPlayer.moveSpeed * 100))
                .FargoStat("MaxSpeed", ItemID.HermesBoots, () => MaxSpeed())
                .FargoStat("Acceleration", ItemID.Magiluminescence, () => Math.Round((1f + Main.LocalPlayer.runAcceleration) * 100f))
                .FargoStat("Deceleration", ItemID.IceBlock, () => Math.Round((1f + Main.LocalPlayer.runSlowdown) * 100f))
                .FargoStat("WingTime", ItemID.AngelWings, WingTime, condition: HasWings)
                .FargoStat("WingMaxSpeed", ItemID.FishronWings, () => Math.Round(Main.LocalPlayer.FargoMutant().StatSheetWingSpeed * 32 / 6.25), condition: HasWings)
                .FargoStat("WingAscentModifier", ItemID.RainbowWings, () => Math.Round(Main.LocalPlayer.FargoMutant().StatSheetMaxAscentMultiplier * 100), condition: HasWings)
                .RegisterCategory();

            // Utility
            FargoCreate("Utility")
                .FargoStat("FishingQuests", ItemID.AnglerEarring, () => Main.LocalPlayer.anglerQuestsFinished)
                .FargoStat("MiningSpeed", ItemID.CopperPickaxe, () => Math.Round(Math.Min(170, 200 - Main.LocalPlayer.pickSpeed * 100)))
                .FargoStat("Luck", ItemID.LadyBug, () => Math.Round(Main.LocalPlayer.luck, 2))
                .FargoStat("ExtraPlacementRange", ItemID.ArchitectGizmoPack, () => Main.LocalPlayer.blockRange)
                .FargoStat("BattleCry", ModContent.ItemType<BattleCry>(), BattleCryText, condition: BattleCryCondition)
                .RegisterCategory();

            // Melee
            FargoCreate("Melee")
                .FargoStat("MeleeDamage", ItemID.CopperBroadsword, () => Damage(DamageClass.Melee))
                .FargoStat("MeleeCritical", ItemID.CopperBroadsword, () => Crit(DamageClass.Melee))
                .FargoStat("MeleeSpeed", ItemID.FeralClaws, () => (int)Math.Round(Main.LocalPlayer.GetAttackSpeed(DamageClass.Melee) * 100))
                .FargoStat("MeleeSize", ItemID.TitanGlove, () => Math.Round(Main.LocalPlayer.GetAdjustedItemScale(Main.LocalPlayer.HeldItem ?? ContentSamples.ItemsByType[ItemID.CopperBroadsword]) * 100f))
                .RegisterCategory();

            // Ranged
            FargoCreate("Ranged")
                .FargoStat("RangedDamage", ItemID.CopperBow, () => Damage(DamageClass.Ranged))
                .FargoStat("RangedCritical", ItemID.CopperBow, () => Crit(DamageClass.Ranged))
                .RegisterCategory();

            // Magic
            FargoCreate("Magic")
                .FargoStat("MagicDamage", ItemID.AmethystStaff, () => Damage(DamageClass.Magic))
                .FargoStat("MagicCritical", ItemID.AmethystStaff, () => Crit(DamageClass.Magic))
                .FargoStat("Mana", ItemID.ManaCrystal, () => Main.LocalPlayer.statManaMax2)
                .FargoStat("ManaRegen", ItemID.BandofStarpower, () => Main.LocalPlayer.manaRegen / 2)
                .FargoStat("ManaCostReduction", ItemID.NaturesGift, () => Math.Round((1.0 - Main.LocalPlayer.manaCost) * 100))
                .RegisterCategory();

            // Summon
            FargoCreate("Summon")
                .FargoStat("SummonDamage", ItemID.SlimeStaff, () => Damage(DamageClass.Summon))
                .FargoStat("MaxMinions", ItemID.PygmyNecklace, () => Main.LocalPlayer.maxMinions)
                .FargoStat("MaxSentries", ItemID.StaffoftheFrostHydra, () => Main.LocalPlayer.maxTurrets)
                .FargoStat("WhipSpeed", ItemID.ThornWhip, () => Math.Round(Main.LocalPlayer.GetAttackSpeed<SummonMeleeSpeedDamageClass>() * 100f))
                .FargoStat("WhipLength", ItemID.FireWhip, () => Math.Round(Main.LocalPlayer.whipRangeMultiplier * 100f))
                .RegisterCategory();
        }

        internal void AddSoulsStats()
        {
            var souls = Fargowiltas.ModLoaded["FargowiltasSouls"] ? ModLoader.GetMod("FargowiltasSouls") : null;
            if (souls != null)
            {
                StatRegistry.TryAddStatToCategory("Summon", "SummonCritical", souls.Find<ModItem>("SpiderEnchant").Type, () => (int)souls.Call("GetSummonCrit"), () => StatSheetLocal("SummonCritical"), 1 + float.Epsilon);
                StatRegistry.TryAddStatToCategory("Combat", "AttackSpeed", souls.Find<ModItem>("MythrilEnchant").Type, () => (int)Math.Round(MathF.Max((float)souls.Call("GetCachedAttackSpeed"), (float)souls.Call("GetAttackSpeed")) * 100), () => StatSheetLocal("AttackSpeed"));

            }
        }

        private static double Damage(DamageClass damageClass) => Math.Round(Main.LocalPlayer.GetTotalDamage(damageClass).Additive * Main.LocalPlayer.GetTotalDamage(damageClass).Multiplicative * 100 - 100);
        private static int Crit(DamageClass damageClass) => (int)Main.LocalPlayer.GetTotalCritChance(damageClass);

        private static bool HasWings() => Main.LocalPlayer.wingTimeMax > 0;

        private static int MaxSpeed() => (int)((Main.LocalPlayer.accRunSpeed + Main.LocalPlayer.maxRunSpeed) / 2f * Main.LocalPlayer.moveSpeed * 3);

        private static string WingTime()
        {
            Player player = Main.LocalPlayer;
            if (player.wingTimeMax / 60 > 60 || player.empressBrooch && !Fargowiltas.ModLoaded["CalamityMod"])
                return StatSheetLocal("WingTimeMoreThan60Sec");
            return StatSheetLocal("WingTimeActual", Math.Round(player.wingTimeMax / 60.0, 2));
        }

        private static int DamageReduction()
        {
            float endurance = Main.LocalPlayer.endurance;
            if (FargoUtils.EternityMode)
            {
                float r = 0.15f;
                if (endurance >= r)
                    endurance = 1 - MathF.Pow(1 - r, endurance / r);
            }
            return (int)Math.Round(endurance * 100);
        }

        private static string BattleCryText()
        {
            FargoPlayer modPlayer = Main.LocalPlayer.FargoMutant();
            if (modPlayer.BattleCry)
                return $"[c/ff0000:{Language.GetTextValue("Mods.Fargowiltas.Items.BattleCry.Battle")}]";
            if (modPlayer.CalmingCry)
                return $"[c/00ffff:{Language.GetTextValue("Mods.Fargowiltas.Items.BattleCry.Calming")}]";
            return Language.GetTextValue("Mods.Fargowiltas.UI.BattleCryNone");
        }

        private static bool BattleCryCondition() => Main.LocalPlayer.HasItem(ModContent.ItemType<BattleCry>()) || Main.LocalPlayer.FargoMutant().BattleCry || Main.LocalPlayer.FargoMutant().CalmingCry;
    }
}
