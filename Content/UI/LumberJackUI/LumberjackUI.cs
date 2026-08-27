using Fargowiltas.Content.Achievements;
using Fargowiltas.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using static Fargowiltas.Content.UI.LumberjackUI.LumberJackBiome;

namespace Fargowiltas.Content.UI.LumberjackUI;

internal class LumberJackUI : FargoUI
{
    UIPanel BackPanel;
    LumberjackInfoPanel InnerPanel;
    UIList BiomeList;

    LumberJackBiome SelectedBiome = null;
    Color oldColor = new Color(73, 94, 171) * 0.9f;
    float swapCooldown = 0;

    float BackWidth = 450;
    float BackHeight = 390 * 1.6f;
    float swapCDMax = 30;

    public override bool MenuToggleSound => true;

    void BiomeOptionPressed(LumberJackBiome newBiome)
    {
        if (swapCooldown > 0)
            return;

        if (SelectedBiome != null)
            oldColor = SelectedBiome.BackgroundColor;
        SelectedBiome = newBiome;
        swapCooldown = swapCDMax;

        RebuildInfoPanel();
    }

    public override void OnInitialize()
    {
        RemoveAllChildren();
        BackPanel = new UIPanel();
        BackPanel.Width.Set(BackWidth, 0);
        BackPanel.Height.Set(BackHeight, 0);
        BackPanel.Left.Set((Main.screenWidth - BackWidth) / 2, 0);
        BackPanel.Top.Set((Main.screenHeight - BackHeight) / 2, 0);
        Append(BackPanel);

        InnerPanel = new LumberjackInfoPanel(null);
        InnerPanel.Width.Set(BackWidth - 6 - 40 - 12, 0);
        InnerPanel.Height.Set(BackHeight - 6, 0);
        InnerPanel.Left.Set(40, 0);
        BackPanel.Append(InnerPanel);

        BiomeList = new UIList();
        BiomeList.Left.Set(0, 0);
        BiomeList.Width.Set(60, 0);
        BiomeList.Top.Set(10, 0);
        BiomeList.Height.Set(BackHeight - 6 - 10, 0);
        BackPanel.Append(BiomeList);

        RebuildList();

        base.OnInitialize();
    }

    void RebuildList()
    {
        BiomeList.Clear();

        List<LumberjackListOption> options = [];
        foreach (var biome in LumberjackBiomeRegistry.GetBiomes())
        {
            bool selected = biome == SelectedBiome;
            var listOption = new LumberjackListOption(biome, selected);
            listOption.Width.Set(0, 1f);
            listOption.Height.Set(42, 0);
            listOption.OnPress = BiomeOptionPressed;
            options.Add(listOption);
        }
        options.Sort();
        BiomeList.AddRange(options);
    }

    void RebuildInfoPanel()
    {
        InnerPanel.ChangeBiomeScene(SelectedBiome);

        if (SelectedBiome == null) return;


    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (swapCooldown > 0)
            swapCooldown--;

        if (Main.GameUpdateCount % 4 == 0)
            RebuildList();

        Player player = Main.LocalPlayer;
        if (BackPanel.ContainsPoint(Main.MouseScreen))
            player.mouseInterface = true;

        if (player.dead || !player.active || player.TalkNPC == null || player.TalkNPC.type != ModContent.NPCType<LumberJack>())
        {
            FargoUIManager.Close(this);
        }
    }

    public override void OnClose()
    {
        base.OnClose();

        SelectedBiome = null;
        swapCooldown = 0;
    }
    public override void OnOpen()
    {
        OnInitialize();
        base.OnOpen();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (BackPanel == null) return;

        Vector2 savingsPos = BackPanel.GetOuterDimensions().Position() + new Vector2(0, BackPanel.GetOuterDimensions().Height - 30);
        ItemSlot.DrawSavings(spriteBatch, savingsPos.X, savingsPos.Y, true);
    }
}

internal class LumberjackListOption : UIElement, IComparable
{
    Point drawFrame;
    public Action<LumberJackBiome> OnPress;
    Asset<Texture2D> texture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Tabs_A");
    LumberJackBiome biome;
    bool selected;

