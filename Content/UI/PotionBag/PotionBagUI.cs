using Fargowiltas.Assets.Textures;
using Fargowiltas.Common.Systems;
using Fargowiltas.Content.Items.Summons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;
using static Fargowiltas.Common.Systems.PotionBagSystem;

namespace Fargowiltas.Content.UI.PotionBag
{
    public class PotionBagUI : FargoUI
    {
        public override bool MenuToggleSound => true;
        public int BackWidth = 350;
        public int BackHeight = 590;

        bool NeedsPotionListBuilding = false;
        int BuildListCooldown = 0;

        UIDragablePanel BackPanel;
        UICloseButton CloseButton;
        PotionListPanel PotionPanel;
        UIPanel PotionList;
        PotionAddInfo AddInfoPanel;
        PotionAddButton AddButton;
        PotionAddSlot AddSlot;
        UISearchBar SearchBar;

        public static string GetCoolerText(string suffix, object[] args = null)
        {
            args ??= [];
            return Language.GetTextValue($"Mods.Fargowiltas.UI.PotionCooler.{suffix}", args);
        }

        public override void OnInitialize()
        {
            RemoveAllChildren();

            Vector2 baseOffset = Main.ScreenSize.ToVector2() / 2f;
            Vector2 offset = new(baseOffset.X - BackWidth / 2, baseOffset.Y - BackHeight / 2);

            BackPanel = new UIDragablePanel();
            BackPanel.Left.Set(offset.X, 0f);
            BackPanel.Top.Set(offset.Y, 0f);
            BackPanel.Width.Set(BackWidth, 0f);
            BackPanel.Height.Set(BackHeight, 0f);
            BackPanel.PaddingLeft = BackPanel.PaddingRight = BackPanel.PaddingTop = BackPanel.PaddingBottom = 0;
            BackPanel.BackgroundColor = new Color(29, 33, 70) * 0.7f;
            Append(BackPanel);

            RebuildPanel();

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (BackPanel != null && BackPanel.ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;

            if (!Main.playerInventory)
            {
                FargoUIManager.Close<PotionBagUI>();
            }

            if (NeedsPotionListBuilding && BuildListCooldown-- <= 0)
            {
                RebuildList();
                NeedsPotionListBuilding = false;
                BuildListCooldown = 10;
            }

            if (Main.LocalPlayer.FargoMutant().NeedRefreshCooler)
            {
                RebuildPanel();
                Main.LocalPlayer.FargoMutant().NeedRefreshCooler = false;
            }

            BackPanel.Left.Set((Main.ScreenSize.X - BackWidth) / 2f, 0f);
            BackPanel.Top.Set((Main.ScreenSize.Y - BackHeight) / 2f, 0f);
        }

        public override void OnOpen()
        {
            RebuildPanel();
            //NeedsPotionListBuilding = true;
            Main.playerInventory = true;
        }

        public override void OnClose()
        {
            NeedsPotionListBuilding = true;
            AddSlot.ReturnItemToPlayer();
            base.OnClose();
        }

        /// <summary>
        /// Attempts to actually consume the potion
        /// </summary>
        void TryConsumePotion()
        {
            Item item = AddSlot.Item.Clone();
            if (item.IsAir || item.buffType == 0 || item.buffTime < 60 * 60 * 2 || item.ModItem is BaseSpawnBooster)
                return;

            if (PotionBagSystem.CanConsumePotion(item.type, item.stack, out int amountToConsume, out int leftOvers))
            {
                AddSlot.Item.stack = leftOvers;
                if (leftOvers >= 0)
                {
                    SoundEngine.PlaySound(SoundID.Unlock);
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    FargoNet.AddPotionToPotionBag(item.type, amountToConsume);
                else
                    PotionBagSystem.AddPotion(item.type, amountToConsume);
                RebuildList();
                SwapItem(AddSlot.Item);
            }
        }

        void SwapItem(Item item)
        {
            AddButton.Hidden = item.IsAir || !PotionBagSystem.CanConsumePotion(item.type, item.stack, out _, out _);
            AddInfoPanel?.UpdateItem(item);
        }

        void RebuildPanel()
        {
            Item slot = null;
            if (AddSlot != null)
                slot = AddSlot.Item.Clone();

            BackPanel.RemoveAllChildren();

            AddSlot = new PotionAddSlot();
            AddSlot.Top.Set(45 - 36, 0);
            AddSlot.Left.Set(22f, 0);
            AddSlot.SwapItem = SwapItem;
            if (slot != null)
                AddSlot.CreateItem(slot);

            AddButton = new PotionAddButton();
            AddButton.Top.Set(64f, 0);
            AddButton.Left.Set(48f - 30f, 0);
            AddButton.Width.Set(60f, 0f);
            AddButton.Height.Set(24f, 0f);
            AddButton.Consume = TryConsumePotion;
            BackPanel.Append(AddButton);

            AddInfoPanel = new PotionAddInfo();
            AddInfoPanel.Height.Set(80, 0f);
            AddInfoPanel.Top.Set(6f, 0);
            AddInfoPanel.Left.Set(100, 0f);
            AddInfoPanel.Width.Set(220, 0f);

            if (slot != null)
            {
                AddSlot.CreateItem(slot);
                SwapItem(slot);
            }

            BackPanel.Append(AddInfoPanel);

            BackPanel.Append(AddSlot);

            PotionPanel = new PotionListPanel();
            PotionPanel.SetPadding(0);
            PotionPanel.Top.Set(60f + 60f, 0f);
            PotionPanel.Left.Set(6f, 0f);
            PotionPanel.Width.Set(BackWidth - 20 - 12f, 0);
            PotionPanel.Height.Set(88 * 5 + 2, 0f);
            PotionPanel.OverflowHidden = true;

            RebuildList();

            SearchBar = new UISearchBar(BackWidth - 30, 26);
            SearchBar.Left.Set(4, 0);
            SearchBar.Top.Set(90, 0);
            SearchBar.OnTextChange += SearchBar_OnTextChange;

            var scrollbar = new UIScrollbar();
            scrollbar.Width.Set(20f, 0);
            scrollbar.Height.Set(88 * 5 + 2, 0);
            scrollbar.Top.Set(60f + 60f, 0f);
            scrollbar.Left.Set(6f + BackWidth - 20 - 12 + 2, 0f);
            PotionPanel.SetScrollbar(scrollbar);

            BackPanel.Append(PotionPanel);
            BackPanel.Append(scrollbar);
            BackPanel.Append(SearchBar);

            CloseButton = new UICloseButton();
            CloseButton.Left.Set(-24, 1f);
            CloseButton.Top.Set(2, 0);
            CloseButton.OnLeftClick += CloseButton_OnLeftClick;
            BackPanel.Append(CloseButton);
        }

        void RebuildList()
        {
            PotionPanel.RemoveAllChildren();

            PotionList = new UIPanel();

            PotionList.Width.Set(0f, 1f);
            PotionList.Height.Set(0f, 1f);
            PotionList.OverflowHidden = true;

            bool SearchMatches(string[] words) => string.IsNullOrEmpty(SearchBar.Input) || words.Any(s => s.StartsWith(SearchBar.Input, StringComparison.OrdinalIgnoreCase));

            int index = 0;
            var items = new List<UIElement>();
            foreach (var potion in PotionBagSystem.Potions.Where(p => SearchMatches(new Item(p.Key.Type).Name.Split(' '))))
            {
                var newItem = new PotionDisplayPanel(potion.Key.Type, potion.Value, new Item(potion.Key.Type).buffType);
                newItem.Left.Set(0f, (index % 4) / 4f);
                newItem.Top.Set(90 * MathF.Floor(index / 4f), 0f);
                items.Add(newItem);
                index++;
            }
            PotionPanel.SetPotions(items);

            PotionPanel.Append(PotionList);
        }

        private void SearchBar_OnTextChange(string oldText, string newText) => NeedsPotionListBuilding = true;

        private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            // cheesy fix for dislocation on close
            (listeningElement.Parent as UIDragablePanel).DragEnd(Main.MouseScreen);
            FargoUIManager.Close<PotionBagUI>();
        }

        /// <summary>
        /// Individual potion panel
        /// </summary>
        private class PotionDisplayPanel : UIFargoPanel, IComparable
        {
            public int type;
            public int Count;
            public int buffType;

            public PotionDisplayPanel(int type, int count, int buffType)
            {
                SetPadding(6f);
                Width.Set(80f, 0);
                Height.Set(90f, 0);
                this.type = type;
                this.Count = count;
                this.buffType = buffType;

                RebuildItem();
            }

            internal static string GetHoverText(int itemID, int count)
            {
                PotionBagSystem.TryGetCount(itemID, out count);
                int buffType = new Item(itemID).buffType;
                float percent = MathF.Min(MathF.Round((float)count / PotionBagSystem.MaxPotions * 100, 2), 100);
                string display = $"[i:{itemID}] {Lang.GetItemNameValue(itemID)} {PotionBagUI.GetCoolerText("Progress", [percent])}";
                if (percent >= 100)
                {
                    bool toggled = Main.LocalPlayer.FargoMutant().PotionToggler.Toggles.Any(t => t.Value.BuffID == buffType && t.Value.ToggleBool);
                    if (toggled)
                    {
                        display += $"\n{PotionBagUI.GetCoolerText("Enabled", [Lang.GetBuffName(buffType)])}";
                    }
                    else
                    {
                        display += $"\n{PotionBagUI.GetCoolerText("Disabled", [Lang.GetBuffName(buffType)])}";
                    }
                }
                else
                {
                    display += $"\n{PotionBagUI.GetCoolerText("MaxEffect", [Lang.GetBuffName(buffType)])}";
                }
                if (BuffID.Sets.IsAFlaskBuff[buffType])
                {
                    string flask = Main.LocalPlayer.FargoMutant().ActiveFlask == -1 ? Language.GetTextValue("Mods.Fargowiltas.UI.BattleCryNone") : $"[i:{Main.LocalPlayer.FargoMutant().ActiveFlask}]";
                    display += $"\n{GetCoolerText("FlaskWarn", [flask])}";
                }
                return display;
            }

            public override int CompareTo(object obj)
            {
                if (obj is PotionDisplayPanel item)
                {
                    int countCmp = -Count.CompareTo(item.Count);
                    if (countCmp != 0)
                        return countCmp;
                    else
                        return type.CompareTo(item.type);
                }
                return base.CompareTo(obj);
            }

            void RebuildItem()
            {
                var slot = new InnerDisplaySlot(Count);
                slot.CreateItem(new Item(type));
                slot.Unchangable = true;
                slot.HAlign = 0.5f;
                Append(slot);

                float top = 56f;
                if (Count < PotionBagSystem.MaxPotions)
                {
                    PotionProgressBar bar = new(type, Count, buffType);
                    bar.Height.Set(20f, 0f);
                    bar.Width.Set(60f, 0f);
                    bar.Top.Set(top, 0f);
                    bar.HAlign = 0.5f;

                    Append(bar);
                }
                else
                {
                    PotionCheckBox box = new(type, Count, buffType);
                    box.Top.Set(top, 0f);
                    box.HAlign = 0.5f;

                    Append(box);
                }
            }

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);
            }

            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                base.DrawSelf(spriteBatch);

