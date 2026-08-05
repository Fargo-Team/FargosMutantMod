using Fargowiltas.Common;
using Fargowiltas.Common.Systems.Recipes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Fargowiltas.Fargowiltas;

namespace Fargowiltas.Content.Items.Misc
{
    public class BattleCry : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Done this way to spoof Item visual scale in the inventory and world.
            // Ticks per second is set to 1 to prevent weird divide by zero error. ¯\_(ツ)_/¯
            DrawAnimationVertical drawAnim = new DrawAnimationVertical(1, 11);
            drawAnim.NotActuallyAnimating = true;

            Main.RegisterItemAnimation(Type, drawAnim);
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 48;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        public override bool AltFunctionUse(Player player) => true;

        public static void GenerateText(bool isBattle, Player player, bool cry)
        {
            string cryToggled = Language.GetTextValue($"Mods.Fargowiltas.Items.BattleCry.{(isBattle ? "Battle" : "Calming")}");
            string toggle = Language.GetTextValue($"Mods.Fargowiltas.Items.BattleCry.{(cry ? "Activated" : "Deactivated")}");
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

        int RealFrame;
        int RealFrameCounter;

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            FargoPlayer modPlayer = player.FargoMutant();
            Texture2D texture = ModContent.Request<Texture2D>("Fargowiltas/Content/Items/Misc/BattleCry", AssetRequestMode.AsyncLoad).Value;
            ++RealFrameCounter;
            if (player.whoAmI == Main.myPlayer)
            {
                if (modPlayer.CalmingCry)
                {
                    if (RealFrame <= 5)
                        RealFrame = 6;
                    if (RealFrameCounter >= 7)
                    {
                        RealFrameCounter = 0;
                        if (++RealFrame > 10)
                            RealFrame = 6;
                    }

                }
                else if (modPlayer.BattleCry)
                {
                    if (RealFrame <= 0)
                        RealFrame = 1;
                    if (RealFrameCounter >= 7)
                    {
                        RealFrameCounter = 0;
                        if (++RealFrame > 5)
                            RealFrame = 1;
                    }
                }
                else
                    RealFrame = 0;
            }
            frame.Y = 48 * RealFrame;
            spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(5)
                .AddRecipeGroup(FargoRecipeGroups.AnyEvilBar, 5)
                .AddIngredient(ItemID.BattlePotion, 5)
                .AddIngredient(ItemID.WaterCandle, 3)
                .AddIngredient(ItemID.CalmingPotion, 5)
                .AddIngredient(ItemID.PeaceCandle, 3)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}