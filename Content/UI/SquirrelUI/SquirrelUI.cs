using Fargowiltas.Content.NPCs.SquirrelNPC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Content.UI.SquirrelUI;

public class SquirrelUI : FargoUI
{
    public override bool MenuToggleSound => true;

    // UI modes
    public static int FeedMode = 0;

    public static int BackHeight = 350;
    public static int BackWidth = (int)(1.6f * BackHeight);

    UIDragablePanel BackPanel;
    SquirrelInnerPanel InnerPanel;
    UICloseButton CloseButton;



    public override void OnOpen()
    {
        Main.playerInventory = true;
        Main.npcChatText = "";
        base.OnOpen();
    }

    public override void OnClose()
    {
        InnerPanel?.ResetPanel();
        base.OnClose();
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

        InnerPanel = new SquirrelInnerPanel();
        InnerPanel.Left.Set(6, 0f);
        InnerPanel.Top.Set(6 + 28, 0f); // 28 for search bar
        InnerPanel.Width.Set(BackWidth - 12, 0f);
        InnerPanel.Height.Set(BackHeight - 12 - 28, 0);
        InnerPanel.PaddingLeft = InnerPanel.PaddingRight = InnerPanel.PaddingTop = InnerPanel.PaddingBottom = 0;
        InnerPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
        BackPanel.Append(InnerPanel);

        CloseButton = new UICloseButton();
        CloseButton.Left.Set(-24, 1f);
        CloseButton.Top.Set(2, 0);
        CloseButton.OnLeftClick += CloseButton_OnLeftClick;
        BackPanel.Append(CloseButton);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Player player = Main.LocalPlayer;
        if (player.talkNPC == -1 || player.TalkNPC.type != ModContent.NPCType<Squirrel>())
        {
            InnerPanel.ResetPanel();
            FargoUIManager.Close<SquirrelUI>();
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
    }

    private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
    {
        Main.LocalPlayer.SetTalkNPC(-1);
        // cheesy fix for dislocation on close
        (listeningElement.Parent as UIDragablePanel).DragEnd(Main.MouseScreen);
        FargoUIManager.Close<SquirrelUI>();
    }
}
