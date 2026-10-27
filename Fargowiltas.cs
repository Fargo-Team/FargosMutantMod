using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Fargowiltas.Common.Systems.Collections;
using Fargowiltas.Content.Buffs;
using Fargowiltas.Content.Items.CaughtNPCs;
using Fargowiltas.Content.Items.Misc;
using Fargowiltas.Content.Items.Summons.Abom;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.NPCs;
using Fargowiltas.Content.NPCs.DevianttNPC;
using Fargowiltas.Content.Projectiles;
using Fargowiltas.Content.UI;
using Fargowiltas.Content.UI.StatSheet;
using Fargowiltas.Utilities.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Fargowiltas.Content.Items.Tiles.EnchantedTreeTileEntity;

[assembly: InternalsVisibleTo("FargowiltasSouls")]
[assembly: InternalsVisibleTo("FargowiltasMusic")]
[assembly: InternalsVisibleTo("FargoSeeds")]
[assembly: InternalsVisibleTo("FargowiltasCrossmod")]
[assembly: InternalsVisibleTo("FargowiltasSoulsDLC")]
[assembly: InternalsVisibleTo("Satanist")]
[assembly: InternalsVisibleTo("SecretsOfTheSouls")]
namespace Fargowiltas;

public class Fargowiltas : Mod
{
    internal static MutantSummonTracker summonTracker;
    internal static DevianttDialogueTracker dialogueTracker;
    internal static SymbolTracker symbolTracker;
    internal static StatTracker statTracker;

    /// <summary>
    /// All mods that should be recognized as derivative from Fargo's Souls. <br></br>
    /// Used to check whether certain universal features should apply to items from this mod, for example Ruminate tooltips. <br></br>
    /// If your mod derives from Souls and includes Souls features, add it to this list.
    /// </summary>
    public static List<string> SoulsMods = ["FargowiltasSouls", "FargowiltasCrossmod", "FargowiltasSoulsDLC"];

    // Hotkeys
    public static ModKeybind HomeKey;

    public static ModKeybind StatKey;

    public static ModKeybind PotionTogglerKey;

    public static ModKeybind DashKey;

    public static ModKeybind SetBonusKey;


    // Swarms (Energized bosses) 
    public static bool SwarmActive;
    public static EnergizedGlobalNPC.Binding Binding;
    public static bool HardmodeSwarmActive;
    public static int SwarmItemsUsed;
    public static bool SwarmSetDefaults;
    public static int SwarmMinDamage
    {
        get
        {
            float dmg;
            if (HardmodeSwarmActive)
                dmg = 63 + 2 * SwarmItemsUsed;
            else
                dmg = 40 + 1 * SwarmItemsUsed;
            if (Main.masterMode)
                dmg /= 1.2f;
            return (int)dmg;
        }

    }

    // Mod loaded bools
    internal static Dictionary<int, string> ModRareEnemies = [];
    internal static List<Action> ModEventActions = [];
    internal static List<Func<bool>> ModEventActiveFuncs = [];

    public List<Stat> ModStats;
    public List<PermaUpgrade> PermaUpgrades;

    internal static Fargowiltas Instance;

    public override uint ExtraPlayerBuffSlots => (uint)(FargoServerConfig.Instance.ExtraBuffSlots ? 22 : 0);

    public Fargowiltas()
    {
        //            Properties = new ModProperties()
        //            {
        //                Autoload = true,
        //                AutoloadGores = true,
        //                AutoloadSounds = true,
        //            }; 
        //            HookIntoLoad();
    }

    public static Mod SoulsMod;
    public static Mod SoulsExtrasMod;
    public static Mod ThoriumMod;
    public static Mod CalamityMod;
    public static Mod MagicStorageMod;
    public static Mod WikiThisMod;
    public static Mod WoTG;
    public static Mod AlchemistNPCMod;
    public static Mod AlchemistNPCLiteMod;

    public override void Load()
    {
        Instance = this;
        SetupModReferences();

        FargoUIManager.LoadUI();

        string path = "Fargowiltas/Content/NPCs/DevianttNPC/Deviantt_Head_Hatless";
        Instance.AddNPCHeadTexture(ModContent.NPCType<Deviantt>(), path);

        ModStats = [];
        PermaUpgrades =
        [
            new(ContentSamples.ItemsByType[ItemID.AegisCrystal], () => Main.LocalPlayer.usedAegisCrystal),
            new(ContentSamples.ItemsByType[ItemID.AegisFruit], () => Main.LocalPlayer.usedAegisFruit),
            new(ContentSamples.ItemsByType[ItemID.ArcaneCrystal], () => Main.LocalPlayer.usedArcaneCrystal),
            new(ContentSamples.ItemsByType[ItemID.Ambrosia], () => Main.LocalPlayer.usedAmbrosia),
            new(ContentSamples.ItemsByType[ItemID.GummyWorm], () => Main.LocalPlayer.usedGummyWorm),
            new(ContentSamples.ItemsByType[ItemID.GalaxyPearl], () => Main.LocalPlayer.usedGalaxyPearl),
            new(ContentSamples.ItemsByType[ItemID.ArtisanLoaf], () => Main.LocalPlayer.ateArtisanBread),
            new(ContentSamples.ItemsByType[ItemID.MinecartPowerup], () => Main.LocalPlayer.unlockedSuperCart)
        ];

        summonTracker = new MutantSummonTracker();
        dialogueTracker = new DevianttDialogueTracker();
        dialogueTracker.AddVanillaDialogue();
        symbolTracker = new SymbolTracker();
        statTracker = new StatTracker();

        for (int i = 0; i < 8; i++)
            AddContent(new PinnedStatDisplay(i));

        HomeKey = KeybindLoader.RegisterKeybind(this, "Home", "Home");

        StatKey = KeybindLoader.RegisterKeybind(this, "Stat", "L");

        PotionTogglerKey = KeybindLoader.RegisterKeybind(this, "PotionToggler", "K");

        DashKey = KeybindLoader.RegisterKeybind(this, "Dash", "J");

        SetBonusKey = KeybindLoader.RegisterKeybind(this, "SetBonus", "V");

        CaughtNPCItem.RegisterItems();
    }

