using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives
{
    public class CityBuster : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return FargoServerConfig.Instance.InstantItems;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 46;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 1);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<CityBusterBomb>();
            Item.shootSpeed = 5f;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(BuffID.NoBuilding))
                return false;
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(10)
                .AddIngredient(ItemID.Dynamite, 50)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class CityBusterBomb : ModProjectile
    {
        public static readonly SoundStyle ArmingSound = new("Fargowiltas/Assets/Sounds/CityBusterArm");
        public static readonly SoundStyle DisarmSound = new("Fargowiltas/Assets/Sounds/CityBusterDisarm");
        public static readonly SoundStyle ExplosionSound = new("Fargowiltas/Assets/Sounds/CityBusterExplosion");
        public static Asset<Texture2D> glowTexture;
        public static Asset<Texture2D> highlightTexture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsInteractable[Type] = true;
            Main.projFrames[Type] = 8;

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
            //Projectile.aiStyle = ProjAIStyleID.Explosive;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600; // 10 seconds
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(ArmingSound with { Variants = [1, 2], PauseBehavior = PauseBehavior.PauseWithGame }, Projectile.Center, delegate (ActiveSound s)
                {
                    s.Position = Projectile.Center;
                    if (!Projectile.active)
                       return false;
                    return true;
                });
            }
        }
        public override void AI()
        {
            float ratio = Projectile.timeLeft / 800f;
            float tps = MathHelper.Lerp(0, 6, ratio);
            if (++Projectile.frameCounter >= tps)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Type] - 1)
                    Projectile.frame = 0;
            }

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
                dust.noGravity = true;
                dust.position = Projectile.Center + new Vector2(2, 3).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
                dust.scale = 1f + Main.rand.Next(5) * 0.1f;
                dust.noGravity = true;
                dust.position = Projectile.Center + new Vector2(2, 3).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;
            }

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                // Roll speed dampening.
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.96f;

                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                // Delayed gravity
                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            }
            // Rotation increased by velocity.X
            Projectile.rotation += Projectile.velocity.X * 0.1f;

            Main.CurrentFrameFlags.HadAnActiveInteractableProjectile = true;

            Player player = Main.player[Projectile.owner];
            Vector2 spot = player.Center;
            if (player.IsProjectileInteractableAndInInteractionRange(Projectile, ref spot))
            {
                Main.HasInteractableObjectThatIsNotATile = true;
                bool mouseOver = Projectile.Hitbox.Contains(Main.MouseWorld.ToPoint());
                if (Main.SmartInteractProj == Projectile.whoAmI || mouseOver)
                {
                    player.noThrow = 2;
                    player.cursorItemIconEnabled = true;
                    player.cursorItemIconID = ModContent.ItemType<CityBuster>();

                    if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0)
                    {
                        Main.mouseRightRelease = false;
                        player.tileInteractAttempted = true;
                        player.tileInteractionHappened = true;
                        player.releaseUseTile = true;
                        Projectile.active = false;
                        if (!Main.dedServ)
                        {
                            SoundEngine.PlaySound(DisarmSound with { Volume = 0.4f, PauseBehavior = PauseBehavior.PauseWithGame }, Projectile.Center);
                        }
                        player.QuickSpawnItem(Projectile.GetSource_DropAsItem(), ModContent.ItemType<CityBuster>());
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

        public override void OnKill(int timeLeft)
        {   
            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(ExplosionSound with { Volume = 0.8f, PauseBehavior = PauseBehavior.PauseWithGame }, Projectile.Center);
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            Player player = Main.player[Projectile.owner];

            Vector2 position = Projectile.Center;
            int radius = 64;     //bigger = boomer
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius * 2; y <= 0; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);

                    if (xPosition < 0 || xPosition >= Main.maxTilesX || yPosition < 0 || yPosition >= Main.maxTilesY)
                        continue;

                    Tile tile = Main.tile[xPosition, yPosition];
                    if (tile == null)
                        continue;

                    if (!FargoGlobalProjectile.OkayToDestroyTileAt(xPosition, yPosition) || FargoGlobalProjectile.TileIsLiterallyAir(tile))
                        continue;

                    if (!player.HasEnoughPickPowerToHurtTile(xPosition, yPosition) || !WorldGen.CanKillTile(xPosition, yPosition))
                        continue;

                    FargoGlobalTile.ClearTileAndLiquid(xPosition, yPosition);
                }
            }

            Main.refreshMap = true;
        }
        public override bool PreDraw(Player player, ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition - new Vector2(0, Projectile.gfxOffY);
            Rectangle rect = new(0, 66 * Projectile.frame, 52, 66);
            Vector2 origin = rect.Size() / 2f;

            Main.EntitySpriteDraw(texture, position, rect, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glowTexture.Value, position, rect, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            Vector2 interactionSpot = player.Center;

            Color highlightColor;
            if (player.IsProjectileInteractableAndInInteractionRange(Projectile, ref interactionSpot) && (Main.SmartCursorIsUsed || PlayerInput.UsingGamepad))
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