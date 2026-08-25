using Fargowiltas.Content.NPCs;
using Fargowiltas.Content.NPCs.AbominationnNPC;
using Fargowiltas.TileEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles;

public class SiblingPylonTile : ModPylon
{
    public const int CrystalVerticalFrameCount = 8;

    public Asset<Texture2D> crystalTexture;
    public Asset<Texture2D> crystalHighlightTexture;
    public Asset<Texture2D> mapIcon;

    public override void Load()
    {
        // We'll need these textures for later, it's best practice to cache them on load instead of continually requesting every draw call.
        crystalTexture = ModContent.Request<Texture2D>(Texture + "_Crystal");
        crystalHighlightTexture = ModContent.Request<Texture2D>(Texture + "_CrystalHighlight");
        mapIcon = ModContent.Request<Texture2D>(Texture + "_MapIcon");
    }

    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.StyleHorizontal = true;
        TEModdedPylon moddedPylon = ModContent.GetInstance<SiblingPylonTileEntity>();
        TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(moddedPylon.PlacementPreviewHook_CheckIfCanPlace, 1, 0, true);
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(moddedPylon.Hook_AfterPlacement, -1, 0, false);

        TileObjectData.addTile(Type);

        TileID.Sets.InteractibleByNPCs[Type] = true;
        TileID.Sets.PreventsSandfall[Type] = true;

        // Adds functionality for proximity of pylons; if this is true, then being near this tile will count as being near a pylon for the teleportation process.
        AddToArray(ref TileID.Sets.CountsAsPylon);

        LocalizedText pylonName = CreateMapEntryName(); //Name is in the localization file
        AddMapEntry(Color.White, pylonName);
    }

    public override NPCShop.Entry GetNPCShopEntry()
    {
        return null;
    }

    //public override NPCShop.Entry GetNPCShopEntry()/* tModPorter See ExamplePylonTile for an example. To register to specific NPC shops, use the new shop system directly in ModNPC.AddShop, GlobalNPC.ModifyShop or ModSystem.PostAddRecipes */
    /*{
			
			return Condition.HappyEnough && (npcType == ModContent.NPCType<Mutant>() || npcType == ModContent.NPCType<Abominationn>() || npcType == ModContent.NPCType<Deviantt>())
				&& NPC.AnyNPCs(ModContent.NPCType<Mutant>())
				&& NPC.AnyNPCs(ModContent.NPCType<Abominationn>())
				&& NPC.AnyNPCs(ModContent.NPCType<Deviantt>())
				? ModContent.ItemType<SiblingPylon>()
				: null;
			
		}
		*/
    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

    public override bool RightClick(int i, int j)
    {
        //Main.mapFullscreen = true;
        //SoundEngine.PlaySound(SoundID.MenuOpen);
        Main.LocalPlayer.TryOpeningFullscreenMap();
        return true;
    }

    public override void MouseOver(int i, int j)
    {
        Main.LocalPlayer.cursorItemIconEnabled = true;
        Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<SiblingPylon>();
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        ModContent.GetInstance<SiblingPylonTileEntity>().Kill(i, j);

        //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 3, 4, ModContent.ItemType<SiblingPylon>());
    }

    bool NearNPC(Vector2 tilePos, int npcType)
        => Main.npc.Any(n => n.active && n.type == npcType && n.Distance(tilePos) < 1000);

    bool NearEnoughSiblings(Vector2 tilePos)
    {
        int siblingsNearby = 0;
        if (NearNPC(tilePos, ModContent.NPCType<Mutant>()))
            siblingsNearby++;
        if (NearNPC(tilePos, ModContent.NPCType<Abominationn>()))
            siblingsNearby++;
        if (NearNPC(tilePos, ModContent.NPCType<Deviantt>()))
            siblingsNearby++;
        return siblingsNearby >= 2;
    }

    public override void ValidTeleportCheck_DestinationPostCheck(TeleportPylonInfo destinationPylonInfo, ref bool destinationPylonValid, ref string errorKey)
    {
        Vector2 tilePos = destinationPylonInfo.PositionInTiles.ToWorldCoordinates();
        if (!NearEnoughSiblings(tilePos))
        {
            destinationPylonValid = false;
            errorKey = "Mods.Fargowiltas.MessageInfo.SiblingPylonNotNearSiblings";
        }
    }

    public override void ValidTeleportCheck_NearbyPostCheck(TeleportPylonInfo nearbyPylonInfo, ref bool destinationPylonValid, ref bool anyNearbyValidPylon, ref string errorKey)
    {
        Vector2 tilePos = nearbyPylonInfo.PositionInTiles.ToWorldCoordinates();
        if (!NearEnoughSiblings(tilePos))
        {
            destinationPylonValid = false;
            errorKey = "Mods.Fargowiltas.MessageInfo.NearbySiblingPylonNotNearSiblings";
        }
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        r = 51f / 255f * 0.75f;
        g = 255f / 255f * 0.75f;
        b = 191f / 255f * 0.75f;
    }
    public override bool AutoSelect(int i, int j, Item item)
    {
        return base.AutoSelect(i, j, item);
    }
    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        DefaultDrawPylonCrystal(spriteBatch, i, j, crystalTexture, crystalHighlightTexture, Vector2.UnitX * -1 + Vector2.UnitY * -12, Color.White * 0.1f, Color.White, 4, CrystalVerticalFrameCount);
    }

    public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
    {
        bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
        DefaultMapClickHandle(mouseOver, pylonInfo, "Mods.Fargowiltas.Items.SiblingPylon.DisplayName", ref mouseOverText);
    }
}