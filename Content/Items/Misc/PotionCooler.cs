using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Fargowiltas.Content.UI;
using Fargowiltas.Content.UI.PotionBag;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
    public class PotionCooler : ModItem
    {
        public override string Texture => "Fargowiltas/Content/Items/Placeholder";

        public override bool IsLoadingEnabled(Mod mod) => FargoServerConfig.Instance.PotionCooler;

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Terraria.Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.LocalPlayer == player)
            {
                FargoUIManager.Toggle<PotionBagUI>();
                return true;
            }

            return base.UseItem(player);
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanRightClick() => true;

        public override bool ConsumeItem(Player player) => false;

        public override void RightClick(Player player)
        {
            if (Main.LocalPlayer == player)
            {
                FargoUIManager.Toggle<PotionBagUI>();
                return;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                tooltips.Add(new TooltipLine(Fargowiltas.Instance, "CoolerInstructions", Language.GetTextValue("Mods.Fargowiltas.Items.PotionCooler.Rumination")));
            }
            else
            {
                tooltips.Add(new TooltipLine(Fargowiltas.Instance, "CoolerInstructionsRuminated", Language.GetTextValue("Mods.Fargowiltas.Items.PotionCooler.Ruminate", PotionBagSystem.MaxPotions)));
            }
            base.ModifyTooltips(tooltips);
        }

        public override void UpdateInventory(Player player)
        {
            player.FargoMutant().PotionCooler = true;
            player.FargoMutant().PotionCoolerBuffer = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(2)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddIngredient(ItemID.IceBlock, 20)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
