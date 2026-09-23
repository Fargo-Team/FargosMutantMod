using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles;

public class SpawnProj : ModProjectile
{
    private readonly int[] bosses = [NPCID.KingSlime, NPCID.EyeofCthulhu, NPCID.EaterofWorldsHead, NPCID.BrainofCthulhu, NPCID.QueenBee, NPCID.Deerclops, NPCID.SkeletronHead, NPCID.TheDestroyer, NPCID.SkeletronPrime, NPCID.Retinazer, NPCID.Spazmatism, NPCID.Plantera, NPCID.Golem, NPCID.DukeFishron, NPCID.CultistBoss, NPCID.MoonLordCore, NPCID.QueenSlimeBoss, NPCID.HallowBoss];

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Spawn");
    }

    public override void SetDefaults()
    {
        Projectile.width = 2;
        Projectile.height = 2;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.hide = true;
    }

    public override bool? CanDamage()
    {
        return false;
    }

    public override void OnKill(int timeLeft)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        if ((int)Projectile.ai[0] == NPCID.CultistBoss && NPC.downedAncientCultist)
        {
            // Lunatic Cultist
            int npc = NPC.NewNPC(new EntitySource_BossSpawn(Main.LocalPlayer, "PreventLunarPillars"), (int)Projectile.Center.X, (int)Projectile.Center.Y, (int)Projectile.ai[0]);
        }
        else if (Projectile.ai[0] == NPCID.PirateShip && FargoWorld.EternityMode) //eternity mode despawns pirate ships without ai[0] set to stop natural spawns
        {
            int n = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)Projectile.Center.X, (int)Projectile.Center.Y, (int)Projectile.ai[0], 0, 1);
        }
        else
        {
            int n = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)Projectile.Center.X, (int)Projectile.Center.Y, (int)Projectile.ai[0]);
        }
    }
}