using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items;
using Fargowiltas.Content.Items.Summons;
using Fargowiltas.Content.Items.Summons.Mutant;
using Fargowiltas.Content.Items.Summons.VanillaCopy;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Fargowiltas.FargoSets;

namespace Fargowiltas.Common.Systems.Recipes
{
    public class MiscRecipeSystem : ModSystem
    {

        public override void AddRecipes()
        {
            if (!FargoServerConfig.Instance.MiscRecipes)
                return;
            AddStatueRecipes();
            AddMiscRecipes();
        }
        public override void PostAddRecipes()
        {
            if (FargoServerConfig.Instance.MiscRecipes)
            {
                foreach (Recipe recipe in Main.recipe.Where(recipe => recipe.HasIngredient(ItemID.BeetleHusk)))
                {
                    if (recipe.TryGetIngredient(ItemID.TurtleHelmet, out Item head))
                    {
                        recipe.RemoveIngredient(head);
                        recipe.AddIngredient(ItemID.ChlorophyteMask);
                    }

                    if (recipe.TryGetIngredient(ItemID.TurtleScaleMail, out Item body))
                    {
                        recipe.RemoveIngredient(body);
                        recipe.AddIngredient(ItemID.ChlorophytePlateMail);
                    }

                    if (recipe.TryGetIngredient(ItemID.TurtleLeggings, out Item legs))
                    {
                        recipe.RemoveIngredient(legs);
                        recipe.AddIngredient(ItemID.ChlorophyteGreaves);
                    }
                }
            }

            //disable shimmer decraft for all summon items
            foreach (Recipe recipe in Main.recipe.Where(recipe => recipe.createItem.ModItem != null && (recipe.createItem.ModItem is BaseSummon || recipe.createItem.ModItem is FleshyDoll)))
            {
                recipe.DisableDecraft();
            }

            // animate recipe groups
            foreach (Recipe recipe in Main.recipe)
            {
                foreach (int groupID in recipe.acceptedGroups)
                {
                    var groupItems = RecipeGroup.recipeGroups[groupID].ValidItems.ToList();
                    foreach (Item item in recipe.requiredItem)
                    {
                        if (RecipeGroup.recipeGroups[groupID].IconicItemId == item.type)
                        {
                            // add tag that it should animate draw
                            item.GetGlobalItem<FargoGlobalItem>().RecipeGroupAnimationItems = groupItems;
                        }
                    }
                }
            }
            
        }
        public override void PostSetupRecipes()
        {
            //finding items with duplicatable items sold
            foreach (Recipe recipe in Main.recipe.Where(recipe => EnchantedTreeTileEntity.IsItemDupable(recipe.createItem.type)))
            {
                int result = recipe.createItem.type;
                if (!EnchantedTreeTileEntity.DuplicatableRecipes.ContainsKey(result))
                {
                    EnchantedTreeTileEntity.DuplicatableRecipes.Add(result, []);
                }
                foreach (Item item in recipe.requiredItem)
                {
                    //is directly dupable or the materials of the current recipe result are dupable
                    if ((EnchantedTreeTileEntity.IsItemDupable(item.type) || EnchantedTreeTileEntity.DupableMaterials.Contains(result) ||
                        (recipe.createItem.ModItem != null && EnchantedTreeTileEntity.DupableMaterialsModded.Contains((recipe.createItem.ModItem.Mod.Name, recipe.createItem.ModItem.Name)))) &&
                        !(item.ModItem != null && EnchantedTreeTileEntity.DontDupeModded.Contains((item.ModItem.Mod.Name, item.ModItem.Name))))
                    {
                        EnchantedTreeTileEntity.DuplicatableRecipes[recipe.createItem.type].Add(item.type);
                    }
                }
            }
            //ts item sucks
            EnchantedTreeTileEntity.DuplicatableRecipes.Add(ItemID.Shellphone, [ItemID.CellPhone]);
            EnchantedTreeTileEntity.DuplicatableRecipes.Add(ItemID.ShellphoneHell, [ItemID.CellPhone]);
            EnchantedTreeTileEntity.DuplicatableRecipes.Add(ItemID.ShellphoneOcean, [ItemID.CellPhone]);
            EnchantedTreeTileEntity.DuplicatableRecipes.Add(ItemID.ShellphoneSpawn, [ItemID.CellPhone]);
        }