    public void SetupModReferences()
    {
        ModLoader.TryGetMod("FargowiltasSouls", out SoulsMod);
        ModLoader.TryGetMod("FargowiltasSoulsDLC", out SoulsExtrasMod);
        ModLoader.TryGetMod("ThoriumMod", out ThoriumMod);
        ModLoader.TryGetMod("CalamityMod", out CalamityMod);
        ModLoader.TryGetMod("MagicStorage", out MagicStorageMod);
        ModLoader.TryGetMod("WikiThis", out WikiThisMod);
        ModLoader.TryGetMod("NoxusBoss", out WoTG);
        ModLoader.TryGetMod("AlchemistNPC", out AlchemistNPCMod);
        ModLoader.TryGetMod("AlchemistNPCLite", out AlchemistNPCLiteMod);
    }

    public override void Unload()
    {
        summonTracker = null;
        dialogueTracker = null;
        symbolTracker = null;
        statTracker = null;


        HomeKey = null;
        StatKey = null;
        PotionTogglerKey = null;
        DashKey = null;
        SetBonusKey = null;

        Instance = null;
    }

    public override void PostSetupContent()
    {
        FargoUIManager.InitializeUI();
        statTracker.AddSoulsStats();

        if (ModLoader.TryGetMod("FargowiltasSouls", out Mod souls))
        {
            souls.Call("AddPassiveItem", ModContent.ItemType<PotionCooler>());
        }


        if (ModLoader.TryGetMod("Wikithis", out Mod wikithis) && Main.netMode != NetmodeID.Server)
        {
            wikithis.Call("AddModURL", this, "https://fargosmods.wiki.gg/wiki/{}");

            // You can also use call ID for some calls!
            //wikithis.Call(0, this, "https://examplemod.wiki.gg/wiki/{}");

            // Alternatively, you can use this instead, if your wiki is on terrariamods.fandom.com
            //wikithis.Call(0, this, "https://terrariamods.fandom.com/wiki/Example_Mod/{}");
            //wikithis.Call("AddModURL", this, "https://terrariamods.fandom.com/wiki/Example_Mod/{}");

            // If there wiki on other languages (such as russian, spanish, chinese, etch), then you can also call that:
            //wikithis.Call(0, this, "https://examplemod.wiki.gg/zh/wiki/{}", GameCulture.CultureName.Chinese)

            // If you want to replace default icon for your mod, then call this. Icon should be 30x30, either way it will be cut.
            //wikithis.Call("AddWikiTexture", this, ModContent.Request<Texture2D>(pathToIcon));
            //wikithis.Call(3, this, ModContent.Request<Texture2D>(pathToIcon));
        }

        //            Mod censusMod = ModLoader.GetMod("Census");
        //            if (censusMod != null)
        //            {
        //                censusMod.Call("TownNPCCondition", NPCType("Deviantt"), "Defeat any rare enemy or... embrace eternity");
        //                censusMod.Call("TownNPCCondition", NPCType("Mutant"), "Defeat any boss or miniboss");
        //                censusMod.Call("TownNPCCondition", NPCType("LumberJack"), $"Chop down enough trees");
        //                censusMod.Call("TownNPCCondition", NPCType("Abominationn"), "Clear any event");
        //                Mod fargoSouls = ModLoader.GetMod("FargowiltasSouls");
        //                if (fargoSouls != null)
        //                {
        //                    censusMod.Call("TownNPCCondition", NPCType("Squirrel"), $"Have a Top Hat Squirrel ([i:{fargoSouls.ItemType("TophatSquirrel")}]) in your inventory");
        //                }
        //            }

        //foreach (KeyValuePair<int, int> npc in CaughtNPCItem.CaughtTownies)
        //    Main.RegisterItemAnimation(npc.Key, new DrawAnimationVertical(6, Main.npcFrameCount[npc.Value]));

        //            /*Mod soulsMod = ModLoader.GetMod("FargowiltasSouls");
        //            if (soulsMod != null)
        //            {
        //                if (!ModRareEnemies.ContainsKey(soulsMod.NPCType("BabyGuardian")))
        //                    ModRareEnemies.Add(soulsMod.NPCType("BabyGuardian"), "babyGuardian");
        //            }*/
    }

