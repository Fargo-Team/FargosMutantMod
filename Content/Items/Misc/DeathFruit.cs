using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
	public class DeathFruit : ModItem
	{
        SoundStyle DeathFruitSound = new SoundStyle("Fargowiltas/Assets/Sounds/DeathFruit");
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Death Fruit");
			// Tooltip.SetDefault("Permanently decreases maximum life by 20\nEffects may not be reversible for characters that have used modded life-increasing items");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Blue;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.consumable = true;

			Item.UseSound = SoundID.Item27;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LifeFruit)
                .AddCondition(Condition.NearShimmer) 
                .Register();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
		{
            if (!CanUse(player) && player.altFunctionUse != 2)
            {
                return false;
            }
            if (!CanUse(player, true) && player.altFunctionUse == 2)
            {
                return false;
            }
			return true;
		}
        public override void HoldItem(Player player)
        {
            if (player.ConsumedLifeCrystals > 0)
            {
                Item.UseSound = DeathFruitSound;
            }
            else
            {
                Item.UseSound = SoundID.Item27;
            }
        }
        public override bool? UseItem(Player player)
		{
            if (player.ConsumedLifeFruit > 0)
            {
                if (player.altFunctionUse != 2)
                {
                    player.ConsumedLifeFruit--;
                }
                else
                    player.ConsumedLifeFruit = 0;
			}
            else if (player.ConsumedLifeCrystals > 0)
            {
                if (player.altFunctionUse != 2)
                {
                    player.ConsumedLifeCrystals--;
                }
            }
            else
            {
                int effect;
                if (player.altFunctionUse == 2)
                {
                    if (CanUse(player, true))
                    {
                        effect = 20;

                        if (player.GetModPlayer<FargoPlayer>().DeathFruitHealth < 20) //this should never occur but just to be sure
                        {
                            effect = player.GetModPlayer<FargoPlayer>().DeathFruitHealth;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (CanUse(player))
                    {
                        effect = -20;
                    }
                    else
                    {
                        return false;
                    }
                }
                player.GetModPlayer<FargoPlayer>().DeathFruitHealth -= effect;
                if (player.statLife > -effect) //don't decrease health under 0
                {
                    player.statLife += effect;
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.HealEffect(effect, true);
                    }
                }
            }
			return true;
		}

        private bool CanUse(Player player, bool rightClick = false)
        {
            if (!rightClick && GetLife(player) > 20)
            {
                return true;
            }
            if (rightClick && (player.GetModPlayer<FargoPlayer>().DeathFruitHealth > 0 || player.ConsumedLifeFruit > 0))
            {
                return true;
            }
            return false;
        }
        private int GetLife(Player player) => player.statLifeMax - player.ConsumedLifeFruit * 5;
    }
}
