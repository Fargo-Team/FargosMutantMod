using Fargowiltas.Common.Systems;
using Terraria;

namespace Fargowiltas.Utilities.Extensions
{
    public static class PlayerExtensions
    {
        public static bool IsTileWithinRange(this Player player, int x, int y)
        {
            int extraRange = player.HeldItem.tileBoost;

            bool left = player.Left.ToTileCoordinates().X - Player.tileRangeX - extraRange <= x;
            bool right = player.Right.ToTileCoordinates().X + Player.tileRangeX + extraRange - 1f >= x;
            bool top = player.Top.ToTileCoordinates().Y - Player.tileRangeY - extraRange <= y;
            bool bottom = player.Bottom.ToTileCoordinates().Y + Player.tileRangeY + extraRange - 2f >= y;

            return left && right && top && bottom;
        }

        public static PotionToggle GetPotionToggle(this Player player, int itemID)
        {
            return player.FargoMutant().PotionToggler.Toggles.TryGetValue(itemID, out PotionToggle value) ? value : null;
        }
        public static bool GetPotionToggleValue(this Player player, int itemID)
        {
            PotionToggle toggle = player.GetPotionToggle(itemID);
            if (toggle == null)
                return false;
            return toggle.ToggleBool;
        }

        public static void SetPotionToggleValue(this Player player, int itemID, bool value)
        {
            if (player.FargoMutant().PotionToggler.Toggles.TryGetValue(itemID, out PotionToggle potionToggle))
                potionToggle.ToggleBool = value;
            else
                Fargowiltas.Instance.Logger.Warn($"Expected toggle not found: {itemID}");
        }
    }
}
