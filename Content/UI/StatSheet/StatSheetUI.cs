using Fargowiltas.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using static Fargowiltas.Content.UI.StatSheet.StatRegistry;

namespace Fargowiltas.Content.UI.StatSheet
{
    public class UIStar : UIElement
    {
        int timer;
        float scale;
        Asset<Texture2D> star;

        public UIStar()
        {
            scale = Main.rand.NextFloat(0.1f, 0.3f);
            timer = 0;
            star = Main.Assets.Request<Texture2D>("Images/Projectile_79", AssetRequestMode.ImmediateLoad);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            float scale = this.scale * MathF.Sin(timer / 45f);
            if (timer > 120)
            {
                return;
            }
            else
            {
                Rectangle frame = star.Value.Frame();
                spriteBatch.Draw(star.Value, GetOuterDimensions().Center(), frame, Color.White, 0f, frame.Size() / 2, scale, SpriteEffects.None, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (timer <= 120)
            {
                timer++;
            }
        }
    }

    public class StatSheetUI : FargoUI
    {
        public override bool MenuToggleSound => true;
        public int BackWidth = 650;
        public int BackHeight = 25 * HowManyPerColumn + 26 + 4; //row height * stat rows + search bar + padding
        public const int HowManyPerColumn = 14;
        public const int HowManyColumns = 2;

        public int LerpTimer = 0;
        public int LineCounter;
        public int ColumnCounter;
        public List<StatCategory> allCategories = [];
        public List<StatCategory> selectedCategories = [];
        public bool InUpgradePanel = false;

        public UIElement Categories;
        public UIDragablePanel BackPanel;
        public UIPanel InnerPanel;
        public UICloseButton CloseButton;
        public UIPanel UpgradePanel;

        public override void OnLoad()
        {
            CombinedUI.AddUI<StatSheetUI>(Language.GetText("Mods.Fargowiltas.UI.StatSheet.CombinedUI"), 1); // TODO: localize this
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

            InnerPanel = new UIPanel();
            InnerPanel.Left.Set(6, 0f);
            InnerPanel.Top.Set(6 + 38, 0f); // 28 for search bar
            InnerPanel.Width.Set(BackWidth - 12, 0f);
            InnerPanel.Height.Set(BackHeight - 12 - 28, 0);
            InnerPanel.PaddingLeft = InnerPanel.PaddingRight = InnerPanel.PaddingTop = InnerPanel.PaddingBottom = 0;
            InnerPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
            BackPanel.Append(InnerPanel);

            Categories = new UIElement();
            Categories.Width.Set(BackWidth - 30, 0);
            Categories.Height.Set(40, 0);
            Categories.Left.Set(12, 0f);
            Categories.Top.Set(2, 0f); // 6 so padding lines up
            BackPanel.Append(Categories);

            CloseButton = new UICloseButton();
            CloseButton.Left.Set(-24, 1f);
            CloseButton.Top.Set(2, 0);
            CloseButton.OnLeftClick += CloseButton_OnLeftClick;
            BackPanel.Append(CloseButton);

            base.OnInitialize();
        }

        public void OnCategorySelect(StatCategory category)
        {
            if (!selectedCategories.Remove(category))
            {
                if (category.Name == "PermaUpgrade")
                {
                    SoundEngine.PlaySound(SoundID.Item176 with { Pitch = -0.5f });
                    selectedCategories.Clear();
                }
                else
                    selectedCategories.Remove(GetCategory("PermaUpgrade"));
                selectedCategories.Add(category);
            }

            RebuildStatList();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Main.GameUpdateCount % 4 == 0) // 15 times a second
            {
                RebuildStatList();
            }
        }

        public void RebuildStatList()
        {
            Player player = Main.LocalPlayer;
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();


            ColumnCounter = LineCounter = 0;

            Categories.RemoveAllChildren();

            int count = 0;
            foreach (StatCategory category in GetCategories())
            {
                if (!category.Condition.Invoke())
                    continue;

                bool selected = selectedCategories.Contains(category);
                var button = new CategoryButton(category, selected);
                button.onPress = OnCategorySelect;
                button.Width.Set(42, 0);
                button.Height.Set(40, 0);
                button.Top.Set(0, 0);
                button.Left.Set(6 + 44 * count, 0);

                Categories.Append(button);
                count++;
            }

            InnerPanel.BackgroundColor = new Color(65, 84, 153, 229);
            if (selectedCategories.Contains(GetCategory("PermaUpgrade")))
            {
                LerpTimer++;
                InnerPanel.BackgroundColor = Color.Lerp(new Color(65, 84, 153, 229), Color.Lerp(Color.Purple, Color.Black, 0.8f), LerpTimer / 15f);
                if (LerpTimer >= 20 && LerpTimer % 8 == 0)
                    SpawnStar();
                if (!InUpgradePanel)
                    RebuildUpgrades();
                return;
            }
            if (InUpgradePanel)
            {
                LerpTimer = 0;
                InUpgradePanel = false;
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
            InnerPanel.RemoveAllChildren();

            List<StatCategory> categories = GetCategories();

            foreach (StatCategory category in categories)
            {

                if (!category.Condition.Invoke() || category.Name == "PermaUpgrade")
                    continue;

                if (selectedCategories.Count > 0 && !selectedCategories.Contains(category))
                    continue;

                AddHeader(category);

                foreach (Stat stat in category.Stats)
                {
                    if (stat.condition.Invoke())
                        AddStat(stat.Name, stat.ModName, stat.Value.Invoke());
                }
            }
        }

        public void RebuildUpgrades()
        {
            InnerPanel.RemoveAllChildren();

            var header = new StatSheetHeader("PermaUpgrade", Language.GetTextValue($"Mods.Fargowiltas.UI.StatSheet.PermaUpgrade"));
            header.Top.Set(10, 0);
            header.Left.Set(0, 0);
            header.Width.Set(0, 0.95f);
            header.Height.Set(36, 0);
            header.HAlign = 0.5f;

            InnerPanel.Append(header);

            float count = 0;
            foreach (PermaUpgrade upgrade in Fargowiltas.Instance.PermaUpgrades)
            {
                var permaItem = new PermaItem(upgrade.Item.type, upgrade.ConsumedBool);
                permaItem.Width.Set(50, 0);
                permaItem.Height.Set(50, 0);
                permaItem.Top.Set(56 + MathF.Floor(count / 12f) * 52, 0);
                permaItem.Left.Set(8 + (count % 12) * 52, 0);

                InnerPanel.Append(permaItem);

                BackHeight = 50 * ((int)MathF.Floor(count / 12f) + 1) + 80 + 34;

                BackPanel.Height.Set(BackHeight, 0f);
                InnerPanel.Height.Set(BackHeight - 12 - 40, 0);

                count++;
            }
            InUpgradePanel = true;
        }

        public void SpawnStar()
        {
            var star = new UIStar();
            star.Width.Set(50, 0);
            star.Height.Set(50, 0);
            star.Top.Set(Main.rand.NextFloat(InnerPanel.GetInnerDimensions().Height - 50), 0);
            star.Left.Set(Main.rand.NextFloat(InnerPanel.GetInnerDimensions().Width - 50), 0);

            InnerPanel.Append(star);
        }

        public void AddStat(string key, string modName = "Fargowiltas", params object[] args) => AddStat(Language.GetTextValue($"Mods.{modName}.UI.StatSheet.{key}", args), key, modName);

        public void AddStat(string text, string key, string modName = "Fargowiltas")
        {
            int left = 8 + ColumnCounter * ((BackWidth - 8) / HowManyColumns);
            int top = 8 + LineCounter * 23; // I don't know why but 23 works perfectly

            //this is before linecounter++ to display correctly:
            BackHeight = 23 * (LineCounter + 1) + 26 + 44; //row height * stat rows + search bar + padding

            if (++ColumnCounter == HowManyColumns)
            {
                LineCounter++;
                ColumnCounter = 0;
            }

            UIText ui = new UIText(key != null ? $"[s:{modName}/{key}] {text}" : text);
            ui.Left.Set(left, 0f);
            ui.Top.Set(top, 0f);

            BackPanel.Height.Set(BackHeight, 0f);
            InnerPanel.Height.Set(BackHeight - 12 - 40, 0);

            InnerPanel.Append(ui);
        }

        public void AddHeader(StatCategory category)
        {
            if (ColumnCounter != 0)
            {
                LineCounter++;
                ColumnCounter = 0;
            }

            int left = 8 + ColumnCounter * ((BackWidth - 8) / HowManyColumns);
            int top = 8 + LineCounter * 23; // I don't know why but 23 works perfectly

            //this is before linecounter++ to display correctly:
            BackHeight = 23 * (LineCounter + 1) + 26 + 26;

            var panel = new StatSheetHeader(category.Name, Language.GetTextValue(category.HeaderLocalPath));
            panel.Left.Set(0, 0f);
            panel.Top.Set(top, 0f);
            panel.Width.Set(0, 0.95f);
            panel.HAlign = 0.5f;
            panel.Height.Set(36, 0);

            InnerPanel.Append(panel);

            BackPanel.Height.Set(BackHeight, 0f);
            InnerPanel.Height.Set(BackHeight - 12 - 28, 0);

            LineCounter += 2;
            ColumnCounter = 0;
        }

        /*public void SetPositionToPoint(Point point)
        {
            BackPanel.Left.Set(point.X, 0f);
            BackPanel.Top.Set(point.Y, 0f);
        }

        public Point GetPositinAsPoint() => new Point((int)BackPanel.Left.Pixels, (int)BackPanel.Top.Pixels);*/

        private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            // cheesy fix for dislocation on close
            (listeningElement.Parent as UIDragablePanel).DragEnd(Main.MouseScreen);
            FargoUIManager.Close<StatSheetUI>();
        }
    }
}