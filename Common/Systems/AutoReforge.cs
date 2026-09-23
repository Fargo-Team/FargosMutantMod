using Fargowiltas.Common.Configs;
using Fargowiltas.Content.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Prefixes;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Fargowiltas.Common.Systems;

public class AutoReforge : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => lateInstantiation && entity.type == NPCID.GoblinTinkerer;
    public override bool PreChatButtonClicked(NPC npc, bool firstButton)
    {
        if (!firstButton && FargoClientConfig.Instance.AutoReforge)
        {
            FargoUIManager.Open<AutoReforgeUI>();
            return false;
        }
        return base.PreChatButtonClicked(npc, firstButton);
    }
}

public class ReforgeItemSlot : FargoItemSlot
{
    private AutoReforgeUI parent;
    public Action<Item> OnSwap;

    public ReforgeItemSlot(AutoReforgeUI parent)
    {
        this.parent = parent;
    }

    public override void OnItemSwap(ref Item oldItem, ref Item newItem)
    {
        OnSwap.Invoke(newItem);
    }

    public override bool CanAcceptItem(Item item) => item.CanHavePrefixes();

    public void Reforge(Item item)
    {
        Player player = Main.LocalPlayer;
        player.BuyItem(ReforgeUtils.GetReforgePrice(item));
        ItemLoader.PreReforge(item);
        item.ResetPrefix();
        item.Prefix(-2);

        ItemLoader.PostReforge(item);
        item.Center = player.Center; // so item popup text is near player
        PopupText.NewText(PopupTextContext.ItemReforge, item, item.stack, noStack: true);
        SoundEngine.PlaySound(in SoundID.Item37);
        parent.RebuildPrice(item);
        parent.hammerSwing?.Invoke(); // start swing animation
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // reforging
        Player player = Main.LocalPlayer;
        Item item = Item;
        if (!parent.isReforging || !HasItem || !player.CanAfford(ReforgeUtils.GetReforgePrice(item))) // no item or can't afford
        {
            parent.isReforging = false;
            return;
        }

        if (Main.GameUpdateCount % 5 == 0)
        {
            if (parent.reservedPrefixs.Count == 0) // vanilla
            {
                Reforge(Item);
                parent.isReforging = false;
            }
            else // auto reforge
            {
                if (!parent.reservedPrefixs.Contains(item.prefix))
                    Reforge(Item);
                else
                {
                    SoundEngine.PlaySound(SoundID.ResearchComplete);
                    parent.isReforging = false;
                }
            }
        }
    }
}

public class ReforgeButton : UIElement
{
    public Action Reforge;
    int HStimer = -1; // visual swing fx


    public void SwingHammer() // starts swing animation
    {
        if (HStimer >= 0)
            return;
        HStimer = 0;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (HStimer == -1)
            return;

        if (HStimer > 3)
        {
            HStimer = -1;
            return;
        }

        HStimer++;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        // swing rotation
        float rotation = 0f;
        if (HStimer >= 0)
            rotation = 0.25f * (HStimer);

        // draw base
        Texture2D texture = TextureAssets.Reforge[0].Value;
        Rectangle frame = texture.Frame();
        Vector2 origin2 = frame.Size() / 2;
        Vector2 offset = frame.Size() - new Vector2(texture.Width, 0);
        spriteBatch.Draw(texture, GetDimensions().Position() - offset + new Vector2(0, 2 * texture.Height), frame, Color.White, rotation, offset, 1f, SpriteEffects.None, 0f);

        // draw glow
        if (ContainsPoint(Main.MouseScreen))
        {
            Texture2D glowTexture = TextureAssets.Reforge[1].Value;
            spriteBatch.Draw(glowTexture, GetDimensions().Position() - offset + new Vector2(0, 2 * texture.Height), null, Color.White, rotation, offset, 1f, SpriteEffects.None, 0f);
        }
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        SoundEngine.PlaySound(SoundID.MenuTick);
        Reforge?.Invoke();
        base.LeftClick(evt);
    }
}

public class PrefixOption : UIPanel
{
    Item item;
    int prefix;
    public Func<int, bool> HasPrefix;
    bool selected => HasPrefix.Invoke(prefix);

    public Action<int> ToggleSelect;

    public PrefixOption(Item Item, int prefixId)
    {
        if (Item.IsAir)
            return;

        this.prefix = prefixId;
        item = Item.Clone();
        item.ResetPrefix();
        item.favorited = false;
        item.Prefix(prefix);
    }

