using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.NPCs
{
    public class EnergizedGlobalNPC : GlobalNPC
    {
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
            const int m = k * k;
            int baseHealth = 18 * k;
            int baseHealthHM = 48 * k;
            bool validBoss = true;
            if (Fargowiltas.SwarmSetDefaults)
            {
                switch (npc.type)
                {
                    case NPCID.KingSlime:
                        npc.lifeMax = baseHealth;
                        break;

                    case NPCID.EyeofCthulhu:
                        npc.lifeMax = baseHealth;
                        break;

                    case NPCID.EaterofWorldsHead:
                        npc.lifeMax = baseHealth / 12;
                        break;

                    case NPCID.BrainofCthulhu:
                        npc.lifeMax = (int)(baseHealth / 2.5f);
                        break;

                    case NPCID.DD2DarkMageT1:
                        npc.lifeMax = (int)(baseHealth / 1.5f);
                        break;

                    case NPCID.Deerclops:
                        npc.lifeMax = baseHealth;
                        break;

                    case NPCID.QueenBee:
                        npc.lifeMax = baseHealth;
                        break;

                    case NPCID.SkeletronHead:
                        npc.lifeMax = baseHealth / 2;
                        break;

                    case NPCID.WallofFlesh:
                        npc.lifeMax = baseHealth;
                        break;

                    case NPCID.QueenSlimeBoss:
                        npc.lifeMax = (int)(baseHealthHM * 0.6f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        break;

                    case NPCID.TheDestroyer:
                        npc.lifeMax = (int)(baseHealthHM * 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        break;

                    case NPCID.Retinazer:
                        npc.lifeMax = baseHealthHM / 2;
                        Fargowiltas.HardmodeSwarmActive = true;
                        break;

                    case NPCID.Spazmatism:
                        npc.lifeMax = baseHealthHM / 2;
                        Fargowiltas.HardmodeSwarmActive = true;
                        break;

                    case NPCID.SkeletronPrime:
                        npc.lifeMax = (int)(baseHealthHM / 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        break;

                    case NPCID.Plantera:
                        npc.lifeMax = baseHealthHM / 2;
                        Fargowiltas.HardmodeSwarmActive = true;
                        break;

                    case NPCID.Golem:
                        npc.lifeMax = baseHealthHM / 6;
                        Fargowiltas.HardmodeSwarmActive = true;
                        break;

                    case NPCID.DD2Betsy:
                        npc.lifeMax = (int)(baseHealthHM / 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        break;

                    case NPCID.DukeFishron:
                        npc.lifeMax = (int)(baseHealthHM / 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.LateHardmodeSwarmActive = true;
                        break;

                    case NPCID.HallowBoss:
                        npc.lifeMax = (int)(baseHealthHM / 1.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.LateHardmodeSwarmActive = true;
                        break;

                    case NPCID.CultistBoss:
                        npc.lifeMax = baseHealthHM / 4;
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.LateHardmodeSwarmActive = true;
                        break;

                    case NPCID.MoonLordCore:
                        npc.lifeMax = (int)(baseHealthHM / 2.5f);
                        Fargowiltas.HardmodeSwarmActive = true;
                        Fargowiltas.LateHardmodeSwarmActive = true;
                        break;

                    case NPCID.DungeonGuardian:
                        npc.lifeMax += 100 * Fargowiltas.SwarmItemsUsed;
                        validBoss = false;
                        Fargowiltas.SwarmNoHyperActive = true;
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
                if (FargoSets.NPCs.SwarmHealth[npc.type] != 0)
                {
                    validBoss = true;
                    npc.lifeMax = FargoSets.NPCs.SwarmHealth[npc.type];
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
        private int go = 1;

        public override bool PreAI(NPC npc)
        {
            if (Fargowiltas.SwarmNoHyperActive)
                return true;
            if (Fargowiltas.LateHardmodeSwarmActive && Main.GameUpdateCount % 3 == 0)
                return true;
            if (Fargowiltas.HardmodeSwarmActive && Main.GameUpdateCount % 2 == 0)
                return true;

            if (npc.type == NPCID.MoonLordFreeEye)
                return true;

            if (Fargowiltas.SwarmActive && !npc.townNPC && npc.lifeMax > 1 && go < 2)
            {
                go++;
                npc.AI();
                float speedToAdd = 0.5f;
                Vector2 newPos = npc.position + npc.velocity * speedToAdd;
                if (!Collision.SolidCollision(newPos, npc.width, npc.height))
                {
                    npc.position = newPos;
                }
            }
            return true;
        }

        public override void PostAI(NPC npc)
        {
            if (go == 2)
            {
                go = 1;
            }
        }
    }
}