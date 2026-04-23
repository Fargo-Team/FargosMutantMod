using Fargowiltas.Content.Buffs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class SuspiciousLookingChest : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<SuspiciousLookingChestBuff>();

        public override bool CanUseItem(Player player)
        {
            if (!(Main.hardMode || Main.remixWorld) && !FargoUtils.EternityMode)
                return false;
            return base.CanUseItem(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!(FargoUtils.EternityMode || Main.remixWorld))
                tooltips.Insert(4, new TooltipLine(Mod, "HardmodeLock", Language.GetTextValue($"Mods.Fargowiltas.Items.SuspiciousLookingChest.HardmodeLock")));

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                    .AddIngredient(ItemID.Chest, 1)
                    .AddRecipeGroup("Fargowiltas:AnyEvilBar", 10)
                    .AddIngredient(ItemID.GoldCoin, 10)
                    .AddTile(TileID.DemonAltar)
                    .Register();
        }
    }
    public class SuspiciousLookingChestBuff : BaseSpawnBoosterBuff
    {
        public SuspiciousLookingChestBuff() : base(() => Main.LocalPlayer.ZoneSnow ? [NPCID.IceMimic] : [NPCID.Mimic], () => Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight, 0.3f)
        {
        }
    }
}