    public override int CompareTo(object obj)
    {
        if (obj is PrefixOption opt)
        {
            return -1 * ReforgeUtils.ComparePrefixs(item, opt.item);
        }
        return 0;
    }

    public override void OnActivate()
    {
        base.OnActivate();

        RemoveAllChildren();
        UIText text = new UIText(Lang.prefix[prefix]);
        text.TextColor = ItemRarity.GetColor(item.rare);
        text.Top.Set(0, 0);
        text.Left.Set(0, 0);
        text.Width.Set(0, 1f);
        text.Height.Set(0, 1f);
        text.HAlign = 0.1f;
        text.VAlign = 0.5f;
        Append(text);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        Color baseColor = new Color(63, 82, 151) * 0.7f;
        BackgroundColor = selected ? Color.Lerp(baseColor, Color.Lime, 0.2f) : baseColor;

        base.DrawSelf(spriteBatch);

        Texture2D texture = TextureAssets.Extra[ExtrasID.StarWrath].Value;

        if (selected)
        {
            spriteBatch.Draw(texture, GetDimensions().Position() - new Vector2(0, 20), null, Color.Yellow * 0.5f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, GetDimensions().Position() - new Vector2(0, 20) + (GetDimensions().Width - 20) * Vector2.UnitX, null, Color.Yellow * 0.5f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
        }

        if (ContainsPoint(Main.MouseScreen))
        {
            Main.HoverItem = item.Clone();
            Main.hoverItemName = item.Name;
        }

    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        ToggleSelect(prefix);
        SoundEngine.PlaySound(SoundID.MenuTick);
    }
}

public class AutoReforgeUI : FargoUI
{
    public override bool MenuToggleSound => true;

    public UIText PriceTag;
    public UIPanel BackPanel;
    public UIPanel PrefixBackPanel;
    public UIList PrefixList;
    public ReforgeItemSlot ItemSlotPanel;

    public bool isReforging = false;
    public int reforgeCD = 0;
    public List<int> reservedPrefixs = [];
    public Action hammerSwing;

    public void ItemSwap(Item newItem)
    {
        isReforging = false;
        reservedPrefixs.Clear();
        RebuildChildren(newItem);
    }

    public void TogglePrefix(int pre)
    {
        isReforging = false;
        if (!reservedPrefixs.Remove(pre))
        {
            reservedPrefixs.Add(pre);
        }
    }

    public void ToggleReforge()
    {
        if (reservedPrefixs.Count != 0)
        {
            isReforging = !isReforging && !reservedPrefixs.Contains(ItemSlotPanel.Item.prefix);
        }
        else if (reforgeCD == 0)
        {
            isReforging = true;
            reforgeCD = 10;
        }
    }

    public override void OnOpen()
    {
        Main.playerInventory = true;
        Main.npcChatText = "";
        SoundEngine.PlaySound(SoundID.MenuTick);
        OnInitialize();
    }

    public override void OnClose()
    {
        ItemSlotPanel.ReturnItemToPlayer();
        isReforging = false;
        reservedPrefixs = [];
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // close if not talking to tinkerer
        if (!Main.playerInventory || Main.LocalPlayer.talkNPC == -1 || Main.LocalPlayer.TalkNPC.type != NPCID.GoblinTinkerer)
            FargoUIManager.Close(this);

        if (reforgeCD > 0)
        {
            reforgeCD--;
        }

        // prevent mouse inputs when hovering
        if (BackPanel != null && BackPanel.ContainsPoint(Main.MouseScreen))
            Main.LocalPlayer.mouseInterface = true;
    }



    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        if (BackPanel != null && ItemSlotPanel.HasItem)
            ItemSlot.DrawSavings(Main.spriteBatch, BackPanel.GetDimensions().X + 10, BackPanel.GetDimensions().Y - 70, true);

    }

    public override void OnActivate()
    {
        RebuildChildren();
    }

    public override void OnInitialize()
    {
        RemoveAllChildren();

        BackPanel = new UIPanel();
        BackPanel.Top.Set(0, 0.5f);
        BackPanel.Left.Set(0, 0.1f);
        BackPanel.Width.Set(0, 0.2f * Main.UIScale);
        BackPanel.Height.Set(0, 0.2f * Main.UIScale);
        Append(BackPanel);

        ItemSlotPanel = new ReforgeItemSlot(this);
        ItemSlotPanel.Top.Set(6, 0.5f);
        ItemSlotPanel.Left.Set(6, 0.1f);
        ItemSlotPanel.Width.Set(52, 0);
        ItemSlotPanel.Height.Set(52, 0);
        ItemSlotPanel.OnSwap = ItemSwap;
        Append(ItemSlotPanel);

        OnActivate();
    }

