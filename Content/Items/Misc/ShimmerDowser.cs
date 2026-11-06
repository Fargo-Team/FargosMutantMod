using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Fargowiltas.Content.Items.Misc;
public class ShimmerDowser : ModItem
{
    public static Asset<Texture2D> HoldoutTexture;
    public bool InBounds { get; private set; }

    public override void SetStaticDefaults()
    {
        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 13));

        if (!Main.dedServ)
        {
            HoldoutTexture = ModContent.Request<Texture2D>(Texture + "_Holdout");
        }
    }

    public override void SetDefaults()
    {
        Item.width = 44;
        Item.height = 42;

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.buyPrice(gold: 2);
        Item.holdStyle = ItemHoldStyleID.HoldFront;
    }

    public override void HoldItem(Player player)
    {
        if (ShimmerDowserSystem.SavedShimmerPosition == Vector2.Zero) // fuck it
            return;

        Vector2 position = ShimmerDowserSystem.SavedShimmerPosition * 16;
        Vector2 zoneSize = Vector2.One * ShimmerDowserSystem.ShimmerDetectionSafeRange * 2;
        Rectangle checkZone = new((int)(position.X - zoneSize.X / 2), 0, (int)zoneSize.X, Main.UnderworldLayer * 16);
        bool inBounds = player.Hitbox.Intersects(checkZone);

        InBounds = inBounds;
        Main.NewText("above shimmer: " + InBounds);
    }

    public override bool ModifyItemDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, ref DrawData? coloredDrawData, ref DrawData? glowMaskDrawData)
    {
        var texture = HoldoutTexture.Value;
        Vector2 origin = texture.Size() * 0.5f;

        origin += new Vector2(3 * (drawData.effect == SpriteEffects.FlipHorizontally ? -1 : 1), -6);

        Main.NewText(InBounds);
        if (drawInfo.heldItem.ModItem is ShimmerDowser dowser && dowser.InBounds)
        {
            Main.NewText("dowser: " + dowser.InBounds);
            origin += Main.rand.NextVector2Circular(1.25f, 1.25f);
        }

        DrawData data = new(texture, drawData.position, null, drawData.color, drawData.rotation, origin, drawData.scale, drawData.effect, 0);
        drawInfo.DrawDataCache.Add(data);

        return false;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<GizmoParts>(), 3)
            .AddRecipeGroup("Fargowiltas:AnyPearl")
            .AddRecipeGroup(RecipeGroupID.Wood, 5)
            .AddRecipeGroup(RecipeGroupID.IronBar, 2)
            .AddTile(TileID.DemonAltar)
            .Register();
    }
}

public class ShimmerDowserSystem : ModSystem
{
    public static Vector2 SavedShimmerPosition => new(shimmerX, shimmerY);
    public static int ShimmerDetectionSafeRange => 1100;

    private static int shimmerX;
    private static int shimmerY;

    public override void SaveWorldData(TagCompound tag)
    {
        tag.Add("shimmerX", shimmerX);
        tag.Add("shimmerY", shimmerY);
    }

    public override void LoadWorldData(TagCompound tag)
    {
        shimmerX = tag.GetInt("shimmerX");
        shimmerY = tag.GetInt("shimmerY");
    }

    public override void Load()
    {
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Shimmer"], Detour_Shimmer);
    }

    public override void AddRecipeGroups()
    {
        RecipeGroup.RegisterGroup("Fargowiltas:AnyPearl", new RecipeGroup(() => "Any Pearl" /* just work */, [ItemID.WhitePearl, ItemID.BlackPearl, ItemID.PinkPearl]));
    }

    private void Detour_Shimmer(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
    {
        orig(self, progress, configuration);

        ShimmerDowserSystem.shimmerX = (int)GenVars.shimmerPosition.X;
        ShimmerDowserSystem.shimmerY = (int)GenVars.shimmerPosition.Y;
    }
}