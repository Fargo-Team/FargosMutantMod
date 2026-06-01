using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.Mutant
{
    public class PrismaticPrimrose : BaseSummon
    {
        public override int NPCType => NPCID.HallowBoss;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // Places it right after the Truffle Worm
        }

        public override void AddRecipes()
        {
            CreateRecipe()
               .AddIngredient(ItemID.EmpressButterfly)
               .AddTile(TileID.WorkBenches)
               .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!NPC.downedEmpressOfLight)
            {
                Main.SkipToTime(0, Main.dayTime);

                if (Main.netMode == NetmodeID.Server) //sync time
                    NetMessage.SendData(MessageID.WorldData, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}