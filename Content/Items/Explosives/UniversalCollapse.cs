using Fargowiltas.Content.Items.Tiles;
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
    public class UniversalCollapse : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 46;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<UniversalCollapseBomb>();
            Item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<GalacticReformer>(100)
            .AddTile<LuminiteOmniforgeTile>()

            .Register();
        }
    }

    public class UniversalCollapseBomb : ModProjectile
    {
        public static Asset<Texture2D> glowTexture;
        public static Asset<Texture2D> highlightTexture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsInteractable[Type] = true;
            Main.projFrames[Type] = 9;

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
            Projectile.timeLeft = 900; // 15 seconds
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                SoundStyle arm = new("Fargowiltas/Assets/Sounds/CityBusterArm");
                /*
                 * SoundEngine.PlaySound(arm with { Variants = [1, 2], PauseBehavior = PauseBehavior.PauseWithGame }, Projectile.Center, delegate (ActiveSound s)
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
            float ratio = Projectile.timeLeft / 1100f;
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
                    player.cursorItemIconID = ModContent.ItemType<UniversalCollapse>();

                    if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0)
                    {
                        Main.mouseRightRelease = false;
                        player.tileInteractAttempted = true;
                        player.tileInteractionHappened = true;
                        player.releaseUseTile = true;
                        Projectile.active = false;
                        if (!Main.dedServ)
                        {
                            SoundEngine.PlaySound(CityBusterBomb.DisarmSound with { Volume = 0.4f, PauseBehavior = PauseBehavior.PauseWithGame }, Projectile.Center);
                        }
                        player.QuickSpawnItem(Projectile.GetSource_DropAsItem(), ModContent.ItemType<UniversalCollapse>());
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].ClearEverything();

                    if (WorldGen.InWorld(i, j))
                        Main.Map.Update(i, j, 255);
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.boss && !npc.dontTakeDamage)
                {
                    npc.SimpleStrikeNPC(int.MaxValue, 0, false, 0, null, false, 0, true);
                }
            }

            if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
            {
                var def = Main.LocalPlayer.statDefense;
                float dr = Main.LocalPlayer.endurance;
                Main.LocalPlayer.statDefense.FinalMultiplier *= 0;
                Main.LocalPlayer.endurance = 0f;

                int damage = Math.Max(9999, Main.LocalPlayer.statLifeMax2 * 2);
                Main.LocalPlayer.Hurt(PlayerDeathReason.ByProjectile(Main.LocalPlayer.whoAmI, Projectile.whoAmI), damage, 0);

                Main.LocalPlayer.statDefense = def;
                Main.LocalPlayer.endurance = dr;
            }

            Main.refreshMap = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition - new Vector2(0, Projectile.gfxOffY);
            Rectangle rect = new(0, 70 * Projectile.frame, 68, 70);
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
