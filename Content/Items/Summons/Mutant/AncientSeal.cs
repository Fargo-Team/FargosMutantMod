using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Mutant
{
    public class AncientSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;

			ItemID.Sets.SortingPriorityBossSpawns[Type] = 19; // Places it right after Celestial Sigil

			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

            ItemID.Sets.ItemNoGravity[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = 1000;
            Item.rare = ItemRarityID.Purple;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<SpawnProj>();
        }

        public override bool CanUseItem(Player player)
        {
            return FargoUtils.ActuallyNight;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pos = new Vector2((int)player.position.X + Main.rand.Next(-800, 800), (int)player.position.Y + Main.rand.Next(-1000, -250));

            // Vanilla
            Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), pos, Vector2.Zero, ModContent.ProjectileType<SpawnProj>(), 0, 0, Main.myPlayer, 1, 3);

            // Modded
            for (int i = NPCID.Count; i < NPCLoader.NPCCount; i++)
            {
                NPC npc = new();
                npc.SetDefaults(i);

                if (npc.boss && npc.type != NPCID.DungeonGuardian)
                {
                    string name = npc.ModNPC != null ? npc.ModNPC.DisplayName.Value : Lang.GetNPCNameValue(npc.netID);
                    SpawnBoss(player, i, name);
                }
            }

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.AncientSeal"), new Color(175, 75, 255));
            }
            else
            {
                Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.AncientSeal"), new Color(175, 75, 255));
            }

            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return false;
        }

        public static int SpawnBoss(Player player, int npcID, string name)
        {
            Main.NewText(Language.GetTextValue("Announcement.HasAwoken", name), new Color(175, 75, 255));
            return NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), (int)player.position.X + Main.rand.Next(-800, 800), (int)player.position.Y + Main.rand.Next(-1000, -250), npcID);
        }
    }
}