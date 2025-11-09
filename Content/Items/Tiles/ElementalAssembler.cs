using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Fargowiltas.Common.Systems.Recipes;

namespace Fargowiltas.Content.Items.Tiles
{
    public class ElementalAssembler : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Elemental Assembler");
            // Tooltip.SetDefault("Functions as several basic crafting stations");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(gold: 50);
            Item.createTile = ModContent.TileType<ElementalAssemblerSheet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AlchemyTable)
                .AddIngredient(ItemID.TinkerersWorkshop)
                .AddIngredient(ItemID.ImbuingStation)
                .AddIngredient(ItemID.DyeVat)
                .AddIngredient(ItemID.LivingLoom)
                .AddIngredient(ItemID.GlassKiln)
                .AddIngredient(ItemID.IceMachine)
                .AddIngredient(ItemID.HoneyDispenser)
                .AddIngredient(ItemID.SkyMill)
                .AddIngredient(ItemID.Solidifier)
                .AddIngredient(ItemID.BoneWelder)
                .AddIngredient(ItemID.LavaBucket)
                .AddIngredient(ItemID.HoneyBucket)
                .AddIngredient(ItemID.TeaKettle)
                .AddRecipeGroup(RecipeGroups.AnyTombstone)
                .AddRecipeGroup(RecipeGroups.AnyDemonAltar)
                .AddIngredient(ItemID.Bone, 5)
                .Register();
        }
    }
}
