using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace Fargowiltas.Common.Configs
{
    public sealed class FargoServerConfig : ModConfig
    {
        public static FargoServerConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => false;

        [Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.Gameplay")]

        [DefaultValue(true)]
        public bool UnlimitedAmmo;

        [DefaultValue(true)]
        public bool UnlimitedConsumableWeapons;

        [DefaultValue(0)]
        [Slider]
        public UnlimitedBuffSelections UnlimitedPotionBuffs;

        [Range(30, 120)]
        [Increment(10)]
        [DefaultValue(30)]
        [Slider]
        public int UnlimitedPotionBuffsAmount;

        //[DefaultValue(true)]
        //public bool UnlimitedPotionBuffsOn120;

        [DefaultValue(true)]
        public bool EasySummons;

        [DefaultValue(true)]
        public bool BossZen;

        [DefaultValue(true)]
        public bool AnglerQuestInstantReset;

        [DefaultValue(true)]
        public bool AnglerQuestPity;

        [DefaultValue(true)]
        public bool ExtraLures;

        [DefaultValue(true)]
        public bool FasterLavaFishing;

        [DefaultValue(true)]
        public bool Fountains;

        [DefaultValue(true)]
        public bool PermanentStationsNearby;

        [DefaultValue(true)]
        public bool TorchGodEX;

        [Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.Content")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool BannerRecipes;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool ContainerRecipes;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool MiscRecipes;

        [DefaultValue(true)]
        public bool NPCSales;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool CatchNPCs;

        [DefaultValue(true)]
        public bool Mutant;

        [DefaultValue(true)]
        public bool Abom;

        [DefaultValue(true)]
        public bool Devi;

        [DefaultValue(true)]
        public bool Lumber;

        [DefaultValue(true)]
        public bool Squirrel;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool InstantItems;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool PotionCooler;

        [Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.StatMultipliers")]

        [Range(1f, 10f)]
        [Increment(.1f)]
        [DefaultValue(1f)]
        public float EnemyHealth;

        [Range(1f, 10f)]
        [Increment(.1f)]
        [DefaultValue(1f)]
        public float BossHealth;

        [Range(1f, 10f)]
        [Increment(.1f)]
        [DefaultValue(1f)]
        public float EnemyDamage;

        [Range(1f, 10f)]
        [Increment(.1f)]
        [DefaultValue(1f)]
        public float BossDamage;

        [DefaultValue(true)]
        public bool BossApplyToAllWhenAlive;

        [Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.Misc")]

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections Halloween;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections Christmas;

        [DefaultValue(true)]
        public bool PiggyBankAcc;

        [DefaultValue(true)]
        public bool ModdedPiggyBankAcc;

        [DefaultValue(false)]
        public bool StalkerMoneyTrough;

        [DefaultValue(true)]
        public bool RottenEggs;

        [DefaultValue(true)]
        public bool SaferBoundNPCs;

        [DefaultValue(true)]
        public bool ExtractSpeed;

        [Range(5, 25)]
        [Increment(5)]
        [DefaultValue(10)]
        [Slider]
        public int FasterBedSpeed;

        [DefaultValue(true)]
        public bool PylonsIgnoreEvents;

        [DefaultValue(false)]
        public bool DisableTombstones;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool ExtraBuffSlots;

    }
}
