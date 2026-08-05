using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
    public class ModeToggle : ModItem
    {
        public override string Texture => "Fargowiltas/Content/Items/Misc/ModeToggle_0";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;
            Item.noUseGraphic = true;
            //Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = false;
            Item.shoot = ModContent.ProjectileType<WorldTokenProj>();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.maxNPCs; i++) //cant use while boss alive
            {
                if (Main.npc[i].active && Main.npc[i].boss)
                {
                    return false;
                }
            }
            return true;
        }

        public override bool? UseItem(Player player)
        {
            string text; ;

            static string DiffText(string difficulty) => Language.GetTextValue($"Mods.Fargowiltas.Items.ModeToggle.{difficulty}");

            if (player.altFunctionUse == 2)
            {
                if (Main.GameMode == GameModeID.Creative)
                {
                    Main.GameMode = GameModeID.Expert;
                    ChangeAllPlayerDifficulty(PlayerDifficultyID.SoftCore);
                    text = DiffText("Expert");
                }
                else
                {
                    Main.GameMode = GameModeID.Creative;
                    ChangeAllPlayerDifficulty(PlayerDifficultyID.Creative);
                    text = DiffText("Journey");
                }
            }
            else
            {
                switch (Main.GameMode)
                {
                    case GameModeID.Normal:
                        Main.GameMode = GameModeID.Expert;
                        ChangeAllPlayerDifficulty(PlayerDifficultyID.SoftCore);
                        text = DiffText("Expert");
                        break;

                    case GameModeID.Expert:
                        Main.GameMode = GameModeID.Master;
                        ChangeAllPlayerDifficulty(PlayerDifficultyID.SoftCore);
                        text = DiffText("Master");
                        break;
                    /*
                case GameModeID.Master:
                    Main.GameMode = GameModeID.Creative;
                    ChangeAllPlayerDifficulty(PlayerDifficultyID.Creative);
                    text = DiffText("Journey");
                    break;
                    */

                    default:
                        Main.GameMode = GameModeID.Normal;
                        ChangeAllPlayerDifficulty(PlayerDifficultyID.SoftCore);
                        text = DiffText("Normal");
                        break;
                }
            }


            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, new Color(175, 75, 255));
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(175, 75, 255));
                NetMessage.SendData(MessageID.WorldData); //sync world
            }

            SoundEngine.PlaySound(SoundID.Roar, player.Center);

            return true;
        }

        private static void ChangeAllPlayerDifficulty(byte diff)
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                {
                    player.difficulty = diff;
                    NetMessage.SendData(MessageID.SyncPlayer, number: player.whoAmI);
                }
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroups.IronBar, 5)
                .AddTile(TileID.WorkBenches)
                .DisableDecraft()
                .Register();
        }

        /*
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Main.itemAnimations[Item.type].Frame = Main.GameMode + 1;
            base.Update(ref gravity, ref maxFallSpeed);
        }
        public override void UpdateInventory(Player player)
        {
            Main.itemTexture[Item.type] = Main.GameMode + 1;
            base.UpdateInventory(player);
        }
        */

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>($"Fargowiltas/Content/Items/Misc/ModeToggle_{Main.GameMode}");
            spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreDrawInWorld(WorldItem item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>($"Fargowiltas/Content/Items/Misc/ModeToggle_{Main.GameMode}");
            Vector2 position = item.position - Main.screenPosition + new Vector2(16, 16);
            Rectangle frame = new Rectangle(0, 0, 32, 32);
            spriteBatch.Draw(texture, position, frame, lightColor, rotation, new Vector2(16, 16), scale, SpriteEffects.None, 0f);
            return false;
        }

    }
}