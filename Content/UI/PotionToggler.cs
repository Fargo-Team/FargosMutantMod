using Fargowiltas.Assets.Textures;
using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public class PotionToggler : FargoUI
    {
        public override bool MenuToggleSound => true;
        public override string InterfaceLayerName => "Fargos: Potion Toggler";
        public override int InterfaceIndex(List<GameInterfaceLayer> layers, int vanillaInventoryIndex) => vanillaInventoryIndex - 1;

        public readonly static Regex RemoveItemTags = new(@"\[[^\[\]]*\]");

        public bool NeedsToggleListBuilding;
        public string DisplayMod;
        public string SortCategory;

        public const int BackWidth = 400;
        public const int BackHeight = 658;

        public UIDragablePanel BackPanel;
        public UIPanel InnerPanel;
        public UIPanel PresetPanel;
        public UIScrollbarClamped Scrollbar;
        public UIToggleList ToggleList;
        public UISearchBar SearchBar;
        public UICloseButton CloseButton;

        public FargoUIPresetButton OffButton;
        public FargoUIPresetButton OnButton;
        public FargoUIPresetButton MinimalButton;
        public FargoUIPresetButton SomeEffectsButton;
        public FargoUIPresetButton[] CustomButton = new FargoUIPresetButton[3];
        public FargoUIDisplayAllButton DisplayAllButton;
        //public FargoUIReloadButton ReloadButton;

        public override void OnLoad()
        {
            CombinedUI.AddUI<PotionToggler>(Language.GetText("Mods.Fargowiltas.UI.PotionToggler"), 2);
        }
        public override void UpdateUI()
        {
            if (!Main.playerInventory && FargoClientConfig.Instance.HideTogglerWhenInventoryIsClosed)
                FargoUIManager.Close<PotionToggler>();
        }
        public override void OnOpen()
        {
            NeedsToggleListBuilding = true;
        }
        public override void OnClose()
        {
            if (FargoClientConfig.Instance.ToggleSearchReset)
            {
                SearchBar.Input = "";

            }
            NeedsToggleListBuilding = true;
        }

        public override void OnInitialize()
        {
            Vector2 baseOffset = CombinedUI.CenterRight;
            Vector2 offset = new(baseOffset.X, baseOffset.Y - BackHeight / 2f);

            NeedsToggleListBuilding = true;
            DisplayMod = "";
            SortCategory = "";

            // This entire layout is cancerous and dangerous to your health because red protected UIElements children
            // If I want to give extra non-children to BackPanel to count as children when seeing if it should drag, I have to abandon
            // all semblence of organization in favour of making it work. Enjoy my write only UI laying out.
            // Oh well, at least it works...

            Scrollbar = new UIScrollbarClamped();
            Scrollbar.SetView(200f, 1000f);
            Scrollbar.Width.Set(20, 0);
            Scrollbar.OverflowHidden = true;
            Scrollbar.OnScrollWheel += HotbarScrollFix;

            ToggleList = [];
            ToggleList.SetScrollbar(Scrollbar);
            ToggleList.OnScrollWheel += HotbarScrollFix;

            BackPanel = new UIDragablePanel(Scrollbar, ToggleList);
            BackPanel.Left.Set(offset.X, 0);
            BackPanel.Top.Set(offset.Y, 0);
            BackPanel.Width.Set(BackWidth, 0);
            BackPanel.Height.Set(BackHeight, 0);
            BackPanel.PaddingLeft = BackPanel.PaddingRight = BackPanel.PaddingTop = BackPanel.PaddingBottom = 0;
            BackPanel.BackgroundColor = new Color(29, 33, 70) * 0.7f;

            InnerPanel = new UIPanel();
            InnerPanel.Width.Set(BackWidth - 12, 0);
            InnerPanel.Height.Set(BackHeight - 70, 0);
            InnerPanel.Left.Set(6, 0);
            InnerPanel.Top.Set(32, 0);
            InnerPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;

            SearchBar = new UISearchBar(BackWidth - 30, 26);
            SearchBar.Left.Set(4, 0);
            SearchBar.Top.Set(4, 0);
            SearchBar.OnTextChange += SearchBar_OnTextChange;

            ToggleList.Width.Set(InnerPanel.Width.Pixels - InnerPanel.PaddingLeft * 2f - Scrollbar.Width.Pixels, 0);
            ToggleList.Height.Set(InnerPanel.Height.Pixels - InnerPanel.PaddingTop * 2f, 0);

            Scrollbar.Height.Set(InnerPanel.Height.Pixels - 16, 0);
            Scrollbar.Left.Set(InnerPanel.Width.Pixels - Scrollbar.Width.Pixels - 18, 0);

            PresetPanel = new UIPanel();
            PresetPanel.Left.Set(5, 0);
            PresetPanel.Top.Set(SearchBar.Height.Pixels + InnerPanel.Height.Pixels + 8, 0);
            PresetPanel.Width.Set(BackWidth - 10, 0);
            PresetPanel.Height.Set(32, 0);
            PresetPanel.PaddingTop = PresetPanel.PaddingBottom = 0;
            PresetPanel.PaddingLeft = PresetPanel.PaddingRight = 0;
            PresetPanel.BackgroundColor = new Color(74, 95, 172);

            OffButton = new FargoUIPresetButton(FargoMutantAssets.UI.Toggler.PresetOff.Value, (toggles) =>
            {
                toggles.SetAll(false);
            }, () => Language.GetTextValue("Mods.Fargowiltas.UI.TurnAllTogglesOff"), () => Main.LocalPlayer.FargoMutant().PotionToggler);
            OffButton.Top.Set(6, 0);
            OffButton.Left.Set(8, 0);

            OnButton = new FargoUIPresetButton(FargoMutantAssets.UI.Toggler.PresetOn.Value, (toggles) =>
            {
                toggles.SetAll(true);
            }, () => Language.GetTextValue("Mods.Fargowiltas.UI.TurnAllTogglesOn"), () => Main.LocalPlayer.FargoMutant().PotionToggler);
            OnButton.Top.Set(6, 0);
            OnButton.Left.Set(30, 0);

            SomeEffectsButton = new FargoUIPresetButton(FargoMutantAssets.UI.Toggler.PresetMinimal.Value, (toggles) =>
            {
                toggles.SomeEffects();
            }, () => Language.GetTextValue("Mods.Fargowiltas.UI.SomeEffectsPreset"), () => Main.LocalPlayer.FargoMutant().PotionToggler);
            SomeEffectsButton.Top.Set(6, 0);
            SomeEffectsButton.Left.Set(52, 0);

            MinimalButton = new FargoUIPresetButton(FargoMutantAssets.UI.Toggler.PresetMinimal.Value, (toggles) =>
            {
                toggles.MinimalEffects();
            }, () => Language.GetTextValue("Mods.Fargowiltas.UI.MinimalEffectsPreset"), () => Main.LocalPlayer.FargoMutant().PotionToggler);
            MinimalButton.Top.Set(6, 0);
            MinimalButton.Left.Set(74, 0);

            CloseButton = new UICloseButton();
            CloseButton.Left.Set(-24, 1f);
            CloseButton.Top.Set(2, 0);
            CloseButton.OnLeftClick += CloseButton_OnLeftClick;

            Append(BackPanel);
            BackPanel.Append(InnerPanel);
            BackPanel.Append(SearchBar);
            BackPanel.Append(PresetPanel);
            InnerPanel.Append(Scrollbar);
            InnerPanel.Append(ToggleList);
            PresetPanel.Append(OffButton);
            PresetPanel.Append(OnButton);
            PresetPanel.Append(SomeEffectsButton);
            PresetPanel.Append(MinimalButton);
            BackPanel.Append(CloseButton);

            const int xOffset = 74; //ensure this matches the Left.Set of preceding button
            for (int i = 0; i < PotionToggleBackend.CustomPresetCount; i++)
            {
                int slot = i + 1;
                CustomButton[i] = new FargoUIPresetButton(FargoMutantAssets.UI.Toggler.PresetCustom.Value,
                toggles => toggles.LoadCustomPreset(slot),
                toggles => toggles.SaveCustomPreset(slot),
                () => Language.GetTextValue("Mods.Fargowiltas.UI.CustomPreset", slot), () => Main.LocalPlayer.FargoMutant().PotionToggler);
                CustomButton[i].Top.Set(6, 0);
                CustomButton[i].Left.Set(xOffset + 22 * slot, 0);
                PresetPanel.Append(CustomButton[i]);

                if (slot == PotionToggleBackend.CustomPresetCount) //after last panel is loaded, load reload button
                {
                    DisplayAllButton = new FargoUIDisplayAllButton(FargoMutantAssets.UI.Toggler.DisplayAllButton.Value,
                        () => Language.GetTextValue("Mods.Fargowiltas.UI.DisplayAll"),
                        () => Language.GetTextValue("Mods.Fargowiltas.UI.DisplayEquipped"));
                    DisplayAllButton.OnLeftClick += DisplayAllButton_OnLeftClick;
                    DisplayAllButton.Top.Set(6, 0);
                    DisplayAllButton.Left.Set(xOffset + 22 * (slot + 1), 0);
                    PresetPanel.Append(DisplayAllButton);
                }

            }

            base.OnInitialize();
        }
        private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            FargoUIManager.Close<PotionToggler>();
        }
        private void DisplayAllButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            DisplayAllButton.DisplayAll = !DisplayAllButton.DisplayAll;
            NeedsToggleListBuilding = true;
        }
        private void SearchBar_OnTextChange(string oldText, string currentText) => NeedsToggleListBuilding = true;

        private void HotbarScrollFix(UIScrollWheelEvent evt, UIElement listeningElement) => Main.LocalPlayer.ScrollHotbar(PlayerInput.ScrollWheelDelta / 120);
        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.mouseInterface && (Main.mouseLeft || Main.mouseRight))
            {
                NeedsToggleListBuilding = true;
            }
            base.Update(gameTime);
            FargoPlayer modPlayer = Main.LocalPlayer.FargoMutant();
            if (NeedsToggleListBuilding && modPlayer.ToggleRebuildCooldown <= 0)
            {
                BuildList();
                NeedsToggleListBuilding = false;
                modPlayer.ToggleRebuildCooldown = 30;
            }
        }

        public void BuildList()
        {
            ToggleList.Clear();
            Player player = Main.LocalPlayer;
            FargoPlayer modPlayer = player.FargoMutant();
            PotionToggleBackend toggler = modPlayer.PotionToggler;

            bool alwaysDisplay = DisplayAllButton.DisplayAll;

            bool SearchMatches(string[] words) => words.Any(s => s.StartsWith(SearchBar.Input, StringComparison.OrdinalIgnoreCase));

            IEnumerable<PotionToggle> toggles = toggler.Toggles.Values.Where((toggle) =>
            {
                string[] words = toggle.GetRawToggleName().Split(' ');
                return
                (modPlayer.ActivePotions.Contains(toggle.BuffID) || alwaysDisplay) &&
                (string.IsNullOrEmpty(SearchBar.Input) || SearchMatches(words));
            });

            if (toggles.Any())
            {
                if (ToggleList.Count > 0) // Don't add for the first header
                    ToggleList.Add(new UIText("", 0.2f)); // Blank line

                foreach (PotionToggle toggle in toggles)
                {
                    ToggleList.Add(new UIPotionToggle(toggle.ItemID, toggle.BuffID));
                }
            }

            if (ToggleList.Count == 0) // empty, no toggles
            {
                ToggleList.Clear();
                ToggleList.Add(new FargoUIHeader($"[i:{ModContent.ItemType<TogglerIconItem>()}] {Language.GetTextValue("Mods.Fargowiltas.UI.NoToggles")}", Fargowiltas.Instance.Name, ModContent.ItemType<TogglerIconItem>(), (BackWidth - 16, 20)));
            }
        }
    }

}
