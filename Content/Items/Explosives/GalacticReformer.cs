using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class GalacticReformer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 42;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<GalacticReformerBomb>();
            Item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Dynamite, 500)
            .AddTile(TileID.Hellforge)

            .Register();
        }
    }

    public class GalacticReformerBomb : ModProjectile
    {
        public static Asset<Texture2D> glowTexture;
        public static Asset<Texture2D> highlightTexture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsInteractable[Type] = true;
            Main.projFrames[Type] = 7;

            if (!Main.dedServ)
            {
                glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow", AssetRequestMode.ImmediateLoad);
                highlightTexture = ModContent.Request<Texture2D>(Texture + "_Highlight", AssetRequestMode.ImmediateLoad);
            }

            ProjectileID.Sets.Explosive[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 720; // 12 seconds
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                SoundStyle arm = new("Fargowiltas/Assets/Sounds/CityBusterArm");
                /*
                SoundEngine.PlaySound(arm with { Variants = [1, 2], PauseBehavior = PauseBehavior.PauseWithGame }, Projectile.Center, delegate (ActiveSound s)
                {
                    s.Position = Projectile.Center;
                    if (!Projectile.active)
                        return false;
                    return true;
                });
                */
            }
        }
        public override void AI()
        {
            float ratio = Projectile.timeLeft / 920f;
            float tps = MathHelper.Lerp(0, 6, ratio);
            if (++Projectile.frameCounter >= tps)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Type] - 1)
                    Projectile.frame = 0;
            }

            Projectile.velocity *= 0.96f;
            Projectile.rotation += (Projectile.velocity.X / 24);
            Projectile.rotation = MathHelper.SmoothStep(Projectile.rotation, 0, 0.18f);

            Lighting.AddLight(Projectile.Center, TorchID.Orange);

            Main.CurrentFrameFlags.HadAnActiveInteractibleProjectile = true;

            Player player = Main.player[Projectile.owner];
            Vector2 spot = player.Center;
            if (player.IsProjectileInteractibleAndInInteractionRange(Projectile, ref spot))
            {
                Main.HasInteractibleObjectThatIsNotATile = true;
                bool mouseOver = Projectile.Hitbox.Contains(Main.MouseWorld.ToPoint());
                if (Main.SmartInteractProj == Projectile.whoAmI || mouseOver)
                {
                    player.noThrow = 2;
                    player.cursorItemIconEnabled = true;
                    player.cursorItemIconID = ModContent.ItemType<GalacticReformer>();

                    if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0)
                    {
                        Main.mouseRightRelease = false;
                        player.tileInteractAttempted = true;
                        player.tileInteractionHappened = true;
                        player.releaseUseTile = true;
                        Projectile.active = false;
                        if (!Main.dedServ)
                        {
                            SoundStyle disarm = new("Fargowiltas/Assets/Sounds/CityBusterDisarm");
                            SoundEngine.PlaySound(disarm with { Volume = 0.4f, PauseBehavior = PauseBehavior.PauseWithGame }, Projectile.Center);
                        }
                        player.QuickSpawnItem(Projectile.GetSource_DropAsItem(), ModContent.ItemType<GalacticReformer>());
                    }
                }
            }
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = 0;
            return base.OnTileCollide(oldVelocity);
        }

        private const int radius = 300; //bigger = boomer

        public override void OnKill(int timeLeft)
        {
            Vector2 position = Projectile.Center;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Math.Sqrt(x * x + y * y) <= radius)   //circle
                    {
                        int xPosition = (int)(x + position.X / 16.0f);
                        int yPosition = (int)(y + position.Y / 16.0f);
                        if (xPosition < 0 || xPosition >= Main.maxTilesX || yPosition < 0 || yPosition >= Main.maxTilesY)
                            continue;

                        Tile tile = Main.tile[xPosition, yPosition];

                        if (tile == null) continue;

                        if (WorldGen.InWorld(xPosition, yPosition))
                        {
                            tile.ClearEverything();
                            Main.Map.Update(xPosition, yPosition, 255);
                        }
                    }

                    //NetMessage.SendTileSquare(-1, xPosition, yPosition, 1);
                }
            }

            Main.refreshMap = true;
            // Play explosion sound
            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
                SoundEngine.PlaySound(SoundID.Item14, position);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition - new Vector2(0, Projectile.gfxOffY);
            Rectangle rect = new(0, 58 * Projectile.frame, 50, 58);
            Vector2 origin = rect.Size() / 2f;

            Main.EntitySpriteDraw(texture, position, rect, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glowTexture.Value, position, rect, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            Player player = Main.player[Projectile.owner];

            Vector2 interactionSpot = player.Center;

            Color highlightColor;
            if (player.IsProjectileInteractibleAndInInteractionRange(Projectile, ref interactionSpot) && (Main.SmartCursorIsUsed || PlayerInput.UsingGamepad))
            {
                int avgBrightness = (lightColor.R + lightColor.G + lightColor.B) / 3;
                if (avgBrightness > 10)
                {
                    bool actuallySelected = Main.SmartInteractProj == Projectile.whoAmI;
                    highlightColor = Colors.GetSelectionGlowColor(actuallySelected, avgBrightness);
                    Main.EntitySpriteDraw(highlightTexture.Value, position, rect, highlightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}
