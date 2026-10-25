using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.NPCs.DevianttNPC.Vanity;

[AutoloadEquip(EquipType.Head)]
[LegacyName("DevianttMask")]
public class MatthewVanity : ModItem
{
    public static int EquipSlotID;
    public override void OnCreated(ItemCreationContext context)
    {
        if (context is InitializationItemCreationContext)
        {
            EquipSlotID = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        }
    }
    public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
    {
        Rectangle hatRect = new(0, 0, 48, 64);
        Vector2 origin = hatRect.Size() / 2f;

        Player player = drawInfo.drawPlayer;
        float yOffset = Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / player.bodyFrame.Height].Y * player.gravDir;
        Vector2 offset = new(4 * -player.direction, (player.gravDir == -1 ? 11 : 0) + yOffset);
        drawInfo.DrawDataCache.Add(drawData with { position = drawData.position + offset, sourceRect = hatRect, origin = origin });
        return false;
    }
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
public class MatthewWorld : ModSystem
{
    internal static bool IsSomeoneWearingMatthew;
    public override void PreUpdatePlayers()
    {
        IsSomeoneWearingMatthew = false;
    }
}
public class MatthewPlayer : ModPlayer
{
    public override void PreUpdate()
    {
        if (!Player.dead && !Player.ghost)
        {
            int id = MatthewVanity.EquipSlotID;
            if (Player.head == id || Player.armor[10].headSlot == id)
            {
                MatthewWorld.IsSomeoneWearingMatthew = true;
            }
        }
    }
}