    private string GetLocalText(string key) => Language.GetTextValue($"Mods.Fargowiltas.UI.{key}");

    private void RebuildChildren(Item item = null)
    {
        BackPanel.RemoveAllChildren();

        if (item == null || item.IsAir)
        {
            var insertText = new UIText(GetLocalText("InsertText"));
            insertText.Top.Set(0, 0);
            insertText.Left.Set(70, 0);
            BackPanel.Append(insertText);
            return;
        }

        // hammer button
        var reforgeButton = new ReforgeButton();
        reforgeButton.Top.Set(0, 0);
        reforgeButton.Left.Set(50, 0);
        reforgeButton.Width.Set(30, 0);
        reforgeButton.Height.Set(30, 0);
        reforgeButton.Reforge = ToggleReforge;
        hammerSwing = reforgeButton.SwingHammer;
        BackPanel.Append(reforgeButton);

        // prefix list
        PrefixBackPanel = new UIPanel();
        PrefixBackPanel.Left.Set(0, 0.5f);
        PrefixBackPanel.Top.Set(0, 0);
        PrefixBackPanel.Width.Set(0, 0.5f);
        PrefixBackPanel.Height.Set(0, 1f);
        BackPanel.Append(PrefixBackPanel);

        PrefixList = [];
        PrefixList.Top.Set(0, 0);
        PrefixList.Left.Set(0, 0);
        PrefixList.Width.Set(0, 0.9f);
        PrefixList.Height.Set(0, 0.98f);
        PrefixList.ListPadding = 6f;
        List<PrefixOption> options = [];
        foreach (int prefix in ReforgeUtils.FindPrefixes(item))
        {
            var option = new PrefixOption(item, prefix);
            option.Width.Set(0, 1f);
            option.Height.Set(30, 0);
            option.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            option.ToggleSelect = TogglePrefix;
            option.HasPrefix = ContainsPrefix;
            option.Activate();
            options.Add(option);
        }
        options.Sort();
        PrefixList.AddRange(options);

        UIScrollbar scrollbar = new();
        scrollbar.Height.Set(0, 0.96f);
        scrollbar.Top.Set(0, 0.02f);
        scrollbar.Left.Set(PrefixBackPanel.GetInnerDimensions().Width - (scrollbar.Width.Pixels / 1.5f), 0);

        PrefixBackPanel.Append(scrollbar);
        PrefixList.SetScrollbar(scrollbar);

        PrefixBackPanel.Append(PrefixList);

        // price
        RebuildPrice(item);

        var autoText = new AutoModeText();
        autoText.GetAuto = IsAuto;
        autoText.Left.Set(90f, 0f);
        autoText.OnLeftClick += ClearPrefixes_OnLeftClick;
        BackPanel.Append(autoText);
    }

    public void RebuildPrice(Item item)
    {
        if (PriceTag != null)
            BackPanel.RemoveChild(PriceTag);

        PriceTag = new UIText(ReforgeUtils.GetPriceString(item));
        PriceTag.Top.Set(60, 0);
        PriceTag.Left.Set(0, 0);
        BackPanel.Append(PriceTag);
    }

    public bool ContainsPrefix(int prefix) => reservedPrefixs.Contains(prefix);

    private bool IsAuto() => reservedPrefixs.Count != 0;

    private void ClearPrefixes_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
    {
        reservedPrefixs.Clear();
    }

    private class AutoModeText : UIText
    {
        public Func<bool> GetAuto;

        public AutoModeText() : base("", 1)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SetText(GetAutoText());
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (ContainsPoint(Main.MouseScreen) && GetAuto.Invoke())
            {
                UICommon.TooltipMouseText(Language.GetTextValue("Mods.Fargowiltas.UI.ClearPrefixes"));
            }
        }

        private string GetAutoText()
        {
            string retVal = Language.GetTextValue($"Mods.Fargowiltas.UI.AutoStatus");
            if (GetAuto.Invoke())
                retVal += $"[c/55FF55:{Language.GetTextValue($"Mods.Fargowiltas.UI.On")}]";
            else
                retVal += $"[c/FF5555:{Language.GetTextValue($"Mods.Fargowiltas.UI.Off")}]";

            return retVal;
        }
    }
}