    public override object Call(params object[] args)
    {
        try
        {
            string code = args[0].ToString();

            switch (code)
            {
                //case "DebuffDisplay":
                //    ModContent.GetInstance<FargoConfig>().DebuffDisplay = (bool)args[1];
                //    break;
                case "AddIndestructibleRectangle":
                    {
                        if (args[1].GetType() == typeof(Rectangle))
                        {
                            Rectangle rectangle = (Rectangle)args[1];
                            FargoGlobalProjectile.CannotDestroyRectangle.Add(rectangle);
                        }
                    }
                    break;
                case "AddIndestructibleTileType":
                    {
                        if (args[1].GetType() == typeof(int))
                        {
                            int tile = (int)args[1];
                            FargoTileSets.InstaCannotDestroy[tile] = true;
                        }
                    }
                    break;
                case "AddIndestructibleWallType":
                    {
                        if (args[1].GetType() == typeof(int))
                        {
                            int wall = (int)args[1];
                            FargoWallSets.InstaCannotDestroy[wall] = true;
                        }
                    }
                    break;
                case "AddEvilAltar":
                    {
                        if (args[1].GetType() == typeof(int))
                        {
                            int tile = (int)args[1];
                            FargoTileSets.EvilAltars[tile] = true;
                        }
                    }
                    break;
                case "AddStatCategory":
                    {
                        if (statTracker.statsInitialized)
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): Categories must be added before AddRecipes");

                        // string, string, string, Func<bool>
                        if (args[1].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[1] must be of type String");
                        if (args[2].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[2] must be of type String");

                        string categoryKey = (string)args[1];


                        string iconPath = args[3].GetType() == typeof(string) ? (string)args[3] : null;
                        Func<bool> condition = args[4].GetType() == typeof(Func<bool>) ? (Func<bool>)args[4] : null;

                        StatCategory.Create((string)args[1], (string)args[2], iconPath, condition).RegisterCategory();
                    }
                    break;
                case "AddStat":
                    {
                        if (statTracker.statsInitialized)
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): Stats must be added before AddRecipes");

                        // string, string, int, Func<object>, Func<string>, float
                        if (args[1].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[1] must be of type String");
                        if (args[2].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[2] must be of type String");
                        if (args[3].GetType() != typeof(Func<object>))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[3] must be of type Func<object>");
                        if (args[4].GetType() != typeof(Func<string>))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[4] must be of type Func<string>");
                        if (args[5].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[5] must be of type String");

                        string categoryName = (string)args[1];
                        if (categoryName == "PermaUpgrade")
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): Invalid category! Consider using AddPermaUpgrade instead");

                        float priority = args[6].GetType() == typeof(float) ? (float)args[6] : -1;

                        StatRegistry.TryAddStatToCategory(categoryName, (string)args[2], (Func<object>)args[3], (Func<string>)args[4], priority, (string)args[5]);
                    }
                    break;
                case "AddPermaUpgrade":
                    {
                        if (args[1].GetType() != typeof(Item))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[1] must be of type Item");
                        if (args[2].GetType() != typeof(Func<bool>))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddStat): args[2] must be of type Func<bool>");

                        Item item = (Item)args[1];
                        Func<bool> ConsumedFunction = (Func<bool>)args[2];
                        PermaUpgrades.Add(new PermaUpgrade(item, ConsumedFunction));
                    }
                    break;
                case "SwarmActive":
                    return SwarmActive;

                case "AddSummon":
                    {
                        if (summonTracker.SummonsFinalized)
                            throw new Exception($"Call Error: Summons must be added before AddRecipes");

                        int itemId;
                        int funcIndex;
                        if (args[2].GetType() == typeof(string))
                        {
                            //Logger.Warn("Fargowiltas: You should provide the summon item ID instead of strings (mod name) and (item name)!");
                            itemId = ModContent.Find<ModItem>(Convert.ToString(args[2]), Convert.ToString(args[3])).Type;
                            funcIndex = 4;
                        }
                        else
                        {
                            itemId = Convert.ToInt32(args[2]);
                            funcIndex = 3;
                        }

                        summonTracker.AddSummon(
                            Convert.ToSingle(args[1]),
                            itemId,
                            args[funcIndex] as Func<bool>,
                            Convert.ToInt32(args[funcIndex + 1])
                        );
                    }
                    break;

                case "AddAbominationnEvent":
                    {
                        if (args[1].GetType() != typeof(Action))
                            throw new Exception("\"Call Error (Fargo Mutant Mod AddAbominationnEvent): args[1] must be of type Action");

                        ModEventActions.Add((Action)args[1]);

                        if (args[2].GetType() != typeof(Func<bool>))
                            throw new Exception("\"Call Error (Fargo Mutant Mod AddAbominationnEvent): args[2] must be of type Func<bool>");

                        ModEventActiveFuncs.Add((Func<bool>)args[2]);
                    }
                    break;

                //                    case "AddEventSummon":
                //                        if (summonTracker.SummonsFinalized)
                //                            throw new Exception($"Call Error: Event summons must be added before AddRecipes");

                //                        summonTracker.AddEventSummon(
                //                            Convert.ToSingle(args[1]),
                //                            args[2] as string,
                //                            args[3] as string,
                //                            args[4] as Func<bool>,
                //                            Convert.ToInt32(args[5])
                //                        );
                //                        break;

                //                    case "GetDownedEnemy":
                //                        if (FargoWorld.DownedBools.ContainsKey(args[1] as string) && FargoWorld.DownedBools[args[1] as string])
                //                            return true;
                //                        return false;
                case "AddDevianttHelpDialogue":
                    if (args[4].GetType() == typeof(string) && args[4].ToString().Length > 0)
                        dialogueTracker.AddDialogue(args[1] as string, (byte)args[2], args[3] as Predicate<string>, args[4] as string);
                    else
                        dialogueTracker.AddDialogue(args[1] as string, (byte)args[2], args[3] as Predicate<string>);

                    break;

                case "LowRenderProj":
                    ((Projectile)args[1]).GetGlobalProjectile<FargoGlobalProjectile>().lowRender = true;
                    break;

                case "DoubleTapDashDisabled":
                    return FargoClientConfig.Instance.DoubleTapDashDisabled;

                case "AddCaughtNPC":
                    {
                        if (args[1].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddCaughtNPC): args[1] must be of type string");
                        if (args[2].GetType() != typeof(int))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddCaughtNPC): args[2] must be of type int");
                        if (args[3].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddCaughtNPC): args[3] must be of type string");
                        string internalName = (string)args[1];
                        int id = (int)args[2];
                        string modName = (string)args[3];
                        CaughtNPCItem item = new(internalName, id);
                        ModLoader.GetMod(modName).AddContent(item);
                        CaughtNPCItem.CaughtTownies.Add(id, item.Type);
                    }
                    break;
                case "AddSymbolPath":
                    {
                        if (symbolTracker.SymbolsFinalized)
                            throw new Exception($"Call Error: Symbols must be added before AddRecipes");

                        if (args[1].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddSymbol): args[1] must be of type string");
                        if (args[2].GetType() != typeof(string))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddSymbol): args[2] must be of type string");
                        string modName = (string)args[1];
                        string filePath = (string)args[2];
                        symbolTracker.AddSymbolPath(modName, filePath);
                    }
                    break;
                case "AddToDebuffDisplayBlacklist":
                    {
                        if (args[1].GetType() != typeof(int))
                            throw new Exception($"Call Error (Fargo Mutant Mod AddToDebuffDisplayBlacklist): args[1] must be of type int");
                        int type = (int)args[1];
                        FargoBuffSets.BuffDisplayBlacklist[type] = true;
                    }
                    break;
            }

        }
        catch (Exception e)
        {
            Logger.Error("Call Error: " + e.StackTrace + e.Message);
        }

