using Fargowiltas.Common;
using Fargowiltas.Content.NPCs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Misc
{
    public class AutoSummoner : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.value = Item.sellPrice(0, 1);
            Item.rare = ItemRarityID.Blue;
        }

        public static void PassiveEffect(Player player, Item item)
        {
            player.FargoMutant().AutoSummon = true;
        }

        public override void UpdateInventory(Player player) => PassiveEffect(player, Item);
        public override void UpdateVanity(Player player) => PassiveEffect(player, Item);
        public override void UpdateEquip(Player player) => PassiveEffect(player, Item);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GizmoParts>(4)
                .AddIngredient(ItemID.RichMahogany, 10)
                .AddIngredient(ItemID.Bone, 4)
                .AddIngredient(ItemID.ManaCrystal)
                .AddIngredient(ItemID.SummoningPotion, 5)
                .AddTile(TileID.BewitchingTable)
                .Register();
        }

        public static void TryAutoSummoner(Player player)
        {
            FargoPlayer fargoPlayer = player.FargoMutant();

            if (player.whoAmI != Main.myPlayer)
                return;

            if (!fargoPlayer.AutoSummon)
                return;

            if (++fargoPlayer.AutoSummonCD < 30)
                return;

            fargoPlayer.AutoSummonCD = 0;

            if (FargoUtils.AnyBossAlive())
            {
                //during boss, can only summon so many times and then no more
                if (fargoPlayer.AutoSummonCap <= 0)
                    return;
            }

            int weaponsUsed = 0;

            //only way to check if max sentries have been reached graaaaah
            int sentrycount = 0;
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.WipableTurret)
                    sentrycount++;
            }

            for (int i = 0; i < 10; i++) //hotbar
            {
                Item item = player.inventory[i];

                if (item != null && item.DamageType == DamageClass.Summon && item.damage > 0 && item.shoot > ProjectileID.None && item.ammo <= 0 && !item.channel
                    && ((ContentSamples.ProjectilesByType[item.shoot].minion && ItemID.Sets.StaffMinionSlotsRequired[item.type] <= player.maxMinions - player.slotsMinions)
                    || (item.sentry && ContentSamples.ProjectilesByType[item.shoot].sentry && sentrycount < player.maxTurrets && !DD2Event.Ongoing)))
                {
                    if (!player.HasAmmo(item) || (item.mana > 0 && player.statMana < item.mana))
                        continue;

                    if (!PlayerLoader.CanUseItem(player, item) || !ItemLoader.CanUseItem(item, player))
                        continue;

                    weaponsUsed++;
                    if (weaponsUsed > 1)
                        break;

                    int damage = player.GetWeaponDamage(item);

                    int itemtime = player.itemTime;
                    int itemtimemax = player.itemTimeMax;
                    int reusedelay = player.reuseDelay;
                    int direction = player.direction;
                    FargoPlayer.AutoSummonShootMethod.Invoke(player, [player.whoAmI, item, damage]); // all the OnSpawn stuff already runs here
                    player.itemTime = itemtime;
                    player.itemTimeMax = itemtimemax;
                    player.reuseDelay = reusedelay;
                    player.direction = direction;
                    player.AddBuff(item.buffType, 3600);

                    fargoPlayer.AutoSummonCap -= ItemID.Sets.StaffMinionSlotsRequired[item.type];

                    SoundEngine.PlaySound(item.UseSound);

                    if (item.mana > 0)
                    {
                        if (player.CheckMana(item.mana / 2, true, false))
                        {
                            player.manaRegenDelay = 300;
                        }
                    }
                    if (item.consumable)
                    {
                        item.stack--;
                    }

                    break;
                }
            }

            float minionsLeft = player.maxMinions - player.slotsMinions;
            fargoPlayer.AutoSummonCap = FargoUtils.AnyBossAlive()
                ? Math.Min(fargoPlayer.AutoSummonCap, minionsLeft) : minionsLeft;
        }
    }
}