internal static class ReforgeUtils
{
    public static int GetReforgePrice(Item item)
    {
        if (item.type <= ItemID.None)
            return -1;

        int num58 = item.value;
        num58 *= item.stack;
        bool canApplyDiscount = true;
        if (ItemLoader.ReforgePrice(item, ref num58, ref canApplyDiscount))
        {
            if (canApplyDiscount && Main.LocalPlayer.discountAvailable)
            {
                num58 = (int)((double)num58 * 0.8);
            }
            num58 = (int)((double)num58 * Main.LocalPlayer.currentShoppingSettings.PriceAdjustment);
            num58 /= 3;
        }
        return num58;
    }
    public static string GetPriceString(Item item)
    {
        return GetPriceString(GetReforgePrice(item));
    }
    public static string GetPriceString(int price, bool reforgeText = true, bool coinIcon = false)
    {
        //int price = GetReforgePrice(item);

        if (price < 0)
            return "";
        string ret = "";
        if (reforgeText)
            ret += "[c/66FF66" + ":" + $"{Language.GetTextValue("Mods.Fargowiltas.UI.ReforgeCost")}:]\n";
        int num59 = 0;
        int num60 = 0;
        int num61 = 0;
        int num62 = 0;
        int num63 = price;
        if (num63 < 1)
        {
            num63 = 1;
        }
        if (num63 >= 1000000)
        {
            if (coinIcon)
                ret += "[i:" + ItemID.PlatinumCoin + "]";
            num59 = num63 / 1000000;
            num63 -= num59 * 1000000;
            ret += "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + num59 + " " + Lang.inter[15].Value + "]\n";
        }
        if (num63 >= 10000)
        {
            if (coinIcon)
                ret += "[i:" + ItemID.GoldCoin + "]";
            num60 = num63 / 10000;
            num63 -= num60 * 10000;
            ret += "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + num60 + " " + Lang.inter[16].Value + "]\n";
        }
        if (num63 >= 100)
        {
            if (coinIcon)
                ret += "[i:" + ItemID.SilverCoin + "]";
            num61 = num63 / 100;
            num63 -= num61 * 100;
            ret += "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + num61 + " " + Lang.inter[17].Value + "]\n";
        }
        if (num63 >= 1)
        {
            if (coinIcon)
                ret += "[i:" + ItemID.CopperCoin + "]";
            num62 = num63;
            ret += "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + num62 + " " + Lang.inter[18].Value + "] ";
        }
        if (num59 > 0)
        {
            
        }
        if (num60 > 0)
        {
            
        }
        if (num61 > 0)
        {
            
        }
        if (num62 > 0)
        {
            
        }
        return ret;
    }

    public static List<int> FindPrefixes(Item item)
    {
        int forcedPrefix = ItemLoader.ChoosePrefix(item, Main.rand);
        List<int> prefixes = new();
        List<PrefixCategory> categories = item.GetPrefixCategories();
        foreach (var category in categories)
        {
            int[] vanillaPrefixes = Item.GetVanillaPrefixes(category);
            foreach (int pre in vanillaPrefixes)
            {
                AddPrefix(pre);
            }
            AddCategory(category);
            if (PrefixLoader.IsWeaponSubCategory(category))
            {
                AddCategory(PrefixCategory.AnyWeapon);
            }
        }
        if (PrefixLegacy.ItemSets.ItemsThatCanHaveLegendary2[item.type])
        {
            AddPrefix(84);
        }
        return prefixes;
        void AddCategory(PrefixCategory category)
        {
            foreach (var prefix in PrefixLoader.GetPrefixesInCategory(category).Where((ModPrefix x) => x.CanRoll(item)))
            {
                prefixes.Add(prefix.Type);
            }
        }
        void AddPrefix(int prefix)
        {
            if (item.CanApplyPrefix(prefix) && !prefixes.Contains(prefix))
                prefixes.Add(prefix);
        }
    }

    public static int ComparePrefixs(Item item1, Item item2)
    {
        int prefix1 = item1.prefix;
        int prefix2 = item2.prefix;
        int cmpValue = item1.value.CompareTo(item2.value);
        if (cmpValue != 0)
            return cmpValue;

        return Lang.prefix[prefix1].Value.CompareTo(Lang.prefix[prefix2].Value);
    }
}