                Asset<Texture2D> line = TextureAssets.Extra[ExtrasID.FairyQueenLance];

                if (ContainsPoint(Main.MouseScreen))
                {
                    Rectangle lineFrame = line.Frame();
                    Vector2 lineOrigin = lineFrame.Size() - new Vector2(lineFrame.Size().X, lineFrame.Size().Y / 2f);
                    Vector2 horizScale = new Vector2(0.03f, (GetDimensions().Width / lineFrame.Height) - 2f);
                    Vector2 vertScale = new Vector2(0.03f, (GetDimensions().Height / lineFrame.Height) - 2f);
                    Color baseColor = Color.Lerp(Color.White, Count >= PotionBagSystem.MaxPotions ? Color.Green : Color.Red, 0.4f);
                    Color color = baseColor * 0.7f;

                    spriteBatch.Draw(line.Value, GetDimensions().Position() + new Vector2(2, GetDimensions().Height / 2f), lineFrame, color, 0f, lineOrigin, vertScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(line.Value, GetDimensions().Position() + new Vector2(GetDimensions().Width - 2, GetDimensions().Height / 2f), lineFrame, color, MathHelper.Pi, lineOrigin, vertScale, SpriteEffects.None, 0f);

                    spriteBatch.Draw(line.Value, GetDimensions().Position() + new Vector2(GetDimensions().Width / 2f, 2), lineFrame, color, MathHelper.PiOver2, lineOrigin, horizScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(line.Value, GetDimensions().Position() + new Vector2(GetDimensions().Width / 2f, GetDimensions().Height - 2), lineFrame, color, 3 * MathHelper.PiOver2, lineOrigin, horizScale, SpriteEffects.None, 0f);
                }
            }

            private class InnerDisplaySlot(int count) : FargoItemSlot
            {
                int Count = count;

                private static Asset<Texture2D> Lock = Main.Assets.Request<Texture2D>("Images/UI/Workshop/PublicityPrivate");
                public override void PostDrawSelf(SpriteBatch spriteBatch)
                {
                    base.PostDrawSelf(spriteBatch);

                    // locked
                    if (Count < PotionBagSystem.MaxPotions)
                    {
                        Vector2 pos = GetDimensions().Position() + new Vector2(GetDimensions().Width - (0.75f * Lock.Size().X / 2f), 0.75f * Lock.Size().Y / 2f) + Vector2.One;
                        spriteBatch.Draw(Lock.Value, pos, Lock.Frame(), Color.White, 0f, Lock.Size() / 2f, 0.75f, SpriteEffects.None, 0f);
                    }
                }
            }

            private class PotionProgressBar : UIElement
            {
                int id;
                int count;
                int buffID;
                public PotionProgressBar(int id, int count, int buffID)
                {
                    this.id = id;
                    this.count = count;
                    this.buffID = buffID;

                    var text = new UIText($"{count}/{PotionBagSystem.MaxPotions}");
                    text.Width = text.Height = new StyleDimension(0f, 1f);
                    text.TextOriginX = text.TextOriginY = 0.5f;
                    Append(text);
                }

                private static Asset<Texture2D> texture = TextureAssets.MagicPixel;
                protected override void DrawSelf(SpriteBatch spriteBatch)
                {
                    Vector2 pos = GetDimensions().Position();
                    Rectangle frame = texture.Frame();
                    Vector2 fullScale = new(GetDimensions().Width, GetDimensions().Height / Main.screenHeight);
                    Vector2 partScale = new(((float)count / PotionBagSystem.MaxPotions) * GetDimensions().Width, GetDimensions().Height / Main.screenHeight);

                    Color fillColor = Color.Lerp(Color.Red, Color.Green, (float)count / MaxPotions);

                    spriteBatch.Draw(texture.Value, pos, frame, Color.DimGray, 0f, Vector2.Zero, fullScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(texture.Value, pos, frame, fillColor, 0f, Vector2.Zero, partScale, SpriteEffects.None, 0f);

                    if (ContainsPoint(Main.MouseScreen))
                    {
                        UICommon.TooltipMouseText(GetHoverText(id, count));
                    }
                }
            }

            private class PotionCheckBox : UIElement
            {
                int itemID;
                int count;
                int buffID;

                public PotionCheckBox(int itemID, int count, int buffID)
                {
                    Width.Set(18, 0);
                    Height.Set(18, 0);
                    this.itemID = itemID;
                    this.count = count;
                    this.buffID = buffID;
                }

                protected override void DrawSelf(SpriteBatch spriteBatch)
                {
                    Vector2 position = GetDimensions().Position();

                    bool toggled = Main.LocalPlayer.FargoMutant().PotionToggler.Toggles.Any(t => t.Value.BuffID == buffID && t.Value.ToggleBool);



                    spriteBatch.Draw(FargoMutantAssets.UI.Toggler.CheckBox.Value, position, Color.White);
                    if (toggled)
                    {
                        spriteBatch.Draw(FargoMutantAssets.UI.Toggler.CheckMark.Value, position + new Vector2(0, -4), Color.White);
                        if (IsMouseHovering)
                        {
                            spriteBatch.Draw(FargoMutantAssets.UI.Toggler.CheckMarkGlow.Value, position + new Vector2(0, -4), Color.White);
                        }
                    }

                    if (IsMouseHovering)
                    {
                        UICommon.TooltipMouseText(GetHoverText(itemID, count));
                    }
                }

                public override void LeftClick(UIMouseEvent evt)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);

                    FargoPlayer modPlayer = Main.LocalPlayer.FargoMutant();

                    KeyValuePair<int, PotionToggle> pair = modPlayer.PotionToggler.Toggles.First(t => t.Value.BuffID == buffID);
                    pair.Value.ToggleBool = !pair.Value.ToggleBool;

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        modPlayer.SyncPotionToggle(pair.Value.ItemID);

                    base.LeftClick(evt);
                }
            }
        }

