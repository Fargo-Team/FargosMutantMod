using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
	public class InstantResearch : ModItem
	{
		public override string Texture => "Fargowiltas/Content/Items/Placeholder";

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 1;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
		}

		public override bool? UseItem(Player player)
		{
			if (player.itemAnimation > 0 && player.itemTime == 0)
			{
				int count = 0;

				for (int i = 0; i < ItemLoader.ItemCount; i++)
				{
					if (CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(i, out int amountNeeded))
					{
						int diff = amountNeeded - player.creativeTracker.ItemSacrifices.GetSacrificeCount(i);
						if (diff > 0)
						{
							player.creativeTracker.ItemSacrifices.RegisterItemSacrifice(i, diff);
							count++;
						}
					}
				}

				FargoUtils.PrintLocalization("Items.InstantResearch.ResearchText", count);
			}

			return true;
		}
	}
}
