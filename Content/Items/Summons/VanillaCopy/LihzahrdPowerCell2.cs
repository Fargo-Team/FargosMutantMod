using Terraria;
using Terraria.ID;

namespace Fargowiltas.Content.Items.Summons.VanillaCopy;

public class LihzahrdPowerCell2 : BaseSummon
{
    public override int NPCType => NPCID.Golem;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        // DisplayName.SetDefault("Lihzahrd Battery Pack");
        // Tooltip.SetDefault("Summons the Golem without an altar");

        ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.LihzahrdPowerCell]; // 16
    }

    public override bool CanUseItem(Player player)
    {
        return NPC.downedPlantBoss;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
           .AddIngredient(ItemID.LihzahrdPowerCell)
           .AddTile(TileID.WorkBenches)
           .Register();
    }
}