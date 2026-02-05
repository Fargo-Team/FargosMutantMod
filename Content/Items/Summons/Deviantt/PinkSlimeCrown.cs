using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class PinkSlimeCrown : ModItem 
    {

        public override void SetStaticDefaults()
        {
			Item.ResearchUnlockCount = 3;

			ItemID.Sets.SortingPriorityBossSpawns[Type] = 0; // Places it before any other boss summons
		}

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            int n = NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)player.position.X + Main.rand.Next(-800, 800), (int)player.position.Y + Main.rand.Next(-800, -250), NPCID.BlueSlime);
            Main.npc[n].SetDefaults(NPCID.Pinky);

            SoundEngine.PlaySound(SoundID.Roar, player.position);

            LocalizedText text = Language.GetText("Announcement.HasAwoken");
            string pinky = Lang.GetNPCNameValue(NPCID.Pinky);

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(text.ToNetworkText(pinky), new Color(175, 75, 255));
            }
            else
            {
                Main.NewText(text.Format(pinky), new Color(175, 75, 255));
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Gel, 20)
                .AddIngredient(ItemID.PinkDye)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
}