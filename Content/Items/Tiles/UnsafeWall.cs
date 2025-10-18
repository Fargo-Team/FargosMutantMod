using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Fargowiltas.Content.Items.Tiles
{
    public abstract class UnsafeWall : ModItem
    {
        private readonly string name;
        private readonly int createWall;
        private readonly int wall;
        private readonly int tile;

        protected UnsafeWall(string name, int createWall, int wall = -1, int tile = -1)
        {
            this.name = name;
            this.createWall = createWall;
            this.wall = wall;
            this.tile = tile;
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = wall;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(createWall);
        }
    }

    public class UnsafeMarbleWall : UnsafeWall
    {
        public UnsafeMarbleWall() : base("Unsafe Marble Wall", WallID.MarbleUnsafe, ItemID.MarbleWall, ItemID.Marble) { }
    }

    public class UnsafeGraniteWall : UnsafeWall
    {
        public UnsafeGraniteWall() : base("Unsafe Granite Wall", WallID.GraniteUnsafe, ItemID.GraniteWall, ItemID.Granite) { }
    }
}