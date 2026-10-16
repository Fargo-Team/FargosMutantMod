using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Vanity;

public class CrabSizedGlasses : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToVanitypet(ModContent.ProjectileType<CoolCrab>(), ModContent.BuffType<CoolCrabBuff>());
        Item.rare = ItemRarityID.Green;
        Item.width = 20;
        Item.height = 40;
        Item.value = Item.sellPrice(0, 0, 0, 10);
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            player.AddBuff(Item.buffType, 3600);
        }
        return true;
    }
}