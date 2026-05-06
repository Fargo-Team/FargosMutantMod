using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Fargowiltas.Content.Achievements;
using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Dusts;
using Fargowiltas.Content.Items;
using Fargowiltas.Content.Items.CaughtNPCs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Vanity;
using Fargowiltas.Content.NPCs;
using Fargowiltas.Content.UI;
using Fargowiltas.Content.UI.StatSheet;
using Fargowiltas.Utilities.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using static Fargowiltas.Content.Items.Misc.BattleCry;
using static Fargowiltas.Content.Items.Tiles.EnchantedTreeTileEntity;
using static Fargowiltas.Fargowiltas;
using static Terraria.ModLoader.ModContent;

////using Fargowiltas.Toggler;

namespace Fargowiltas
{
    public class FargoPlayer : ModPlayer
    {
        //        //public ToggleBackend Toggler = new ToggleBackend();
        //        public Dictionary<string, bool> TogglesToSync = new Dictionary<string, bool>();

        public PotionToggleBackend PotionToggler = new();
        public List<int> DisabledPotionToggles = [];
        public HashSet<int> ActivePotions = [];
        public Dictionary<int, bool> PotionTogglesToSync = [];
        public int ToggleRebuildCooldown = 0;

        public bool HasClickedWrench;

        public bool extractSpeed;
        public bool HasDrawnDebuffLayer;
        internal bool BattleCry;
        internal bool CalmingCry;

        internal int originalSelectedItem;
        internal bool autoRevertSelectedItem;
        public float luckPotionBoost;
        public float ElementalAssemblerNearby;

        public float StatSheetMaxAscentMultiplier;
        public float StatSheetWingSpeed;

        public int DeathFruitHealth;
        public bool bigSuck;
        public bool CoolCrab;

        public bool AutoSummon;
        public int AutoSummonCD;
        public float AutoSummonCap;
        public static MethodInfo AutoSummonShootMethod;

        public int StationSoundCooldown;

        internal Dictionary<string, bool> FirstDyeIngredients = [];

        public bool[] ItemHasBeenOwned; // If you've owned this item type ever
        public HashSet<ItemDefinition> ItemHasBeenOwnedCache = []; // Only used for saving and loading
        public bool[] ItemHasBeenOwnedAtThirtyStack; // If you've owned this 30 of this item type ever
        public HashSet<ItemDefinition> ItemHasBeenOwnedAtThirtyStackCache = []; // Only used for saving and loading

        public int DeathCamTimer = 0;
        public int SpectatePlayer = 0;
#pragma warning disable CS8632
        public Fruit? grabbedFruit = null;
#pragma warning restore CS8632
        public Vector2 LastInteractedChizard = Vector2.Zero;

        public List<BaseSpawnBoosterBuff> ActiveSpawnBoosters = [];

