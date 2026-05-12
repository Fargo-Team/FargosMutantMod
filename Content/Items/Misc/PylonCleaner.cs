using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
	public class PylonCleaner : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 20;
			Item.useTime = 20;
		}

		public override bool? UseItem(Player player)
		{
			if (player.itemAnimation > 0 && player.itemTime == 0 && player.whoAmI == Main.myPlayer)
			{
				//iterate to every pylon, drop a purity renewal
				foreach (TeleportPylonInfo pylonInfo in Main.PylonSystem.Pylons)
                {
				  	Vector2 pos = pylonInfo.PositionInTiles.ToWorldCoordinates();
					
					int projType = ModContent.ProjectileType<PurityNukeProj>();
					if (pylonInfo.TypeOfPylon == TeleportPylonType.Hallow)
						projType = ModContent.ProjectileType<HallowNukeProj>();

					int p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, Vector2.Zero, projType, 0, 0f, Main.myPlayer);
					if (p != Main.maxProjectiles)
						Main.projectile[p].timeLeft = 2;

					if (pylonInfo.TypeOfPylon == TeleportPylonType.GlowingMushroom)
                    {
						projType = ModContent.ProjectileType<MushroomNukeProj>();
						p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, Vector2.Zero, projType, 0, 0f, Main.myPlayer);
						if (p != Main.maxProjectiles)
							Main.projectile[p].timeLeft = 6;
					}
				}
			}

			return true;
		}

        public override void AddRecipes()
        {
			CreateRecipe()
                .AddIngredient<GizmoParts>(5)
                .AddIngredient(ItemID.PurificationPowder, 100)
                .AddIngredient(ItemID.HolyWater, 10)
				.AddTile(TileID.Bottles)
				.Register();
        }
    }
}
