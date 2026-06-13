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
using static Fargowiltas.Common.Systems.Collections.FargoItemSets;
using static Fargowiltas.Fargowiltas;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Common
{
    public class FargoWorld : ModSystem
    {
        internal static int AbomClearCD;
        internal static int WoodChopped;
        internal static byte PortableSundialCooldown;

        internal static bool OverloadGoblins;
        internal static bool OverloadPirates;
        internal static bool OverloadPumpkinMoon;
        internal static bool OverloadFrostMoon;
        internal static bool OverloadMartians;
        internal static bool OverloadedSlimeRain;

        internal static bool Matsuri;
        internal static bool GeneratedSacrificeCounts;
        internal static bool BlockPortaDialCooldown;

        internal static bool EternityMode;

        internal static bool[] CurrentSpawnRateTile;
        internal static Dictionary<string, bool> DownedBools = [];

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
            "goblinScout",
            "pumpking",
            "mourningWood",
            "iceQueen",
            "santank",
            "everscream"
       ];

        public override void PreWorldGen()
        {
            SetWorldBool(FargoServerConfig.Instance.DrunkWorld, ref Main.drunkWorld);
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

            SacrificeCount = SacrificeCountDefault.Clone() as int[];
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

            EternityMode = (bool?)Fargowiltas.SoulsMod?.Call("EternityMode") == true;

            CurrentSpawnRateTile = new bool[Main.netMode == NetmodeID.Server ? 255 : 1];
        }

        public override void OnWorldLoad()
        {

            ResetFlags();
            if (!GeneratedSacrificeCounts)
            {
                SacrificeCount = SacrificeCountDefault.Clone() as int[];
                GeneratedSacrificeCounts = true;
            }
        }
        public override void ClearWorld()
        {
            foreach (string tag in tags)
            {
                DownedBools[tag] = false;
            }
            Matsuri = false;
            EternityMode = false;
            FargoGlobalProjectile.CannotDestroyRectangle.Clear();
            EnchantedTreeSheet.EnchantedTrees = [];
        }
        public override void OnWorldUnload()
        {
            ResetFlags();
        }

        public override void SaveWorldData(TagCompound tag)
        {
            List<string> downed = [];

            foreach (string downedTag in tags)
            {
                if (DownedBools.TryGetValue(downedTag, out bool down) && down)
                    downed.AddWithCondition(downedTag, down);
            }

            tag.Add("downed", downed);
            tag.Add("matsuri", Matsuri);

            tag.Add("FargoIndestructibleRectangles", FargoGlobalProjectile.CannotDestroyRectangle.ToList());

            List<string> sacrificeItems = [];
            for (int i = 0; i < SacrificeCount.Length; i++)
            {
                int count = SacrificeCount[i];
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
            tag.Add("PortableSundialCooldown", PortableSundialCooldown);
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
                        SacrificeCount[type] = count;
                    }
                    else // modded item
                    {
                        if (TryFind(name, out ModItem item))
                            SacrificeCount[item.Type] = count;
                    }
                }
            }
            GeneratedSacrificeCounts = tag.Get<bool>("GeneratedSacrificeCounts");
            PortableSundialCooldown = tag.Get<byte>("PortableSundialCooldown");
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
            SwarmActive = reader.ReadBoolean();
            HardmodeSwarmActive = reader.ReadBoolean();
            Binding = (EnergizedGlobalNPC.Binding)reader.ReadInt32();
            EternityMode = reader.ReadBoolean();
            // These can't be bytes because a sign is required and
            // signed bytes range between -127 and 127, which is not enough for the NPC array
            FargoGlobalNPC.eaterBoss = reader.ReadInt16();
            FargoGlobalNPC.beeBoss = reader.ReadInt16();
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
            writer.Write(SwarmActive);
            writer.Write(HardmodeSwarmActive);
            writer.Write((int)Binding);
            writer.Write(EternityMode);
            // These can't be bytes because signed bytes are required and
            // they range between -127 and 127, which is not enough for the NPC array
            writer.Write((short)FargoGlobalNPC.eaterBoss);
            writer.Write((short)FargoGlobalNPC.beeBoss);
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
            if (Main.netMode != NetmodeID.MultiplayerClient && SwarmActive
                && !FargoUtils.AnyBossAlive() && !Main.IsNPCActiveAndOneOfTypes(FargoGlobalNPC.dungeonGuardian, NPCID.DungeonGuardian) && !Main.IsNPCActiveAndOneOfTypes(FargoGlobalNPC.darkMage, NPCID.DD2DarkMageT1))
            {
                SwarmActive = false;
                HardmodeSwarmActive = false;
                Binding = EnergizedGlobalNPC.Binding.None;
                FargoGlobalNPC.LastWoFIndex = -1;
                FargoGlobalNPC.WoFDirection = 0;
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
            current = tileCounts[TileType<RegalStatueSheet>()] > 0;

            if (Main.netMode == NetmodeID.MultiplayerClient && current != oldSpawnRateTile)
            {
                ModPacket packet = Instance.GetPacket();
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

        public override void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate)
        {
            if (Main.gameMenu)
                return;
            int sleeping = Main.CurrentFrameFlags.SleepingPlayersCount;
            if (sleeping > 0 && sleeping == Main.CurrentFrameFlags.ActivePlayersCount)
            {
                double speed = FargoServerConfig.Instance.FasterBedSpeed / 5;
                timeRate *= speed;
                tileUpdateRate *= speed;
                eventUpdateRate *= speed;
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
            summonTracker.FinalizeSummonData();
            symbolTracker.FinalizeSymbols();
            statTracker.FinalizeStats();
        }

        public override void PreUpdateNPCs()
        {
            if (!Main.IsNPCActiveAndOneOfTypes(FargoGlobalNPC.eaterBoss, NPCID.EaterofWorldsHead))
            {
                FargoGlobalNPC.eaterBoss = -1;
            }
            if (!Main.IsNPCActiveAndOneOfTypes(FargoGlobalNPC.beeBoss, NPCID.QueenBee))
            {
                FargoGlobalNPC.beeBoss = -1;
            }
            if (!Main.IsNPCActiveAndOneOfTypes(FargoGlobalNPC.dungeonGuardian, NPCID.DungeonGuardian))
            {
                FargoGlobalNPC.dungeonGuardian = -1;
            }
            if (!Main.IsNPCActiveAndOneOfTypes(FargoGlobalNPC.darkMage, NPCID.DD2DarkMageT1))
            {
                FargoGlobalNPC.darkMage = -1;
            }
            if (!FargoUtils.AnyBossAlive())
            {
                FargoGlobalNPC.Boss = -1;
            }
        }
        public override void PostUpdateEverything()
        {
            EternityMode = (bool?)Fargowiltas.SoulsMod?.Call("EternityMode") == true;
        }
    }
}