        private readonly string[] tags =
        [
            "RedHusk",
            "OrangeBloodroot",
            "YellowMarigold",
            "LimeKelp",
            "GreenMushroom",
            "TealMushroom",
            "CyanHusk",
            "SkyBlueFlower",
            "BlueBerries",
            "PurpleMucos",
            "VioletHusk",
            "PinkPricklyPear",
            "BlackInk"
        ];
        public override void Initialize()
        {
            ItemHasBeenOwned = ItemID.Sets.Factory.CreateBoolSet(false);
            ItemHasBeenOwnedAtThirtyStack = ItemID.Sets.Factory.CreateBoolSet(false);
        }
        public override void Load()
        {
            AutoSummonShootMethod = typeof(Player).GetMethod("ItemCheck_Shoot", BindingFlags.NonPublic | BindingFlags.Instance);
            base.Load();
        }
        public override void SaveData(TagCompound tag)
        {
            string name = "FargoDyes" + Player.name;
            List<string> dyes = [];

            foreach (string tagString in tags)
            {

                if (FirstDyeIngredients.TryGetValue(tagString, out bool value))
                {
                    dyes.AddWithCondition(tagString, FirstDyeIngredients[tagString]);
                }
                else
                {
                    dyes.AddWithCondition(tagString, false);
                }
            }

            tag.Add(name, dyes);
            tag.Add("DeathFruitHealth", DeathFruitHealth);

            if (BattleCry)
                tag.Add($"FargoBattleCry{Player.name}", true);

            if (CalmingCry)
                tag.Add($"FargoCalmingCry{Player.name}", true);

            if (HasClickedWrench)
                tag.Add("HasClickedWrench", true);

            for (int i = 0; i < ItemHasBeenOwned.Length; i++)
            {
                if (ItemHasBeenOwned[i])
                {
                    ItemHasBeenOwnedCache.Add(new ItemDefinition(i));
                }
            }
            tag.Add("OwnedItemsListDef", ItemHasBeenOwnedCache.ToList());

            for (int i = 0; i < ItemHasBeenOwnedAtThirtyStack.Length; i++)
            {
                if (ItemHasBeenOwnedAtThirtyStack[i])
                {
                    ItemHasBeenOwnedAtThirtyStackCache.Add(new ItemDefinition(i));
                }
            }
            tag.Add("OwnedItemsAtThirtyListDef", ItemHasBeenOwnedAtThirtyStackCache.ToList());

            var togglesOff = new List<ItemDefinition>();
            if (PotionToggler != null && PotionToggler.Toggles != null)
            {
                foreach (KeyValuePair<int, PotionToggle> entry in PotionToggler.Toggles)
                {
                    if (!PotionToggler.Toggles[entry.Key].ToggleBool)
                    {
                        int itemID = entry.Key;
                        togglesOff.Add(new ItemDefinition(itemID));
                    }

                }
            }
            tag.Add($"{Mod.Name}.{Player.name}.PotionTogglesOffDef", togglesOff);
        }

