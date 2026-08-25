using Fargowiltas.Assets.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Fargowiltas.Content.UI;

public class CombinedUI : FargoUI
{
    public override int InterfaceIndex(List<GameInterfaceLayer> layers, int vanillaInventoryIndex) => vanillaInventoryIndex;
    public override string InterfaceLayerName => "Fargos: Combined UI";

    public override bool MenuToggleSound => true;

    // note: lower priority -> further up in list
    public static void AddUI<T>(LocalizedText text, float priority) where T : FargoUI
    {
        AddUI(FargoUIManager.Get<T>(), text, priority);
    }
    public static void AddUI(FargoUI ui, LocalizedText text, float priority)
    {
        if (List.Any(e => e.UI == ui))
            return;
        var element = new CombinedUIElement(text, ui, priority);
        List.Add(element);
    }

    public UIPanel BackPanel;

    public static Vector2 CenterRight
    {
        get
        {
            var dims = FargoUIManager.Get<CombinedUI>().BackPanel.GetDimensions();
            return new Vector2(dims.X + dims.Width + 6, dims.Y + dims.Height / 2);
        }
    }

    public static List<CombinedUIElement> List = [];
    public static int ListElements => List.Count;
    public override void OnLoad()
    {
        //FargoUIManager.Open<CombinedUI>();
    }
    public override void UpdateUI()
    {
        if (!Main.playerInventory)
            FargoUIManager.Close<CombinedUI>();
    }
    public override void OnInitialize()
    {
        // placement
        int x = CombinedUIButton.x;
        int y = CombinedUIButton.y;

        int width = 180;
        int wrenchHalfWidth = 15;

        int spacing = 6;
        int elementHeight = 40;
        int elementWidth = width - 2 * spacing;

        int height = spacing + (elementHeight + spacing) * ListElements;

        Vector2 offset = new(x - width + wrenchHalfWidth * 2, y + 40);

        // back panel

        BackPanel = new UIPanel();
        BackPanel.Left.Set(offset.X, 0f);
        BackPanel.Top.Set(offset.Y, 0);
        BackPanel.Width.Set(width, 0);
        BackPanel.Height.Set(height, 0);
        BackPanel.PaddingLeft = BackPanel.PaddingRight = BackPanel.PaddingTop = BackPanel.PaddingBottom = 0;
        BackPanel.BackgroundColor = new Color(29, 33, 70) * 0.3f;

        Append(BackPanel);
        // append elements

        // top left
        int elX = spacing;
        int elY = spacing;

        List.Sort((x, y) => x.Priority.CompareTo(y.Priority));

        for (int i = 0; i < ListElements; i++)
        {
            var button = List[i];
            button.Left.Set(elX, 0);
            button.Top.Set(elY, 0);
            button.Width.Set(elementWidth, 0);
            button.Height.Set(elementHeight, 0);

            button.OnLeftClick += Button_OnLeftClick;
            BackPanel.Append(button);

            elY += elementHeight + spacing;
        }
    }
    private static void Button_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
    {
        if (listeningElement is CombinedUIElement element)
        {
            ToggleUI(element.UI);
        }
    }
    public static void ToggleUI(FargoUI ui)
    {
        if (FargoUIManager.IsOpen(ui))
        {
            FargoUIManager.Close(ui);
        }
        else
        {
            Main.playerInventory = true;
            FargoUIManager.Open<CombinedUI>();

            for (int i = 0; i < ListElements; i++)
            {
                if (List[i].UI != ui)
                    FargoUIManager.Close(List[i].UI);
            }

            FargoUIManager.Open(ui);
        }
    }
    public static void ToggleUI<T>() where T : FargoUI
    {
        ToggleUI(FargoUIManager.Get<T>());
    }
    public override void OnActivate()
    {
        base.OnActivate();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        BackPanel.Draw(spriteBatch);
    }
}

public class CombinedUIElement : UIElement
{
    public UIPanel Panel;
    public UIText Text;
    public FargoUI UI;
    public float Priority;

    public bool Selected;

    public CombinedUIElement(LocalizedText text, FargoUI ui, float priority)
    {
        //Panel = new UIPanel();
        //Panel.Width.Set(0, 1);
        //Panel.Height.Set(0, 1);
        //Append(Panel);
        Text = new UIText(text);
        Text.Top.Set(0, 0);
        Text.Left.Set(6, 0);
        Text.Width.Set(-6, 1);
        Text.Height.Set(0, 1);
        Text.TextOriginX = 0;
        Text.TextOriginY = 0.5f;
        Append(Text);

        UI = ui;
        Priority = priority;
    }

    public int Frame;

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        CalculatedStyle dimensions = GetDimensions();

        bool isSelected = FargoUIManager.IsOpen(UI);
        bool isHovered = IsMouseHovering;
        Asset<Texture2D> Texture = FargoMutantAssets.UI.CombinedUITab;
        Rectangle rect = new(0, Texture.Height() / 4 * Frame, Texture.Width(), Texture.Height() / 4);
        rect.Height -= 2;
        spriteBatch.Draw(Texture.Value, new((int)dimensions.X, (int)dimensions.Y), rect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        //Utils.DrawSplicedPanel(spriteBatch, baseTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, color * num);
        if (isSelected)
        {
            Frame = 2;
            if (isHovered)
                Frame = 3;

        }
        if (isHovered)
        {
            Main.LocalPlayer.mouseInterface = true;
            if (!isSelected)
                Frame = 1;
        }

        if (!isHovered && !isSelected)
            Frame = 0;
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        //Selected = !Selected;
    }
}
