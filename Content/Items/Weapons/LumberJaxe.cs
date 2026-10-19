using Fargowiltas.Content.Achievements;
using Fargowiltas.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.Items.Weapons;

public class LumberJaxe : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 15;
        Item.DamageType = DamageClass.Melee;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 15; //this is tool speed
        Item.useAnimation = 30; //this is swing speed
        Item.axe = 75 / 5;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.value = 5000;
        Item.rare = ItemRarityID.Orange;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;

        Item.useTurn = true;
    }

    public override void UseStyle(Player player, Rectangle heldItemFrame)
    {
        if (Main.myPlayer == player.whoAmI)
        {
            //player.head and etc only check armor slots due to probably update order bullshit, so vanity slots are also checked directly
            if (((player.head == EquipLoader.GetEquipSlot(Mod, "LumberjackMask", EquipType.Head) || player.head == EquipLoader.GetEquipSlot(Mod, "LumberHat", EquipType.Head)) && player.body == EquipLoader.GetEquipSlot(Mod, "LumberjackBody", EquipType.Body) && player.legs == EquipLoader.GetEquipSlot(Mod, "LumberjackPants", EquipType.Legs)) 
                || ((player.armor[10].headSlot == EquipLoader.GetEquipSlot(Mod, "LumberjackMask", EquipType.Head) || player.armor[10].headSlot == EquipLoader.GetEquipSlot(Mod, "LumberHat", EquipType.Head)) && player.armor[11].bodySlot == EquipLoader.GetEquipSlot(Mod, "LumberjackBody", EquipType.Body) && player.armor[12].legSlot == EquipLoader.GetEquipSlot(Mod, "LumberjackPants", EquipType.Legs)))
            {
                ModContent.GetInstance<LumberjaxeAchievement>().Condition.Complete();
            }
        }
        base.UseStyle(player, heldItemFrame);
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffType<WoodDrop>(), 600);
    }
}