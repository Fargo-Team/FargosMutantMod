using Fargowiltas.Content.UI.NPCUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public static class FargoUIManager
    {
        public struct FargoUserInterface
        {
            public UserInterface UserInterface;
            public FargoUI FargoUI;
            public FargoUserInterface(FargoUI fargoUI)
            {
                UserInterface = new();
                FargoUI = fargoUI;
            }
        }
        public static Dictionary<string, FargoUserInterface> UserInterfaces = [];

        private static GameTime LastUpdateUIGameTime { get; set; }

        public static void Register(FargoUI ui)
        {
            UserInterfaces.Add(ui.GetType().Name, new(ui));
        }
        public static FargoUserInterface GetFromDict(FargoUI ui) => UserInterfaces[ui.GetType().Name];
        public static FargoUserInterface GetFromDict<T>() where T : FargoUI => UserInterfaces[typeof(T).Name];
        public static T Get<T>() where T : FargoUI => GetFromDict<T>().FargoUI as T;
        public static void Open<T>() where T : FargoUI
        {
            if (IsOpen<T>())
                return;
            var ui = GetFromDict<T>();
            ui.UserInterface.SetState(ui.FargoUI);
            ui.FargoUI.OnOpen();
            if (ui.FargoUI.MenuToggleSound)
                SoundEngine.PlaySound(SoundID.MenuOpen);
        }
        public static void Open(FargoUI fargoUI)
        {
            if (IsOpen(fargoUI))
                return;
            var ui = GetFromDict(fargoUI);
            ui.UserInterface.SetState(ui.FargoUI);
            ui.FargoUI.OnOpen();
            if (ui.FargoUI.MenuToggleSound)
                SoundEngine.PlaySound(SoundID.MenuOpen);
        }
        public static void Close<T>() where T : FargoUI
        {
            if (!IsOpen<T>())
                return;
            var ui = GetFromDict<T>();
            ui.UserInterface.SetState(null);
            ui.FargoUI.OnClose();
            if (ui.FargoUI.MenuToggleSound)
                SoundEngine.PlaySound(SoundID.MenuClose);
        }
        public static void Close(FargoUI fargoUI)
        {
            if (!IsOpen(fargoUI))
                return;
            var ui = GetFromDict(fargoUI);
            ui.UserInterface.SetState(null);
            ui.FargoUI.OnClose();
            if (ui.FargoUI.MenuToggleSound)
                SoundEngine.PlaySound(SoundID.MenuClose);
        }
        public static void Toggle<T>() where T: FargoUI
        {
            if (!IsOpen<T>())
                Open<T>();
            else
                Close<T>();
        }
        public static void Toggle(FargoUI fargoUI)
        {
            if (!IsOpen(fargoUI))
                Open(fargoUI);
            else
                Close(fargoUI);
        }
        public static bool IsOpen<T>() where T : FargoUI
        {
            var ui = GetFromDict<T>();
            return ui.UserInterface?.CurrentState != null;
        }
        public static bool IsOpen(FargoUI fargoUI)
        {
            var ui = GetFromDict(fargoUI);
            return ui.UserInterface?.CurrentState != null;
        }

        public static void LoadUI()
        {
            if (!Main.dedServ)
            {
                foreach (var ui in UserInterfaces)
                {
                    ui.Value.FargoUI.OnLoad();
                }
            }
        }
        public static void InitializeUI()
        {
            if (!Main.dedServ)
            {
                foreach (var ui in UserInterfaces)
                {
                    ui.Value.FargoUI.Activate();
                }
            }
        }
        public static void UpdateUI(GameTime gameTime)
        {
            LastUpdateUIGameTime = gameTime;

            foreach (var ui in UserInterfaces)
            {
                ui.Value.FargoUI.UpdateUI();
                if (ui.Value.UserInterface?.CurrentState != null)
                    ui.Value.UserInterface.Update(gameTime);
            }
        }

        public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex((layer) => layer.Name == "Vanilla: Inventory");
            if (index != -1)
            {
                foreach (var ui in UserInterfaces)
                {
                    int insertIndex = ui.Value.FargoUI.InterfaceIndex(layers, index);
                    var userInterface = ui.Value.UserInterface;
                    string name = ui.Value.FargoUI.InterfaceLayerName;
                    layers.Insert(index - 1, new LegacyGameInterfaceLayer(name, delegate
                    {
                        if (LastUpdateUIGameTime != null && userInterface?.CurrentState != null)
                            userInterface.Draw(Main.spriteBatch, LastUpdateUIGameTime);

                        return true;
                    }, InterfaceScaleType.UI));
                }

            }

            int resourceIndex = layers.FindIndex((layer) => layer.Name == "Vanilla: Ingame Options");
            if (resourceIndex != -1)
            {
                layers.Insert(resourceIndex, new LegacyGameInterfaceLayer("Fargo's Mutant: Buff Overlay", delegate ()
                {
                    FargoBuffOverlay.Draw(Main.spriteBatch, Main.LocalPlayer);
                    return true;
                }, InterfaceScaleType.UI));
            }
                
            if (FargoUIManager.IsOpen<DevianttNPCUI>())
            {
                foreach (GameInterfaceLayer layer in layers)
                {   
                    if (layer.Name == "Vanilla: Map / Minimap" || layer.Name == "Vanilla: Hotbar" || layer.Name == "Vanilla: Info Accessories Bar" || layer.Name == "Vanilla: Resource Bars")
                    {
                        layer.Active = false;
                    }
                
                }
            }
            
        }
    }
}
