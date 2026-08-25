namespace Fargowiltas.Content.UI;

/*
public class StatButton : UIState
{
    public UIImage Icon;
    public UIHoverTextImageButton IconHighlight;

    public override void OnActivate()
    {
        Icon = new UIImage(Fargowiltas.UserInterfaceManager.StatsButtonTexture);
        Icon.Left.Set(570, 0f);
        Icon.Top.Set(245, 0f);
        Append(Icon);

        IconHighlight = new UIHoverTextImageButton(Fargowiltas.UserInterfaceManager.StatsButton_MouseOverTexture, Language.GetTextValue("Mods.Fargowiltas.UI.StatButton"));
        IconHighlight.Left.Set(-2, 0f);
        IconHighlight.Top.Set(-2, 0f);
        IconHighlight.SetVisibility(1f, 0f);
        IconHighlight.OnLeftClick += IconHighlight_OnClick;
        Icon.Append(IconHighlight);

        base.OnActivate();
    }

    private void IconHighlight_OnClick(UIMouseEvent evt, UIElement listeningElement)
    {
        if (!Main.playerInventory || Main.LocalPlayer.chest != -1)
        {
            return;
        }

        Fargowiltas.UserInterfaceManager.ToggleStatSheet();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Main.playerInventory && Main.LocalPlayer.chest == -1)
            base.Draw(spriteBatch);
    }
}
*/
