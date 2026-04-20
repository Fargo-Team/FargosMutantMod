using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Tiles
{
    public class ChestWizard : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(10, 10));
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ChestWizardSheet>();
            Item.value = Terraria.Item.sellPrice(gold: 3);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                tooltips.Add(new TooltipLine(Fargowiltas.Instance, "ChizardInstructions", Language.GetTextValue("Mods.Fargowiltas.Items.ChestWizard.Rumination")));
            }
            else
            {
                tooltips.Add(new TooltipLine(Fargowiltas.Instance, "ChizardInstructionsRuminated", Language.GetTextValue("Mods.Fargowiltas.Items.ChestWizard.Ruminate")));
            }
                base.ModifyTooltips(tooltips);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldChest)
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddIngredient(ItemID.Lens, 3)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
