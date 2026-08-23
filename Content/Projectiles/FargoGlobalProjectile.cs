using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems.Collections;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Fargowiltas.Content.Projectiles
{
    public class FargoGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private bool firstTick = true;
        public bool lowRender;

        public static HashSet<Rectangle> CannotDestroyRectangle = [];

        public float DamageMultiplier = 1;
        public static MethodInfo GetPickaxeDamage_Method;
        public override void Load()
        {
            base.Load();
            GetPickaxeDamage_Method = typeof(Player).GetMethod("GetPickaxeDamage", FargoUtils.UniversalBindingFlags);
        }
        public override void SetDefaults(Projectile projectile)
        {
            //if (projectile.CountsAsClass(DamageClass.Summon) || projectile.minion || projectile.sentry || projectile.minionSlots > 0 || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type])
            //{
            //    if (!ProjectileID.Sets.IsAWhip[projectile.type])
            //        lowRender = true;
            //}

            //switch (projectile.type)
            //{
            //    case ProjectileID.FlowerPetal:
            //    case ProjectileID.HallowStar:
            //    case ProjectileID.RainbowFront:
            //    case ProjectileID.RainbowBack:
            //        lowRender = true;
            //        break;

            //    default:
            //        break;
            //}

            if (projectile.friendly)
                lowRender = true;
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            FargoServerConfig config = FargoServerConfig.Instance;
            if (config.EnemyDamage != 1 || config.BossDamage != 1)
            {
                bool boss = source is EntitySource_Parent parent && parent.Entity is NPC npc && config.BossDamage > config.EnemyDamage && // only relevant if boss health is higher than enemy health
                (npc.CountsAsBoss() || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || (config.BossApplyToAllWhenAlive && FargoUtils.AnyBossAlive()));
                if (boss)
                    DamageMultiplier = config.BossDamage;
                else
                {
                    if (source is EntitySource_Parent parentt && parentt.Entity is Projectile parentProj && parentProj.TryGetGlobalProjectile(out FargoGlobalProjectile parentFGP) && parentFGP.DamageMultiplier > config.EnemyDamage)
                        DamageMultiplier = parentFGP.DamageMultiplier;
                    else
                        DamageMultiplier = config.EnemyDamage;
                }
            }

            if (projectile.bobber && projectile.owner == Main.myPlayer && FargoServerConfig.Instance.ExtraLures && source is EntitySource_ItemUse)
            {
                int split;
                int itemType = Main.player[projectile.owner].HeldItem.type;

                //check held item type for fishing rods instead of projectile, since bobber projectiles are overridden by the Bobber items
                switch (itemType)
                {
                    case ItemID.GoldenFishingRod:
                        split = 3;
                        break;
                    case ItemID.HotlineFishingHook:
                    case ItemID.SittingDucksFishingRod:
                    case ItemID.MechanicsRod:
                        split = 2;
                        break;
                    /*case ItemID.ScarabFishingRod:
                    case ItemID.FiberglassFishingPole:
                    case ItemID.BloodFishingRod:
                    case ItemID.Fleshcatcher:
                    case ItemID.FisherofSouls:
                        split = 2;
                        break;*/
                    default:
                        split = 1;
                        break;
                }


                //if (Main.player[projectile.owner].HasBuff(BuffID.Fishing))
                //split++;

                if (split > 1)
                    SplitProj(projectile, split);
            }
        }

        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.FlyingPiggyBank && FargoServerConfig.Instance.StalkerMoneyTrough)
            {
                Player player = Main.player[projectile.owner];
                float dist = Vector2.Distance(projectile.Center, player.Center);

                if (dist > 3000)
                {
                    projectile.Center = player.Top;
                }
                else if (projectile.Center != player.Center)
                {
                    Vector2 velocity = (player.Center + projectile.DirectionFrom(player.Center) * 3 * 16 - projectile.Center) / (dist < 3f * 16 ? 30f : 60f);
                    projectile.position += velocity;
                }

                if (projectile.timeLeft < 2 && projectile.timeLeft > 0)
                    projectile.timeLeft = 2;
            }

            if (firstTick)
            {
                firstTick = false;

                if (projectile.owner != Main.myPlayer && !projectile.hostile && !projectile.trap && projectile.friendly)
                    lowRender = true;
            }

            if (projectile.bobber && projectile.lavaWet && FargoServerConfig.Instance.FasterLavaFishing)
            {
                if (projectile.ai[0] == 0 && projectile.ai[1] == 0 && projectile.localAI[1] < 600)
                    projectile.localAI[1]++;
            }

            if (Fargowiltas.SwarmActive && projectile.hostile && projectile.damage > 0 && projectile.damage < Fargowiltas.SwarmMinDamage)
                projectile.damage = Fargowiltas.SwarmMinDamage;

            return true;
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.type == ProjectileID.FlyingPiggyBank && FargoServerConfig.Instance.StalkerMoneyTrough)
            {
                //functionally, this makes money trough toggle the piggy bank on/off
                foreach (Projectile p in Main.projectile.Where(p => p.active && p.type == projectile.type && p.owner == projectile.owner))
                    p.timeLeft = 0;
            }
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= DamageMultiplier;
        }

        public static void SplitProj(Projectile projectile, int number)
        {
            Projectile split;

            double spread = 0.3 / number;

            for (int i = 0; i < number / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int factor = j == 0 ? 1 : -1;
                    split = NewProjectileDirectSafe(projectile.GetSource_FromThis(), projectile.Center, projectile.velocity.RotatedBy(factor * spread * (i + 1)), projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1]);

                    if (split != null)
                    {
                        split.GetGlobalProjectile<FargoGlobalProjectile>().firstTick = false;
                    }
                }
            }

            if (number % 2 == 0)
            {
                projectile.active = false;
            }
        }

        public static Projectile NewProjectileDirectSafe(IEntitySource source, Vector2 pos, Vector2 vel, int type, int damage, float knockback, int owner = 255, float ai0 = 0f, float ai1 = 0f)
        {
            int p = Projectile.NewProjectile(source, pos, vel, type, damage, knockback, owner, ai0, ai1);
            return p < Main.maxProjectiles ? Main.projectile[p] : null;
        }
        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            bool pets = FargoClientConfig.Instance.NoTransparentPets && Main.projPet[projectile.type] && !projectile.minion;
            if (lowRender && !projectile.hostile && FargoClientConfig.Instance.TransparentFriendlyProjectiles < 1 && !pets)
            {
                Color? color = projectile.ModProjectile?.GetAlpha(lightColor);
                if (color != null)
                {
                    return color.Value * FargoClientConfig.Instance.TransparentFriendlyProjectiles;
                }
                lightColor *= projectile.Opacity * FargoClientConfig.Instance.TransparentFriendlyProjectiles;
                return lightColor;
            }

            return base.GetAlpha(projectile, lightColor);
        }
        public static bool OkayToDestroyTile(Tile tile)
        {
            if (tile == null)
            {
                return false;
            }
            bool noDungeon = !NPC.downedBoss3 && (FargoWallSets.DungeonWall[tile.WallType] || FargoTileSets.DungeonTile[tile.TileType]);

            bool noHMOre = FargoTileSets.HardmodeOre[tile.TileType] && !NPC.downedMechBossAny;
            bool noChloro = tile.TileType == TileID.Chlorophyte && !(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);
            bool noLihzahrd = (tile.TileType == TileID.LihzahrdBrick || tile.WallType == WallID.LihzahrdBrickUnsafe) && !NPC.downedGolemBoss;
            bool noAbyss = false;

            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                if (calamity.TryFind("AbyssGravel", out ModTile gravel) && calamity.TryFind("Voidstone", out ModTile voidstone))
                    noAbyss = tile.TileType == gravel.Type || tile.TileType == voidstone.Type;
            }

            if (noDungeon || noHMOre || noChloro || noLihzahrd || noAbyss || TileBelongsToMagicStorage(tile) ||
                FargoTileSets.InstaCannotDestroy[tile.TileType] ||
                FargoWallSets.InstaCannotDestroy[tile.WallType])
                return false;

            return true;
        }
        public static bool InstaDestructionCheck(int x, int y, Player player, Item pickaxe, bool bypassVanillaCanPlace = false) // This is the mother method that should be used for destructive items
        {
            if (!OkayToDestroyTileAt(x, y, bypassVanillaCanPlace))
                return false;

            if (!Main.tile[x, y].HasTile)
                return true;

            if (!CanMine(x, y, player, pickaxe) || !WorldGen.CanKillTile(x, y))
                return false;

            return true;
        }
        public static bool CanMine(int x, int y, Player player, Item pickaxe) // doing this to avoid iterating through player inventory looking for pickaxe for every block in insta-items
        {
            Tile tile = Main.tile[x, y];
            int hitBufferIndex = player.hitTile.HitObject(x, y, 1);
            if ((int)GetPickaxeDamage_Method.Invoke(player, [x, y, pickaxe.pick, hitBufferIndex, tile]) == 0)
                return false;

            return true;
        }
        public static bool OkayToDestroyTileAt(int x, int y, bool bypassVanillaCanPlace = false) // Testing for blocks that should not be destroyed
        {
            if (!WorldGen.InWorld(x, y))
                return false;
            Tile tile = Main.tile[x, y];
            if (tile == null)
            {
                return false;
            }
            if (CannotDestroyRectangle != null && CannotDestroyRectangle.Count != 0)
            {
                foreach (Rectangle rect in CannotDestroyRectangle)
                {
                    if (rect.Contains(x * 16, y * 16))
                    {
                        return false;
                    }
                }
            }
            Rectangle area = new(x, y, 3, 3);

            return OkayToDestroyTile(tile);
        }

        public static bool TileIsLiterallyAir(Tile tile)
        {
            return tile.TileType == TileID.Dirt && tile.WallType == WallID.None && tile.LiquidAmount == 0 /*&& tile.sTileHeader == 0 && tile.bTileHeader == 0 && tile.bTileHeader2 == 0 && tile.bTileHeader3 == 0*/ && tile.TileFrameX == 0 && tile.TileFrameY == 0;
        }

        public static bool TileBelongsToMagicStorage(Tile tile)
        {
            return Fargowiltas.MagicStorageMod is Mod mStore && mStore == TileLoader.GetTile(tile.TileType)?.Mod;
        }
    }
}