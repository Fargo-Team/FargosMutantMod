using Fargowiltas.Content.Items.Misc;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public class StatSheetUI : FargoUI
    {
        public override bool MenuToggleSound => true;
        public int BackWidth = 650;
        public int BackHeight = 25 * HowManyPerColumn + 26 + 4; //row height * stat rows + search bar + padding
        public const int HowManyPerColumn = 14;
        public const int HowManyColumns = 2;

        public int LineCounter;
        public int ColumnCounter;

        public UISearchBar SearchBar;
        public UIDragablePanel BackPanel;
        public UIPanel InnerPanel;

        public struct Stat
        {
            public int ItemID;
            public Func<string> TextFunction;

            public Stat(int itemID, Func<string> textFunction)
            {
                ItemID = itemID;
                TextFunction = textFunction;
            }
        }

        public struct PermaUpgrade
        {
            public Item Item;
            public Func<bool> ConsumedBool;

            public PermaUpgrade(Item item, Func<bool> consumedBool)
            {
                Item = item;
                ConsumedBool = consumedBool;
            }
        }
        public override void OnLoad()
        {
            CombinedUI.AddUI<StatSheetUI>(Language.GetText("Mods.Fargowiltas.UI.StatSheet"), 1); // TODO: localize this
        }
        public override void OnInitialize()
        {
            Vector2 baseOffset = CombinedUI.CenterRight;
            Vector2 offset = new(baseOffset.X, baseOffset.Y - BackHeight / 2);

            BackPanel = new UIDragablePanel();
            BackPanel.Left.Set(offset.X, 0f);
            BackPanel.Top.Set(offset.Y, 0f);
            BackPanel.Width.Set(BackWidth, 0f);
            BackPanel.Height.Set(BackHeight, 0f);
            BackPanel.PaddingLeft = BackPanel.PaddingRight = BackPanel.PaddingTop = BackPanel.PaddingBottom = 0;
            BackPanel.BackgroundColor = new Color(29, 33, 70) * 0.7f;
            Append(BackPanel);

            SearchBar = new UISearchBar(BackWidth - 8, 26);
            SearchBar.Left.Set(4, 0f);
            SearchBar.Top.Set(6, 0f); // 6 so padding lines up
            BackPanel.Append(SearchBar);

            InnerPanel = new UIPanel();
            InnerPanel.Left.Set(6, 0f);
            InnerPanel.Top.Set(6 + 28, 0f); // 28 for search bar
            InnerPanel.Width.Set(BackWidth - 12, 0f);
            InnerPanel.Height.Set(BackHeight - 12 - 28, 0);
            InnerPanel.PaddingLeft = InnerPanel.PaddingRight = InnerPanel.PaddingTop = InnerPanel.PaddingBottom = 0;
            InnerPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
            BackPanel.Append(InnerPanel);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Main.GameUpdateCount % (!SearchBar.IsEmpty ? 2 : 4) == 0) // 15 times a second, or 30 times a second if searchbar has text
            {
                RebuildStatList();
            }
        }

        public void RebuildStatList()
        {
            Player player = Main.LocalPlayer;
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            InnerPanel.RemoveAllChildren();
            ColumnCounter = LineCounter = 0;

            double Damage(DamageClass damageClass) => Math.Round(player.GetTotalDamage(damageClass).Additive * player.GetTotalDamage(damageClass).Multiplicative * 100 - 100);
            int Crit(DamageClass damageClass) => (int)player.GetTotalCritChance(damageClass);

            AddStat("MeleeDamage", ItemID.CopperBroadsword, Damage(DamageClass.Melee));
            AddStat("MeleeCritical", ItemID.CopperBroadsword, Crit(DamageClass.Melee));
            AddStat("MeleeSpeed", ItemID.CopperBroadsword, (int)Math.Round(player.GetAttackSpeed(DamageClass.Melee) * 100));
            AddStat("RangedDamage", ItemID.CopperBow, Damage(DamageClass.Ranged));
            AddStat("RangedCritical", ItemID.CopperBow, Crit(DamageClass.Ranged));
            AddStat("MagicDamage", ItemID.WandofSparking, Damage(DamageClass.Magic));
            AddStat("MagicCritical", ItemID.WandofSparking, Crit(DamageClass.Magic));
            AddStat("ManaCostReduction", ItemID.WandofSparking, Math.Round((1.0 - player.manaCost) * 100));
            AddStat("SummonDamage", ItemID.SlimeStaff, Damage(DamageClass.Summon));
            if (Fargowiltas.ModLoaded["FargowiltasSouls"])
                AddStat("SummonCritical", ItemID.SlimeStaff, (int)ModLoader.GetMod("FargowiltasSouls").Call("GetSummonCrit"));
            else
                AddStat("");
            AddStat("MaxMinions", ItemID.SlimeStaff, player.maxMinions);
            AddStat("MaxSentries", ItemID.SlimeStaff, player.maxTurrets);

            if (Fargowiltas.ModLoaded["FargowiltasSouls"])
                AddStat("AttackSpeed", ModLoader.GetMod("FargowiltasSouls").Find<ModItem>("MythrilEnchant").Type, (int)Math.Round((float)ModLoader.GetMod("FargowiltasSouls").Call("GetAttackSpeed") * 100));
            else
                AddStat("");

            AddStat("ArmorPenetration", ItemID.SharkToothNecklace, player.GetArmorPenetration(DamageClass.Generic));
            AddStat("Aggro", ItemID.FleshKnuckles, player.aggro);


            AddStat("Life", ItemID.LifeCrystal, player.statLifeMax2);
            AddStat("LifeRegen", ItemID.BandofRegeneration, player.lifeRegen / 2);
            AddStat("Mana", ItemID.ManaCrystal, player.statManaMax2);
            AddStat("ManaRegen", ItemID.ManaCrystal, player.manaRegen / 2);
            AddStat("Defense", ItemID.CobaltShield, player.statDefense);
            float endurance = player.endurance;
            if (FargoUtils.EternityMode) // DR scaling shows on stat sheet
            {
                float r = 0.15f;
                if (endurance >= r)
                    endurance = 1 - MathF.Pow(1 - r, endurance / r);
            }
            AddStat("DamageReduction", ItemID.WormScarf, Math.Round(endurance * 100));
            AddStat("Luck", ItemID.Torch, Math.Round(player.luck, 2));
            AddStat("FishingQuests", ItemID.AnglerEarring, player.anglerQuestsFinished);
            AddStat("BattleCry", ModContent.ItemType<BattleCry>(), modPlayer.BattleCry ? $"[c/ff0000:{Language.GetTextValue("Mods.Fargowiltas.Items.BattleCry.Battle")}]" : 
                modPlayer.CalmingCry ? $"[c/00ffff:{Language.GetTextValue("Mods.Fargowiltas.Items.BattleCry.Calming")}]" : Language.GetTextValue("Mods.Fargowiltas.UI.BattleCryNone"));
            AddStat("MaxSpeed", ItemID.HermesBoots, (int)((player.accRunSpeed + player.maxRunSpeed) / 2f * player.moveSpeed * 3));

            string RenderWingStat(double stat) => stat <= 0 ? Language.GetTextValue("Mods.Fargowiltas.UI.WingNull") : stat.ToString();
            AddStat("WingTime", ItemID.AngelWings, player.wingTimeMax / 60 > 60 || player.empressBrooch && !Fargowiltas.ModLoaded["CalamityMod"] ? 
                Language.GetTextValue("Mods.Fargowiltas.UI.WingTimeMoreThan60Sec") : Language.GetTextValue("Mods.Fargowiltas.UI.WingTimeActual", RenderWingStat(Math.Round(player.wingTimeMax / 60.0, 2))));
            AddStat("WingMaxSpeed", ItemID.AngelWings, RenderWingStat(Math.Round(modPlayer.StatSheetWingSpeed * 32 / 6.25)));
            AddStat("WingAscentModifier", ItemID.AngelWings, RenderWingStat(Math.Round(modPlayer.StatSheetMaxAscentMultiplier * 100)));
            AddStat("WingHover", ItemID.AngelWings, modPlayer.CanHover == null ? Language.GetTextValue("Mods.Fargowiltas.UI.WingNull") :
                (bool)modPlayer.CanHover ? Language.GetTextValue("Mods.Fargowiltas.UI.WingHoverTrue") : Language.GetTextValue("Mods.Fargowiltas.UI.WingHoverFalse"));

            foreach (Stat stat in Fargowiltas.Instance.ModStats)
            {
                AddStat(stat.TextFunction.Invoke(), stat.ItemID);
            }

            foreach (PermaUpgrade upgrade in Fargowiltas.Instance.PermaUpgrades)
            {
                if (upgrade.ConsumedBool.Invoke())
                    AddStat(upgrade.Item.Name, upgrade.Item.type);         
            }
        }

        public void AddStat(string key, int item = -1, params object[] args) => AddStat(Language.GetTextValue($"Mods.Fargowiltas.UI.{key}", args), item);

        public void AddStat(string text, int item = -1)
        {
            int left = 8 + ColumnCounter * ((BackWidth - 8) / HowManyColumns);
            int top = 8 + LineCounter * 23; // I don't know why but 23 works perfectly

            //this is before linecounter++ to display correctly:
            BackHeight = 25 * (LineCounter + 1) + 26 + 4; //row height * stat rows + search bar + padding

            if (++ColumnCounter == HowManyColumns)
            {
                LineCounter++;
                ColumnCounter = 0;
            }

            UIText ui = new UIText(item > -1 ? $"[i:{item}] {text}" : text);
            ui.Left.Set(left, 0f);
            ui.Top.Set(top, 0f);

            string[] words = text.Split(' ');
            if (!SearchBar.IsEmpty)
            {
                if (words.Any(s => s.StartsWith(SearchBar.Input, StringComparison.OrdinalIgnoreCase)))
                {
                    float fade = MathHelper.Lerp(0.1f, 0.9f, (float)(Math.Sin(Main.GameUpdateCount / 10f) + 1f) / 2f);
                    Color color = Color.Lerp(Color.Yellow, Color.Goldenrod, fade);
                    ui.TextColor = color;
                }
                else
                    // Gray out text when filtered by search
                    ui.TextColor = Color.Gray * 1.5f;
            }

            

            BackPanel.Height.Set(BackHeight, 0f);
            InnerPanel.Height.Set(BackHeight - 12 - 28, 0);

            InnerPanel.Append(ui);
        }

        /*public void SetPositionToPoint(Point point)
        {
            BackPanel.Left.Set(point.X, 0f);
            BackPanel.Top.Set(point.Y, 0f);
        }

        public Point GetPositinAsPoint() => new Point((int)BackPanel.Left.Pixels, (int)BackPanel.Top.Pixels);*/
    }
}