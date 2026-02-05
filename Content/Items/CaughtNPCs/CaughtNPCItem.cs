using Fargowiltas.Common.Configs;
using Fargowiltas.Content.NPCs;
using Fargowiltas.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.CaughtNPCs
{
    public class CaughtNPCItem : ModItem
    {
        internal static Dictionary<int, int> CaughtTownies = new();

        public override string Name => _name;

        public string _name;
        public int AssociatedNpcId;

        public CaughtNPCItem()
        {
            _name = base.Name;
            AssociatedNpcId = NPCID.None;
        }

        public CaughtNPCItem(string internalName, int associatedNpcId)
        {
            _name = internalName;
            AssociatedNpcId = associatedNpcId;
        }

        public override bool IsLoadingEnabled(Mod mod) => AssociatedNpcId != NPCID.None;

        protected override bool CloneNewInstances => true;

        public override ModItem Clone(Item item)
        {
            CaughtNPCItem clone = base.Clone(item) as CaughtNPCItem;
            clone._name = _name;
            clone.AssociatedNpcId = AssociatedNpcId;
            return clone;
        }

        public override bool IsCloneable => true;

        public override void Unload()
        {
            CaughtTownies.Clear();
        }

        public override string Texture => AssociatedNpcId < NPCID.Count ? $"Terraria/Images/NPC_{AssociatedNpcId}" : NPCLoader.GetNPC(AssociatedNpcId).Texture;

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, Main.npcFrameCount[AssociatedNpcId]));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.DefaultToCapturedCritter(AssociatedNpcId);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item44;

            /*if (AssociatedNpcId == NPCID.Angler)
            {
                Item.bait = 15;
            }*/
        }

        public override void PostUpdate()
        {
            if (AssociatedNpcId != NPCID.Guide || !Item.lavaWet || NPC.AnyNPCs(NPCID.WallofFlesh))
            {
                return;
            }

            NPC.SpawnWOF(Item.position);
            Item.TurnToAir();
        }

        public override bool CanUseItem(Player player)
        {
            //replaced tile in range check because items like toolbox exist and npcs just dont spawn if placed too far
            //what the fuck is terraria code anyway
            return player.Distance(Main.MouseWorld) < 4 * 16
                && !Collision.SolidCollision(Main.MouseWorld - player.DefaultSize / 2, (int)player.DefaultSize.X, (int)player.DefaultSize.Y)
                && NPC.CountNPCS(AssociatedNpcId) < 5;
        }

        public override bool? UseItem(Player player)
        {
            return null;
        }

        public static void RegisterItems()
        {
            CaughtTownies = new Dictionary<int, int>();

            // manually register mutant and vanillas
            Add("Abominationn", ModContent.NPCType<Abominationn>());
            Add("Angler", NPCID.Angler);
            Add("ArmsDealer", NPCID.ArmsDealer);
            Add("Clothier", NPCID.Clothier);
            Add("Cyborg", NPCID.Cyborg);
            Add("Demolitionist", NPCID.Demolitionist);
            Add("Deviantt", ModContent.NPCType<Deviantt>());
            Add("Dryad", NPCID.Dryad);
            Add("DyeTrader", NPCID.DyeTrader);
            Add("GoblinTinkerer", NPCID.GoblinTinkerer);
            Add("Golfer", NPCID.Golfer);
            Add("Guide", NPCID.Guide);
            Add("LumberJack", ModContent.NPCType<LumberJack>());
            Add("Mechanic", NPCID.Mechanic);
            Add("Merchant", NPCID.Merchant);
            Add("Mutant", ModContent.NPCType<Mutant>());
            Add("Nurse", NPCID.Nurse);
            Add("Painter", NPCID.Painter);
            Add("PartyGirl", NPCID.PartyGirl);
            Add("Pirate", NPCID.Pirate);
            Add("SantaClaus", NPCID.SantaClaus);
            Add("SkeletonMerchant", NPCID.SkeletonMerchant);
            //if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls))
            Add("Squirrel", ModContent.NPCType<Squirrel>());
            Add("Steampunker", NPCID.Steampunker);
            Add("Stylist", NPCID.Stylist);
            Add("Tavernkeep", NPCID.DD2Bartender);
            Add("TaxCollector", NPCID.TaxCollector);
            Add("TravellingMerchant", NPCID.TravellingMerchant);
            Add("Truffle", NPCID.Truffle);
            Add("WitchDoctor", NPCID.WitchDoctor);
            Add("Wizard", NPCID.Wizard);

            Add("Zoologist", NPCID.BestiaryGirl);
            Add("Princess", NPCID.Princess);

            //town pets
            Add("TownDog", NPCID.TownDog);
            Add("TownCat", NPCID.TownCat);
            Add("TownBunny", NPCID.TownBunny);
            //town slimes
            Add("NerdySlime", NPCID.TownSlimeBlue);
            Add("CoolSlime", NPCID.TownSlimeGreen);
            Add("ElderSlime", NPCID.TownSlimeOld);
            Add("ClumsySlime", NPCID.TownSlimePurple);
            Add("DivaSlime", NPCID.TownSlimeRainbow);
            Add("SurlySlime", NPCID.TownSlimeRed);
            Add("MysticSlime", NPCID.TownSlimeYellow);
            Add("SquireSlime", NPCID.TownSlimeCopper);
        }

        public static void Add(string internalName, int id)
        {
            CaughtNPCItem item = new(internalName, id);
            Fargowiltas.Instance.AddContent(item);
            CaughtTownies.Add(id, item.Type);
        }
    }

    public class CaughtGlobalNPC : GlobalNPC
    {
        private static HashSet<int> npcCatchableWasFalse;

        public override void Load()
        {
            npcCatchableWasFalse = new HashSet<int>();
        }

        public override void Unload()
        {
            if (npcCatchableWasFalse != null)
            {
                foreach (var type in npcCatchableWasFalse)
                {
                    //Failing to unload this properly causes it to bleed into un-fargowiltas gameplay, causing various issues such as clients not being able to join a server
                    Main.npcCatchable[type] = false;
                }
                npcCatchableWasFalse = null;
            }
        }

        public override void SetDefaults(NPC npc)
        {
            int type = npc.type;
            if (CaughtNPCItem.CaughtTownies.ContainsKey(type) && FargoServerConfig.Instance.CatchNPCs)
            {
                npc.catchItem = (short)CaughtNPCItem.CaughtTownies.FirstOrDefault(x => x.Key.Equals(type)).Value;
                if (!Main.npcCatchable[type])
                {
                    npcCatchableWasFalse.Add(type);
                    Main.npcCatchable[type] = true;
                }
            }
        }
    }
}
