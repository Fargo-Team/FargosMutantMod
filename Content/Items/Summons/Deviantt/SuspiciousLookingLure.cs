using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class SuspiciousLookingLure : BaseSpawnBooster
    {
        public override int BuffType => 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = 0;
            Item.buffTime = 0;
            Item.useStyle = ItemUseStyleID.None;
            Item.value /= 2;
            Item.bait = 13;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.JourneymanBait)
                .AddIngredient(ItemID.DeepRedPaint)
                .AddIngredient(ItemID.Lens, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class SuspiciousLureFishingPlayer : ModPlayer
    {
        public override bool? CanConsumeBait(Item bait) => bait.type == ModContent.ItemType<SuspiciousLookingLure>() ? true : base.CanConsumeBait(bait);

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (Main.bloodMoon && attempt.playerFishingConditions.BaitItemType == ModContent.ItemType<SuspiciousLookingLure>())
            {
                npcSpawn = Main.rand.NextBool() ? NPCID.ZombieMerman : NPCID.EyeballFlyingFish;
                itemDrop = -1;
            }
        }

        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            if (bait.type == ModContent.ItemType<SuspiciousLookingLure>())
            {
                if (Main.bloodMoon)
                    Player.displayedFishingInfo = Language.GetTextValue("GameUI.FishingWarning");
                else fishingLevel = -1;
            }
        }
    }
}