        // Panel containing all other potion display panels
        private class PotionListPanel : UIFargoPanel
        {
            int _offset;
            int rowCount;
            List<UIElement> _items;
            UIScrollbar _scrollbar;

            public PotionListPanel()
            {
                _offset = 0;
                rowCount = 0;
                OverflowHidden = true;
            }

            public void SetPotions(List<UIElement> potions)
            {
                _items = potions;
                Recalculate();
            }

            private void UpdateScrollbar()
            {
                if (_scrollbar != null)
                {
                    _scrollbar.SetView(5, rowCount);
                }
            }

            public void SetScrollbar(UIScrollbar scrollbar)
            {
                this._scrollbar = scrollbar;
            }

            public bool ChangeOffset(int amount)
            {
                int oldOffset = _offset;
                _offset = (int)MathHelper.Clamp(_offset + amount, 0, Math.Max(0, rowCount - 5));
                if (_offset != oldOffset)
                    SoundEngine.PlaySound(SoundID.MenuTick);
                else
                    return false;
                Recalculate();
                return true;
            }

            public override void ScrollWheel(UIScrollWheelEvent evt)
            {
                base.ScrollWheel(evt);

                int sign = -Math.Sign(evt.ScrollWheelValue);
                if (ChangeOffset(sign) && _scrollbar != null)
                    _scrollbar.ViewPosition += sign;
            }

