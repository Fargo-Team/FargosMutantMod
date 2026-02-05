using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
    public class PortableSundial : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.mana = 15;
            Item.UseSound = SoundID.Item4;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.npc.Any(n => n.active && n.boss))
            {
                Item.useAnimation = 120;
                Item.useTime = 120;
            }
            else
            {
                Item.useAnimation = 30;
                Item.useTime = 30;
            }

            //return !Main.fastForwardTime/* tModPorter Note: Removed. Suggestion: IsFastForwardingTime(), fastForwardTimeToDawn or fastForwardTimeToDusk */;
            return !Main.IsFastForwardingTime();
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == ItemAlternativeFunctionID.ActivatedAndUsed)
            {
                Main.sundialCooldown = 0;
                SoundEngine.PlaySound(SoundID.Item4, player.position);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.MiscDataSync, number: Main.myPlayer, number2: 3f);
                    return true;
                }

                //Main.fastForwardTime/* tModPorter Note: Removed. Suggestion: IsFastForwardingTime(), fastForwardTimeToDawn or fastForwardTimeToDusk */ = true;
                if (Main.dayTime)
                    Main.fastForwardTimeToDusk = true;
                else
                    Main.fastForwardTimeToDawn = true;
            }
            else
            {
                int noon = 27000;
                int midnight = 16200;
                if (Main.dayTime && Main.time < noon)
                {
                    Main.time = noon;
                }
                else if (Main.time < midnight)
                {
                    Main.time = midnight;
                }
                else
                {
                    Main.dayTime = !Main.dayTime;
                    Main.time = 0;

                    if (Main.dayTime)
                    {
                        BirthdayParty.CheckMorning();

                        Chest.SetupTravelShop();
                    }
                    else
                    {
                        BirthdayParty.CheckNight();

                        //change moon phases when switching to night
                        if (!Main.dayTime && ++Main.moonPhase > 7)
                            Main.moonPhase = 0;
                    }
                }
            }
            NetMessage.SendData(MessageID.WorldData);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sundial)
                .AddIngredient(ItemID.Moondial)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }
}