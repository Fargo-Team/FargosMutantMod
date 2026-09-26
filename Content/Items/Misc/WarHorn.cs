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

namespace Fargowiltas.Content.Items.Misc;

public class WarHorn : ModItem
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
        Item.width = 52;
        Item.height = 46;
        Item.value = Item.sellPrice(0, 0, 5);
        Item.rare = ItemRarityID.Pink;
        Item.useAnimation = 30;
        Item.useTime = 30;
        Item.useStyle = ItemUseStyleID.Shoot;
    }

    public override bool AltFunctionUse(Player player) => true;

    public override bool CanRightClick() => true;

    public override bool ConsumeItem(Player player) => false;

    public override void RightClick(Player player)
    {
        FargoPlayer modPlayer = player.FargoMutant();

        if (!modPlayer.WarCry && !modPlayer.PeaceCry)
        {
            ToggleCry2(true, player, ref modPlayer.WarCry);
        }
        else if (modPlayer.WarCry)
        {
            ToggleCry2(true, player, ref modPlayer.WarCry);
            ToggleCry2(false, player, ref modPlayer.PeaceCry);
        }
        else
        {
            ToggleCry2(false, player, ref modPlayer.PeaceCry);
        }

        if (!Main.dedServ)
            SoundEngine.PlaySound(new SoundStyle("Fargowiltas/Assets/Sounds/Horn"), player.Center);
    }

    public static void GenerateText(bool isWar, Player player, bool cry)
    {
        string cryToggled = Language.GetTextValue($"Mods.Fargowiltas.Items.WarHorn.{(isWar ? "War" : "Peace")}");
        string toggle = Language.GetTextValue($"Mods.Fargowiltas.Items.WarHorn.{(cry ? "Activated" : "Deactivated")}");
        string punctuation = Language.GetTextValue($"Mods.Fargowiltas.MessageInfo.Common.{(isWar ? "Exclamation" : "Period")}");

        string text = Language.GetTextValue("Mods.Fargowiltas.Items.WarHorn.CryText2", cryToggled, toggle, player.name, punctuation);
        Color color = isWar ? new Color(255, 0, 0) : new Color(0, 255, 255);

        FargoUtils.PrintText(text, color);
    }

    public static void SyncCry2(Player player)
    {
        if (player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            ModPacket packet = modPlayer.Mod.GetPacket();
            packet.Write((byte)PacketID.SyncWarCry);
            packet.Write(player.whoAmI);
            packet.Write(modPlayer.WarCry);
            packet.Write(modPlayer.PeaceCry);
            packet.Send();
        }
    }

    void ToggleCry2(bool isWar, Player player, ref bool cry)
    {
        cry = !cry;

        if (cry)
        {
            FargoPlayer modPlayer = player.FargoMutant();
            if (modPlayer.BattleCry)
            {
                modPlayer.BattleCry = false;
                DeactivateOtherCry(true, player);
            }
            if (modPlayer.CalmingCry)
            {
                modPlayer.CalmingCry = false;
                DeactivateOtherCry(false, player);
            }
        }

        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            GenerateText(isWar, player, cry);
        }
        else if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)PacketID.BroadcastWarCry);
            packet.Write(isWar);
            packet.Write(player.whoAmI);
            packet.Write(cry);
            packet.Send();
            SyncCry2(player);
        }
    }

    void DeactivateOtherCry(bool isBattle, Player player)
    {
        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            BattleCry.GenerateText(isBattle, player, false);
        }
        else if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)PacketID.BroadcastBattleCry);
            packet.Write(isBattle);
            packet.Write(player.whoAmI);
            packet.Write(false);
            packet.Send();
            BattleCry.SyncCry1(player);
        }
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            FargoPlayer modPlayer = player.FargoMutant();
            if (player.altFunctionUse == 2)
            {
                if (modPlayer.WarCry)
                    ToggleCry2(true, player, ref modPlayer.WarCry);

                ToggleCry2(false, player, ref modPlayer.PeaceCry);
            }
            else
            {
                if (modPlayer.PeaceCry)
                    ToggleCry2(false, player, ref modPlayer.PeaceCry);

                ToggleCry2(true, player, ref modPlayer.WarCry);
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
        Texture2D texture = ModContent.Request<Texture2D>("Fargowiltas/Content/Items/Misc/WarHorn", AssetRequestMode.AsyncLoad).Value;
        ++RealFrameCounter;
        if (player.whoAmI == Main.myPlayer)
        {
            if (modPlayer.PeaceCry)
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
            else if (modPlayer.WarCry)
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
            .AddIngredient<BattleCry>(5)
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.SoulofLight, 5)
            .AddIngredient(ItemID.SoulofNight, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}