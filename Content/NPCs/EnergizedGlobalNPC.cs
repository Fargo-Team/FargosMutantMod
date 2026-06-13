using Fargowiltas.Common;
using Fargowiltas.Common.Systems.Collections;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Fargowiltas.Content.NPCs.EnergizedGlobalNPC;

namespace Fargowiltas.Content.NPCs
{
    public class EnergizedGlobalNPC : GlobalNPC
    {
        public enum Binding
        {
            None,
            PreHardmode,
            PreMechs,
            PostMechs,
            PostPlantera,
            PreMoonLord
        }
        public override bool InstancePerEntity => true;
        public bool SwarmActive(NPC npc) => npc.GetGlobalNPC<FargoGlobalNPC>().SwarmActive;
        public bool SwarmHealth = false;

        internal static int[] Bosses = [
            NPCID.KingSlime,
            NPCID.EyeofCthulhu,
            //NPCID.EaterofWorldsHead,
            NPCID.BrainofCthulhu,
            NPCID.QueenBee,
            NPCID.SkeletronHead,
            NPCID.QueenSlimeBoss,
            NPCID.TheDestroyer,
            NPCID.SkeletronPrime,
            NPCID.Retinazer,
            NPCID.Spazmatism,
            NPCID.Plantera,
            NPCID.Golem,
            NPCID.DukeFishron,
            NPCID.HallowBoss,
            NPCID.CultistBoss,
            NPCID.MoonLordCore,
            NPCID.MartianSaucerCore,
            NPCID.Pumpking,
            NPCID.IceQueen,
            NPCID.DD2Betsy,
            NPCID.DD2OgreT3,
            NPCID.IceGolem,
            NPCID.SandElemental,
            NPCID.Paladin,
            NPCID.Everscream,
            NPCID.MourningWood,
            NPCID.SantaNK1,
            NPCID.HeadlessHorseman,
            NPCID.PirateShip
        ];

