using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.NPCs;
using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static Fargowiltas.FargoSets;
using static Fargowiltas.Fargowiltas;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas
{
    public class FargoWorld : ModSystem
    {
        internal static int AbomClearCD;
        internal static int WoodChopped;

        internal static bool OverloadGoblins;
        internal static bool OverloadPirates;
        internal static bool OverloadPumpkinMoon;
        internal static bool OverloadFrostMoon;
        internal static bool OverloadMartians;
        internal static bool OverloadedSlimeRain;

        internal static bool Matsuri;
        internal static bool GeneratedSacrificeCounts;

        internal static bool[] CurrentSpawnRateTile;
        internal static Dictionary<string, bool> DownedBools = new Dictionary<string, bool>();

        // Do not change the order or name of any of these value names, it will fuck up loading. Any new additions should be added at the end.
        private readonly string[] tags =
        [
            "lumberjack",
            "betsy",
            "boss",
            "rareEnemy",
            "pinky",
            "undeadMiner",
            "tim",
            "doctorBones",
            "mimic",
            "wyvern",
            "runeWizard",
            "nymph",
            "moth",
            "rainbowSlime",
            "paladin",
            "medusa",
            "clown",
            "iceGolem",
            "sandElemental",
            "mothron",
            "mimicHallow",
            "mimicCorrupt",
            "mimicCrimson",
            "mimicJungle",
            "goblinSummoner",
            "flyingDutchman",
            "dungeonSlime",
            "pirateCaptain",
            "skeletonGun",
            "skeletonMage",
            "boneLee",
            "darkMage",
            "ogre",
            "headlessHorseman",
            "babyGuardian",
            "squirrel",
            "worm",
            "nailhead",
            "zombieMerman",
            "eyeFish",
            "bloodEel",
            "goblinShark",
            "dreadnautilus",
            "gnome",
            "redDevil",
            "goldenSlime",
            "goblinScout",
            "pumpking",
            "mourningWood",
            "iceQueen",
            "santank",
            "everscream"
       ];

        public override void PreWorldGen()
        {
            SetWorldBool(FargoServerConfig.Instance.DrunkWorld, ref Main.drunkWorld) ;
            SetWorldBool(FargoServerConfig.Instance.BeeWorld, ref Main.notTheBeesWorld);
            SetWorldBool(FargoServerConfig.Instance.WorthyWorld, ref Main.getGoodWorld);
            SetWorldBool(FargoServerConfig.Instance.CelebrationWorld, ref Main.tenthAnniversaryWorld);
            SetWorldBool(FargoServerConfig.Instance.ConstantWorld, ref Main.dontStarveWorld);
            SetWorldBool(FargoServerConfig.Instance.NoTrapsWorld, ref Main.noTrapsWorld);
            SetWorldBool(FargoServerConfig.Instance.RemixWorld, ref Main.remixWorld);
            SetWorldBool(FargoServerConfig.Instance.ZenithWorld, ref Main.zenithWorld);

            foreach (string tag in tags)
            {
                DownedBools[tag] = false;
            }

            FargoSets.Items.SacrificeCount = FargoSets.Items.SacrificeCountDefault.Clone() as int[];
            GeneratedSacrificeCounts = true;

            WoodChopped = 0;
        }

        private void SetWorldBool(SeasonSelections toggle, ref bool flag)
        {
            switch (toggle)
            {
                case SeasonSelections.AlwaysOn:
                    flag = true;
                    break;
                case SeasonSelections.AlwaysOff:
                    flag = false;
                    break;
                case SeasonSelections.Normal:
                    break;
            }
        }

        private void ResetFlags()
        {
            AbomClearCD = 0;

            OverloadGoblins = false;
            OverloadPirates = false;
            OverloadPumpkinMoon = false;
            OverloadFrostMoon = false;
            OverloadMartians = false;
            OverloadedSlimeRain = false;

            Matsuri = false;

            foreach (string tag in tags)
            {
                DownedBools[tag] = false;
            }

            CurrentSpawnRateTile = new bool[Main.netMode == NetmodeID.Server ? 255 : 1];
        }

        public override void OnWorldLoad()
        {

            ResetFlags();
            if (!GeneratedSacrificeCounts)
            {
                FargoSets.Items.SacrificeCount = FargoSets.Items.SacrificeCountDefault.Clone() as int[];
                GeneratedSacrificeCounts = true;
            }
        }
        public override void ClearWorld()
        {
            EnchantedTreeSheet.EnchantedTrees = [];
        }
        public override void OnWorldUnload()
        {
            FargoGlobalProjectile.CannotDestroyRectangle.Clear();
            ResetFlags();
        }

        public override void SaveWorldData(TagCompound tag)
        {
            List<string> downed = new List<string>();

            foreach (string downedTag in tags)
            {
                if (DownedBools.TryGetValue(downedTag, out bool down) && down)
                    downed.AddWithCondition(downedTag, down);
            }

            tag.Add("downed", downed);
            tag.Add("matsuri", Matsuri);

            tag.Add("FargoIndestructibleRectangles", FargoGlobalProjectile.CannotDestroyRectangle.ToList());

            List<string> sacrificeItems = [];
            for (int i = 0; i < FargoSets.Items.SacrificeCount.Length; i++)
            {
                int count = FargoSets.Items.SacrificeCount[i];
                if (count > 0)
                {
                    if (i >= ItemID.Count) // modded item, variable type, add name instead
                    {
                        if (ItemLoader.GetItem(i) is ModItem modItem && modItem != null)
                        {
                            sacrificeItems.Add(modItem.FullName + "_" + count);
                        }
                    }
                    else // vanilla item
                    {
                        sacrificeItems.Add(i + "_" + count);
                    }

                    
                }
            }
            tag.Add("sacrificeItems", sacrificeItems);
            tag.Add("GeneratedSacrificeCounts", GeneratedSacrificeCounts);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("downed");
            foreach (string downedTag in tags)
            {
                DownedBools[downedTag] = downed.Contains(downedTag);
            }
            Matsuri = tag.Get<bool>("matsuri");

            var savedRectangles = tag.GetList<Rectangle>("FargoIndestructibleRectangles");
            foreach (Rectangle rectangle in savedRectangles)
                FargoGlobalProjectile.CannotDestroyRectangle.Add(rectangle);

            IList<string> sacrificeItems = tag.GetList<string>("sacrificeItems");
            foreach (string sacrificeItem in sacrificeItems)
            {
                string[] nameAndCount = sacrificeItem.Split("_");
                string name = nameAndCount[0];
                if (int.TryParse(nameAndCount[1], out int count))
                {
                    if (int.TryParse(name, out int type) && type < ItemID.Count) // vanilla item
                    {
                        Items.SacrificeCount[type] = count;
                    }
                    else // modded item
                    {
                        if (TryFind(name, out ModItem item))
                            Items.SacrificeCount[item.Type] = count;
                    }
                }
            }
            GeneratedSacrificeCounts = tag.Get<bool>("GeneratedSacrificeCounts");
        }

        public override void NetReceive(BinaryReader reader)
        {
            foreach (string tag in tags)
            {
                DownedBools[tag] = reader.ReadBoolean();
            }

            AbomClearCD = reader.ReadInt32();
            WoodChopped = reader.ReadInt32();
            Matsuri = reader.ReadBoolean();
            Fargowiltas.SwarmActive = reader.ReadBoolean();
            Fargowiltas.HardmodeSwarmActive = reader.ReadBoolean();
            Fargowiltas.SwarmNoHyperActive = reader.ReadBoolean();
        }

        public override void NetSend(BinaryWriter writer)
        {
            foreach (string tag in tags)
            {
                writer.Write(DownedBools.TryGetValue(tag, out bool value) && value);
            }

            writer.Write(AbomClearCD);
            writer.Write(WoodChopped);
            writer.Write(Matsuri);
            writer.Write(Fargowiltas.SwarmActive);
            writer.Write(Fargowiltas.HardmodeSwarmActive);
            writer.Write(Fargowiltas.SwarmNoHyperActive);
        }

        public override void PostUpdateWorld()
        {
            // seasonals
            //SeasonSelections halloween = GetInstance<FargoConfig>().Halloween;
            //SeasonSelections xmas = GetInstance<FargoConfig>().Christmas;


            SetWorldBool(FargoServerConfig.Instance.Halloween, ref Main.halloween);
            SetWorldBool(FargoServerConfig.Instance.Christmas, ref Main.xMas);

            //seeds
            SetWorldBool(FargoServerConfig.Instance.DrunkWorld, ref Main.drunkWorld);
            SetWorldBool(FargoServerConfig.Instance.BeeWorld, ref Main.notTheBeesWorld);
            SetWorldBool(FargoServerConfig.Instance.WorthyWorld, ref Main.getGoodWorld);
            SetWorldBool(FargoServerConfig.Instance.CelebrationWorld, ref Main.tenthAnniversaryWorld);
            SetWorldBool(FargoServerConfig.Instance.ConstantWorld, ref Main.dontStarveWorld);
            SetWorldBool(FargoServerConfig.Instance.NoTrapsWorld, ref Main.noTrapsWorld);
            SetWorldBool(FargoServerConfig.Instance.RemixWorld, ref Main.remixWorld);
            SetWorldBool(FargoServerConfig.Instance.ZenithWorld, ref Main.zenithWorld);

            if (Matsuri)
            {
                LanternNight.NextNightIsLanternNight = true;
            }

            // swarm reset in case something goes wrong
            if (Main.netMode != NetmodeID.MultiplayerClient && Fargowiltas.SwarmActive
                && NoBosses() && !NPC.AnyNPCs(NPCID.EaterofWorldsHead) && !NPC.AnyNPCs(NPCID.DungeonGuardian) && !NPC.AnyNPCs(NPCID.DD2DarkMageT1))
            {
                Fargowiltas.SwarmActive = false;
                Fargowiltas.HardmodeSwarmActive = false;
                Fargowiltas.SwarmNoHyperActive = false;
                FargoGlobalNPC.LastWoFIndex = -1;
                FargoGlobalNPC.WoFDirection = 0;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData);
            }

            if (AbomClearCD > 0)
            {
                AbomClearCD--;
            }

            if (OverloadGoblins && Main.invasionType != InvasionID.GoblinArmy)
            {
                OverloadGoblins = false;
            }

            if (OverloadPirates && Main.invasionType != InvasionID.PirateInvasion)
            {
                OverloadPirates = false;
            }

            if (OverloadPumpkinMoon && !Main.pumpkinMoon)
            {
                OverloadPumpkinMoon = false;
            }

            if (OverloadFrostMoon && !Main.snowMoon)
            {
                OverloadFrostMoon = false;
            }

            if (OverloadMartians && Main.invasionType != InvasionID.MartianMadness)
            {
                OverloadMartians = false;
            }

            if (OverloadedSlimeRain && !Main.slimeRain)
            {
                OverloadedSlimeRain = false;
            }
        }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            ref bool current = ref CurrentSpawnRateTile[0];
            bool oldSpawnRateTile = current;
            current = tileCounts[ModContent.TileType<RegalStatueSheet>()] > 0;

            if (Main.netMode == NetmodeID.MultiplayerClient && current != oldSpawnRateTile)
            {
                ModPacket packet = Fargowiltas.Instance.GetPacket();
                packet.Write((byte)PacketID.RegalStatue);
                packet.Write(current);
                packet.Send();
            }
        }

        public override void PreUpdateWorld()
        {
            bool rate = false;
            for (int i = 0; i < CurrentSpawnRateTile.Length; i++)
            {
                if (CurrentSpawnRateTile[i])
                {
                    Player player = Main.player[i];
                    if (player.active)
                    {
                        if (!player.dead)
                        {
                            rate = true;
                        }
                    }
                    else
                    {
                        CurrentSpawnRateTile[i] = false;
                    }
                }
            }

            if (rate)
            {
                Main.checkForSpawns += 81;
            }
        }

        private bool NoBosses() => Main.npc.All(i => !i.active || !i.boss);

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            //Fargowiltas.UserInterfaceManager.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            base.ModifyInterfaceLayers(layers);
            //Fargowiltas.UserInterfaceManager.ModifyInterfaceLayers(layers);
        }

        public override void AddRecipes()
        {
            Fargowiltas.summonTracker.FinalizeSummonData();
            Fargowiltas.symbolTracker.FinalizeSymbols();
            Fargowiltas.statTracker.FinalizeStats();
        }
    }
}