    public LumberjackListOption(LumberJackBiome biome, bool selected)
    {
        this.biome = biome;
        this.selected = selected;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (ContainsPoint(Main.MouseScreen))
        {
            drawFrame.X = 2;
            string mouseText = biome.IsAvailable ? $"{biome.GetLocalizedText("Name")}" : "???";
            UICommon.TooltipMouseText(mouseText);
        }
        else
        {
            drawFrame.X = 3;
        }
        drawFrame.Y = 0;

        if (selected)
        {
            drawFrame.Y += 1;
            drawFrame.X -= 2;
        }

        Vector2 center = GetOuterDimensions().Center();
        Point frameOffset = new Point(drawFrame.X == 1 ? -1 : 0, drawFrame.Y == 0 ? -1 : 0); // i hate this game.
        Rectangle frame = texture.Value.Frame(4, 2, drawFrame.X, drawFrame.Y, frameOffset.X, frameOffset.Y);
        Vector2 origin2 = frame.Size() / 2;
        spriteBatch.Draw(texture.Value, center - new Vector2(1, 0), frame, Color.White, -MathHelper.PiOver2, origin2, 1f, SpriteEffects.None, 0);

        Rectangle iconFrame = biome.frame ?? biome.icon.Frame();
        spriteBatch.Draw(biome.icon.Value, center - 6f * Vector2.UnitX, iconFrame, biome.IsAvailable ? Color.White : Color.Black, 0f, iconFrame.Size() / 2, 1f, SpriteEffects.None, 0f);
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);

        SoundEngine.PlaySound(SoundID.MenuTick);
        if (selected)
            return;

        OnPress.Invoke(biome);
    }

    public override int CompareTo(object obj)
    {
        if (obj is LumberjackListOption opt)
        {
            if (biome.IsAvailable && !opt.biome.IsAvailable)
                return -1;
            if (!biome.IsAvailable && opt.biome.IsAvailable)
                return 1;

            return biome.GetBuyPrice().CompareTo(opt.biome.GetBuyPrice());
        }
        return base.CompareTo(obj);
    }
}

internal class LumberjackInfoPanel : UIPanel
{
    readonly Asset<Texture2D> line = Main.Assets.Request<Texture2D>("Images/Extra_178");
    public Color oldColor;
    public LumberJackBiome biome;
    private float maxTime = 30;
    float timer;

    UIPanel TitlePanel;
    UIPanel WoodPanel;
    LumberJackItemPanel CritterPanel;
    LumberJackItemPanel FruitPanel;
    UIPanel DialoguePanel;
    LumberJackBottomPanel BottomPanel;
    LumberJackPurchaseButton BuyButton;

    public LumberjackInfoPanel(LumberJackBiome biome)
    {
        this.biome = biome;
        this.oldColor = Color.Transparent;
        timer = 0;

        RebuildPanels();
    }

    internal void ChangeBiomeScene(LumberJackBiome newBiome)
    {
        if (biome != null)
            oldColor = biome.IsAvailable ? biome.BackgroundColor : Color.Transparent;
        biome = newBiome;
        timer = maxTime;

        RebuildPanels();
    }

