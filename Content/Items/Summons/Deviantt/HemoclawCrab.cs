using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class HemoclawCrab : BaseSpawnBooster
    {
        public override int BuffType => 0; //irrelevant, being overriden

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = 0;
            Item.buffTime = 0;
            Item.useStyle = ItemUseStyleID.None;
            Item.value /= 2;
            Item.bait = 66;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SeafoodDinner)
                .AddIngredient(ItemID.DeepRedPaint)
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class HemoclawFishingPlayer : ModPlayer
    {
        public override bool? CanConsumeBait(Item bait) => bait.type == ModContent.ItemType<HemoclawCrab>() ? true : base.CanConsumeBait(bait);

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (Main.bloodMoon && attempt.playerFishingConditions.BaitItemType == ModContent.ItemType<HemoclawCrab>())
            {
                npcSpawn = Main.rand.NextBool() ? NPCID.GoblinShark : NPCID.BloodEelHead;
                itemDrop = -1;
            }
        }

        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            if (bait.type == ModContent.ItemType<HemoclawCrab>())
            {
                if (Main.bloodMoon)
                    Player.displayedFishingInfo = Language.GetTextValue("GameUI.FishingWarning");
                else fishingLevel = -1;
            }
        }
    }
}