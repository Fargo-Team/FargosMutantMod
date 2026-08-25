using Fargowiltas.Content.NPCs.SquirrelNPC;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace Fargowiltas.Content.UI.SquirrelUI;

public class SquirrelItemSlot : FargoItemSlot
{
    SquirrelInnerPanel parent;
    public int mode;
    public bool IsSelected { get { return mode == parent.mode; } }

    public Action<Item> OnSwap;

    public SquirrelItemSlot(SquirrelInnerPanel parent, int mode) : base()
    {
        this.parent = parent;
        this.mode = mode;
    }

    public override bool PreDrawSelf(SpriteBatch spriteBatch)
    {
        Opacity = IsSelected ? 1f : 0.5f;
        return base.PreDrawSelf(spriteBatch);
    }

    public virtual bool? HasValidItem()
    {
        if (!HasItem)
            return null;
        return true;
    }
}

public class SquirrelFeedSlot : SquirrelItemSlot
{
    public SquirrelFeedSlot(SquirrelInnerPanel parent) : base(parent, SquirrelUI.FeedMode) { }

    public override bool? HasValidItem()
    {
        if (!HasItem)
            return null;
        return Squirrel.CanSacrifice(Item);
    }
}
