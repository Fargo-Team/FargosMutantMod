using Fargowiltas.Content.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Buffs
{
    public class WoodDrop : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (!npc.boss && !npc.SpawnedFromStatue)
            {
                npc.buffTime[buffIndex]++;
                npc.GetGlobalNPC<FargoGlobalNPC>().woodDrop = true;
            }
        }
    }
}