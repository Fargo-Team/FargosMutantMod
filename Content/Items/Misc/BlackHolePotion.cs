using Fargowiltas.Content.Buffs;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Fargowiltas.Content.Items.Misc
{
    [LegacyName("BigSuckPotion")]
	public class BlackHolePotion : ModItem
	{
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;

            ItemID.Sets.DrinkParticleColors[Type] = [
                Color.Orange,
                Color.Black
            ];
        }

        public override void SetDefaults()
		{
            Item.DefaultToFood(14, 24, ModContent.BuffType<BigSuckBuff>(), 60 * 10, true);
		}

        public override bool? UseItem(Player player)
        {
			//player.AddBuff(ModContent.BuffType<BigSuckBuff>(), 180);
            return base.UseItem(player);
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
