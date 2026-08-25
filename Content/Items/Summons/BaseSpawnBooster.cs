using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons;

public abstract class BaseSpawnBooster : ModItem
{
    public abstract int BuffType { get; }

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        ItemID.Sets.SortingPriorityBossSpawns[Type] = 0; // Places it before any other boss summons
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 9999;
        Item.value = Item.sellPrice(0, 0, 2);
        Item.rare = ItemRarityID.Blue;
        Item.useAnimation = 30;
        Item.useTime = 30;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.consumable = true;
        Item.buffType = BuffType;
        Item.buffTime = 60 * 60 * 4;
        Item.UseSound = SoundID.Item2;
    }
}