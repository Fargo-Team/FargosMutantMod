using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt;

public class PinkSlimeCrown : BaseSpawnBooster
{
    public override int BuffType => ModContent.BuffType<PinkSlimeCrownBuff>();

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.value = Item.sellPrice(0, 0, 2);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Gel, 20)
            .AddIngredient(ItemID.PinkDye)
            .AddTile(TileID.DyeVat)
            .Register();
    }
}
public class PinkSlimeCrownBuff : BaseSpawnBoosterBuff
{
    //buff itself does not increase spawn rates the normal way since pinkies spawn by replacing blue slimes
    public PinkSlimeCrownBuff() : base(() => [NPCID.Pinky], () => !Main.LocalPlayer.ZoneUnderworldHeight, 1f)
    {
    }
}

public class PinkSlimeCrownGlobal : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => lateInstantiation && entity.type == NPCID.BlueSlime;
    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        int whoAmI = npc.FindClosestPlayer();
        if (whoAmI == -1)
            return;
        Player player = Main.player[whoAmI];

        if (source is EntitySource_SpawnNPC && npc.lastInteraction == 255 && !npc.SpawnedFromStatue && player != null && player.HasBuff(ModContent.BuffType<PinkSlimeCrownBuff>()))
        {
            //vanilla chance is RollLuck(180), 4 is extra generous to make sure multiple spawn from one use
            if (player.RollLuck(4) == 0)
            {
                npc.Transform(-4);
            }
        }

    }
}