using Fargowiltas.Common.Systems.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Fargowiltas.Content.UI.LumberjackUI
{
    public class LumberjackBiomeRegistry : ModSystem
    {
        private static Dictionary<string, LumberJackBiome> _registry = new Dictionary<string, LumberJackBiome>();

        internal static void Register(LumberJackBiome value)
        {
            string key = value.ID;
            if (!_registry.ContainsKey(key))
            {
                _registry[key] = value;
            }
        }

        public static List<LumberJackBiome> GetBiomes() => [.. _registry.Values];



        public override void PostSetupContent()
        {
            base.PostSetupContent();

            AddBiomes();
        }

        void AddBiomes()
        {
            string LocalPath = "Mods.Fargowiltas.UI.LumberJack.Biomes";
            Asset<Texture2D> vanillaIcons = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow");

            LumberJackBiome.Create("Purity", LocalPath, Item.buyPrice(0, 3), Color.ForestGreen, TeleportPylonType.SurfacePurity)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 0, 0))
                .SetWood(ItemID.Wood, 50)
                .AddCritter(ItemID.LadyBug, 5, () => Main.WindyEnoughForKiteDrops ? 0.5f : 0f)
                .AddCritter([ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly, ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly, ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly], 1, 5, () => 0.33f)
                .AddCritter([ItemID.Grasshopper, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.Bird, ItemID.BlueJay, ItemID.Cardinal], 1, 5, () => 0.2f)
                .AddFruit([ItemID.Lemon, ItemID.Peach, ItemID.Apricot, ItemID.Grapefruit, ItemID.Apple], 1, 5, () => 1f)
                .AddFruit(ItemID.EucaluptusSap, 1, () => 0.01f)
                .Register();

            LumberJackBiome.Create("Desert", LocalPath, Item.buyPrice(0, 5), Color.DarkOrange, TeleportPylonType.Desert)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 3, 0))
                .SetWood(ItemID.Cactus, 100)
                .AddCritter([ItemID.Scorpion, ItemID.BlackScorpion], 5, 1, () => 1f)
                .Register();

            LumberJackBiome.Create("Snow", LocalPath, Item.buyPrice(0, 5), Color.LightGray, TeleportPylonType.Snow)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 5, 0))
                .SetWood(ItemID.BorealWood, 50)
                .AddFruit([ItemID.Cherry, ItemID.Plum], 5, 1, () => 1f)
                .Register();

            LumberJackBiome.Create("Caverns", LocalPath, Item.buyPrice(0, 7), Color.Black, TeleportPylonType.Underground)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 2, 0))
                .AddFruit([ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Amber, ItemID.Diamond], 2, 5, () => 1f)
                .AddCritter(ItemID.Mouse, 5, () => 0.5f)
                .AddCritter([ItemID.GemSquirrelAmethyst, ItemID.GemSquirrelTopaz, ItemID.GemSquirrelSapphire, ItemID.GemSquirrelEmerald, ItemID.GemSquirrelRuby, ItemID.GemSquirrelAmber, ItemID.GemSquirrelDiamond], 1, 5, () => 0.25f)
                .AddCritter([ItemID.GemBunnyAmethyst, ItemID.GemBunnyTopaz, ItemID.GemBunnySapphire, ItemID.GemBunnyEmerald, ItemID.GemBunnyRuby, ItemID.GemBunnyAmber, ItemID.GemBunnyDiamond], 1, 5, () => 0.25f)
                .AddCritter([ItemID.FairyCritterBlue, ItemID.FairyCritterGreen, ItemID.FairyCritterPink], 1, 3, () => 0.25f)
                .Register();

            LumberJackBiome.Create("Underworld", LocalPath, Item.buyPrice(0, 25), Color.OrangeRed, TeleportPylonType.Underground)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 1, 2))
                .SetWood(ItemID.AshWood, 50)
                .AddCritter([ItemID.HellButterfly, ItemID.MagmaSnail, ItemID.Lavafly], 1, 1, () => 1f)
                .AddFruit([ItemID.SpicyPepper, ItemID.Pomegranate], 1, 1, () => 1f)
                .Register();

            LumberJackBiome.Create("Jungle", LocalPath, Item.buyPrice(0, 7), Color.LawnGreen, TeleportPylonType.Jungle)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 6, 1))
                .SetWood(ItemID.RichMahogany, 50)
                .AddCritter([ItemID.Buggy, ItemID.Sluggy, ItemID.Grubby, ItemID.Frog], 5, 1, () => 1f)
                .AddFruit([ItemID.Mango, ItemID.Pineapple], 5, 1, () => 1f)
                .Register();

            LumberJackBiome.Create("Crimson", LocalPath, Item.buyPrice(0, 20), Color.Crimson)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 12, 0))
                .SetWood(ItemID.Shadewood, 50)
                .AddFruit([ItemID.BloodOrange, ItemID.Rambutan], 5, 1, () => 1f)
                .Register();

            LumberJackBiome.Create("Corrupt", LocalPath, Item.buyPrice(0, 20), Color.Purple)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 7, 0))
                .SetWood(ItemID.Ebonwood, 50)
                .AddFruit([ItemID.Elderberry, ItemID.BlackCurrant], 5, 1, () => 1f)
                .Register();

            LumberJackBiome.Create("Beach", LocalPath, Item.buyPrice(0, 15), Color.Yellow, TeleportPylonType.Beach)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 12, 1))
                .SetWood(ItemID.PalmWood, 50)
                .AddCritter(ItemID.Seagull, 3, () => 1f)
                .AddFruit([ItemID.Banana, ItemID.Coconut], 5, 1, () => 1f)
                .Register();

            LumberJackBiome.Create("Hallow", LocalPath, Item.buyPrice(0, 20), Color.DeepPink, TeleportPylonType.Hallow)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 1, 1))
                .SetWood(ItemID.Pearlwood, 50)
                .AddFruit([ItemID.Starfruit, ItemID.Dragonfruit], 5, 1, () => 1f)
                .AddCritter([ItemID.LightningBug, ItemID.FairyCritterBlue, ItemID.FairyCritterGreen, ItemID.FairyCritterPink], 1, 5, () => 1f)
                .AddCritter(ItemID.EmpressButterfly, 1, () => NPC.downedPlantBoss ? 1f : 0f)
                .Register();

            LumberJackBiome.Create("Mushroom", LocalPath, Item.buyPrice(0, 20), Color.Blue, TeleportPylonType.GlowingMushroom)
                .SetIcon(vanillaIcons, vanillaIcons.Frame(16, 5, 8, 1))
                .SetWood(ItemID.GlowingMushroom, 50)
                .AddCritter(ItemID.TruffleWorm, 1, () => 0.3f)
                .AddCritter(ItemID.GlowingSnail, 5, () => 0.7f)
                .Register();
        }

        private static string UnderworldDialogue()
        {
            int angler = NPC.FindFirstNPC(NPCID.Angler);
            if (angler >= 0)
                return Language.GetTextValue("Mods.Fargowiltas.UI.LumberJack.Biomes.Underworld.DescriptionAlt", Main.npc[angler].GivenName);
            return "";
        }
    }

    public class LumberJackBiome
    {
        public struct LumberJackItem
        {
            public List<int> types;
            public int stack;
            public int rollAmount;
            public Func<float> chance;

            internal LumberJackItem(List<int> types, int stack, int rollAmount, Func<float> chance)
            {
                this.types = types;
                this.stack = stack;
                this.rollAmount = rollAmount;
                this.chance = chance;
            }
        }

        public Asset<Texture2D> icon;
        public Rectangle? frame;
        internal bool registered = false;

        public readonly string ID;
        public readonly string localPath;
        public readonly TeleportPylonType? PylonType;
        private int _price;
        public readonly Color BackgroundColor;
        public (int type, int amount) Wood; // tuple
        public List<LumberJackItem> Fruits { get; internal set; } = [];
        public List<LumberJackItem> Critters { get; internal set; } = [];

        private LumberJackBiome(string ID, int BuyPrice, Color bgColor, string localPath, TeleportPylonType? pylonType = null)
        {
            this.ID = ID;
            this._price = BuyPrice;
            this.BackgroundColor = bgColor;
            this.localPath = localPath;
            this.PylonType = pylonType;
            Wood = new(0, 0);
            Fruits = [];
            Critters = [];
        }

        #region Chain Methods
        public static LumberJackBiome Create(string ID, string localPath, int buyPrice, Color bgColor, TeleportPylonType? pylonType = null)
        {
            LumberJackBiome biome = new LumberJackBiome(ID, buyPrice, bgColor, localPath, pylonType);
            return biome;
        }

        public LumberJackBiome SetIcon(Asset<Texture2D> icon, Rectangle? frame = null)
        {
            if (!registered)
            {
                this.icon = icon;
                this.frame = frame;
            }
            return this;
        }

        public LumberJackBiome SetWood(int woodType, int stack)
        {
            if (!registered)
            {
                Wood.type = woodType;
                Wood.amount = stack;

                FargoItemSets.TreeTreasureObtainable[woodType] = true;
            }
            return this;
        }

        public LumberJackBiome AddFruit(List<int> types, int stack, int rollAmount, Func<float> chance)
        {
            if (!registered)
            {
                LumberJackItem fruit = new(types, stack, rollAmount, chance);
                Fruits.Add(fruit);

                foreach(int type in types)
                    FargoItemSets.TreeTreasureObtainable[type] = true;
            }
            return this;
        }

        public LumberJackBiome AddFruit(int type, int stack, Func<float> chance)
        {
            if (!registered)
            {
                LumberJackItem fruit = new([type], stack, 1, chance);
                Fruits.Add(fruit);

                FargoItemSets.TreeTreasureObtainable[type] = true;
            }
            return this;
        }


        public LumberJackBiome AddCritter(List<int> types, int stack, int rollAmount, Func<float> chance)
        {
            if (!registered)
            {
                LumberJackItem critter = new(types, stack, rollAmount, chance);
                Critters.Add(critter);

                foreach (int type in types)
                    FargoItemSets.TreeTreasureObtainable[type] = true;
            }
            return this;
        }

        public LumberJackBiome AddCritter(int type, int stack, Func<float> chance)
        {
            if (!registered)
            {
                LumberJackItem critter = new([type], stack, 1, chance);
                Critters.Add(critter);

                FargoItemSets.TreeTreasureObtainable[type] = true;
            }
            return this;
        }
        #endregion

        public void Register()
        {
            if (registered) return;

            LumberjackBiomeRegistry.Register(this);
            registered = true;
        }

        public void RollTreasures(ref Player player)
        {
            // Wood
            if (Wood.type > 0)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Wood.type), Wood.type, Wood.amount);
            }

            WeightedRandom<LumberJackItem> wr = new WeightedRandom<LumberJackItem>();
            LumberJackItem itemToSpawn;

            // Fruit
            if (Fruits.Count > 0)
            {
                foreach (var fruit in Fruits)
                {
                    if (fruit.chance.Invoke() <= 0)
                        continue;

                    wr.Add(fruit, fruit.chance.Invoke());
                }
                itemToSpawn = wr.Get();
                for (int i = 0; i < itemToSpawn.rollAmount; i++)
                {
                    int type = Main.rand.Next(itemToSpawn.types);
                    player.QuickSpawnItem(player.GetSource_OpenItem(type), type, itemToSpawn.stack);
                }
            }

            // Critters
            wr.Clear();
            if (Critters.Count > 0)
            {
                foreach (var critter in Critters)
                {
                    if (critter.chance.Invoke() <= 0)
                        continue;

                    wr.Add(critter, critter.chance.Invoke());
                }
                itemToSpawn = wr.Get();
                for (int i = 0; i < itemToSpawn.rollAmount; i++)
                {
                    int type = Main.rand.Next(itemToSpawn.types);
                    player.QuickSpawnItem(player.GetSource_OpenItem(type), type, itemToSpawn.stack);
                }
            }
        }

        public int GetBuyPrice()
        {
            Item item = new Item();
            item.value = _price;
            Main.LocalPlayer.GetItemExpectedPrice(item, out long _, out long buyPrice);
            return (int)buyPrice;
        }

        public int GetPylonItemType() => PylonType.HasValue ? TETeleportationPylon.GetPylonItemTypeFromTileStyle((int)PylonType) : -1;

        public string GetLocalizedText(string suffix) => Language.GetTextValue($"{localPath}.{ID}.{suffix}");

        public bool IsAvailable => Main.PylonSystem.HasAnyPylon() && (!PylonType.HasValue || Main.PylonSystem.HasPylonOfType(PylonType.Value));
    }
}