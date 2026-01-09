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
        }
        public override void AI()
        {
            NPC.life = NPC.lifeMax = maxHP;
            NPC.spriteDirection = NPC.direction;
            if (FargoGlobalNPC.AnyBossAlive())
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.SimpleStrikeNPC(int.MaxValue, 0, false, 0, null, false, 0, true);
            }
        }

        public override bool CheckDead()
        {
            NPC.life = NPC.lifeMax = maxHP;
            return false;
        }
    }
}
