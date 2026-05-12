using Fargowiltas.Content.NPCs;
using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Fargowiltas.Fargowiltas;

namespace Fargowiltas.Content.Items.Misc
{
    public class SuperDummy : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == ItemAlternativeFunctionID.ActivatedAndUsed)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<SuperDummyNPC>())
                            {
                                NPC npc = Main.npc[i];
                                npc.life = 0;
                                npc.HitEffect();
                                Main.npc[i].SimpleStrikeNPC(int.MaxValue, 0, false, 0, null, false, 0, true);
                                //Main.npc[i].StrikeNPCNoInteraction(int.MaxValue, 0, 0, false, false, false);
                            }
                        }
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient) //tell server to clear
                    {
                        var netMessage = Mod.GetPacket();
                        netMessage.Write((byte)PacketID.KillSuperDummy);
                        netMessage.Send();
                    }
                }
            }
            else if (NPC.CountNPCS(ModContent.NPCType<SuperDummyNPC>()) < 50)
            {
                Vector2 pos = new((int)Main.MouseWorld.X - 9, (int)Main.MouseWorld.Y - 20);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, Vector2.Zero, ModContent.ProjectileType<SpawnProj>(), 0, 0, player.whoAmI, ModContent.NPCType<SuperDummyNPC>());
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(4)
                .AddIngredient(ItemID.TargetDummy)
                .AddIngredient(ItemID.FallenStar)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}
