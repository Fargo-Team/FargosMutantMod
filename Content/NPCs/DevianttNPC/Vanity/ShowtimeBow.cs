using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.NPCs.DevianttNPC.Vanity;

[AutoloadEquip(EquipType.Back)]
[LegacyName("DevianttPants")]
public class ShowtimeBow : ModItem
{
    public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
    {
        Rectangle rect = new(0, 0, 56, 56);
        Vector2 origin = rect.Size() / 2f;

        Player player = drawInfo.drawPlayer;
        float yOffset = Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / player.bodyFrame.Height].Y;
        Vector2 offset = new(14 * -player.direction, (-4 + yOffset) * player.gravDir);
        drawInfo.DrawDataCache.Add(drawData with { position = drawData.position + offset, sourceRect = rect, origin = origin });
        return false;
    }
    public override void SetDefaults()
    {
        Item.width = 50;
        Item.height = 40;
        Item.rare = ItemRarityID.Blue;
        Item.vanity = true;
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 8)
            .AddIngredient(ItemID.ChumBucket, 2)
            .AddTile(TileID.Loom)
            .Register();
    }
}

/*
public class DevianttBowDrawLayer : PlayerDrawLayer
{
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.shadow == 0 && drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, "DevianttBow", EquipType.Back);

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player player = drawInfo.drawPlayer;
        Texture2D texture = ModContent.Request<Texture2D>("Fargowiltas/Content/Items/Vanity/DevianttBow_DrawLayer").Value;
        Vector2 position = drawInfo.bodyVect + drawInfo.Position - Vector2.UnitX * (float)(drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2);

        position += new Vector2(player.direction == -1 ? 2 : -2, player.gravDir == 1 ? -6 : 3) +
            Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / player.bodyFrame.Height] * player.gravDir;

        Vector2 offset = new(player.direction > 0 ? -11.5f : -player.width - 7.5f, 6);
        position -= offset;

        Color color = Lighting.GetColor(position.ToTileCoordinates());

        var data = new DrawData(texture, position - Main.screenPosition, null, color, player.headRotation, texture.Size() * 0.5f, 1, drawInfo.playerEffect, 0);
        drawInfo.DrawDataCache.Add(data);
    }
}
*/