    void RebuildPanels()
    {

        RemoveAllChildren();
        if (biome == null) return;

        if (!biome.IsAvailable)
        {
            var missingText = new UIText(Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.MissingPylon"))
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            Append(missingText);
            return;
        }

        float panelWidth = Width.Pixels;
        float panelHeight = Height.Pixels;

        TitlePanel = new UIPanel();
        TitlePanel.Width.Set((0.5f * panelWidth) - 18, 0);
        TitlePanel.Height.Set(50, 0);
        TitlePanel.Left.Set(0, 0);
        TitlePanel.Top.Set(0, 0);
        Append(TitlePanel);

        UIText biomeText = new UIText($"{biome.GetLocalizedText("Name")} {Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.Biome")}")
        {
            HAlign = 0.5f,
            VAlign = 0.5f
        };
        TitlePanel.Append(biomeText);

        WoodPanel = new UIPanel();
        WoodPanel.Width.Set((panelWidth / 2) - 18, 0);
        WoodPanel.Height.Set(50, 0);
        WoodPanel.Left.Set((0.5f * panelWidth) - 6, 0);
        WoodPanel.Top.Set(0, 0);
        Append(WoodPanel);

        string wText = biome.Wood.type > 0 ? $"{Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.WoodTitle")} [i:{biome.Wood.type}] x{biome.Wood.amount}" : "None";
        UIText woodText = new UIText(wText)
        {
            HAlign = 0.5f,
            VAlign = 0.5f
        };
        WoodPanel.Append(woodText);

        FruitPanel = new LumberJackItemPanel("FruitTitle", biome.Fruits);
        FruitPanel.Width.Set((panelWidth / 2) - 18, 0);
        FruitPanel.Height.Set(250, 0);
        FruitPanel.Left.Set(0, 0);
        FruitPanel.Top.Set(50 + 6, 0);
        Append(FruitPanel);

        CritterPanel = new LumberJackItemPanel("CritterTitle", biome.Critters);
        CritterPanel.Width.Set((panelWidth / 2) - 18, 0);
        CritterPanel.Height.Set(250, 0);
        CritterPanel.Left.Set((0.5f * panelWidth) - 6, 0);
        CritterPanel.Top.Set(50 + 6, 0);
        Append(CritterPanel);

        DialoguePanel = new UIPanel();
        DialoguePanel.Width.Set(panelWidth, 0);
        DialoguePanel.Height.Set(220, 0);
        DialoguePanel.Top.Set(250 + 12 + 50 + 6, 0);
        DialoguePanel.Left.Set(0, 0);
        DialoguePanel.HAlign = 0.5f;
        Append(DialoguePanel);

        UIText dialogue = new UIText(biome.GetLocalizedText("Description"));
        dialogue.IsWrapped = true;
        dialogue.Width.Set(0, 1f);
        DialoguePanel.Append(dialogue);

        BottomPanel = new LumberJackBottomPanel(biome);
        BottomPanel.Top.Set(50 + 200 + 12 + 6 + 270, 0);
        BottomPanel.Left.Set(0, 0);
        BottomPanel.Width.Set(panelWidth, 0);
        BottomPanel.Height.Set(50, 0);

        Append(BottomPanel);

        BuyButton = new LumberJackPurchaseButton(biome)
        {
            VAlign = 0.5f
        };
        BuyButton.Left.Set(panelWidth - 100 - 18, 0);
        BuyButton.Width.Set(40, 0);
        BuyButton.Height.Set(40, 0);
        BottomPanel.Append(BuyButton);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Main.GameUpdateCount % 4 == 0)
        {
            //RebuildPanels();
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (biome == null) return;

        Color drawColor = Color.Lerp(biome.IsAvailable ? biome.BackgroundColor : Color.Transparent, oldColor, timer / maxTime) * 0.5f;
        Vector2 position = GetOuterDimensions().Center() + new Vector2(2 - GetOuterDimensions().Width / 2, GetOuterDimensions().Height / 2 - 2);
        spriteBatch.Draw(line.Value, position, line.Frame(), drawColor, -MathHelper.PiOver2, Vector2.Zero, new Vector2(1f, (Width.Pixels / 2) - 2), SpriteEffects.None, 0f);

        if (timer > 0)
            timer--;

        if (!biome.IsAvailable)
        {
            Item pylon = new Item(biome.PylonType.HasValue ? biome.GetPylonItemType() : ItemID.TeleportationPylonVictory);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

            ItemSlot.DrawItemIcon(pylon, ItemSlot.Context.InventoryItem, spriteBatch, GetInnerDimensions().Center() + new Vector2(0, 56), 2f, 50, Color.Black);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
        }
    }

    protected override void DrawChildren(SpriteBatch spriteBatch)
    {
        if (biome == null)
            return;

        base.DrawChildren(spriteBatch);
    }
}

internal class LumberJackBottomPanel : UIPanel
{
    LumberJackBiome biome;

    internal LumberJackBottomPanel(LumberJackBiome biome)
    {
        this.biome = biome;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        //base.DrawSelf(spriteBatch);

        Vector2 moneyPos = GetInnerDimensions().Position() + new Vector2(GetInnerDimensions().Width / 4, -GetInnerDimensions().Height - 12);
        ItemSlot.DrawMoney(spriteBatch, Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.Cost"), moneyPos.X, moneyPos.Y, Utils.CoinsSplit(biome.GetBuyPrice()), true);
    }
}

internal class LumberJackItemPanel : UIPanel
{
    string suffix;

    UIList ItemList;

