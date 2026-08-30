using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Vanity;

[AutoloadEquip(EquipType.Head)]
public class DevianttMask : ModItem
{
    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.rare = ItemRarityID.Blue;
        Item.vanity = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.RuneHat)
            .AddIngredient(ItemID.MetalDetector)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}

public class DevianttMaskDrawLayer : PlayerDrawLayer
{
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.shadow == 0 && drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "DevianttMask", EquipType.Head);

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player player = drawInfo.drawPlayer;
        Texture2D texture = ModContent.Request<Texture2D>("Fargowiltas/Content/Items/Vanity/DevianttMask_Head_DrawLayer").Value;

        // what the fuck is this
        Vector2 position = drawInfo.helmetOffset +
            new Vector2((int)(drawInfo.Position.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) +
            (float)(drawInfo.drawPlayer.width / 2)),
            (int)(drawInfo.Position.Y +
            (float)drawInfo.drawPlayer.height -
            (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) +
            drawInfo.drawPlayer.headPosition +
            drawInfo.headVect +
            new Vector2(player.direction == -1 ? 2 : -2, player.gravDir == 1 ? -6 : 5) +
            Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / player.bodyFrame.Height] * player.gravDir;

        position.Y += 5.5f;
        position.X -= 2 * player.direction;

        Color color = Lighting.GetColor(position.ToTileCoordinates());

        var data = new DrawData(texture, position - Main.screenPosition, null, color, player.headRotation, texture.Size() * 0.5f, 1, drawInfo.playerEffect);
        data.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(data);
        return;
    }
}