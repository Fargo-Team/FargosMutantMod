using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Abom;

public class MatsuriLantern : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = 0; // Places it before any other boss summons

        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;

        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 1;
        Item.value = Item.sellPrice(0, 0, 2);
        Item.rare = ItemRarityID.Blue;
        Item.useAnimation = 30;
        Item.useTime = 30;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.consumable = true; //visually indicate it as consumable
    }

    public override bool ConsumeItem(Player player) => false;

    public override bool CanUseItem(Player player) => !FargoWorld.Matsuri;

    public override bool? UseItem(Player player)
    {
        FargoWorld.Matsuri = true;
        FargoUtils.PrintLocalization("MessageInfo.StartLanternNight", new Color(175, 75, 255));

        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.WorldData);

        Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.position);

        Item.SetDefaults(ModContent.ItemType<SpentLantern>());

        return true;
    }
}