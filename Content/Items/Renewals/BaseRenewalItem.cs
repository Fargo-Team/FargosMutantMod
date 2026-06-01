using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Renewals
{
    public abstract class BaseRenewalItem : ModItem
    {
        private readonly string name;
        private readonly string tooltip;
        private readonly int material;
        private readonly bool supreme;
        private readonly int supremeMaterial;

        protected BaseRenewalItem(string name, string tooltip, int material, bool supreme = false, int supremeMaterial = -1)
        {
            this.name = name;
            this.tooltip = tooltip;
            this.material = material;
            this.supreme = supreme;
            this.supremeMaterial = supremeMaterial;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = supreme ? ItemRarityID.Lime : ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            var recipe = Recipe.Create(Type);

            if (supreme)
            {
                recipe.AddIngredient(supremeMaterial, 10);
                recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
                recipe.AddTile(TileID.AlchemyTable);
            }
            else
            {
                recipe.AddIngredient(ItemID.Bottle);
                recipe.AddIngredient(material, 100);
                recipe.AddTile(TileID.Bottles);
            }

            recipe.Register();
        }
    }
}
