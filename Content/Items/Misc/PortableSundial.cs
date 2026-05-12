using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            Item.rare = ItemRarityID.Lime;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.mana = 15;
            Item.UseSound = SoundID.Item4;
        }

        int drawTimer;

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

            return !Main.IsFastForwardingTime();
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == ItemAlternativeFunctionID.ActivatedAndUsed)
            {
                if (Main.sundialCooldown == 0)
                {
                    Main.sundialCooldown = 8;
                    SoundEngine.PlaySound(SoundID.Item4, player.position);

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.MiscDataSync, number: Main.myPlayer, number2: 3f);
                        return true;
                    }

                    if (Main.dayTime)
                        Main.fastForwardTimeToDusk = true;
                    else
                        Main.fastForwardTimeToDawn = true;
                }
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

                        Main.AnglerQuestSwap();

                        Main.CheckForMoonEventsScoreDisplay();
                        Main.CheckForMoonEventsStartingTemporarySeasons();
                        Main.checkXMas();
                        Main.checkHalloween();

                        Main.moonPhase++;
                        if (Main.moonPhase >= 8)
                            Main.moonPhase = 0;

                        if (Main.drunkWorld && Main.netMode != NetmodeID.MultiplayerClient)
                            WorldGen.crimson = !WorldGen.crimson;
                    }
                    else
                    {
                        BirthdayParty.CheckNight();
                    }
                }
            }
            NetMessage.SendData(MessageID.WorldData);
            return true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (Main.sundialCooldown == 0)
            {
                Texture2D texture = ModContent.Request<Texture2D>(Texture + "_glow").Value;
                Color color3 = new(100, 100, 100, 0);
                for (int j = 0; j < 4; j++)
                {
                    int rng = Main.rand.Next(-5, 6);
                    spriteBatch.Draw(texture, position + new Vector2(rng * 0.15f, rng * 0.35f), frame, color3, 0f, origin, scale, SpriteEffects.None, 0f);
                }
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (Main.sundialCooldown == 0)
            {
                Texture2D texture = ModContent.Request<Texture2D>(Texture + "_glow").Value;
                Vector2 cent = Item.Bottom - Main.screenPosition - new Vector2(0, (texture.Size() / 2).Y).RotatedBy(rotation);
                Color color3 = new(100, 100, 100, 0);
                for (int j = 0; j < 4; j++)
                {
                    int rng = Main.rand.Next(-5, 6);
                    spriteBatch.Draw(texture, cent + new Vector2(rng * 0.15f, rng * 0.35f), null, color3, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(5)
                .AddIngredient(ItemID.Sundial)
                .AddIngredient(ItemID.Moondial)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }
}