        //        public override void Initialize()
        //        {
        //            //Toggler.Load(this);
        //        }
        public override void LoadData(TagCompound tag)
        {
            string name = "FargoDyes" + Player.name;

            IList<string> dyes = tag.GetList<string>(name);
            foreach (string downedTag in tags)
            {
                FirstDyeIngredients[downedTag] = dyes.Contains(downedTag);
            }

            DeathFruitHealth = tag.GetInt("DeathFruitHealth");
            BattleCry = tag.ContainsKey($"FargoBattleCry{Player.name}");
            CalmingCry = tag.ContainsKey($"FargoCalmingCry{Player.name}");
            HasClickedWrench = tag.ContainsKey("HasClickedWrench");

            ItemHasBeenOwned = ItemID.Sets.Factory.CreateBoolSet(false);
            if (tag.TryGet<IList<ItemDefinition>>("OwnedItemsListDef", out var ownedList))
            {
                ItemHasBeenOwnedCache = [.. ownedList];
                foreach (var entry in ItemHasBeenOwnedCache.Where(i => i.Type != -1))
                {
                    ItemHasBeenOwned[entry.Type] = true;
                }
            }
            ItemHasBeenOwnedAtThirtyStack = ItemID.Sets.Factory.CreateBoolSet(false);
            if (tag.TryGet<IList<ItemDefinition>>("OwnedItemsAtThirtyListDef", out var ownedAtThirtyStackList))
            {
                ItemHasBeenOwnedAtThirtyStackCache = [.. ownedAtThirtyStackList];
                foreach (var entry in ItemHasBeenOwnedAtThirtyStackCache.Where(i => i.Type != -1))
                {
                    ItemHasBeenOwnedAtThirtyStack[entry.Type] = true;
                }
            }

            if (tag.TryGet<IList<ItemDefinition>>($"{Mod.Name}.{Player.name}.PotionTogglesOffDef", out var disabledToggleIDs))
            {
                DisabledPotionToggles = [.. PotionToggleLoader.LoadedToggles.Keys.Where(disabledToggleIDs.Select(t => t.Type).ToHashSet().Contains)];
            }
        }
        public void SyncPotionToggle(int itemID)
        {
            if (!PotionTogglesToSync.ContainsKey(itemID))
                PotionTogglesToSync.Add(itemID, Player.GetPotionToggle(itemID).ToggleBool);
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)PacketID.SyncDeathFruit);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)DeathFruitHealth);
            packet.Send(toWho, fromWho);

            foreach (KeyValuePair<int, bool> toggle in PotionTogglesToSync)
            {
                packet = Mod.GetPacket();

                packet.Write((byte)PacketID.SyncOnePotionToggle); // sync one toggle
                packet.Write((byte)Player.whoAmI);
                packet.Write(toggle.Key);
                packet.Write(toggle.Value);

                packet.Send(toWho, fromWho);
            }

            PotionTogglesToSync.Clear();
        }

        // Called in ExampleMod.Networking.cs
        public void ReceivePlayerSync(BinaryReader reader)
        {
            DeathFruitHealth = reader.ReadByte();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            FargoPlayer clone = (FargoPlayer)targetCopy;
            clone.DeathFruitHealth = DeathFruitHealth;
            clone.PotionToggler = PotionToggler;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            FargoPlayer clone = (FargoPlayer)clientPlayer;

            if (DeathFruitHealth != clone.DeathFruitHealth)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);

            if (clone.PotionToggler.Toggles != PotionToggler.Toggles)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)13);
                packet.Write((byte)Player.whoAmI);
                packet.Write((byte)PotionToggler.Toggles.Count);

                for (int i = 0; i < PotionToggler.Toggles.Count; i++)
                {
                    packet.Write(PotionToggler.Toggles.Values.ElementAt(i).ToggleBool);
                }

                packet.Send();
            }
        }
        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
        {
            foreach (string tag in tags)
            {
                FirstDyeIngredients[tag] = false;
            }
        }

        public override void OnEnterWorld()
        {
            SyncCry(Player);

            PotionToggler.TryLoad();
            PotionToggler.LoadPlayerToggles(Player);
            DisabledPotionToggles.Clear();
        }

        public override void ResetEffects()
        {
            extractSpeed = false;
            HasDrawnDebuffLayer = false;
            bigSuck = false;
            CoolCrab = false;
            AutoSummon = false;
            if (!Player.controlUseItem)
            {
                grabbedFruit = null;
            }
            ActivePotions.Clear();
            ActiveSpawnBoosters.Clear();
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {

            if (Fargowiltas.HomeKey.JustPressed)
            {
                AutoUseMirror();
            }

            if (Fargowiltas.StatKey.JustPressed)
                CombinedUI.ToggleUI<StatSheetUI>();

            if (Fargowiltas.PotionTogglerKey.JustPressed)
                CombinedUI.ToggleUI<PotionToggler>();
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            
            if (Player.chest == -1 && FargoUIManager.IsOpen<ChizardSearchBar>())
            {
                ChizardSearchBar bar = FargoUIManager.Get<ChizardSearchBar>();
                bar.ItemInsert.CreateItem(inventory[slot].Clone());
                inventory[slot].TurnToAir();
                SoundEngine.PlaySound(SoundID.Grab);
            }
            return base.ShiftClickSlot(inventory, context, slot);
        }
        public override void PreUpdate()
        {
            PotionToggler.TryLoad();
        }
        public override void PreUpdateBuffs()
        {
            /*
            foreach (var potToggle in PotionToggleLoader.LoadedToggles.Values)
            {
                if (Player.HasBuff(potToggle.BuffID))
                    ActivePotions.Add(potToggle.BuffID);

                if (!Player.GetPotionToggleValue(potToggle.ItemID))
                {
                    Player.ClearBuff(potToggle.BuffID);
                    Player.buffImmune[potToggle.BuffID] = true;
                }
            }
            */
        }
        public override void PostUpdateBuffs()
        {
            if (FargoServerConfig.Instance.UnlimitedPotionBuffs is UnlimitedBuffSelections.On || (FargoServerConfig.Instance.UnlimitedPotionBuffs is UnlimitedBuffSelections.BossOnly && FargoGlobalNPC.AnyBossAlive()))
            {
                foreach (Item item in Player.bank.item)
                {
                    FargoGlobalItem.TryUnlimBuff(item, Player);
                }

                foreach (Item item in Player.bank2.item)
                {
                    FargoGlobalItem.TryUnlimBuff(item, Player);
                }
            }

            if (FargoServerConfig.Instance.PiggyBankAcc || FargoServerConfig.Instance.ModdedPiggyBankAcc)
            {
                foreach (Item item in Player.bank.item)
                {
                    FargoGlobalItem.TryPiggyBankAcc(item, Player);
                }

                foreach (Item item in Player.bank2.item)
                {
                    FargoGlobalItem.TryPiggyBankAcc(item, Player);
                }
            }

            foreach (var potToggle in PotionToggleLoader.LoadedToggles.Values)
            {
                if (Player.HasBuff(potToggle.BuffID))
                {
                    ActivePotions.Add(potToggle.BuffID);
                }
                else if (Player.buffImmune[potToggle.BuffID])
                {
                    ActivePotions.Remove(potToggle.BuffID);
                }

                /*if (!Player.GetPotionToggleValue(potToggle.ItemID))
                {
                    Player.buffImmune[potToggle.BuffID] = true;
                }*/
            }
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (bigSuck && drawInfo.shadow == 0)
            {
                Vector2 pos = Player.Center + Main.rand.NextVector2CircularEdge(1000, 500) + Main.rand.NextVector2Square(-100, 100);
                Dust d = Dust.NewDustPerfect(pos, DustType<BigSuckDust>(), Alpha: 255, Scale: Main.rand.NextFloat(0.8f, 1.5f));
                d.customData = Player;
                drawInfo.DustCache.Add(d.dustIndex);
            }
        }
        public override void PostUpdateEquips()
        {
            AutoSummoner.TryAutoSummoner(Player);
        }
        public override void UpdateDead()
        {
            StationSoundCooldown = 0;
            AutoSummonCD = 0;
            if (FargoClientConfig.Instance.MultiplayerDeathSpectate && Player.dead && Main.netMode != NetmodeID.SinglePlayer && Main.player.Any(p => p != null && !p.dead && !p.ghost))
            {
                Spectate();
            }
        }
        public void FindNewSpectateTarget() => SpectatePlayer = SpectatePlayer = Main.player.First(ValidSpectateTarget).whoAmI;
        public bool ValidSpectateTarget(Player p) => p != null && !p.dead && !p.ghost;
        public void Spectate()
        {
            if (SpectatePlayer < 0 || SpectatePlayer > Main.maxPlayers)
                FindNewSpectateTarget();
            if (SpectatePlayer < 0 || SpectatePlayer > Main.maxPlayers)
                return;
            Player spectatePlayer = Main.player[SpectatePlayer];
            if (spectatePlayer == null || !spectatePlayer.active || spectatePlayer.dead || spectatePlayer.ghost)
            {
                FindNewSpectateTarget();
                spectatePlayer = Main.player[SpectatePlayer];
            }

            if (spectatePlayer == null || !spectatePlayer.active || spectatePlayer.dead || spectatePlayer.ghost)
                return;

            if (Main.mouseLeft && Main.mouseLeftRelease)
            {
                for (int i = 0; i < Main.maxPlayers + 1; i++)
                {
                    SpectatePlayer--;
                    if (SpectatePlayer < 0)
                        SpectatePlayer = Main.maxPlayers - 1;
                    if (ValidSpectateTarget(Main.player[SpectatePlayer]))
                        break;
                }
            }
            else if (Main.mouseRight && Main.mouseRightRelease)
            {
                for (int i = 0; i < Main.maxPlayers + 1; i++)
                {
                    SpectatePlayer++;
                    if (SpectatePlayer >= Main.maxPlayers)
                        SpectatePlayer = 0;
                    if (ValidSpectateTarget(Main.player[SpectatePlayer]))
                        break;
                }
            }
            spectatePlayer = Main.player[SpectatePlayer];

            Vector2 spectatePos = spectatePlayer.Center;
            if (Player.Center.Distance(spectatePos) > 2000)
            {
                DeathCamTimer++;
                if (DeathCamTimer > 60)
                {
                    Player.Center = spectatePos + spectatePos.DirectionTo(Player.Center) * 1000;
                    DeathCamTimer = 0;
                }

            }
            else
            {
                DeathCamTimer++;
                float lerp = DeathCamTimer / 200f;
                lerp = MathHelper.Clamp(lerp, 0, 1);
                Player.Center = Vector2.Lerp(Player.Center, spectatePos, lerp);
            }
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            FindNewSpectateTarget();
        }
        public override void PostUpdateMiscEffects()
        {
            if (ElementalAssemblerNearby > 0)
            {
                ElementalAssemblerNearby -= 1;
                Player.alchemyTable = true;
            }

            if (StationSoundCooldown > 0)
                StationSoundCooldown--;

            if (ToggleRebuildCooldown > 0)
                ToggleRebuildCooldown--;

            if (Player.equippedWings == null)
                ResetStatSheetWings();

            ForceBiomes();
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            #region Stat Sliders
            FargoServerConfig config = FargoServerConfig.Instance;
            if (config.EnemyDamage != 1 || config.BossDamage != 1)
            {
                bool boss = config.BossDamage > config.EnemyDamage && // only relevant if boss health is higher than enemy health
                    (npc.boss || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || (config.BossApplyToAllWhenAlive && FargoGlobalNPC.AnyBossAlive()));
                if (boss)
                    modifiers.FinalDamage *= config.BossDamage;
                else
                    modifiers.FinalDamage *= config.EnemyDamage;
            }
            #endregion
        }
        public void ResetStatSheetWings()
        {
            StatSheetMaxAscentMultiplier = 0;
            StatSheetWingSpeed = 0;
        }

        private void ForceBiomes()
        {
            if (FargoGlobalNPC.SpecificBossIsAlive(ref FargoGlobalNPC.eaterBoss, NPCID.EaterofWorldsHead)
                && Player.Distance(Main.npc[FargoGlobalNPC.eaterBoss].Center) < 3000)
            {
                Player.ZoneCorrupt = true;
            }

            if (FargoGlobalNPC.SpecificBossIsAlive(ref FargoGlobalNPC.brainBoss, NPCID.BrainofCthulhu)
                && Player.Distance(Main.npc[FargoGlobalNPC.brainBoss].Center) < 3000)
            {
                Player.ZoneCrimson = true;
            }

            if ((FargoGlobalNPC.SpecificBossIsAlive(ref FargoGlobalNPC.plantBoss, NPCID.Plantera)
                && Player.Distance(Main.npc[FargoGlobalNPC.plantBoss].Center) < 3000)
                || (FargoGlobalNPC.SpecificBossIsAlive(ref FargoGlobalNPC.beeBoss, NPCID.QueenBee)
                && Player.Distance(Main.npc[FargoGlobalNPC.beeBoss].Center) < 3000))
            {
                Player.ZoneJungle = true;
            }

            if (FargoServerConfig.Instance.Fountains)
            {
                switch (Main.SceneMetrics.ActiveFountainColor)
                {
                    case -1: //no fountain active
                        goto default;

                    case 0: //pure water, ocean
                        Player.ZoneBeach = true;
                        break;

                    case 2: //corrupt
                        Player.ZoneCorrupt = true;
                        break;

                    case 3: //jungle
                        Player.ZoneJungle = true;
                        break;

                    case 4: //hallow
                        if (Main.hardMode)
                            Player.ZoneHallow = true;
                        break;

                    case 5: //ice
                        Player.ZoneSnow = true;
                        break;

                    case 6: //oasis
                        goto case 12;

                    case 8: //cavern
                        goto default;

                    case 9: //blood fountain
                        goto default;

                    case 10: //crimson
                        Player.ZoneCrimson = true;
                        break;

                    case 12: //desert fountain
                        Player.ZoneDesert = true;
                        if (Player.Center.Y > 3200f)
                            Player.ZoneUndergroundDesert = true;
                        break;

                    default:
                        break;
                }
            }
        }

        public override void PostUpdate()
        {
            if (autoRevertSelectedItem)
            {
                if (Player.itemTime == 0 && Player.itemAnimation == 0)
                {
                    Player.selectedItem = originalSelectedItem;
                    autoRevertSelectedItem = false;
                }
            }

            if (FargoWorld.OverloadedSlimeRain && Main.rand.NextBool(20))
            {
                SlimeRainSpawns();
            }
        }

        public void SlimeRainSpawns()
        {
            int type = NPCID.GreenSlime;

            int[] slimes = [NPCID.SlimeSpiked, NPCID.SandSlime, NPCID.IceSlime, NPCID.SpikedIceSlime, NPCID.MotherSlime, NPCID.SpikedJungleSlime, NPCID.DungeonSlime, NPCID.UmbrellaSlime, NPCID.ToxicSludge, NPCID.CorruptSlime, NPCID.Crimslime, NPCID.IlluminantSlime];

            int rand = Main.rand.Next(50);

            if (rand == 0)
            {
                type = NPCID.Pinky;
            }
            else if (rand < 20)
            {
                type = slimes[Main.rand.Next(slimes.Length)];
            }

            Vector2 pos = new Vector2((int)Player.position.X + Main.rand.Next(-800, 800), (int)Player.position.Y + Main.rand.Next(-800, -250));

            //Projectile.NewProjectile( pos, Vector2.Zero, ModContent.ProjectileType<SpawnProj>(), 0, 0, Main.myPlayer, type);
        }

        public override bool PreModifyLuck(ref float luck)
        {
            if (FargoWorld.Matsuri && !Main.IsItRaining && !Main.IsItStorming)
            {
                LanternNight.GenuineLanterns = true;
                LanternNight.ManualLanterns = false;
            }

            return base.PreModifyLuck(ref luck);
        }

        public override void ModifyLuck(ref float luck)
        {
            luck += luckPotionBoost;

            luckPotionBoost = 0; //look nowhere else works ok
        }
        public override void ModifyScreenPosition()
        {

            if (FargoClientConfig.Instance.MultiplayerDeathSpectate && Main.LocalPlayer.dead && Main.netMode != NetmodeID.SinglePlayer && Main.player.Any(p => p != null && !p.dead && !p.ghost))
            {
                Main.screenPosition = Player.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2);
            }


        }
        public void AutoUseMirror()
        {
            int potionofReturn = -1;
            int recallPotion = -1;
            int magicMirror = -1;

            for (int i = 0; i < Player.inventory.Length; i++)
            {
                switch (Player.inventory[i].type)
                {
                    case ItemID.PotionOfReturn:
                        potionofReturn = i;
                        break;

                    case ItemID.RecallPotion:
                        recallPotion = i;
                        break;

                    case ItemID.MagicMirror:
                    case ItemID.IceMirror:
                    case ItemID.CellPhone:
                    case ItemID.Shellphone:
                        magicMirror = i;
                        break;
                }
            }

            if (potionofReturn != -1)
                QuickUseItemAt(potionofReturn);
            else if (recallPotion != -1)
                QuickUseItemAt(recallPotion);
            else if (magicMirror != -1)
                QuickUseItemAt(magicMirror);
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default with { Base = -(DeathFruitHealth) };
            mana = StatModifier.Default;
        }

        public void QuickUseItemAt(int index, bool use = true)
        {
            if (!autoRevertSelectedItem && Player.selectedItem != index && Player.inventory[index].type != ItemID.None)
            {
                originalSelectedItem = Player.selectedItem;
                autoRevertSelectedItem = true;
                Player.selectedItem = index;
                Player.controlUseItem = true;
                if (use && CombinedHooks.CanUseItem(Player, Player.inventory[Player.selectedItem]))
                {
                    if (Player.whoAmI == Main.myPlayer)
                        Player.ItemCheck();
                    //Player.ItemCheck(Main.myPlayer);
                }
            }
        }

        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (vendor.type == ModContent.NPCType<Squirrel>())
            {
                foreach (var npc in Main.npc.Where(n => n.active && n.townNPC && CaughtNPCItem.CaughtTownies.ContainsKey(n.type)))
                {
                    if (item.type == CaughtNPCItem.CaughtTownies[npc.type])
                    {
                        ModContent.GetInstance<BuyNPCAchievement>().Condition.Complete();
                    }
                }
            }
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            static Item createItem(int type)
            {
                Item i = new Item(type);
                return i;
            }

            bool midnight = Player.name.Equals("midnight", StringComparison.OrdinalIgnoreCase);
            bool midnight2 = Player.name.Equals("midnight.", StringComparison.OrdinalIgnoreCase);
            bool midnight3 = Player.name.Equals("midnight295", StringComparison.OrdinalIgnoreCase);
            bool midnight4 = Player.name.Equals("midnight295.", StringComparison.OrdinalIgnoreCase);

            if (!mediumCoreDeath && (midnight || midnight2 || midnight3 || midnight4))
            {
                yield return createItem(ModContent.ItemType<MutantPants>());
                yield return createItem(ModContent.ItemType<MutantBody>());
                yield return createItem(ModContent.ItemType<MutantMask>());
            }

            if (!mediumCoreDeath && Player.name.Contains("javyz", StringComparison.OrdinalIgnoreCase))
            {
                yield return createItem(ItemType<CrabSizedGlasses>());
            }
        }


        //        /*public override void clientClone(ModPlayer clientClone)
        //        {
        //            FargoPlayer modPlayer = clientClone as FargoPlayer;
        //            modPlayer.Toggler = Toggler;
        //        }*/

        //        /*public void SyncToggle(string key)
        //        {
        //            if (!TogglesToSync.ContainsKey(key))
        //                TogglesToSync.Add(key, player.GetToggle(key).ToggleBool);
        //        }*/

        //        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        //        {
        //            foreach (KeyValuePair<string, bool> toggle in TogglesToSync)
        //            {
        //                ModPacket packet = mod.GetPacket();

        //                packet.Write((byte)80);
        //                packet.Write((byte)player.whoAmI);
        //                packet.Write(toggle.Key);
        //                packet.Write(toggle.Value);

        //                packet.Send(toWho, fromWho);
        //            }

        //            TogglesToSync.Clear();
        //        }

        //        /*public override void SendClientChanges(ModPlayer clientPlayer)
        //        {
        //            FargoPlayer modPlayer = clientPlayer as FargoPlayer;
        //            if (modPlayer.Toggler.Toggles != Toggler.Toggles)
        //            {
        //                ModPacket packet = mod.GetPacket();
        //                packet.Write((byte)79);
        //                packet.Write((byte)player.whoAmI);
        //                packet.Write((byte)Toggler.Toggles.Count);

        //                for (int i = 0; i < Toggler.Toggles.Count; i++)
        //                {
        //                    packet.Write(Toggler.Toggles.Values.ElementAt(i).ToggleBool);
        //                }

        //                packet.Send();
        //            }
        //        }*/

    }
}
