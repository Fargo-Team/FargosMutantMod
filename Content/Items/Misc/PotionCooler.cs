using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Fargowiltas.Content.UI;
using Fargowiltas.Content.UI.PotionBag;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Item.value = Item.sellPrice(0, 0, 20);
            Item.rare = ItemRarityID.Blue;
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
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 10)
                .AddIngredient(ItemID.IceBlock, 20)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddIngredient<GizmoParts>(2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
