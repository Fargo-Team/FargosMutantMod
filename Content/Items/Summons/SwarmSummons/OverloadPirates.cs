using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons
{
    public class OverloadPirates : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pirate's Bounty");
            // Tooltip.SetDefault("Summons an Overloaded Pirate Invasion\nUse again to stop the event");
            ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.PirateMap];
            ;
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
            if (FargoWorld.OverloadPirates)
            {
                // cancel it
                Main.invasionSize = 1;
                FargoWorld.OverloadPirates = false;

                if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.OverloadPiratesStop"), new Color(175, 75, 255));
                }
                else
                {
                    Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.OverloadPiratesStop"), 175, 75, 255);
                }
            }
            else
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.invasionDelay = 0;
                    Main.StartInvasion(3);
                    Main.invasionSize = 15000;
                    Main.invasionSizeStart = 15000;
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, -3f);
                }

                FargoWorld.OverloadPirates = true;
                SoundEngine.PlaySound(SoundID.Roar, player.position);
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PirateMap)
                .AddIngredient(null, "Overloader", 10)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }
}
