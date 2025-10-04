using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Fargowiltas.Content.UI
{
    public class UIScrollbarClamped : UIScrollbar
    {
        public static FieldInfo _isDragging = typeof(UIScrollbar).GetField("_isDragging", FargoUtils.UniversalBindingFlags);

        public static bool IsDragging = false;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            // This assumes that there's only one of these on the screen, which should be true as long as it's only used for elements of the combined UI.
            // If this UI element is ever used outside of the combined UI, this implementation needs to be changed.
            IsDragging = false; 
            if (_isDragging.GetValue(this) is bool dragging && dragging)
                IsDragging = true;
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
        }
        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
        }
    }
}
