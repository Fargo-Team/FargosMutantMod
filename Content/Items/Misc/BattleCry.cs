using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Fargowiltas.Fargowiltas;

namespace Fargowiltas.Content.Items.Misc
{
    public class BattleCry : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        int drawTimer = 0;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 38;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.Pink;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        public override bool AltFunctionUse(Player player) => true;

        public static void GenerateText(bool isBattle, Player player, bool cry)
        {
            string cryToggled = Language.GetTextValue($"Mods.Fargowiltas.Items.BattleCry.{(isBattle ? "Battle" : "Calming")}");
            string toggle = Language.GetTextValue($"Mods.Fargowiltas.Items.BattleCry.{(cry? "Activated" : "Deactivated")}");
            string punctuation = Language.GetTextValue($"Mods.Fargowiltas.MessageInfo.Common.{(isBattle ? "Exclamation" : "Period")}");

            string text = Language.GetTextValue("Mods.Fargowiltas.Items.BattleCry.CryText", cryToggled, toggle, player.name, punctuation);
            Color color = isBattle ? new Color(255, 0, 0) : new Color(0, 255, 255);

            FargoUtils.PrintText(text, color);
        }

        public static void SyncCry(Player player)
        {
            if (player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient)
            {
                FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

                ModPacket packet = modPlayer.Mod.GetPacket();
                packet.Write((byte)PacketID.SyncBattleCry);
                packet.Write(player.whoAmI);
                packet.Write(modPlayer.BattleCry);
                packet.Write(modPlayer.CalmingCry);
                packet.Send();
            }
        }

        void ToggleCry(bool isBattle, Player player, ref bool cry)
        {
            cry = !cry;

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                GenerateText(isBattle, player, cry);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
            {
                var packet = Mod.GetPacket();
                packet.Write((byte)PacketID.BroadcastBattleCry);
                packet.Write(isBattle);
                packet.Write(player.whoAmI);
                packet.Write(cry);
                packet.Send();

                SyncCry(player);
            }
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                FargoPlayer modPlayer = player.FargoMutant();
                if (player.altFunctionUse == 2)
                {
                    if (modPlayer.BattleCry)
                        ToggleCry(true, player, ref modPlayer.BattleCry);

                    ToggleCry(false, player, ref modPlayer.CalmingCry);
                }
                else
                {
                    if (modPlayer.CalmingCry)
                        ToggleCry(false, player, ref modPlayer.CalmingCry);

                    ToggleCry(true, player, ref modPlayer.BattleCry);
                }

            }

            if (!Main.dedServ)
                SoundEngine.PlaySound(new SoundStyle("Fargowiltas/Assets/Sounds/Horn"), player.Center);

            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            FargoPlayer modPlayer = player.FargoMutant();
            float glowscale = (Main.mouseTextColor / 400f - 0.35f) * 0.3f + 0.9f;
            glowscale *= scale;
            float modifier = 0.5f + (float)Math.Sin(drawTimer / 30f) / 3;
            Texture2D texture = ModContent.Request<Texture2D>("Fargowiltas/Content/Items/Misc/BattleCry_Glow").Value;
            if (player.whoAmI == Main.myPlayer)
            {
                if (modPlayer.CalmingCry)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        Vector2 afterimageOffset = (MathHelper.TwoPi * j / 12f).ToRotationVector2() * 2;// + (Vector2.UnitX * 1.8f);
                        Color glowColor = Color.Lerp(Color.SkyBlue, Color.CornflowerBlue, modifier) * 0.5f;
                        Main.EntitySpriteDraw(texture, position + afterimageOffset, frame, glowColor, 0, texture.Size() * 0.55f, glowscale, SpriteEffects.None, 0f);
                    }
                }
                else if (modPlayer.BattleCry)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        Vector2 afterimageOffset = (MathHelper.TwoPi * j / 12f).ToRotationVector2() * 2; // + (Vector2.UnitX * 1.8f);
                        Color glowColor = Color.Lerp(Color.Red, Color.PaleVioletRed, modifier) * 0.5f;
                        Main.EntitySpriteDraw(texture, position + afterimageOffset, frame, glowColor, 0, texture.Size() * 0.55f, glowscale, SpriteEffects.None, 0f);
                    }
                }
            }   drawTimer++;
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BattlePotion, 15)
                .AddIngredient(ItemID.WaterCandle, 5)
                .AddIngredient(ItemID.CalmingPotion, 15)
                .AddIngredient(ItemID.PeaceCandle, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}