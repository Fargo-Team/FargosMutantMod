using Fargowiltas.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc;

public class HerbFertilizer : ModItem
{
    public override string Texture => "Fargowiltas/Content/Items/Placeholder";
    public static List<int> ValidItems = [ItemID.Vertebrae, ItemID.RottenChunk, ItemID.JungleSpores, ItemID.AntlionMandible, ItemID.SharkFin];
    public override void SetStaticDefaults()
    {
        foreach (int i in ValidItems)
            ItemID.Sets.ExtractinatorMode[i] = Type;
    }
    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 16;

        Item.maxStack = 9999;
        Item.consumable = true;

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 15;
        Item.useAnimation = 15;
        Item.autoReuse = true;

        Item.noMelee = true;
        Item.noUseGraphic = true;

        Item.shoot = ModContent.ProjectileType<HerbFertilizerProjectile>();
        Item.shootSpeed = 8f;

        Item.value = Item.sellPrice(copper: 10);
        Item.rare = ItemRarityID.Green;

        Item.UseSound = SoundID.Item1;
    }
    public override void AddRecipes()
    {
        foreach (int i in ValidItems)
            RecipeHelper.CreateSimpleRecipe(i, Type, TileID.Extractinator, 1, 5, true, false);
    }
    public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
    {
        resultType = Type;
        resultStack = 5;
    }
}

public class HerbFertilizerProjectile : ModProjectile
{
    public override string Texture => "Fargowiltas/Content/Projectiles/Explosion";
    public override void SetDefaults()
    {
        Projectile.width = 32;
        Projectile.height = 32;

        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 20;
    }
    public override void AI()
    {
        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass);
        Fertilize(Projectile.Center.ToTileCoordinates());
    }

    private static void Fertilize(Point position)
    {
        if (!WorldGen.InWorld(position.X, position.Y, 1))
            return;

        for (int x = position.X - 1; x <= position.X + 1; x++)
        {
            for (int y = position.Y - 1; y <= position.Y + 1; y++)
            {
                if (!WorldGen.InWorld(x, y, 1))
                    continue;

                Tile tile = Main.tile[x, y];

                if (!tile.HasTile)
                    continue;

                if (!Main.tileAlch[tile.TileType])
                    continue;

                for (int i = 0; i < 100; i++)
                    WorldGen.GrowAlch(x, y);

                WorldGen.SquareTileFrame(x, y);

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, x, y, 3);
            }
        }
    }
}

