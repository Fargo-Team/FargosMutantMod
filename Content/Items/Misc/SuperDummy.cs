using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Fargowiltas.Content.Projectiles;
using Fargowiltas.Content.NPCs;
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
            else if (NPC.CountNPCS(ModContent.NPCType<SuperDummyNPC>()) < 50 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 pos = new((int)Main.MouseWorld.X - 9, (int)Main.MouseWorld.Y - 20);

                int n = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)pos.X, (int)pos.Y, ModContent.NPCType<SuperDummyNPC>());
                if (n != Main.maxNPCs)
                {
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, number: n);
                    else if (player.whoAmI == Main.myPlayer)
                        Main.npc[n].direction = Main.npc[n].Center.X < player.Center.X ? 1 : -1;
                }

                //Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, Vector2.Zero, ModContent.ProjectileType<SpawnProj>(), 0, 0, player.whoAmI, ModContent.NPCType<SuperDummyNPC>());

                //NPC.NewNPC((int)pos.X, (int)pos.Y, ModContent.NPCType<NPCs.SuperDummy>());
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TargetDummy)
                .AddIngredient(ItemID.FallenStar)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}
