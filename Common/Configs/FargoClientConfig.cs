using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Fargowiltas.Common.Configs
{
    public sealed class FargoClientConfig : ModConfig
    {
        public static FargoClientConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.Fargowiltas.Configs.FargoClientConfig.Headers.UserInterface")]

        [DefaultValue(true)]
        public bool AutoReforge;

        [DefaultValue(true)]
        public bool ExpandedTooltips;

        [DefaultValue(false)]
        public bool HideUnlimitedBuffs;

        [DefaultValue(true)]
        public bool ExactTooltips;

        [DefaultValue(true)]
        public bool ColoredDamageTypes;

        [DefaultValue(true)]
        public bool AnimatedRecipeGroups;

        [DefaultValue(0.75f)]
        [Slider]
        public float DebuffOpacity;

        [DefaultValue(0.75f)]
        [Slider]
        public float DebuffFaderRatio;

        [DefaultValue(true)]
        public bool HideTogglerWhenInventoryIsClosed;

        [DefaultValue(false)]
        public bool ToggleSearchReset;

        [Header("$Mods.Fargowiltas.Configs.FargoClientConfig.Headers.Misc")]

        [DefaultValue(false)]
        public bool DoubleTapDashDisabled;

        [DefaultValue(false)]
        public bool DoubleTapSetBonusDisabled;

        [DefaultValue(true)]
        public bool MultiplayerDeathSpectate;

        [DefaultValue(true)]
        public bool DisableScopeView;

        [DefaultValue(1f)]
        [Slider]
        public float TransparentFriendlyProjectiles;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            TransparentFriendlyProjectiles = Utils.Clamp(TransparentFriendlyProjectiles, 0f, 1f);
        }
    }
}
