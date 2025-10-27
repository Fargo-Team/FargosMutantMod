using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
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

        [DefaultValue(true)]
        public bool UnlimitedPotionBuffsOn120;

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

        [Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.WorldStates")]
        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections Halloween;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections Christmas;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections DrunkWorld;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections BeeWorld;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections WorthyWorld;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections CelebrationWorld;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections ConstantWorld;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections NoTrapsWorld;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections RemixWorld;

        [DefaultValue(0)]
        [DrawTicks]
        public SeasonSelections ZenithWorld;
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

        [DefaultValue(true)]
        public bool PiggyBankAcc;

        [DefaultValue(true)]
        public bool ModdedPiggyBankAcc;

        [DefaultValue(true)]
        public bool StalkerMoneyTrough;

        [DefaultValue(true)]
        public bool RottenEggs;

        [DefaultValue(true)]
        public bool SaferBoundNPCs;

        [DefaultValue(true)]
        public bool ExtractSpeed;

        [DefaultValue(true)]
        public bool PylonsIgnoreEvents;

        [DefaultValue(false)]
        public bool SafeTerraformers;

        [DefaultValue(false)]
        public bool DisableTombstones;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool ExtraBuffSlots;

    }
}