        return base.Call(args);
    }

    internal enum PacketID : byte
    {
        RegalStatue = 1,
        AbomClearEvent,
        AnglerReset,
        SyncNPCMaxLife,
        ClientUpdateWorld,
        BroadcastBattleCry,
        SyncBattleCry,
        BroadcastWarCry,
        SyncWarCry,
        SyncDeathFruit,
        DropMeteor,
        SyncTreeFruit,
        SyncTreeEntities,
        SyncPotionToggles,
        SyncOnePotionToggle,
        BetsySummon,
        SyncChestContents,
        RequestTakeItemFromChest,
        ChangeChizardHat,
        AddPotionToBag,
        SyncPortableSundial,
        SyncOwnedItems,
        SyncInactiveNPC,
        SyncWorldTime,
        SyncOwnedItem,
        AddDeviSummon,
        SyncSpawnRates,
        SyncWeatherBalloon,
        SyncAnemometer,
        SyncForbiddenScarab,
        SyncMatsuriLantern,
        SyncPartyInvite,
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        byte data = reader.ReadByte();
        if (Enum.IsDefined(typeof(PacketID), data))
        {
            switch ((PacketID)data)
            {
                case PacketID.RegalStatue:
                    {
                        if (whoAmI >= 0 && whoAmI < FargoWorld.CurrentSpawnRateTile.Length)
                        {
                            FargoWorld.CurrentSpawnRateTile[whoAmI] = reader.ReadBoolean();
                        }
                    }
                    break;

                // Abominationn clear events
                case PacketID.AbomClearEvent:
                    {
                        if (Main.netMode == NetmodeID.Server)
                        {
                            if (IsEventOccurring)
                            {
                                TryClearEvents(out _);
                            }
                        }
                    }
                    break;

                // Angler reset
                case PacketID.AnglerReset:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        Main.AnglerQuestSwap();
                    }
                    break;

                // Sync npc max life
                case PacketID.SyncNPCMaxLife:
                    {
                        int n = reader.ReadInt32();
                        int lifeMax = reader.ReadInt32();
                        if (Main.netMode == NetmodeID.MultiplayerClient && n >= 0 && n < Main.maxNPCs)
                            Main.npc[n].lifeMax = lifeMax;
                    }
                    break;

                //client requested server to update world
                case PacketID.ClientUpdateWorld:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData);
                    }
                    break;

                //client requested server to broadcast battle cry message
                case PacketID.BroadcastBattleCry:
                    {
                        bool isBattle = reader.ReadBoolean();
                        int p = reader.ReadInt32();
                        bool cry = reader.ReadBoolean();
                        BattleCry.GenerateText(isBattle, Main.player[p], cry);
                    }
                    break;

                //client sync battle cry states to others
                case PacketID.SyncBattleCry:
                    {
                        int p = reader.ReadInt32();
                        Main.player[p].FargoMutant().BattleCry = reader.ReadBoolean();
                        Main.player[p].FargoMutant().CalmingCry = reader.ReadBoolean();
                    }
                    break;

                //same as above for war cry
                case PacketID.BroadcastWarCry:
                    {
                        bool isWar = reader.ReadBoolean();
                        int p = reader.ReadInt32();
                        bool cry = reader.ReadBoolean();
                        WarHorn.GenerateText(isWar, Main.player[p], cry);
                    }
                    break;

                case PacketID.SyncWarCry:
                    {
                        int p = reader.ReadInt32();
                        Main.player[p].FargoMutant().WarCry = reader.ReadBoolean();
                        Main.player[p].FargoMutant().PeaceCry = reader.ReadBoolean();
                    }
                    break;

                case PacketID.SyncDeathFruit: // sync death fruit health
                    {
                        int p = (int)reader.ReadByte();
                        int deathFruitHealth = reader.ReadByte();
                        if (p >= 0 && p < Main.maxPlayers && Main.player[p].active)
                        {
                            Main.player[p].GetModPlayer<FargoPlayer>().DeathFruitHealth = deathFruitHealth;
                        }
                    }
                    break;
                case PacketID.DropMeteor: // drop a meteor
                    {
                        if (Main.netMode == NetmodeID.Server)
                            WorldGen.dropMeteor();
                    }
                    break;
                case PacketID.SyncTreeFruit:
                    {
                        int treeindex = reader.ReadInt32();
                        FargoUtils.TryGetTileEntityAs(EnchantedTreeSheet.EnchantedTrees[treeindex].X, EnchantedTreeSheet.EnchantedTrees[treeindex].Y, out EnchantedTreeTileEntity tree);
                        tree.ItemType = reader.ReadInt32();
                        tree.Prefix = reader.ReadInt32();
                        int fruitlength = reader.ReadInt32();

                        tree.Fruits = [];
                        for (int i = 0; i < fruitlength; i++)
                        {
                            Fruit fruit = new(reader.ReadInt32(), reader.ReadVector2(), reader.ReadVector2(), reader.ReadVector2(), reader.ReadInt32(), reader.ReadInt32());
                            fruit.grabCooldown = reader.ReadInt32();
                            fruit.despawnTimer = reader.ReadSingle();
                            tree.Fruits.Add(fruit);
                        }
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, tree.ID, tree.Position.X, tree.Position.Y);
                        }
                    }
                    break;
                case PacketID.SyncTreeEntities:
                    {
                        EnchantedTreeSheet.EnchantedTrees = [];
                        int arrayLength = reader.ReadInt32();
                        for (int m = 0; m < arrayLength; m++)
                        {
                            EnchantedTreeSheet.EnchantedTrees.Add(new Point16(reader.ReadInt32(), reader.ReadInt32()));
                        }
                        if (Main.netMode == NetmodeID.Server)
                        {
                            FargoNet.SendEnchantedTreesListPacket();
                        }
                    }
                    break;
                case PacketID.SyncPotionToggles: // Sync potion toggles
                    {
                        Player player = Main.player[reader.ReadByte()];
                        FargoPlayer modPlayer = player.FargoMutant();
                        byte count = reader.ReadByte();
                        List<int> keys = PotionToggleLoader.LoadedToggles.Keys.ToList();

                        for (int i = 0; i < count; i++)
                        {
                            modPlayer.PotionToggler.Toggles[keys[i]].ToggleBool = reader.ReadBoolean();
                        }
                    }
                    break;
                case PacketID.SyncOnePotionToggle: // Sync one potion toggle
                    {
                        Player player = Main.player[reader.ReadByte()];
                        player.SetPotionToggleValue(reader.ReadInt32(), reader.ReadBoolean());
                    }
                    break;
                case PacketID.BetsySummon:
                    {
                        if (Main.netMode == NetmodeID.Server)
                        {
                            FargowiltasDetours.BetsyEggUsed = true;
                            Item egg = new(ModContent.ItemType<BetsyEgg>());
                            Player player = Main.player[reader.ReadInt32()];
                            Point standPos = new(reader.ReadInt32(), reader.ReadInt32());
                            DD2Event.SummonCrystal(standPos.X, standPos.Y, player.whoAmI);
                            DD2Event.TimeLeftBetweenWaves = 0;
                            NPC.waveNumber = 6;
                            NPC.waveKills = 220;
                            DD2Event.CheckProgress(NPCID.DD2GoblinT3);
                            player.QuickSpawnItem(egg.GetSource_FromThis(), ItemID.DD2EnergyCrystal, (int)(140f * NPC.GetBalance())); // give all missing crystals
                            FargowiltasDetours.BetsyEggUsed = false;
                            NetMessage.SendData(MessageID.WorldData);
                        }
                    }
                    break;
                case PacketID.SyncChestContents:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        int chestX = reader.ReadInt32();
                        int chestY = reader.ReadInt32();
                        FargoNet.SendChizardChestContentsToClient(whoAmI, chestX, chestY);
                    }
                    break;
                case PacketID.RequestTakeItemFromChest:
                    int chestx = reader.ReadInt32();
                    int chesty = reader.ReadInt32();
                    int itemAmount = reader.ReadInt32();
                    int itemindex = reader.ReadInt32();

                    int c = Chest.FindChest(chestx, chesty);
                    if (c >= 0)
                    {
                        if (Main.netMode == NetmodeID.Server)
                        {
                            int itemtype = reader.ReadInt32();
                            int itemstack = reader.ReadInt32();
                            int itemprefix = reader.ReadInt32();
                            Item testItem = Main.chest[c].item[itemindex];
                            ModPacket packet = Instance.GetPacket();
                            packet.Write((byte)PacketID.RequestTakeItemFromChest);
                            packet.Write(chestx);
                            packet.Write(chesty);
                            packet.Write(itemAmount);
                            packet.Write(itemindex);
                            if (testItem.type == itemtype && testItem.stack == itemstack && testItem.prefix == itemprefix && Main.chest[c].frame == 0)
                            {
                                packet.Write(true);
                            }
                            else
                            {
                                packet.Write(false);
                            }
                            packet.Send(whoAmI);
                            NetMessage.SendData(MessageID.SyncChestItem, whoAmI, -1, null, c, itemindex);
                        }
                        else if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            bool success = reader.ReadBoolean();
                            if (success)
                            {
                                //give the item
                                ChizardSearchBar bar = FargoUIManager.Get<ChizardSearchBar>();
                                bar.HandleTakeItem(Main.chest[c], Main.chest[c].item[itemindex], itemAmount);
                            }
                            else
                            {
                                //update the item list if the request failed
                                ChizardSearchBar bar = FargoUIManager.Get<ChizardSearchBar>();
                                bar.SearchBar_OnTextChange(bar.search.Input, bar.search.Input);
                                SoundEngine.PlaySound(SoundID.MenuTick);
                            }
                        }
                    }
                    break;
                case PacketID.ChangeChizardHat:
                    int chizard = reader.ReadInt32();
                    int hat = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ChestWizardTileEntity chizardEntity = (ChestWizardTileEntity)TileEntity.ByID[chizard];
                        chizardEntity.hatID = hat;
                        NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, chizard);
                    }
                    break;
                case PacketID.AddPotionToBag:
                    {
                        int id = reader.ReadInt32();
                        int count = reader.ReadInt32();
                        if (Main.netMode == NetmodeID.Server)
                        {
                            PotionBagSystem.AddPotion(id, count);
                            NetMessage.SendData(MessageID.WorldData);
                        }
                    }
                    break;
                case PacketID.SyncPortableSundial:
                    {
                        byte prevCD = FargoWorld.PortableSundialCooldown;
                        FargoWorld.PortableSundialCooldown = reader.ReadByte();
                        if (prevCD == 0 && FargoWorld.PortableSundialCooldown == 4)
                        {
                            if (Main.dayTime)
                            {
                                Main.fastForwardTimeToDawn = true;
                            }
                            else
                            {
                                Main.fastForwardTimeToDusk = true;
                            }
                        }
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket sendCooldownToClient = GetPacket();
                            sendCooldownToClient.Write((byte)PacketID.SyncPortableSundial);
                            sendCooldownToClient.Write(FargoWorld.PortableSundialCooldown);
                            sendCooldownToClient.Send(ignoreClient: whoAmI);
                        }
                    }
                    break;
                case PacketID.SyncOwnedItems:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        int listCount = reader.ReadInt32();
                        List<int> list = new(listCount);
                        bool capture = false;
                        List<int> listBack = [];
                        for (int i = 0; i < listCount; i++)
                        {
                            int item = reader.ReadInt32();
                            list.Insert(i, item);
                            foreach (Player p in Main.ActivePlayers)
                            {
                                if (!capture && p.whoAmI != whoAmI)
                                {
                                    capture = true;
                                    listBack = p.FargoMutant().ItemHasBeenOwned.GetTrueIndexes();
                                }
                                Main.player[whoAmI].FargoMutant().ItemHasBeenOwned[item] = true;
                            }
                        }

                        if (list.Count != 0)
                        {
                            ModPacket syncOthers = GetPacket();
                            syncOthers.Write((byte)PacketID.SyncOwnedItems);
                            syncOthers.Write((byte)whoAmI);
                            syncOthers.Write(listCount);
                            for (int i = 0; i < listCount; i++)
                            {
                                syncOthers.Write(list[i]);
                            }
                            syncOthers.Send(-1, whoAmI);
                        }


                        if (capture && listBack.Count != 0)
                        {
                            ModPacket syncBack = GetPacket();
                            syncBack.Write((byte)PacketID.SyncOwnedItems);
                            syncBack.Write((byte)Main.myPlayer);
                            syncBack.Write(listBack.Count);
                            for (int i = 0; i < listBack.Count; i++)
                            {
                                syncBack.Write(listBack[i]);
                            }
                            syncBack.Send(whoAmI);
                        }
                    }
                    else
                    {
                        byte index = reader.ReadByte();
                        int listCount = reader.ReadInt32();
                        for (int i = 0; i < listCount; i++)
                        {
                            int item = reader.ReadInt32();
                            Main.LocalPlayer.FargoMutant().ItemHasBeenOwned[item] = true;
                            if (index != Main.maxPlayers)
                            {
                                Main.player[index].FargoMutant().ItemHasBeenOwned[item] = true;
                            }
                        }
                    }
                    break;
                case PacketID.SyncInactiveNPC:
                    {
                        byte index = reader.ReadByte();
                        Main.npc[index].active = false;
                        NetMessage.SendData(MessageID.SyncNPC, -1, whoAmI, null, index);
                    }
                    break;
                case PacketID.SyncWorldTime:
                    {
                        Main.time = 0;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket syncOthersTime = GetPacket();
                            syncOthersTime.Write((byte)PacketID.SyncWorldTime);
                            syncOthersTime.Send(-1, whoAmI);
                        }
                    }
                    break;
                case PacketID.SyncOwnedItem:
                    {
                        int type = reader.ReadInt32();
                        if (Main.netMode == NetmodeID.Server)
                        {
                            foreach (Player p in Main.ActivePlayers)
                            {
                                p.FargoMutant().ItemHasBeenOwned[type] = true;
                            }

                            ModPacket sendOthers = GetPacket();
                            sendOthers.Write((byte)PacketID.SyncOwnedItem);
                            sendOthers.Write(type);
                            sendOthers.Send(-1, whoAmI);
                        }
                        else
                        {
                            foreach (Player p in Main.ActivePlayers)
                            {
                                p.FargoMutant().ItemHasBeenOwned[type] = true;
                            }
                        }
                    }
                    break;

                case PacketID.AddDeviSummon:
                    {
                        int type = reader.ReadInt32();
                        int player = reader.ReadByte();
                        if (Main.netMode == NetmodeID.Server)
                        {
                            Main.player[player].FargoMutant().ActiveSpawnBoosters.Add((BaseSpawnBoosterBuff)BuffLoader.GetBuff(type));
                        }
                    }
                    break;
                // From server to client
                case PacketID.SyncSpawnRates:
                    {
                        Main.LocalPlayer.FargoMutant().spawnRate = reader.ReadInt32();
                        Main.LocalPlayer.FargoMutant().maxSpawns = reader.ReadInt32();
                    }
                    break;
                // From server to client
                case PacketID.SyncWeatherBalloon:
                    {
                        WeatherBalloon.MakeItRain(whoAmI);
                    }
                    break;
                // From server to client
                case PacketID.SyncAnemometer:
                    {
                        Main.windSpeedTarget = Main.windSpeedCurrent = 0.8f; //40mph?
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketID.SyncAnemometer);
                            packet.Send(-1, whoAmI);
                        }
                    }
                    break;
                // From server to client
                case PacketID.SyncForbiddenScarab:
                    {
                        Main.windSpeedTarget = Main.windSpeedCurrent = 0.8f; //40mph?
                        if (Main.netMode == NetmodeID.Server)
                        {
                            if (Sandstorm.Happening)
                                Sandstorm.IntendedSeverity = 0.4f + Main.rand.NextFloat();
                            else if (Main.rand.NextBool(3))
                                Sandstorm.IntendedSeverity = 0f;
                            else
                                Sandstorm.IntendedSeverity = Main.rand.NextFloat() * 0.3f;

                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketID.SyncForbiddenScarab);
                            packet.Write(Sandstorm.IntendedSeverity);
                            packet.Send();
                        }
                        else
                        {
                            Sandstorm.IntendedSeverity = reader.ReadSingle();
                        }
                    }
                    break;
                // From server to client
                case PacketID.SyncMatsuriLantern:
                    {
                        FargoWorld.Matsuri = !FargoWorld.Matsuri;
                    }
                    break;
                // From server to client
                case PacketID.SyncPartyInvite:
                    {
                        PartyInvite.TryForParty();
                        NetMessage.SendData(MessageID.WorldData);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    internal static bool IsEventOccurring =>
        Main.invasionType != 0
        || Main.pumpkinMoon
        || Main.snowMoon
        || Main.eclipse
        || Main.bloodMoon
        || Main.WindyEnoughForKiteDrops
        || Main.IsItRaining
        || Main.IsItStorming
        || Main.slimeRain
        || BirthdayParty.PartyIsUp
        || DD2Event.Ongoing
        || Sandstorm.Happening
        || NPC.LunarApocalypseIsUp
        || ModEventActiveFuncs.Any(f => f.Invoke());

    private static readonly List<Vector2>? dD2Event_deadGoblinSpots = typeof(DD2Event).GetField("_deadGoblinSpots", FargoUtils.UniversalBindingFlags).GetValue(null) as List<Vector2>;
    internal static bool TryClearEvents(out bool didClearSomething)
    {
        bool canClearEvent = FargoWorld.AbomClearCD <= 0;
        didClearSomething = false;
        if (canClearEvent)
        {
            FargoWorld.AbomClearCD = 7200;
            if (Main.invasionType != 0)
            {
                didClearSomething = true;
                Main.invasionType = 0;
                FargoUtils.PrintLocalization("MessageInfo.CancelEvent", new Color(175, 75, 255));
            }

            if (Main.pumpkinMoon)
            {
                didClearSomething = true;
                Main.pumpkinMoon = false;
                FargoUtils.PrintLocalization("MessageInfo.CancelPumpkinMoon", new Color(175, 75, 255));
            }

            if (Main.snowMoon)
            {
                didClearSomething = true;
                Main.snowMoon = false;
                FargoUtils.PrintLocalization("MessageInfo.CancelFrostMoon", new Color(175, 75, 255));
            }

            if (Main.eclipse)
            {
                didClearSomething = true;
                Main.eclipse = false;
                FargoUtils.PrintLocalization("MessageInfo.CancelEclipse", new Color(175, 75, 255));
            }

            if (Main.bloodMoon)
            {
                didClearSomething = true;
                Main.bloodMoon = false;
                FargoUtils.PrintLocalization("MessageInfo.CancelBloodMoon", new Color(175, 75, 255));
            }

            if (Main.WindyEnoughForKiteDrops)
            {
                didClearSomething = true;
                Main.windSpeedTarget = 0;
                Main.windSpeedCurrent = 0;
                FargoUtils.PrintLocalization("MessageInfo.CancelWindyDay", new Color(175, 75, 255));
            }

            if (Main.slimeRain)
            {
                didClearSomething = true;
                Main.StopSlimeRain(false);
                Main.slimeWarningDelay = 1;
                Main.slimeWarningTime = 1;
            }

            if (BirthdayParty.PartyIsUp)
            {
                didClearSomething = true;
                BirthdayParty.CheckNight();
            }

            if (DD2Event.Ongoing)
            {
                didClearSomething = true;
                DD2Event.Ongoing = false;
                dD2Event_deadGoblinSpots?.Clear();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.totalInvasionPoints = 0f;
                    NPC.waveKills = 0f;
                    NPC.waveNumber = 0;
                    DD2Event.WipeEntities();
                }
                FargoUtils.PrintLocalization("MessageInfo.CancelOOA", new Color(175, 75, 255));
            }

            if (Sandstorm.Happening)
            {
                didClearSomething = true;
                Sandstorm.Happening = false;
                Sandstorm.TimeLeft = 0;
                Sandstorm.IntendedSeverity = 0;
                FargoUtils.PrintLocalization("MessageInfo.CancelSandstorm", new Color(175, 75, 255));
            }

            // Keep in mind, only tower strengths are netsynced. It is best this way to avoid unnecessary netpackets
            if (NPC.downedTowers && NPC.LunarApocalypseIsUp)
            {
                didClearSomething = true;
                NPC.LunarApocalypseIsUp =
                    NPC.TowerActiveSolar =
                    NPC.TowerActiveVortex =
                    NPC.TowerActiveNebula =
                    NPC.TowerActiveStardust = false;

                NPC.ShieldStrengthTowerSolar =
                    NPC.ShieldStrengthTowerVortex =
                    NPC.ShieldStrengthTowerNebula =
                    NPC.ShieldStrengthTowerStardust = 0;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.UpdateTowerShieldStrengths);

                // Purge all towers
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.type is NPCID.LunarTowerNebula or NPCID.LunarTowerSolar or NPCID.LunarTowerStardust or NPCID.LunarTowerVortex)
                    {
                        // This makes them automatically deactivate and netsync in AI
                        n.ai[0] = 3f;
                        n.ai[1] = 59f;
                    }
                }
                FargoUtils.PrintLocalization("MessageInfo.CancelLunarEvent", new Color(175, 75, 255));
            }
            if (Main.IsItRaining || Main.IsItStorming)
            {
                didClearSomething = true;
                Main.cloudAlpha = 0;
                Main.StopRain();
                if (Main.maxRaining != Main.oldMaxRaining)
                {
                    Main.oldMaxRaining = Main.maxRaining;
                }
                FargoUtils.PrintLocalization("MessageInfo.CancelRain", new Color(175, 75, 255));
            }

            foreach (Action action in ModEventActions)
            {
                action.Invoke();
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        return canClearEvent;
    }

    // SpawnBoss(player, mod.NPCType("MyBoss"), true, 0, 0, "DerpyBoi 2", false);
    internal static void SpawnBoss(Player player, int bossType, bool spawnMessage = true, int overrideDirection = 0, int overrideDirectionY = 0, string overrideDisplayName = "", bool namePlural = false)
    {
        if (overrideDirection == 0)
        {
            overrideDirection = Main.rand.NextBool(2) ? -1 : 1;
        }

        if (overrideDirectionY == 0)
        {
            overrideDirectionY = -1;
        }

        Vector2 npcCenter = player.Center + new Vector2(MathHelper.Lerp(500f, 800f, (float)Main.rand.NextDouble()) * overrideDirection, 800f * overrideDirectionY);
        SpawnBoss(player, bossType, spawnMessage, npcCenter, overrideDisplayName, namePlural);
    }

    // SpawnBoss(player, mod.NPCType("MyBoss"), true, player.Center + new Vector2(0, 800f), "DerpFromBelow", false);
    internal static int SpawnBoss(Player player, int bossType, bool spawnMessage = true, Vector2 npcCenter = default, string overrideDisplayName = "", bool namePlural = false)
    {
        if (npcCenter == default)
        {
            npcCenter = player.Center;
        }

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)npcCenter.X, (int)npcCenter.Y, bossType);
            Main.npc[npcID].Center = npcCenter;
            Main.npc[npcID].netUpdate2 = true;

            if (spawnMessage)
            {
                string npcName = !string.IsNullOrEmpty(Main.npc[npcID].GivenName) ? Main.npc[npcID].GivenName : overrideDisplayName;
                //if ((npcName == null || string.IsNullOrEmpty(npcName)) && Main.npc[npcID].modNPC != null)
                //{
                //    npcName = Main.npc[npcID].modNPC.DisplayName.GetDefault();
                //}

                if (namePlural)
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.HaveAwoken", npcName), 175, 75);
                    }
                    else
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.HaveAwoken", npcName), new Color(175, 75, 255));
                        }
                }
                else
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Main.NewText(Language.GetTextValue("Announcement.HasAwoken", npcName), 175, 75);
                    }
                    else
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", npcName), new Color(175, 75, 255));
                        }
                }
            }
        }

        return 200;
    }
}

