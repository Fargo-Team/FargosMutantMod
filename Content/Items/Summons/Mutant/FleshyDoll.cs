using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Mutant
{
    public class FleshyDoll : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;

			ItemID.Sets.SortingPriorityBossSpawns[Type] = 5; // Places it right after Deer Thing and Abeemination, and before Gelatin Crystal
		}

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.position.Y / 16 > Main.maxTilesY - 200 && !NPC.AnyNPCs(NPCID.WallofFlesh);
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnWOF(player.Center);
            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return true;
        }

        public override void PostUpdate()
        {
            if (Item.lavaWet && !NPC.AnyNPCs(NPCID.WallofFlesh))
            {
                NPC.SpawnWOF(Item.position);
                Item.TurnToAir();
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
               .AddIngredient(ItemID.GuideVoodooDoll)
               .AddTile(TileID.WorkBenches)
               .Register();
        }
    }
}
