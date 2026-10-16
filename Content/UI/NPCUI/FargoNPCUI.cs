using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Content.UI.NPCUI;

public class FargoNPCUI : FargoUI
{
    #region 

    public virtual Texture2D NPCPortraitFrame => ModContent.Request<Texture2D>("").Value;
    public Texture2D NPCPortrait;

    public string NPCDialogue;

    public string NPCName;
    public void SetNPCDialogue(string portrait, string dialogue, string npcName)
    {
        NPCPortrait = ModContent.Request<Texture2D>(portrait, AssetRequestMode.ImmediateLoad).Value;
        NPCDialogue = Language.GetTextValue(dialogue);
        NPCName = npcName;
    }
    #endregion

    #region Initialization

    public override bool MenuToggleSound => false;

    public override int InterfaceIndex(List<GameInterfaceLayer> layers, int vanillaInventoryIndex) => vanillaInventoryIndex + 1;
    public override string InterfaceLayerName => "Fargos: NPC UI";

    public UIText name;
    public UIText dialogue;
    public UIPanel area;
    public UIImage box;
    public UIImage portrait;
    public UIImageButton shopButton;

    public override void UpdateUI()
    {
        //Main.NewText("wuh");
        //
        if (Main.gameMenu)
            FargoUIManager.Close<FargoNPCUI>();


        /*if (Main.playerInventory)
        {
            IngameFancyUI.Close();
            FargoUIManager.Close<FargoNPCUI>();
        }*/




    }

    public override void OnOpen()
    {
        RemoveAllChildren();
        OnInitialize();
    }

    public override void OnInitialize()
    {
        //NPCShop shop = new(Modcon)

        area = new UIPanel();
        area.Left.Set(-area.Width.Pixels - 1225, 1);
        area.Top.Set(75, 0);
        area.Width.Set(750, 0);
        area.Height.Set(400, 0);
        area.BackgroundColor = new Color(29, 33, 70) * 0.7f;

        portrait = new UIImage(ModContent.Request<Texture2D>("Fargowiltas/Content/UI/Assets/NPCUI/ralsei3"));
        portrait.Left.Set(25, 0);
        portrait.Top.Set(55, 0);
        portrait.Width.Set(200, 0);
        portrait.Height.Set(200, 0);
        //portrait.ImageScale = 3;

        name = new UIText("npc name");
        name.Left.Set(75, 0);
        name.Top.Set(25, 0);
        name.Width.Set(50, 0);
        name.Height.Set(50, 0);

        shopButton = new UIImageButton(ModContent.Request<Texture2D>("Fargowiltas/Content/UI/NPCUI/ShopButton"));
        shopButton.OnLeftClick += new MouseEvent(shopButton_OnClick);
        shopButton.Left.Set(25, 0);
        shopButton.Top.Set(55, 0);


        area.Append(portrait);
        area.Append(name);
        area.Append(shopButton);
        Append(area);

        //base.OnInitialize();
    }
    #endregion

    private void shopButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
    {
        NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
        Main.playerInventory = true;
        //Main.npcShop = Main.MaxShopIDs - 1;
        Main.instance.shop[Main.MaxShopIDs + 1].SetupShop(npc.type);
        SoundEngine.PlaySound(SoundID.MenuClose);
        FargoUIManager.Close<FargoNPCUI>();
    }



    public override void Update(GameTime gameTime)
    {
        //Main.mapReady = false;
        base.Update(gameTime);
    }

    protected override void DrawChildren(SpriteBatch spriteBatch)
    {
        base.DrawChildren(spriteBatch);
    }
}
