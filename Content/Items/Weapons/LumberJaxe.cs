using Fargowiltas.Content.Achievements;
using Fargowiltas.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.Items.Weapons
{
    public class LumberJaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.axe = 30;
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
                if (player.head == EquipLoader.GetEquipSlot(Mod, "LumberjackMask", EquipType.Head) && player.body == EquipLoader.GetEquipSlot(Mod, "LumberjackBody", EquipType.Body) && player.legs == EquipLoader.GetEquipSlot(Mod, "LumberjackPants", EquipType.Legs))
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
}