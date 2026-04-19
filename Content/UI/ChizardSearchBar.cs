using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public class ChizardSearchBar : FargoUI
    {
        public UIPanel panel; //bg panel
        public UIPanel[] selectText; //buttons for selecting item to take out
        public UICloseButton closeButton;
        public UIText[] ItemShow;
        public UISearchBar search; //search bar

        public int[] ChestWithItem = [-1, -1, -1, -1, -1]; //chest indexs with items found from search (one index for each select button)
        public int[] IndexOfItem = [-1, -1, -1, -1, -1]; //index of the item in each chest found from search
        public override void OnLoad()
        {
            base.OnLoad();
        }
        public override void OnClose()
        {
            if (FargoClientConfig.Instance.ToggleSearchReset)
            {
                search.Input = "";

            }
            base.OnClose();
        }
        public override void Update(GameTime gameTime)
        {
            Point tilepos = Main.LocalPlayer.FargoMutant().LastInteractedChizard.ToPoint();
            Vector2 pos = Main.LocalPlayer.FargoMutant().LastInteractedChizard.ToWorldCoordinates();
            FargoUtils.TryGetTileEntityAs(tilepos.X, tilepos.Y, out ChestWizardTileEntity TE);
            //if too far or destroyed, disable ui
            if (pos.Distance(Main.LocalPlayer.Center) > 100 || Main.tile[tilepos].TileType != ModContent.TileType<ChestWizardSheet>()){
                FargoUI ui = FargoUIManager.Get<ChizardSearchBar>();
                FargoUIManager.Close(ui);
                SoundEngine.PlaySound(SoundID.MenuClose);
                Main.LocalPlayer.FargoMutant().LastInteractedChizard = Vector2.Zero;
            }

            //disable item use when using ui elements
            if (panel.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            //show item name when hovering over select button
            //for (int i = 0; i < selectText.Length; i++)
            //{
            //    if (selectText[i].IsMouseHovering && ChestWithItem[i] >= 0 && IndexOfItem[i] >= 0)
            //    {
            //        Item item = Main.chest[ChestWithItem[i]].item[IndexOfItem[i]];
            //        Main.instance.MouseText(item.AffixName(), item.rare);
            //    }

            //}
            base.Update(gameTime);
        }
        public override void OnInitialize()
        {
            //Vector2 baseOffset = CombinedUI.CenterRight;
            //Vector2 offset = new(baseOffset.X, baseOffset.Y - 400 / 2f);

            panel = new UIPanel();
            panel.Height.Set(220, 0);
            panel.Width.Set(325, 0);
            panel.Left.Set(6, 0);
            panel.Top.Set(32, 0);
            panel.BackgroundColor = new Color(148, 62, 82) * 0.8f;
            panel.Left.Set(-140, 0.5f);
            panel.Top.Set(50, 0.5f);

            search = new UISearchBar(400 - 8, 26);
            search.Width.Set(300 - 8, 0);
            search.Top.Set(0, 0);
            search.Left.Set(0, 0);
            search.BackPanel.BackgroundColor = new Color(104, 52, 52);
            search.OnTextChange += SearchBar_OnTextChange;

            closeButton = new();
            closeButton.OnLeftClick += CloseButton_OnLeftClick;
            closeButton.Left.Set(-11, 1);
            closeButton.Top.Set(-12, 0);

            selectText = new UIPanel[5];
            ItemShow = new UIText[5];
            for (int i = 0; i < selectText.Length; i++)
            {
                selectText[i] = new UIPanel();
                UIPanel spanel = selectText[i];
                spanel.Height.Set(26, 0);
                spanel.Top.Set((26 + 8)*(i+1), 0);
                spanel.Width.Set(search.Width.Pixels, 0);
                spanel.Left.Set(search.Left.Pixels, 0);
                spanel.BackgroundColor = new Color(104, 52, 52);
                spanel.OnLeftClick += SelectButton_OnLeftClick;
                panel.Append(selectText[i]);

                ItemShow[i] = new UIText("");
                
                ItemShow[i].HAlign = 0f;
                ItemShow[i].VAlign = 0.6f;
                spanel.Append(ItemShow[i]);

            }
            

            Append(panel);
            panel.Append(search);
            panel.Append(closeButton);
            base.OnInitialize();
        }

        private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            FargoUI ui = FargoUIManager.Get<ChizardSearchBar>();
            FargoUIManager.Close(ui);
            SoundEngine.PlaySound(SoundID.MenuClose);
            Main.LocalPlayer.FargoMutant().LastInteractedChizard = Vector2.Zero;
        }
        public void HandleTakeItem(Chest chest, Item item)
        {
            //Chest chest = Main.chest[ChestWithItem[index]];
            //Item item = chest.item[IndexOfItem[index]];

            Point tilepos = Main.LocalPlayer.FargoMutant().LastInteractedChizard.ToPoint();
            Vector2 pos = Main.LocalPlayer.FargoMutant().LastInteractedChizard.ToWorldCoordinates();

            FargoUtils.TryGetTileEntityAs(tilepos.X, tilepos.Y, out ChestWizardTileEntity entity);
            Chest.VisualizeChestTransfer(new Vector2(chest.x, chest.y).ToWorldCoordinates(), Main.LocalPlayer.Center, item, item.stack);
            Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_TileInteraction(tilepos.X, tilepos.Y), item, item.stack);

            item.TurnToAir();
            chest.frame = 2;
            chest.frameCounter = 60;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                FargoNet.SendChizardTookItem(chest.item.ToList().IndexOf(item), chest.x, chest.y);
            }
            SoundEngine.PlaySound(SoundID.Item8, new Vector2(chest.x, chest.y).ToWorldCoordinates());
            SearchBar_OnTextChange(search.Input, search.Input);
        }
        private void SelectButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            int index = -1;
            for (int i = 0; i < ChestWithItem.Length; i++)
            {
                if (ChestWithItem[i] >= 0 && IndexOfItem[i] >= 0 && listeningElement == selectText[i])
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
            {
                
                Chest chest = Main.chest[ChestWithItem[index]];
                
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    //makes sure the item is still there before spawning it in multiplayer
                    FargoNet.RequestTakeItemOut(IndexOfItem[index], chest.item[IndexOfItem[index]], chest.x, chest.y);
                }
                else
                {
                    HandleTakeItem(chest, chest.item[IndexOfItem[index]]);
                }

            }
        }
        
        public void SearchBar_OnTextChange(string oldText, string currentText)
        {
            Player player = Main.LocalPlayer;
            Vector2 pos = player.Center;

            int[] chestWithItem = [-1, -1, -1, -1, -1];
            int[] indexOfItem = [-1, -1, -1, -1, -1];
            int[] scoreOfItem = [-1, -1, -1, -1, -1];
            
            for (int i = 0; i < Main.chest.Length; i++)
            {
                Chest chest = Main.chest[i];
                if (chest == null || TileLoader.IsLockedChest(chest.x, chest.y, Main.tile[chest.x,chest.y].TileType) || Chest.IsLocked(chest.x,chest.y)) continue;
                
                if (new Point(chest.x, chest.y).ToWorldCoordinates().Distance(pos) < 1000)
                {
                    
                    for (int j = 0; j < chest.item.Length; j++)
                    {
                        int score = 0;
                        Item item = chest.item[j];
                        if (item != null && item.type != ItemID.None)
                        {
                            string name = Lang.GetItemName(item.type).Value;
                            for (int c = currentText.Length; c > 0; c--)
                            {
                                if (name.ToLower().Contains(currentText.ToLower().Substring(0, c)))
                                {
                                    score = c;
                                    break;
                                }
                            }
                        }
                        for (int s = 0; s < scoreOfItem.Length; s++)
                        {
                            
                            if (score > scoreOfItem[s] && (scoreOfItem[s] == -1 || !scoreOfItem.Contains(-1)) && score != 0)
                            {
                                indexOfItem[s] = j;
                                chestWithItem[s] = i;
                                scoreOfItem[s] = score;
                                break;
                            }
                        }
                    }
                }
            }
            //Main.NewText(in);
            for (int i = 0; i < scoreOfItem.Length; i++)
            {
                if (chestWithItem[i] >= 0 && indexOfItem[i] >= 0)
                {
                    ChestWithItem[i] = chestWithItem[i];
                    IndexOfItem[i] = indexOfItem[i];

                    Item item = Main.chest[chestWithItem[i]].item[indexOfItem[i]];
                    string text = "";
                    if (item.prefix > 0)
                    {
                        text = "[i/p" + item.prefix;
                    }
                    else if (item.stack > 1)
                    {
                        text = "[i/s" + item.stack;
                    }
                    else
                    {
                        text = "[i";
                    }
                    string name = item.AffixName();
                    if (name.Length > 28) name = name.Substring(0, 28);
                    text += ":" + item.type + "] " + "[c/" + ItemRarity.GetColor(item.rare).Hex3() + ":"+ name+"]";
                    ItemShow[i].SetText(text);
                }
                else
                {
                    ChestWithItem[i] = -1;
                    IndexOfItem[i] = -1;
                    ItemShow[i].SetText("");
                }
            }
        }
    }
}
