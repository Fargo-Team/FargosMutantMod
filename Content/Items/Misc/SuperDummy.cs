using Fargowiltas.Content.NPCs;
using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.type == ModContent.NPCType<SuperDummyNPC>())
                    {
                        n.active = false;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            ModPacket deactivate = Mod.GetPacket();
                            deactivate.Write((byte)Fargowiltas.PacketID.SyncInactiveNPC);
                            deactivate.Write((byte)n.whoAmI);
                            deactivate.Send();
                        }
                    }
                }
            }
            else if (NPC.CountNPCS(ModContent.NPCType<SuperDummyNPC>()) < 50 && player.whoAmI == Main.myPlayer)
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
