using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public class FargoUI : UIState, ILoadable
    {
        ///<summary>
        /// The mod this belongs to.
        /// </summary>
        public Mod Mod { get; internal set; }
        public static int Index { get; internal set; }
        void ILoadable.Load(Mod mod)
        {
            Mod = mod;
            FargoUIManager.Register(this);
        }
        void ILoadable.Unload() 
        { 

        }
        ///<summary>
        /// Whether the UI plays the vanilla menu open/close sound when opened/closed.
        /// </summary>
        public virtual bool MenuToggleSound => false;
        ///<summary>
        /// Allows you to perform one-time loading tasks.
        /// </summary>
        public virtual void OnLoad() { }
        ///<summary>
        /// When the UI is opened.
        /// </summary>
        public virtual void OnOpen() { }
        ///<summary>
        /// When the UI is closed.
        /// </summary>
        public virtual void OnClose() { }
        ///<summary>
        /// Updates that happen whether the UI is open or not.
        /// </summary>
        public virtual void UpdateUI() { }

        ///<summary>
        /// Which index in the list of GameInterfaceLayers the UI should be inserted into. <br />
        /// Inserts before the vanilla inventory index by default.
        /// </summary>
        public virtual int InterfaceIndex(List<GameInterfaceLayer> layers, int vanillaInventoryIndex) { return vanillaInventoryIndex; }
        public virtual string InterfaceLayerName => $"Fargos: {GetType().Name}";
    }
}
