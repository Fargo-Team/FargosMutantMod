using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace Fargowiltas.Utilities
{
    public static class RecipeHelper
    {
        /// <summary>
        /// Creates and registers a simple crafting recipe for an item.
        /// </summary>
        /// <param name="ingredientID">The type of the item being used as an ingredient.</param>
        /// <param name="resultID">The type of the resulting crafted item.</param>
        /// <param name="tileID">The type of the crafting station tile where the recipe can be crafted.</param>
        /// <param name="ingredientAmount">The amount of the ingredient item required in the recipe. Defaults to 1.</param>
        /// <param name="resultAmount">The amount of the resulting item created by the recipe. Defaults to 1.</param>
        /// <param name="disableDecraft">A flag indicating whether the crafted item can be de-crafted. Defaults to false.</param>
        /// <param name="usesRecipeGroup">A flag indicating whether the supplied ingredient ID is a recipe group. Defaults to <see langword="false"/>.</param>
        public static void CreateSimpleRecipe(int ingredientID, int resultID, int tileID, int ingredientAmount = 1, int resultAmount = 1, bool disableDecraft = false, RecipeGroup group = null, params Condition[] conditions)
        {
            var recipe = Recipe.Create(resultID, resultAmount);

            if (group != null)
            {
                recipe.AddRecipeGroup(group, ingredientAmount);
            }
            else
            {
                recipe.AddIngredient(ingredientID, ingredientAmount);
            }

            recipe.AddTile(tileID);

            foreach (Condition condition in conditions)
            {
                recipe.AddCondition(condition);
            }

            if (disableDecraft)
            {
                recipe.DisableDecraft();
            }

            recipe.Register();
        }

        /// <summary>
        /// Generates and returns the text for use in recipe group registration, using a vanilla <see cref="ItemID"/>.
        /// </summary>
        /// <param name="vanillaId">The item ID of the vanilla item for which the recipe group text is generated.</param>
        /// <returns>A formatted string containing the "Any" recipe group identifier followed by the name of the specified item.</returns>
        public static string GenerateAnyItemRecipeGroupText(int vanillaId)
        {
            return $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemName(vanillaId)}";
        }

        /// <summary>
        /// Generates and returns the text for use in recipe group registration, using a vanilla or Fargowiltas localization key.
        /// </summary>
        /// <param name="localizationKey">The localization key corresponding to the item for which the recipe group text is being generated.</param>
        /// <param name="isVanillaKey">True if the provided key is a vanilla localization key, false if it is a Fargowiltas localziation key.</param>    
        /// <returns>A formatted string containing the "Any" recipe group identifier followed by the localized name of the specified item.</returns>
        public static string GenerateAnyItemRecipeGroupText(string localizationKey, bool isVanillaKey = false)
        {
            return $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue(isVanillaKey ? localizationKey : $"Mods.Fargowiltas.RecipeGroups.{localizationKey}")}";
        }

        /// <summary>
        /// Generates and returns the text for a recipe group registration associated with banners, using a vanilla localization key.
        /// </summary>
        /// <param name="vanillaKey">The localization key for the vanilla text.</param>
        /// <returns>A formatted string containing the "Any" recipe group identifier, the localized text, and the localized name for banners.</returns>
        public static string GenerateAnyBannerRecipeGroupText(string vanillaKey)
        {
            return $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue(vanillaKey)} {Language.GetTextValue("MapObject.Banner")}";
        }
    }
}
