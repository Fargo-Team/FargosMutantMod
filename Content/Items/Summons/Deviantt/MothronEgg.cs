using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class MothronEgg : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<MothronEggBuff>();
    }
    public class MothronEggBuff : BaseSpawnBoosterBuff
    {
        public MothronEggBuff() : base(() => [NPCID.Mothron], () => Main.eclipse && NPC.downedPlantBoss && (Main.LocalPlayer.ZoneOverworldHeight || (Main.remixWorld && Main.LocalPlayer.ZoneRockLayerHeight)), 0.2f)
        {
        }
    }
}