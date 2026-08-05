using Fargowiltas.Common;
using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Fargowiltas.Content.UI;
using Fargowiltas.Content.UI.PotionBag;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
    public class PotionCooler : ModItem
    {
        public override string Texture => "Fargowiltas/Content/Items/Misc/PotionCooler";

        public override bool IsLoadingEnabled(Mod mod) => FargoServerConfig.Instance.PotionCooler;

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new PotionCoolerDrawAnimation());
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noUseGraphic = true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                return true;

            if (Main.LocalPlayer == player)
            {
                FargoUIManager.Toggle<PotionBagUI>();
                return true;
            }

            return base.UseItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            SoundEngine.PlaySound(SoundID.Grab);
            Item.ChangeItemType(ModContent.ItemType<PotionCoolerInactive>());
            return false;
        }

        public override bool ConsumeItem(Player player) => false;

        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            Item.ChangeItemType(ModContent.ItemType<PotionCoolerInactive>());
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

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Asset<Texture2D> texture = TextureAssets.Item[Type];
            Rectangle drawFrame = texture.Frame(1, 2, 0, PotionBagSystem.AnyCompletedPotions ? 1 : 0);
            return base.PreDrawInInventory(spriteBatch, position, drawFrame, drawColor, itemColor, drawFrame.Size() * 0.5f, scale);
        }

        public override bool PreDrawInWorld(WorldItem item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.itemFrame[whoAmI] = PotionBagSystem.AnyCompletedPotions ? 1 : 0;
            return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
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
                .AddRecipeGroup(RecipeGroups.IronBar, 5)
                .AddIngredient(ItemID.IceBlock, 20)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Anvils)
                .Register();

        }
    }

    public class PotionCoolerInactive : ModItem
    {
        public override string Texture => "Fargowiltas/Content/Items/Misc/PotionCooler_Inactive";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 0;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 1);
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noUseGraphic = true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                return true;

            if (Main.LocalPlayer == player)
            {
                FargoUIManager.Toggle<PotionBagUI>();
                return true;
            }

            return base.UseItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            SoundEngine.PlaySound(SoundID.Grab);
            Item.ChangeItemType(ModContent.ItemType<PotionCooler>());
            return false;
        }

        public override bool ConsumeItem(Player player) => false;
        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            Item.ChangeItemType(ModContent.ItemType<PotionCooler>());
        }
    }

    public class PotionCoolerDrawAnimation : DrawAnimation
    {
        public override void Update()
        {
            base.Frame = PotionBagSystem.AnyCompletedPotions ? 1 : 0;
        }

        public override Rectangle GetFrame(Texture2D texture, int frameCounterOverride = -1)
        {
            return texture.Frame(1, 2, 0, base.Frame);
        }
    }
}
