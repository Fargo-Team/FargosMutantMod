using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Fargowiltas.Content.UI
{
    public class FargoItemSlot : UIElement
    {
        const float baseWidth = 52f;

        private static Asset<Texture2D> backPanel => TextureAssets.InventoryBack;
        private Item _item;

        public Item Item { get { return _item; } }

        public FargoItemSlot(float scale = 1f)
        {
            _item = new Item();
            _item.TurnToAir();

            this.scale = scale;
            Width.Set(scale * baseWidth, 0);
            Height.Set(scale * baseWidth, 0);
        }

        protected override sealed void DrawSelf(SpriteBatch spriteBatch)
        {
            HandleItem();

            if (PreDrawSelf(spriteBatch))
            {
                base.DrawSelf(spriteBatch);

                DrawItemSlot(spriteBatch);
            }
            PostDrawSelf(spriteBatch);
        }

        public void HandleItem()
        {
            if (!ContainsPoint(Main.MouseScreen))
                return;

            bool canAccept = Main.mouseItem.IsAir || CanAcceptItem(Main.mouseItem.Clone());
            if (!Unchangable && canAccept)
            {
                Item oldItem = _item.Clone();
                bool leftClick = Main.mouseLeftRelease && Main.mouseLeft;
                if (leftClick)
                {
                    ItemSlot.LeftClick(ref _item);
                }
                ItemSlot.RightClick(ref _item);
                if (_item.IsNotSameTypePrefixAndStack(oldItem))
                {
                    OnItemSwap(ref oldItem, ref _item);
                }
            }
            ItemSlot.MouseHover(ref _item);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;

            // prevent 'phantom' items
            if (_item.type == ItemID.None || _item.stack < 1)
                _item = new Item();
        }

        private void DrawItemSlot(SpriteBatch spriteBatch)
        {

            Vector2 position = GetOuterDimensions().Position();
            Vector2 center = GetOuterDimensions().Center();
            if (DrawItemFrame)
                spriteBatch.Draw(backPanel.Value, center, backPanel.Value.Frame(), Main.inventoryBack * Opacity, 0f, backPanel.Value.Frame().Size() / 2, scale, SpriteEffects.None, 0);


            if (HasItem)
            {
                ItemSlot.DrawItemIcon(_item, ItemSlot.Context.InventoryItem, spriteBatch, center, scale, baseWidth - 10 * scale, ItemColor * Opacity);

                if (_item.stack > 1)
                {
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, _item.stack.ToString(), position + new Vector2(28f - (18f * scale), 28f - (2f * scale)), Color.White * Opacity, 0f, Vector2.Zero, new Vector2(scale), -1f, scale);
                }
            }
        }

        #region Public Members
        /// <summary>
        /// The scale to draw the item slot. <para/>
        /// </summary>
        public float scale = 1f;

        /// <summary>
        /// The opacity to draw the item slot. <para/>
        /// </summary>
        public float Opacity = 1f;

        public bool HasItem => !_item.IsAir;

        /// <summary>
        /// If true, then the frame behind the item will be drawn.
        /// <para/> Defaults to <see langword="true"/>.
        /// </summary>
        public bool DrawItemFrame = true;

        /// <summary>
        /// If true, then the item will be displayed as an item tooltip when hovered.
        /// <para/> Defaults to <see langword="true"/>.
        /// </summary>
        public bool ItemHover = true;

        /// <summary>
        /// If true, the grab sound will play when the item is swapped.
        /// <para/> Defaults to <see langword="true"/>.
        /// </summary>
        public bool PlaySwapSound = true;

        /// <summary>
        /// If true, this item slot cannot be affected by the player in any way.
        /// <para/> Defaults to <see langword="false"/>.
        /// </summary>
        public bool Unchangable = false;

        /// <summary>
        /// Determines what color the item should be drawn as.
        /// <para/> Defaults to <see cref="Color.White"/>.
        /// </summary>
        public Color ItemColor = Color.White;
        #endregion

        #region Public Methods
        /// <summary>
        /// Removes the stored item from this item slot.
        /// <para/> This will do nothing if this item slot does not contain an item.
        /// </summary>
        /// <returns>A clone of the removed item, or <see langword="null"/> if there is no item to remove</returns>
        public Item RemoveItem()
        {
            if (!HasItem)
                return null;

            Item i = _item.Clone();
            _item.TurnToAir(true);
            return i;
        }

        /// <summary>
        /// Returns the stored item to the client.
        /// </summary>
        public void ReturnItemToPlayer()
        {
            if (!HasItem)
                return;

            Main.LocalPlayer.QuickSpawnItem(new EntitySource_WorldEvent(), _item.Clone(), _item.stack);
            _item.TurnToAir(true);
        }

        /// <summary>
        /// Creates a new item in the item slot. Does nothing if the item slot is not empty.
        /// </summary>
        /// <param name="typeToClone"></param>
        /// <param name="prefix"></param>
        public void CreateItem(Item newItem)
        {
            if (HasItem)
                return;
            _item = newItem;
            _item.newAndShiny = true;
        }

        /// <summary>
        /// Creates a new item in the item slot. Does nothing if the item slot is not empty.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stack"></param>
        /// <param name="prefix"></param>
        public void CreateItem(int type, int stack = 1, int prefix = 0) => CreateItem(new Item(type, stack, prefix));

        /// <summary>
        /// Transforms the stored item into the given item.
        /// </summary>
        /// <param name="newItem"></param>
        public void TransformItem(Item newItem)
        {
            _item = newItem;
        }

        /// <summary>
        /// Transforms the stored item into a new item of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stack"></param>
        /// <param name="prefix"></param>
        public void TransformItem(int type, int stack = 1, int prefix = 0)
        {
            Item newItem = new(type, stack, prefix);
            TransformItem(newItem);
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Allows you to draw thing behind this item slot, or to modify the way this item slot is drawn. Return <see langword="false"/> to prevent the default drawing of this item slot.
        /// <para/> Returns <see langword="true"/> by default.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <returns></returns>
        public virtual bool PreDrawSelf(SpriteBatch spriteBatch) => true;

        /// <summary>
        /// Allows you to draw things in front of this item slot. This method is called even if PreDrawSelf returns false.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void PostDrawSelf(SpriteBatch spriteBatch) { }


        /// <summary>
        /// Allows you to make things happen when the held item is swapped.
        /// <para/> NOTE: Make sure to check <see cref="Item.IsAir"/> before using <paramref name="oldItem"/> or <paramref name="newItem"/>.
        /// </summary>
        /// <param name="oldItem"></param>
        public virtual void OnItemSwap(ref Item oldItem, ref Item newItem) { }

        /// <summary>
        /// Allows you to determine whether the given item can be stored in the item slot.
        /// <para/> Returns <see langword="true"/> by default.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanAcceptItem(Item item) => true;
        #endregion
    }
}
