using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
    public class PortableSundial : ModItem
    {
        private static readonly MethodInfo SkipToTime_MethodInfo = typeof(Main).GetMethod("SkipToTime", FargoUtils.UniversalBindingFlags);
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
            return FargoWorld.PortableSundialCooldown == 0;
        }

        public override bool CanUseItem(Player player)
        {
            bool value = !Main.IsFastForwardingTime();
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
            if (player.altFunctionUse == ItemAlternativeFunctionID.ShouldBeActivated && FargoWorld.PortableSundialCooldown > 0)
            {
                value = false;
            }
            return value;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == ItemAlternativeFunctionID.ActivatedAndUsed)
            {
                if (FargoWorld.PortableSundialCooldown == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item4, player.Center);
                    FargoWorld.PortableSundialCooldown = 4;

                    if (Main.dayTime)
                    {
                        Main.fastForwardTimeToDawn = true;
                    }
                    else
                    {
                        Main.fastForwardTimeToDusk = true;
                    }

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket setCooldown = Mod.GetPacket();
                        setCooldown.Write((byte)Fargowiltas.PacketID.SyncPortableSundial);
                        setCooldown.Write((byte)4);
                        setCooldown.Send();
                    }
                    return true;
                }
            }
            else
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    return true;
                }
                int noon = 27000;
                int midnight = 16200;
                if (Main.dayTime && Main.time < noon)
                {
                    SkipToTime_MethodInfo.Invoke(null, [noon, true]);
                }
                else if (Main.time < midnight)
                {
                    SkipToTime_MethodInfo.Invoke(null, [midnight, false]);
                }
                else
                {
                    bool currentlyNight = Main.dayTime;
                    FargoWorld.BlockPortaDialCooldown = true;
                    SkipToTime_MethodInfo.Invoke(null, [0, !Main.dayTime]);
                    FargoWorld.BlockPortaDialCooldown = false;
                    if (currentlyNight != Main.dayTime && Main.dayTime)
                    {
                        Chest.SetupTravelShop();
                        NetMessage.SendTravelShop(-1);
                    }
                    /*Main.dayTime = !Main.dayTime;
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
                    }*/
                }
                return true;
            }
            return base.UseItem(player);
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

        public override void PostDrawInWorld(WorldItem item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (Main.sundialCooldown == 0)
            {
                Texture2D texture = ModContent.Request<Texture2D>(Texture + "_glow").Value;
                Vector2 cent = item.Bottom - Main.screenPosition - new Vector2(0, (texture.Size() / 2).Y).RotatedBy(rotation);
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