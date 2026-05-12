using Fargowiltas.Content.Items.Misc;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Fargowiltas.Content.Items.Tiles
{
    public class Semistation : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.createTile = ModContent.TileType<SemistationSheet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(5)
                .AddIngredient(ItemID.Sunflower, 5)
                .AddIngredient(ItemID.Campfire, 5)
                .AddIngredient(ItemID.HeartLantern, 5)
                .AddIngredient(ItemID.StarinaBottle, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class SemistationSheet : ModTile
    {
        public virtual Color color => new Color(221, 85, 125);

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16];
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(color, name);
        }

        public override bool CanDrop(int i, int j) => false;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!closer)
            {
                if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.Semistation>(), 30);
            }
        }
    }
}