    internal LumberJackItemPanel(string suffix, List<LumberJackItem> items)
    {
        this.suffix = suffix;

        RebuildItems(items);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    void RebuildItems(List<LumberJackItem> items)
    {
        RemoveAllChildren();

        UIText title = new UIText(Language.GetTextValue($"Mods.Fargowiltas.UI.LumberJack.{suffix}"));
        title.Top.Set(0, 0);
        title.HAlign = 0.5f;
        Append(title);

        float tWeight = 0;
        foreach (LumberJackItem item in items)
        {
            tWeight += item.chance.Invoke();
        }

        ItemList = new UIList();
        ItemList.Width.Set(0, 1f);
        ItemList.Height.Set(0, 1f);
        ItemList.Top.Set(30, 0);
        Append(ItemList);

        List<LumberJackItemElement> elements = [];
        foreach (LumberJackItem item in items)
        {
            if (item.chance.Invoke() <= 0) continue;

            var uElement = new LumberJackItemElement(item, tWeight);
            uElement.Height.Set(50, 0);
            uElement.Width.Set(0, 1f);
            elements.Add(uElement);
        }
        if (elements.Count == 0)
        {
            UIText empty = new UIText(Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.Empty"))
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            Append(empty);
        }
        else
        {
            ItemList.AddRange(elements);
            Append(ItemList);
        }
    }
}

internal class LumberJackItemElement : UIPanel
{
    UIText infoText;
    LumberJackItem item;
    int typeIndex = 0;
    int typeTimer = 0;

    internal LumberJackItemElement(LumberJackItem item, float totalWeight)
    {
        this.item = item;

        infoText = new UIText($"{MathF.Round(100 * (item.chance.Invoke() / totalWeight), 0)}%")
        {
            HAlign = 0.4f,
            VAlign = 0.5f
        };
        Append(infoText);

        if (item.rollAmount <= 1)
            return;

        var rollText = new UIText($"[s:Fargowiltas/Dice]x{item.rollAmount}");
        rollText.VAlign = 0.5f;
        rollText.HAlign = 1f;
        Append(rollText);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        int typeCount = item.types.Count;
        if (typeTimer++ > 60)
        {
            typeIndex++;
            typeTimer = 0;
            if (typeIndex == typeCount)
                typeIndex = 0;
        }

        Item drawItem = new Item(item.types[typeIndex])
        {
            stack = item.stack
        };
        ItemSlot.DrawItemIcon(drawItem, ItemSlot.Context.InventoryItem, spriteBatch, GetInnerDimensions().Position() + new Vector2(GetInnerDimensions().Height / 2), 1f, GetInnerDimensions().Width, Color.White);

        if (ContainsPoint(Main.MouseScreen))
        {
            UICommon.TooltipMouseText(GetHoverText());
        }
    }

    string GetHoverText()
    {
        string text = $"{Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.ItemHoverTitle")} ";
        foreach (var type in item.types)
        {
            text += $"[i/s{item.stack}:{type}]";
        }
        if (item.rollAmount > 1)
            text += $"\n[s:Fargowiltas/Dice] {Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.ItemHoverRoll", item.rollAmount)}";

        return text;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}

internal class LumberJackPurchaseButton : UIElement
{
    public LumberJackBiome biome;

    internal LumberJackPurchaseButton(LumberJackBiome biome)
    {
        this.biome = biome;
    }

    const string filePath = "Fargowiltas/Assets/Textures/UI/LumberJackPurchaseButton";
    readonly Asset<Texture2D> texture = ModContent.Request<Texture2D>(filePath);
    readonly Asset<Texture2D> glowTexture = ModContent.Request<Texture2D>(filePath + "_MouseOver");
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (ContainsPoint(Main.MouseScreen))
        {
            Rectangle frame = glowTexture.Frame();

            spriteBatch.Draw(glowTexture.Value, GetOuterDimensions().Center(), frame, Color.White, 0f, frame.Size() / 2, 1f, SpriteEffects.None, 0f);

            UICommon.TooltipMouseText(Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.Buy"));
        }
        else
        {
            Rectangle frame = texture.Frame();

            spriteBatch.Draw(texture.Value, GetOuterDimensions().Center(), frame, Color.White, 0f, frame.Size() / 2, 1f, SpriteEffects.None, 0f);
        }
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);

        Player player = Main.LocalPlayer;
        if (player.CanAfford(biome.GetBuyPrice()))
        {
            if (Main.myPlayer == player.whoAmI)
            {
                ModContent.GetInstance<TreeTreasureAchievements.T1TreeTreasureAchievement>().Condition.Complete();


                switch (biome.PylonType)
                {
                    case TeleportPylonType.SurfacePurity:
                        ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().PurityCondition.Complete();
                        break;
                    case TeleportPylonType.Desert:
                        ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().DesertCondition.Complete();
                        break;
                    case TeleportPylonType.Underground:
                        {
                            if (biome.ID == "Underworld") // this check can be removed in 1.4.5 due to Underworld Pylon existing
                                ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().UnderworldCondition.Complete();
                            ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().CavernCondition.Complete();
                        }
                        break;
                    case TeleportPylonType.Snow:
                        {
                            ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().SnowCondition.Complete();
                        }
                        break;
                    case TeleportPylonType.Jungle:
                        ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().JungleCondition.Complete();
                        break;
                    case TeleportPylonType.Beach:
                        ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().BeachCondition.Complete();
                        break;
                    case TeleportPylonType.Hallow:
                        ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().HallowCondition.Complete();
                        break;
                    case TeleportPylonType.GlowingMushroom:
                        ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().MushroomCondition.Complete();
                        break;
                    case null:
                        {
                            if (biome.ID == "Crimson")
                                ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().CrimsonCondition.Complete();

                            if (biome.ID == "Corrupt")
                                ModContent.GetInstance<TreeTreasureAchievements.T2TreeTreasureAchievement>().CorruptCondition.Complete();
                        }
                        break;
                }

            }
            SoundEngine.PlaySound(SoundID.Coins);
            player.BuyItem(biome.GetBuyPrice());
            biome.RollTreasures(ref player);
            player.SetTalkNPC(-1);
            Main.playerInventory = true;
        }
        else
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);

        SoundEngine.PlaySound(SoundID.MenuTick);
    }
}
