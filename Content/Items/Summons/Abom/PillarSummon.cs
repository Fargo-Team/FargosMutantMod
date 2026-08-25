using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Abom
{
    public class PillarSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityMiscImportants[Type] = 18; // Places it right before Celestial Sigil
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.Cyan;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<SpawnProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int[] pillars = [NPCID.LunarTowerNebula, NPCID.LunarTowerSolar, NPCID.LunarTowerStardust, NPCID.LunarTowerVortex];
            if (!NPC.AnyNPCs(NPCID.LunarTowerSolar))
            {
                NPC.ShieldStrengthTowerSolar = 0;
            }
            if (!NPC.AnyNPCs(NPCID.LunarTowerVortex))
            {
                NPC.ShieldStrengthTowerVortex = 0;
            }
            if (!NPC.AnyNPCs(NPCID.LunarTowerNebula))
            {
                NPC.ShieldStrengthTowerNebula = 0;
            }
            if (!NPC.AnyNPCs(NPCID.LunarTowerStardust))
            {
                NPC.ShieldStrengthTowerStardust = 0;
            }
            for (int i = 0; i < pillars.Length; i++)
            {
                Vector2 pos = new Vector2((int)player.position.X + 400 * i - 600, (int)player.position.Y - 200);
                Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), pos, Vector2.Zero, type, 0, 0, Main.myPlayer, pillars[i]);
            }

            FargoUtils.PrintLocalization("MessageInfo.StartLunarEvent", new Color(175, 75, 255));

            SoundEngine.PlaySound(SoundID.Roar, player.position);

            return false;
        }
    }
}