using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class AmalgamatedSkull : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<AmalgamatedSkullBuff>();
    }
    public class AmalgamatedSkullBuff : BaseSpawnBoosterBuff
    {
        public AmalgamatedSkullBuff() : base(() => [NPCID.SkeletonSniper, NPCID.TacticalSkeleton, NPCID.SkeletonCommando], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}