        public override void SetDefaults(NPC npc)
        {
            const int k = 1000;
            //const int m = k * k;
            int baseHealth = 18 * k;
            int baseHealthHM = 48 * k;
            bool validBoss = true;
            if (Fargowiltas.SwarmSetDefaults)
            {
                switch (npc.type)
                {
                    case NPCID.KingSlime:
                        npc.lifeMax = baseHealth;
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.EyeofCthulhu:
                        npc.lifeMax = baseHealth;
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.EaterofWorldsHead:
                        npc.lifeMax = baseHealth / 12;
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.BrainofCthulhu:
                        npc.lifeMax = (int)(baseHealth / 2.5f);
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.DD2DarkMageT1:
                        npc.lifeMax = (int)(baseHealth / 1.5f);
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.Deerclops:
                        npc.lifeMax = baseHealth;
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.QueenBee:
                        npc.lifeMax = baseHealth;
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.SkeletronHead:
                        npc.lifeMax = baseHealth / 2;
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.WallofFlesh:
                        npc.lifeMax = baseHealth;
                        Fargowiltas.Binding = Binding.PreHardmode;
                        break;

                    case NPCID.QueenSlimeBoss:
                        npc.lifeMax = (int)(baseHealthHM * 0.6f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PreMechs;
                        break;

                    case NPCID.TheDestroyer:
                        npc.lifeMax = (int)(baseHealthHM * 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PreMechs;
                        break;

                    case NPCID.Retinazer:
                        npc.lifeMax = baseHealthHM / 2;
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PreMechs;
                        break;

                    case NPCID.Spazmatism:
                        npc.lifeMax = baseHealthHM / 2;
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PreMechs;
                        break;

                    case NPCID.SkeletronPrime:
                        npc.lifeMax = (int)(baseHealthHM / 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PreMechs;
                        break;

                    case NPCID.Plantera:
                        npc.lifeMax = baseHealthHM / 2;
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PostMechs;
                        break;

                    case NPCID.Golem:
                        npc.lifeMax = baseHealthHM / 6;
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PostPlantera;
                        break;

                    case NPCID.DD2Betsy:
                        npc.lifeMax = (int)(baseHealthHM / 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PostPlantera;
                        break;

                    case NPCID.DukeFishron:
                        npc.lifeMax = (int)(baseHealthHM / 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PostPlantera;
                        break;

                    case NPCID.HallowBoss:
                        npc.lifeMax = (int)(baseHealthHM / 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PostPlantera;
                        break;

                    case NPCID.CultistBoss:
                        npc.lifeMax = baseHealthHM / 4;
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PreMoonLord;
                        break;

                    case NPCID.MoonLordCore:
                        npc.lifeMax = (int)(baseHealthHM / 2.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.Binding = Binding.PreMoonLord;
                        break;

                    case NPCID.DungeonGuardian:
                        npc.lifeMax += 100 * Fargowiltas.SwarmItemsUsed;
                        validBoss = false;
                        break;

                    default:
                        validBoss = false;
                        break;
                }
            }
            else
                validBoss = false;
            if (Fargowiltas.SwarmActive)
            {
                if (!validBoss)
                {
                    validBoss = true;
                    switch (npc.type)
                    {
                        case NPCID.Creeper:
                            npc.lifeMax = 1 * k;
                            break;

                        case NPCID.EaterofWorldsBody:
                        case NPCID.EaterofWorldsTail:
                            npc.lifeMax = baseHealth / 12;
                            break;

                        case NPCID.SkeletronHand:
                            npc.lifeMax = baseHealth / 12;
                            break;

                        case NPCID.PrimeCannon:
                        case NPCID.PrimeLaser:
                        case NPCID.PrimeSaw:
                        case NPCID.PrimeVice:
                            npc.lifeMax = baseHealthHM / 5;
                            break;

                        case NPCID.Probe:
                            npc.lifeMax = baseHealthHM / 50;
                            break;

                        case NPCID.PlanterasHook:
                        case NPCID.PlanterasTentacle:
                            npc.lifeMax = baseHealthHM / 20;
                            break;
                        case NPCID.Spore:
                            npc.lifeMax = baseHealthHM / 40;
                            break;

                        case NPCID.GolemHead:
                        case NPCID.GolemFistLeft:
                        case NPCID.GolemHeadFree:
                            npc.lifeMax = baseHealthHM / 4;
                            break;

                        case NPCID.MoonLordHand:
                        case NPCID.MoonLordHead:
                            npc.lifeMax = baseHealthHM / 4;
                            break;

                        default:
                            validBoss = false;
                            break;
                    }
                }
                if (FargoNPCSets.SwarmHealth[npc.type] != 0)
                {
                    validBoss = true;
                    npc.lifeMax = FargoNPCSets.SwarmHealth[npc.type];
                }

                if (validBoss && Fargowiltas.SwarmItemsUsed > 1)
                {
                    npc.lifeMax *= Fargowiltas.SwarmItemsUsed;
                    SwarmHealth = true;
                }

                int minDamage = Fargowiltas.SwarmMinDamage * 2;
                if (!npc.townNPC && npc.lifeMax > 10 && npc.damage > 0 && npc.damage < minDamage)
                    npc.damage = minDamage;
            }
        }

    }

    public class EnergizedModPlayer : ModPlayer
    {
        public void SetMovement(float runSpeed, int dashType, bool infFlight)
        {
            Player.accRunSpeed = runSpeed;

            if (Player.dashType != 0)
                Player.dashType = dashType;
            Player.empressBrooch = false;
            Player.blockExtraJumps = true;
            if (Player.mount.Active)
                Player.mount.Dismount(Player);
        }
        public void SetMoveStats()
        {
            switch (Fargowiltas.Binding)
            {
                case Binding.None:
                    return;
                case Binding.PreHardmode:
                    SetMovement(6.75f, DashID.ShieldOfCthulhu, false);
                    break;
                case Binding.PreMechs:
                    SetMovement(7.5f, DashID.CrystalAssassin, false);
                    break;
                case Binding.PostMechs:
                    SetMovement(7.5f, DashID.CrystalAssassin, false);
                    break;
                case Binding.PostPlantera:
                    SetMovement(7.5f, DashID.TabiAndMasterNinjaGear, false);
                    break;
                case Binding.PreMoonLord:
                    SetMovement(7.5f, DashID.TabiAndMasterNinjaGear, true);
                    break;
            }
        }
        public override void PostUpdateEquips()
        {
            SetMoveStats();
        }
        public override void PostUpdateRunSpeeds()
        {
            SetMoveStats();
        }
        private static readonly MethodInfo VerticalWingSpeeds_Method = typeof(ItemLoader).GetMethod("VerticalWingSpeeds", FargoUtils.UniversalBindingFlags);
        public delegate void Orig_VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend);
        internal static void VerticalWingSpeeds_Detour(Orig_VerticalWingSpeeds orig, Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            switch (Fargowiltas.Binding)
            {
                case Binding.None:
                    orig(player, ref ascentWhenFalling, ref ascentWhenRising, ref maxCanAscendMultiplier, ref maxAscentMultiplier, ref constantAscend);
                    return;
                case Binding.PreHardmode:
                    break;
                case Binding.PreMechs:
                    break;
                case Binding.PostMechs:
                    break;
                case Binding.PostPlantera:
                    break;
                case Binding.PreMoonLord:
                    break;
            }
        }
        private static readonly MethodInfo HorizontalWingSpeeds_Method = typeof(ItemLoader).GetMethod("HorizontalWingSpeeds", FargoUtils.UniversalBindingFlags);
        public delegate void Orig_HorizontalWingSpeeds(Player player);
        internal static void HorizontalWingSpeeds_Detour(Orig_HorizontalWingSpeeds orig, Player player)
        {
            switch (Fargowiltas.Binding)
            {
                case Binding.None:
                    orig(player);
                    return;
                case Binding.PreHardmode:
                    break;
                case Binding.PreMechs:
                    break;
                case Binding.PostMechs:
                    break;
                case Binding.PostPlantera:
                    break;
                case Binding.PreMoonLord:
                    break;
            }
        }
        internal static WingStats GetWingStats_Detour(On_Player.orig_GetWingStats orig, Player self, int wingID)
        {
            var stats = orig(self, wingID);
            switch (Fargowiltas.Binding)
            {
                case Binding.None:
                    break;
                case Binding.PreHardmode:
                    stats = ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.CreativeWings];
                    break;
                case Binding.PreMechs:
                    stats = ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.HarpyWings];
                    break;
                case Binding.PostMechs:
                    stats = ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FlameWings];
                    break;
                case Binding.PostPlantera:
                    stats = ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.SpectreWings];
                    break;
                case Binding.PreMoonLord:
                    stats = ArmorIDs.Wing.Sets.Stats[ArmorIDs.Wing.FishronWings];
                    break;
            }
            return stats;
        }
        public static int GetHookType()
        {
            return Fargowiltas.Binding switch
            {
                Binding.PreHardmode => ItemID.DiamondHook,
                Binding.PreMechs => ItemID.DualHook,
                Binding.PostMechs => ItemID.DualHook,
                Binding.PostPlantera => ItemID.SpookyHook,
                Binding.PreMoonLord => ItemID.SpookyHook,
                _ => 0
            };
        }
        internal static Item QuickGrapple_GetItemToUse_Detour(On_Player.orig_QuickGrapple_GetItemToUse orig, Player self)
        {
            var item = orig(self);
            int hookOverride = GetHookType();
            if (hookOverride != 0)
            {
                item = ContentSamples.ItemsByType[hookOverride];
            }
            return item;
        }
        public override void Load()
        {
            MonoModHooks.Add(VerticalWingSpeeds_Method, VerticalWingSpeeds_Detour);
            MonoModHooks.Add(HorizontalWingSpeeds_Method, HorizontalWingSpeeds_Detour);
            On_Player.GetWingStats += GetWingStats_Detour;
            On_Player.QuickGrapple_GetItemToUse += QuickGrapple_GetItemToUse_Detour;
        }
    }
}