using Fargowiltas.Common.Systems.Collections;
using Fargowiltas.Content.NPCs;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace Fargowiltas.Content.UI.SquirrelUI
{
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
            opacity = IsSelected ? 1f : 0.5f;
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

    public class SquirrelPotionSlot : SquirrelItemSlot
    {
        UIPanel panel;
        UIText text;

        public SquirrelPotionSlot(SquirrelInnerPanel parent) : base(parent, SquirrelUI.PotionMode) { }

        public override bool? HasValidItem()
        {
            if (!HasItem)
                return null;
            return Item.buffType > 0;
        }

        public string GetDisplayString(Item item)
        {
            if (item.IsAir)
                return "";

            int stack = FargoItemSets.SacrificeCount[item.type];
            return $"{stack}/30";
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            if (!HasItem || !IsSelected)
                return;

            base.DrawChildren(spriteBatch);
        }

        public override void OnInitialize()
        {
            text = new("")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            text.Width.Set(0, 1f);

            panel = new UIPanel();
            panel.Top.Set(0, 1f);
            panel.Width.Set(0, 1f);
            panel.Height.Set(30, 0f);

            Append(panel);
            panel.Append(text);

            base.OnInitialize();
        }

        public override void OnItemSwap(ref Item oldItem, ref Item newItem)
        {
            panel.Top.Set(-30, 0f);
            text.SetText(GetDisplayString(newItem));
        }
    }
}
