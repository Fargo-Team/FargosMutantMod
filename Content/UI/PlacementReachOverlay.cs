using Fargowiltas.Content.BuilderToggles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Fargowiltas.Content.UI;

public class PlacementReachOverlay
{
    public static bool ShouldDraw(Player player)
    => !Main.hideUI
        && player.whoAmI == Main.myPlayer
        && player.active
        && !player.dead
        && !player.ghost
        && ModContent.GetInstance<PlacementReachBuilderToggle>().CurrentState != 0;

    public static void Draw(SpriteBatch spriteBatch, Player player)
    {
        if (!ShouldDraw(player))
            return;

        int state = ModContent.GetInstance<PlacementReachBuilderToggle>().CurrentState;

        int x = Player.tileRangeX;
        int y = Player.tileRangeY;

        Texture2D texture = TextureAssets.Extra[ExtrasID.LaserRuler].Value;

        if (state == 1)
        {
            for (int i = -x; i <= x; i++)
            {
                DrawBox(texture, i, -y, spriteBatch);
                DrawBox(texture, i, y, spriteBatch);
            }

            for (int j = -y; j <= y - 2; j++)
            {
                DrawBox(texture, -x, j + 1, spriteBatch);
                DrawBox(texture, x, j + 1, spriteBatch);
            }
        }

        if (state == 2)
        {
            for (int i = -x; i <= x; i++)
            {
                for (int j = -y; j <= y; j++)
                    DrawBox(texture, i, j, spriteBatch);
            }
        }


        int textXPos = (int)(x + player.Center.X / 16f);
        int textYPos = (int)(y + player.Center.Y / 16f);

        if (textXPos < 0 || textXPos >= Main.maxTilesX || textYPos < 0 || textYPos >= Main.maxTilesY)
            return;

        Tile textTile = Main.tile[textXPos, textYPos];
        if (textTile == null)
            return;

        string text = Language.GetTextValue("Mods.Fargowiltas.UI.PlacementRange");
        string range = $"{x * 2}x{y * 2}";

        Vector2 textSize = FontAssets.ItemStack.Value.MeasureString(text);
        Vector2 textPosition = player.Bottom - new Vector2(textSize.X / 2, -textSize.Y * (y - 1));

        Vector2 rangeSize = FontAssets.ItemStack.Value.MeasureString(range);
        Vector2 rangePosition = player.Bottom + new Vector2(-rangeSize.X / 2, textSize.Y * (y)).Floor();

        ChatManager.DrawColorCodedStringWithShadow(
            Main.spriteBatch,
            FontAssets.ItemStack.Value,
            text,
            textPosition.Floor() - Main.screenPosition,
            Color.Lime,
            0f,
            Vector2.Zero,
            Vector2.One);

        ChatManager.DrawColorCodedStringWithShadow(
            Main.spriteBatch,
            FontAssets.ItemStack.Value,
            range,
            rangePosition.Floor() - Main.screenPosition,
            Color.Lime,
            0f,
            Vector2.Zero,
            Vector2.One);

    }

    public static void DrawBox(Texture2D texture, int x, int y, SpriteBatch spriteBatch)
    {
        int rightClickState = PlacementReachBuilderToggle.RightClickState;
        Player player = Main.LocalPlayer;
        int xPosition = (int)(x + player.Center.X / 16f);
        int yPosition = (int)(y + player.Center.Y / 16f);

        if (xPosition < 0 || xPosition >= Main.maxTilesX || yPosition < 0 || yPosition >= Main.maxTilesY)
            return;

        Tile tile = Main.tile[xPosition, yPosition];
        if (tile == null)
            return;

        Rectangle rect = new(0, 0, 16, 16);

        Vector2 tilePosition = new Vector2(xPosition * 16, yPosition * 16);
        float opacity = rightClickState != 0 ?
            MathHelper.Lerp(1, 0, tilePosition.Distance(Main.MouseWorld) / 64)
            :
            MathHelper.Lerp(0, 1, tilePosition.Distance(Main.MouseWorld) / 128);

        spriteBatch.Draw(
            texture,
            tilePosition - Main.screenPosition,
            rect,
            (Color.Lime * opacity) * 0.6f,
            0,
            Vector2.Zero,
            1,
            SpriteEffects.None,
            0);
    }

}
