using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons
{
    public abstract class BaseSummon : ModItem
    {
        public abstract int NPCType { get; }

        /// <summary>
        /// Used when <see cref="Lang.GetNPCNameValue"/> or <see cref="ModNPC.DisplayName"/> can't get its name properly
        /// </summary>
        public virtual string NPCName { get; }

        public virtual bool ResetTimeWhenUsed => false;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
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
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<SpawnProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ResetTimeWhenUsed)
            {
                Main.time = 0;

                if (Main.netMode == NetmodeID.Server) //sync time
                    NetMessage.SendData(MessageID.WorldData, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
            }

            Vector2 pos = new Vector2((int)player.position.X + Main.rand.Next(-800, 800), (int)player.position.Y + Main.rand.Next(-800, -250));

            if (NPCType == NPCID.Golem)
            {
                pos = player.Center;
                for (int i = 0; i < 30; i++)
                {
                    pos.Y -= 16;

                    if (pos.Y <= 0 || WorldGen.SolidTile((int)pos.X / 16, (int)pos.Y / 16))
                    {
                        pos.Y += 16;
                        break;
                    }
                }
            }

            Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), pos, Vector2.Zero, ModContent.ProjectileType<SpawnProj>(), 0, 0, Main.myPlayer, NPCType);

            LocalizedText text = Language.GetText("Announcement.HasAwoken");
            string npcName = NPCName ?? (ModContent.GetModNPC(NPCType) == null ? Lang.GetNPCNameValue(NPCType) : ModContent.GetModNPC(NPCType).DisplayName.Value);

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(text.ToNetworkText(npcName), new Color(175, 75, 255));
            }
            else if (NPCType != NPCID.KingSlime)
            {
                Main.NewText(text.Format(npcName), new Color(175, 75, 255));
            }

            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return false;
        }
    }
}