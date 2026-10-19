using Fargowiltas.Assets.Textures;
using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Fargowiltas.Content.Items.Summons;
using Fargowiltas.Content.UI;
using Fargowiltas.Content.UI.PotionBag;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc;

[LegacyName("PotionCoolerInactive")]
public class PotionCooler : ModItem
{
    public override string Texture => "Fargowiltas/Content/Items/Misc/PotionCooler";

    public override bool IsLoadingEnabled(Mod mod) => FargoServerConfig.Instance.PotionCooler;


    public static SoundStyle StorePotion = new("Fargowiltas/Assets/Sounds/PotionCoolerStore");

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

        if (Main.LocalPlayer == player)
        {
            FargoUIManager.Toggle<PotionBagUI>();
            return true;
        }

        return base.UseItem(player);
    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    public override bool ConsumeItem(Player player) => false;

    public override bool CanRightClick() => true;
    public override void RightClick(Player player)
    {
        if (Main.LocalPlayer == player)
        {
            Item item = Main.mouseItem;
            bool cantInput = item.IsAir || item.buffType == 0 || item.buffTime < 60 * 60 * 2 || item.ModItem is BaseSpawnBooster;
            if (!cantInput)
            {
                if (PotionBagSystem.CanConsumePotion(item.type, item.stack, out int consumeAmount, out int leftover))
                {
                    item.stack = leftover;
                    SoundEngine.PlaySound(StorePotion with { Volume = 0.6f});
                    inputInterpolant = 30;
                    PotionBagUI.NeedsPotionListBuilding = true;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        FargoNet.AddPotionToPotionBag(item.type, consumeAmount);
                    else
                        PotionBagSystem.AddPotion(item.type, consumeAmount);
                }
            }
            else
                FargoUIManager.Toggle<PotionBagUI>();
            return;
        }
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        /*
        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
        {
            tooltips.Add(new TooltipLine(Fargowiltas.Instance, "CoolerInstructions", Language.GetTextValue("Mods.Fargowiltas.Items.PotionCooler.Rumination")));
        }
        else
        {
            tooltips.Add(new TooltipLine(Fargowiltas.Instance, "CoolerInstructionsRuminated", Language.GetTextValue("Mods.Fargowiltas.Items.PotionCooler.Ruminate", PotionBagSystem.MaxPotions)));
        }
        */
        base.ModifyTooltips(tooltips);
    }

    public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Asset<Texture2D> texture = TextureAssets.Item[Type];
        Rectangle drawFrame = texture.Frame(1, 2, 0, PotionBagSystem.AnyCompletedPotions ? 1 : 0);

        inputInterpolant = MathHelper.Clamp(inputInterpolant, 0, 30);
        if (!Main.gamePaused)
            inputInterpolant -= 1.3f;
        float ratio = inputInterpolant / 30f;
        scale = MathHelper.Lerp(scale, scale * 1.2f, ratio);

        sb.Draw(texture.Value, position, drawFrame, drawColor, 0, drawFrame.Size() / 2f, scale, SpriteEffects.None, 1);
        return false;
        //return base.PreDrawInInventory(sb, position, drawFrame, drawColor, itemColor, drawFrame.Size() * 0.5f, scale);
    }

    public float inputInterpolant;
    public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Item item = Main.mouseItem;
        bool cantInput = item.IsAir || item.buffType == 0 || item.buffTime < 60 * 60 * 2 || item.ModItem is BaseSpawnBooster;

        PotionBagSystem.TryGetCount(item.type, out int count);
        if (!cantInput)
        {
            bool hoveringCoolor = Main.HoverItem?.type == ModContent.ItemType<PotionCooler>();

            int adjustedCount = count + (hoveringCoolor ? item.stack : 0);
            adjustedCount = (int)MathHelper.Clamp(adjustedCount, 0, PotionBagSystem.MaxPotions);
            string potionCount = adjustedCount + "/" + PotionBagSystem.MaxPotions;
            Vector2 textSize = FontAssets.ItemStack.Value.MeasureString(potionCount);
            Vector2 textPosition = position + new Vector2(-8 - (textSize.X / potionCount.Length + 1), 8);
            Color colorToUse = hoveringCoolor ? Color.LimeGreen : Color.White;

            if (count >= PotionBagSystem.MaxPotions && hoveringCoolor)
            {
                Texture2D bigCross = ModContent.Request<Texture2D>("Fargowiltas/Assets/Textures/UI/BigCross", AssetRequestMode.ImmediateLoad).Value;
                Rectangle xFrame = bigCross.Frame();
                sb.Draw(bigCross, position, xFrame, drawColor, 0, xFrame.Size() / 2f, scale, SpriteEffects.None, 1);
            }
            else
                Utils.DrawBorderString(sb, potionCount, textPosition, colorToUse, scale);
        }
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        Main.itemFrame[whoAmI] = PotionBagSystem.AnyCompletedPotions ? 1 : 0;
        return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
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