        private static void AddStatueRecipes()
        {
            // Functional
            AddStatueRecipe(ItemID.BatStatue, ItemID.BatBanner);
            AddStatueRecipe(ItemID.ChestStatue, ItemID.MimicBanner);
            AddStatueRecipe(ItemID.CrabStatue, ItemID.CrabBanner);
            AddStatueRecipe(ItemID.JellyfishStatue, ItemID.JellyfishBanner);
            AddStatueRecipe(ItemID.PiranhaStatue, ItemID.PiranhaBanner);
            AddStatueRecipe(ItemID.SharkStatue, ItemID.SharkBanner);
            AddStatueRecipe(ItemID.SkeletonStatue, ItemID.SkeletonBanner);
            AddStatueRecipe(ItemID.BoneSkeletonStatue, ItemID.SkeletonBanner);
            AddStatueRecipe(ItemID.SlimeStatue, ItemID.SlimeBanner);
            AddStatueRecipe(ItemID.WallCreeperStatue, ItemID.SpiderBanner);
            AddStatueRecipe(ItemID.UnicornStatue, ItemID.UnicornBanner);
            AddStatueRecipe(ItemID.DripplerStatue, ItemID.DripplerBanner);
            AddStatueRecipe(ItemID.WraithStatue, ItemID.WraithBanner);
            AddStatueRecipe(ItemID.UndeadVikingStatue, ItemID.UndeadVikingBanner);
            AddStatueRecipe(ItemID.MedusaStatue, ItemID.MedusaBanner);
            AddStatueRecipe(ItemID.HarpyStatue, ItemID.HarpyBanner);
            AddStatueRecipe(ItemID.PigronStatue, ItemID.PigronBanner);
            AddStatueRecipe(ItemID.HopliteStatue, ItemID.GreekSkeletonBanner);
            AddStatueRecipe(ItemID.GraniteGolemStatue, ItemID.GraniteGolemBanner);
            AddStatueRecipe(ItemID.BloodZombieStatue, ItemID.BloodZombieBanner);
            AddStatueRecipe(ItemID.BombStatue, ItemID.Bomb, 99);
            AddStatueRecipe(ItemID.HeartStatue, ItemID.LifeCrystal, 6);
            AddStatueRecipe(ItemID.StarStatue, ItemID.ManaCrystal, 6);
            AddStatueRecipe(ItemID.ZombieArmStatue, ItemID.ZombieBanner);
            AddStatueRecipe(ItemID.CorruptStatue, ItemID.EaterofSoulsBanner);
            AddStatueRecipe(ItemID.EyeballStatue, ItemID.DemonEyeBanner);
            AddStatueRecipe(ItemID.GoblinStatue, ItemID.GoblinPeonBanner);
            AddStatueRecipe(ItemID.HornetStatue, ItemID.HornetBanner);
            AddStatueRecipe(ItemID.ImpStatue, ItemID.FireImpBanner);

            // Non-functional
            AddStatueRecipe(ItemID.ShieldStatue);
            AddStatueRecipe(ItemID.AnvilStatue);
            AddStatueRecipe(ItemID.AxeStatue);
            AddStatueRecipe(ItemID.BoomerangStatue);
            AddStatueRecipe(ItemID.BootStatue);
            AddStatueRecipe(ItemID.BowStatue);
            AddStatueRecipe(ItemID.HammerStatue);
            AddStatueRecipe(ItemID.PickaxeStatue);
            AddStatueRecipe(ItemID.SpearStatue);
            AddStatueRecipe(ItemID.SunflowerStatue);
            AddStatueRecipe(ItemID.SwordStatue);
            AddStatueRecipe(ItemID.PotionStatue);
            AddStatueRecipe(ItemID.AngelStatue);
            AddStatueRecipe(ItemID.CrossStatue);
            AddStatueRecipe(ItemID.GargoyleStatue);
            AddStatueRecipe(ItemID.GloomStatue);
            AddStatueRecipe(ItemID.PillarStatue);
            AddStatueRecipe(ItemID.PotStatue);
            AddStatueRecipe(ItemID.ReaperStatue);
            AddStatueRecipe(ItemID.WomanStatue);
            AddStatueRecipe(ItemID.TreeStatue);

            // Lihzahrd
            AddStatueRecipe(ItemID.LihzahrdGuardianStatue, ItemID.LihzahrdBanner, isLihzahrdStatue: true);
            AddStatueRecipe(ItemID.LihzahrdStatue, ItemID.LihzahrdBanner, isLihzahrdStatue: true);
            AddStatueRecipe(ItemID.LihzahrdWatcherStatue, ItemID.LihzahrdBanner, isLihzahrdStatue: true);

            var recipe = Recipe.Create(ItemID.KingStatue);
            recipe.AddIngredient(ItemID.Throne);
            recipe.AddIngredient(ItemID.TeleportationPotion);
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.QueenStatue);
            recipe.AddIngredient(ItemID.Throne);
            recipe.AddIngredient(ItemID.TeleportationPotion);
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.DisableDecraft();
            recipe.Register();
        }

