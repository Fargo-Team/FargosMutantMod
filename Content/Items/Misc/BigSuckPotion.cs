using Fargowiltas.Content.Buffs;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
	public class BigSuckPotion : ModItem
	{
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }

        public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Blue;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.consumable = true;
			Item.useTurn = true;

			Item.UseSound = SoundID.Item3;
			Item.value = Item.buyPrice(silver: 10);
		}

        public override bool? UseItem(Player player)
        {
			player.AddBuff(ModContent.BuffType<BigSuckBuff>(), 180);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.Meteorite, 5)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ItemID.Moonglow)
                .AddIngredient(ItemID.Fireblossom)
                .AddTile(TileID.Bottles) 
                .DisableDecraft()
                .Register();
        }
    }
}
