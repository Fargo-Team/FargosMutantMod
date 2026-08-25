using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.SwarmSummons;

public class OverloadFrostMoon : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Naughty Present Sack");
        // Tooltip.SetDefault("Summons an Overloaded Frost Moon\nUse again to stop the event");

        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.NaughtyPresent]; // 15
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

    public override bool CanUseItem(Player player)
    {
        return !Main.IsItDay();
    }

    public override bool? UseItem(Player player)
    {
        if (FargoWorld.OverloadFrostMoon)
        {
            // cancel it
            Main.snowMoon = false;
            FargoWorld.OverloadFrostMoon = false;

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.OverloadFrostMoonStop"), new Color(175, 75, 255));
            }
            else
            {
                Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.OverloadFrostMoonStop"), 175, 75, 255);
            }
        }
        else
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.OverloadFrostMoonStart"), new Color(50, 255, 130));
            }
            else
            {
                Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.OverloadFrostMoonStart"), 50, 255, 130);
            }

            Main.snowMoon = true;
            Main.pumpkinMoon = false;
            Main.bloodMoon = false;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.waveKills = 0f;
                NPC.waveNumber = 20;

                if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.OverloadFrostMoonWave20"), new Color(175, 75, 255));
                }
                else
                {
                    Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.OverloadFrostMoonWave20"), 175, 75, 255);
                }
            }
            else
            {
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, -5f);
            }

            FargoWorld.OverloadFrostMoon = true;
            SoundEngine.PlaySound(SoundID.Roar, player.position);
        }

        return true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.NaughtyPresent)
            .AddIngredient(null, "Overloader", 10)
            .AddTile(TileID.CrystalBall)
            .Register();
    }
}