        private static void AddStatueRecipe(int statue, int extraIngredient = -1, int extraIngredientAmount = 1, bool isLihzahrdStatue = false)
        {
            var recipe = Recipe.Create(statue);

            if (extraIngredient != -1)
            {
                recipe.AddIngredient(extraIngredient, extraIngredientAmount);
            }

            recipe.AddIngredient(isLihzahrdStatue ? ItemID.LihzahrdBrick : ItemID.StoneBlock, 50);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.DisableDecraft();
            recipe.Register();
        }

        private static void AddMiscRecipes()
        {
            RecipeHelper.CreateSimpleRecipe(ItemID.IceBlade, ItemID.EnchantedSword, TileID.CrystalBall, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Pumpkin, ItemID.MagicalPumpkinSeed, TileID.LivingLoom, ingredientAmount: 500, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.FishingSeaweed, ItemID.Seaweed, TileID.LivingLoom, ingredientAmount: 5, disableDecraft: true);
            RecipeHelper.CreateSimpleRecipe(ItemID.Deathweed, ItemID.AbigailsFlower, TileID.Tombstones, ingredientAmount: 5, disableDecraft: true, conditions: Condition.InGraveyard);


            var recipe = Recipe.Create(ItemID.Terragrim);
            recipe.AddIngredient(ItemID.EnchantedSword, 1);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddIngredient(ItemID.Diamond, 3);
            recipe.AddTile(TileID.DemonAltar);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.GemSquirrelAmber);
            recipe.AddRecipeGroup(RecipeGroupID.Squirrels);
            recipe.AddIngredient(ItemID.Amber, 5);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.GemSquirrelAmethyst);
            recipe.AddRecipeGroup(RecipeGroupID.Squirrels);
            recipe.AddIngredient(ItemID.Amethyst, 5);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.GemSquirrelDiamond);
            recipe.AddRecipeGroup(RecipeGroupID.Squirrels);
            recipe.AddIngredient(ItemID.Diamond, 5);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.GemSquirrelEmerald);
            recipe.AddRecipeGroup(RecipeGroupID.Squirrels);
            recipe.AddIngredient(ItemID.Emerald, 5);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.GemSquirrelRuby);
            recipe.AddRecipeGroup(RecipeGroupID.Squirrels);
            recipe.AddIngredient(ItemID.Ruby, 5);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.GemSquirrelSapphire);
            recipe.AddRecipeGroup(RecipeGroupID.Squirrels);
            recipe.AddIngredient(ItemID.Sapphire, 5);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.GemSquirrelTopaz);
            recipe.AddRecipeGroup(RecipeGroupID.Squirrels);
            recipe.AddIngredient(ItemID.Topaz, 5);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.FlowerBoots);
            recipe.AddIngredient(ItemID.HermesBoots);
            recipe.AddIngredient(ItemID.Daybloom);
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddIngredient(ItemID.Shiverthorn);
            recipe.AddIngredient(ItemID.Moonglow);
            recipe.AddIngredient(ItemID.Waterleaf);
            recipe.AddIngredient(ItemID.Deathweed);
            recipe.AddIngredient(ItemID.Fireblossom);
            recipe.AddTile(TileID.LivingLoom);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.LivingLoom);
            recipe.AddIngredient(ItemID.Loom);
            recipe.AddIngredient(ItemID.Vine, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.BabyBirdStaff);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(ItemID.Bird, 1);
            recipe.AddTile(TileID.LivingLoom);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.LifeCrystal);
            recipe.AddIngredient(ItemID.GlowingMushroom, 15);
            recipe.AddIngredient(ItemID.HealingPotion, 3);
            recipe.AddIngredient(ItemID.JungleSpores, 3);
            recipe.AddTile(TileID.DemonAltar);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.JungleRose);
            recipe.AddIngredient(ItemID.NaturesGift);
            recipe.AddIngredient(ItemID.RedHusk);
            recipe.AddTile(TileID.LivingLoom);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.NaturesGift);
            recipe.AddIngredient(ItemID.JungleRose);
            recipe.AddIngredient(ItemID.CyanHusk);
            recipe.AddTile(TileID.LivingLoom);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.AmberMosquito);
            recipe.AddIngredient(ItemID.Amber, 15);
            recipe.AddIngredient(ItemID.Firefly);
            recipe.AddTile(TileID.CookingPots);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.NaturesGift);
            recipe.AddIngredient(ItemID.Moonglow, 15);
            recipe.AddIngredient(ItemID.ManaCrystal);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.ShroomiteBar);
            recipe.AddIngredient(ItemID.ChlorophyteBar);
            recipe.AddIngredient(ItemID.DarkBlueSolution);
            recipe.AddTile(TileID.Autohammer);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.WebSlinger);
            recipe.AddIngredient(ItemID.GrapplingHook);
            recipe.AddIngredient(ItemID.WebRopeCoil, 8);
            recipe.AddTile(TileID.CookingPots);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.FartInABalloon);
            recipe.AddIngredient(ItemID.CloudinaBalloon);
            recipe.AddIngredient(ItemID.WhoopieCushion);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.BalloonHorseshoeFart);
            recipe.AddIngredient(ItemID.BlueHorseshoeBalloon);
            recipe.AddIngredient(ItemID.WhoopieCushion);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.TeleportationPylonVictory);
            recipe.AddIngredient(ItemID.TeleportationPylonDesert);
            recipe.AddIngredient(ItemID.TeleportationPylonHallow);
            recipe.AddIngredient(ItemID.TeleportationPylonJungle);
            recipe.AddIngredient(ItemID.TeleportationPylonMushroom);
            recipe.AddIngredient(ItemID.TeleportationPylonOcean);
            recipe.AddIngredient(ItemID.TeleportationPylonPurity);
            recipe.AddIngredient(ItemID.TeleportationPylonSnow);
            recipe.AddIngredient(ItemID.TeleportationPylonUnderground);
            recipe.AddIngredient(ItemID.PlatinumCoin);
            recipe.AddTile(TileID.DemonAltar);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.KiteBlueAndYellow);
            recipe.AddIngredient(ItemID.KiteBlue);
            recipe.AddIngredient(ItemID.KiteYellow);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.KiteRedAndYellow);
            recipe.AddIngredient(ItemID.KiteRed);
            recipe.AddIngredient(ItemID.KiteYellow);
            recipe.AddTile(TileID.Solidifier);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.MusicBox);
            recipe.AddIngredient(ItemID.Wood, 35);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.WetBomb);
            recipe.AddIngredient(ItemID.Bomb, 1);
            recipe.AddIngredient(ItemID.WaterBucket, 1);
            recipe.AddIngredient(ItemID.Glass, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.DisableDecraft();
            recipe.Register();

            recipe = Recipe.Create(ItemID.Tombstone);
            recipe.AddIngredient(ItemID.StoneBlock, 100);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.DisableDecraft();
            recipe.Register();

            List<int> familiars = [ItemID.FamiliarWig, ItemID.FamiliarShirt, ItemID.FamiliarPants];
            List<int> familiarStations = [TileID.LivingLoom, TileID.Loom, TileID.Loom];
            for (int i = 0; i < familiars.Count; i++)
            {
                recipe = Recipe.Create(familiars[i]);
                recipe.AddIngredient(ItemID.Silk, 12);
                recipe.AddTile(familiarStations[i]);
                recipe.DisableDecraft();
                recipe.Register();
            }
        }
    }
}
