using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Summons;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class SuspiciousLookingChest : BaseSummon
    {
        public override int NPCType => Main.LocalPlayer.ZoneSnow ? NPCID.IceMimic : NPCID.Mimic;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 6; // Places it right after Gelatin Crystal
        }
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
                    .AddIngredient<GizmoParts>(2)
                    .AddIngredient(ItemID.Chest, 1)
                    .AddRecipeGroup("Fargowiltas:AnyEvilBar", 10)
                    .AddIngredient(ItemID.GoldCoin, 10)
                    .AddTile(TileID.DemonAltar)
                    .Register();
        }
    }
}