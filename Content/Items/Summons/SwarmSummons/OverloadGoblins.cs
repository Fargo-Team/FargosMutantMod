using Fargowiltas.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadGoblins : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Goblin War Banner");
            // Tooltip.SetDefault("Summons an Overloaded Goblin Invasion\nUse again to stop the event");

            ItemID.Sets.SortingPriorityMiscImportants[Type] = ItemID.Sets.SortingPriorityMiscImportants[ItemID.GoblinBattleStandard]; // 4
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 1000;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            if (FargoWorld.OverloadGoblins)
            {
                // cancel it
                Main.invasionSize = 1;
                FargoWorld.OverloadGoblins = false;

                if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.OverloadGoblinsStop"), new Color(175, 75, 255));
                }
                else
                {
                    Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.OverloadGoblinsStop"), 175, 75, 255);
                }
            }
            else
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.invasionDelay = 0;
                    Main.StartInvasion(1);
                    Main.invasionSize = 15000;
                    Main.invasionSizeStart = 15000;
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, -1f);
                }

                FargoWorld.OverloadGoblins = true;
                SoundEngine.PlaySound(SoundID.Roar, player.position);
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoblinBattleStandard)
                .AddIngredient(null, "Overloader", 10)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }
}
