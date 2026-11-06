using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Fargowiltas.Content.Items.Misc;
public class ShimmerDowser : ModItem
{
    public static Asset<Texture2D> Holdout;
    public static Asset<Texture2D> HoldoutGlow;

    public override void SetStaticDefaults()
    {
        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 13));

        if (!Main.dedServ)
        {
            Holdout = ModContent.Request<Texture2D>(Texture + "_Holdout");
            HoldoutGlow = ModContent.Request<Texture2D>(Texture + "_Holdout_Glow");
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

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        bool shimmerExists = ShimmerDowserSystem.SavedShimmerPosition != Vector2.Zero;
        if (shimmerExists)
            return;

        tooltips.Add(new TooltipLine(Mod, "Fargowiltas:NoShimmer", Language.GetTextValue($"Mods.{Mod.Name}.Items.{Name}.NoShimmer")));
    }

    public override bool ModifyItemDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, ref DrawData? coloredDrawData, ref DrawData? glowMaskDrawData)
    {
        var modPlayer = drawInfo.drawPlayer.GetModPlayer<ShimmerDowserPlayer>();

        var texture = Holdout.Value;
        Vector2 origin = texture.Size() * 0.5f;
        Vector2 offset = new(3 * (drawData.effect == SpriteEffects.FlipHorizontally ? -1 : 1), -6);

        origin += Main.rand.NextVector2Circular(2, 2) * modPlayer.ShineOpacity;

        DrawData data = new(texture, drawData.position - offset, null, drawData.color, drawData.rotation, origin, drawData.scale, drawData.effect, 0);
        drawInfo.DrawDataCache.Add(data);

        if (modPlayer.InShimmerBounds || modPlayer.ShineOpacity > 0)
        {
            float angle = float.Pi * 2 * float.Sin(Main.GlobalTimeWrappedHourly);
            Color glowColor = Color.White with { A = 0 } * GetGemGlow(drawInfo.drawPlayer) * 0.5f;

            DrawData glowData = new(HoldoutGlow.Value, drawData.position - offset, null, glowColor, drawData.rotation, origin, drawData.scale, drawData.effect, 0);
            drawInfo.DrawDataCache.Add(glowData);

            int amount = 10;
            double spread = 2 * float.Pi / amount;
            for (int i = 0; i < amount; i++)
            {
                Vector2 rotated = new Vector2(0, 4).RotatedBy(spread * i + angle);
                glowData = new(HoldoutGlow.Value, drawData.position - offset + rotated, null, glowColor, drawData.rotation, origin, drawData.scale, drawData.effect, 0);
                drawInfo.DrawDataCache.Add(glowData);
            }
        }

        return false;
    }

    private float GetGemGlow(Player player)
    {
        float InverseLerp(float a, float b, float t) => float.Max(0, (t - a) / (b - a));

        var modPlayer = player.GetModPlayer<ShimmerDowserPlayer>();
        float position = ShimmerDowserSystem.SavedShimmerPosition.Y;

        float finalPosition = position * 16;
        float startPos = (position - ShimmerDowserSystem.ShimmerSafeRange) * 16;
        float progress = InverseLerp(startPos, finalPosition, player.Center.Y);

        return float.Min(1, progress) * modPlayer.ShineOpacity;
    }
}

public class ShimmerDowserPlayer : ModPlayer
{
    public float ShineOpacity;

    public bool InShimmerBounds
    {
        get
        {
            if (ShimmerDowserSystem.SavedShimmerPosition == Vector2.Zero)
                return false;

            Vector2 position = ShimmerDowserSystem.SavedShimmerPosition * 16;
            Vector2 zoneSize = Vector2.One * ShimmerDowserSystem.ShimmerDetectionZone * 2;
            Rectangle checkZone = new((int)(position.X - zoneSize.X / 2), 0, (int)zoneSize.X, (int)(position.Y + zoneSize.Y / 2));
            return Player.Hitbox.Intersects(checkZone);
        }
    }

    public override void PostUpdate()
    {
        float inc = 0.05f;
        if (!InShimmerBounds && ShineOpacity == 0)
            return;

        ShineOpacity += InShimmerBounds ? inc : -inc;
        ShineOpacity = float.Clamp(ShineOpacity, 0, 1);
    }

}

public class ShimmerDowserSystem : ModSystem
{
    public static Vector2 SavedShimmerPosition => new(shimmerX, shimmerY);
    public static int ShimmerDetectionZone => 1100;
    public static int ShimmerSafeRange => 500;

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

    public override void NetSend(BinaryWriter writer)
    {
        writer.Write7BitEncodedInt(shimmerX);
        writer.Write7BitEncodedInt(shimmerY);
    }

    public override void NetReceive(BinaryReader reader)
    {
        shimmerX = reader.Read7BitEncodedInt();
        shimmerY = reader.Read7BitEncodedInt();
    }

    public override void Load()
    {
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Shimmer"], Detour_Shimmer);
    }

    private void Detour_Shimmer(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
    {
        orig(self, progress, configuration);

        ShimmerDowserSystem.shimmerX = (int)GenVars.shimmerPosition.X;
        ShimmerDowserSystem.shimmerY = (int)GenVars.shimmerPosition.Y;
    }
}