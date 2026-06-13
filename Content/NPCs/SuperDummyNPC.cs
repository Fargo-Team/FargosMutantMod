using Fargowiltas.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.NPCs
{
    public class SuperDummyNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);

            Main.npcFrameCount[Type] = 11;
        }
        const int maxHP = int.MaxValue / 10;
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.TargetDummy);
            NPC.lifeMax = maxHP;
            NPC.aiStyle = -1;
            NPC.width = 28;
            NPC.height = 50;
            NPC.immortal = false;
            NPC.npcSlots = 0;
            NPC.dontCountMe = true;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = maxHP;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
        public override void OnSpawn(IEntitySource source)
        {
            NPC.life = NPC.lifeMax = maxHP;
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
        }
        public override void AI()
        {
            NPC.life = NPC.lifeMax = maxHP;
            NPC.spriteDirection = NPC.direction;
            DrawOffsetY = -2;
            if (FargoUtils.AnyBossAlive())
            {
                NPC.active = false;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            NPC.localAI[0] = hit.Damage;
            if (NPC.localAI[0] < 20f)
                NPC.localAI[0] = 20f;

            if (NPC.localAI[0] > 120f)
                NPC.localAI[0] = 120f;

            NPC.localAI[1] = hit.HitDirection;
        }

        public override void FindFrame(int frameHeight)
        {
            int hitdirection = (int)NPC.localAI[1] * -NPC.direction;

            if (NPC.localAI[0] > 24f)
                NPC.localAI[0] = 24f;

            if (NPC.localAI[0] > 0f)
                NPC.localAI[0] -= 1f;

            if (NPC.localAI[0] < 0f)
                NPC.localAI[0] = 0f;

            int framecounter = (hitdirection == -1) ? 4 : 6;
            int frame = (int)NPC.localAI[0] / framecounter;
            if (NPC.localAI[0] % framecounter != 0f)
                frame++;

            if (frame != 0 && hitdirection == 1)
                frame += 5;

            NPC.frame.Y = frame * frameHeight;
        }

        public override bool CheckDead()
        {
            NPC.life = NPC.lifeMax = maxHP;
            return false;
        }
    }
}