            public override void RecalculateChildren()
            {
                base.RecalculateChildren();

                int index = 0;
                foreach (var item in _items)
                {
                    item.Left.Set(0f, (index % 4) / 4f);
                    item.Top.Set(88 * (MathF.Floor(index / 4f) - _offset), 0f);
                    index++;
                }
                rowCount = (int)MathF.Ceiling(index / 4f);
            }

            public void UpdateOrder()
            {
                _items.Sort(SortMethod);
                UpdateScrollbar();
            }

            public override void Recalculate()
            {
                UpdateOrder();
                RemoveAllChildren();
                foreach (UIElement item in _items)
                {
                    Append(item);
                }

                base.Recalculate();
                UpdateScrollbar();
            }

            private int SortMethod(UIElement item1, UIElement item2) => item1.CompareTo(item2);

            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                if (ContainsPoint(Main.MouseScreen))
                    PlayerInput.LockVanillaMouseScroll("PotionCooler");

                if (_scrollbar != null)
                    _offset = (int)_scrollbar.GetValue();

                base.DrawSelf(spriteBatch);
            }

            protected override void DrawChildren(SpriteBatch spriteBatch)
            {
                base.DrawChildren(spriteBatch);
            }
        }

        private class PotionAddSlot : FargoItemSlot
        {
            public Action<Item> SwapItem;

            public PotionAddSlot()
            {

            }

            public override bool CanAcceptItem(Item item) => item.buffType > 0 && item.buffTime >= 60 * 60 * 2;

            public override void OnItemSwap(ref Item oldItem, ref Item newItem)
            {
                SwapItem?.Invoke(newItem);
                base.OnItemSwap(ref oldItem, ref newItem);
            }
        }

        private class PotionAddButton : UIPanel
        {
            public bool Hidden = true;
            public PotionAddSlot Slot;
            public Action Consume;

            public PotionAddButton()
            {
                SetPadding(0f);
                UIText text = new UIText(GetCoolerText("Store"));
                text.Width.Set(0, 1f);
                text.Height.Set(0, 1f);
                text.TextOriginX = 0.5f;
                text.TextOriginY = 0.5f;
                Append(text);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (Hidden)
                    return;

                base.Draw(spriteBatch);
            }

            public override void LeftClick(UIMouseEvent evt)
            {
                if (Hidden) return;
                SoundEngine.PlaySound(SoundID.MenuTick);
                Consume.Invoke();
            }
        }

        private class PotionAddInfo : UIPanel
        {
            public Item item;
            int Count;

            public PotionAddInfo()
            {
                SetPadding(4f);
                this.item = new Item();
                item.TurnToAir();
                BuildPanel();
            }

            public void UpdateItem(Item item)
            {
                this.item = item;
                BuildPanel();
            }

            void BuildPanel()
            {
                RemoveAllChildren();

                if (item.IsAir)
                {
                    var emptyText = new UIText(Language.GetTextValue("Mods.Fargowiltas.UI.PotionCooler.EmptyInfo"));
                    emptyText.VAlign = emptyText.HAlign = 0.5f;
                    Append(emptyText);
                    return;
                }

                PotionBagSystem.TryGetCount(item.type, out Count);

                var itemName = new UIText(item.Name);
                itemName.HAlign = 0.5f;
                itemName.Width.Set(0f, 1f);

                string displayText = "";
                if (Count >= 30)
                {
                    displayText += GetCoolerText("Maxed");
                }
                else
                {
                    PotionBagSystem.CanConsumePotion(item.type, item.stack, out int consumeAmount, out int leftovers);

                    displayText += GetCoolerText("Completion", [PotionBagSystem.MaxPotions - Count]);

                    var convert = new UIText($"{item.stack} > {Math.Max(leftovers, 0)}", 0.75f);
                    convert.HAlign = 0.5f;
                    convert.Width.Set(0, 1);
                    convert.Top.Set(52, 0);
                    Append(convert);
                }

                var description = new UIText(displayText, 0.75f);
                description.HAlign = 0.5f;
                description.Width.Set(0, 1);
                description.Top.Set(22, 0);
                Append(description);
                Append(itemName);
                return;

            }
        }
    }
}
