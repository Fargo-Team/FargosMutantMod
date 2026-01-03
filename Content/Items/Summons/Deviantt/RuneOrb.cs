using Fargowiltas.Common.Systems.Recipes;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class RuneOrb : BaseSummon
    {
        public override int NPCType => NPCID.RuneWizard;
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 6; // Places it right after Gelatin Crystal
		}

        public override bool CanUseItem(Player player)
        {
            return FargoUtils.ActuallyNight || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                  /*.AddIngredient(ItemID.WizardHat)
                  .AddRecipeGroup(RecipeGroups.AnyGemRobe)
                  .AddIngredient(ItemID.Bone, 10)
                  .AddTile(TileID.MythrilAnvil)*/
                  .AddIngredient(ItemID.LargeAmber)
                  .AddIngredient(ItemID.SoulofLight, 8)
                  .AddIngredient(ItemID.SoulofNight, 8)
                  .AddTile(TileID.CrystalBall)
                  .Register();
        }
    }
}