using Fargowiltas.Common;
using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems.Collections;
using Fargowiltas.Content.Items.Summons.Abom;
using Fargowiltas.Content.Items.Tiles;
using Fargowiltas.Content.NPCs.SquirrelNPC;
using Fargowiltas.Content.UI.Emotes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.Content.Items
{
    public class FargoGlobalItem : GlobalItem
    {
        private static readonly int[] Hearts = [ItemID.Heart, ItemID.CandyApple, ItemID.CandyCane];
        private static readonly int[] Stars = [ItemID.Star, ItemID.SoulCake, ItemID.SugarPlum];
        public static readonly int[] AlwaysUsableVanillaSummons = [ItemID.SlimeCrown, ItemID.SuspiciousLookingEye, ItemID.WormFood, ItemID.BloodySpine,
        ItemID.Abeemination, ItemID.DeerThing, ItemID.QueenSlimeCrystal, ItemID.MechanicalWorm, ItemID.MechanicalEye, ItemID.MechanicalSkull,
        ItemID.MechdusaSummon, ItemID.CelestialSigil, ItemID.BloodMoonStarter, ItemID.GoblinBattleStandard, ItemID.SnowGlobe, ItemID.PirateMap,
        ItemID.SolarTablet, ItemID.PumpkinMoonMedallion, ItemID.NaughtyPresent];
        public static readonly int[] NightSettingSummons = [ItemID.SuspiciousLookingEye, ItemID.MechanicalEye, ItemID.MechanicalSkull, ItemID.MechanicalWorm,
            ItemID.BloodMoonStarter, ItemID.PumpkinMoonMedallion, ItemID.NaughtyPresent];

        private bool firstTick = true;

        public List<int> RecipeGroupAnimationItems = null;

        //float and glow when true
        public bool FromEnchantedTree = false;
        //follow cursor when = myplayer
        public int Grabbed = -1;


        public override bool InstancePerEntity => true;

        static string ExpandedTooltipLoc(string line) => Language.GetTextValue($"Mods.Fargowiltas.ExpandedTooltips.{line}");

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }

        //public override bool CloneNewInstances => true;

        TooltipLine FountainTooltip(string biome) => new TooltipLine(Mod, "Tooltip0", $"[s:Fargowiltas/FountainEffect] [c/AAAAAA:{ExpandedTooltipLoc($"Fountain{biome}")}]");

        //For the shop sale tooltip system.
        public class ShopTooltip
        {
            public List<int> NpcIDs = new();
            public List<string> NpcNames = new();
            public string Condition;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            var fargoServerConfig = FargoServerConfig.Instance;

            if (FargoClientConfig.Instance.ExpandedTooltips)
            {
                TooltipLine line;
                //Shop sale tooltips. Very engineered. Adds tooltips to ALL npc shop sales. Aims to handle any edge case as well as possible.
                if (FargoItemSets.RegisteredShopTooltips[item.type] == null)
                {
                    List<ShopTooltip> registeredShopTooltips = [];
                    foreach (var shop in NPCShopDatabase.AllShops)
                    {
                        foreach (var entry in shop.ActiveEntries.Where(e => !e.Item.IsAir && e.Item.type == item.type))
                        {
                            /*
                            Item npcItem = null;
                            foreach (var tryNPCItem in ContentSamples.ItemsByType.Where(i => i.Value.ModItem != null && i.Value.ModItem is CaughtNPCItem modItem && modItem.AssociatedNpcId == shop.NpcType))
                            {
                                npcItem = tryNPCItem.Value;
                                break;
                            }

                            npcItem ??= item;
                            */

                            string conditions = "";
                            int i = 0;
                            foreach (var condition in entry.Conditions)
                            {
                                string grammar = i > 0 ? ", " : "";
                                conditions += grammar + condition.Description.Value;
                                i++;
                            }
                            string conditionLine = i > 0 ? ": " + conditions : "";
                            string npcName = ContentSamples.NpcsByNetId[shop.NpcType].FullName;
                            int npcID = TownNPCProfiles.GetHeadIndexSafe(ContentSamples.NpcsByNetId[shop.NpcType]);

                            if (registeredShopTooltips.Any(t => t.NpcNames.Any(n => n == npcName) && t.Condition == conditionLine)) //sometimes it makes duplicates otherwise
                                continue;

                            bool registered = false;

                            foreach (ShopTooltip regTooltip in registeredShopTooltips)
                            {
                                if (regTooltip.Condition == conditionLine && !regTooltip.NpcNames.Contains(npcName))
                                {
                                    regTooltip.NpcNames.Add(npcName);
                                    regTooltip.NpcIDs.Add(npcID);
                                    registered = true;
                                    break;
                                }
                            }
                            if (!registered)
                            {
                                ShopTooltip tooltip = new();
                                tooltip.NpcIDs.Add(npcID);
                                tooltip.NpcNames.Add(npcName);
                                tooltip.Condition = conditionLine;
                                registeredShopTooltips.Add(tooltip);
                            }

                            break; //only one line per npc
                        }
                    }
                    FargoItemSets.RegisteredShopTooltips[item.type] = registeredShopTooltips;
                }

                foreach (ShopTooltip tooltip in FargoItemSets.RegisteredShopTooltips[item.type])
                {

                    List<int> displayIDs = tooltip.NpcIDs?.ToList();
                    int id = 0;
                    if (displayIDs.Count != 0)
                    {
                        int timer = (int)(Main.GlobalTimeWrappedHourly * 60);
                        int index = timer / 60;
                        index %= displayIDs.Count;
                        id = displayIDs[index];
                    }

                    string names = "";
                    int i = 0;
                    foreach (string npcName in tooltip.NpcNames)
                    {
                        string grammar = i > 0 ? ", " : "";
                        names += grammar + npcName;
                        i++;
                    }
                    if (i > 5)
                        names = ExpandedTooltipLoc("SeveralVendors");
                    string text = $"[h:{id}] [c/AAAAAA:{ExpandedTooltipLoc("SoldBy")} {names}{tooltip.Condition}]";

                    if (id == -1 && names.Contains("Skeleton Merchant"))
                        text = $"[s:Fargowiltas/SkeletonMerchantHead] [c/AAAAAA:{ExpandedTooltipLoc("SoldBy")} {names}{tooltip.Condition}]";
                    else if (id == -1)
                        text = $"[s:Fargowiltas/UnknownNPC] [c/AAAAAA:{ExpandedTooltipLoc("SoldBy")} {names}{tooltip.Condition}]";

                    line = new TooltipLine(Mod, "TooltipNPCSold", text);
                    tooltips.Add(line);
                }

                switch (item.type)
                {
                    case ItemID.PureWaterFountain:
                        if (fargoServerConfig.Fountains)
                            tooltips.Add(FountainTooltip("Ocean"));
                        break;

                    case ItemID.OasisFountain:
                    case ItemID.DesertWaterFountain:
                        if (fargoServerConfig.Fountains)
                            tooltips.Add(FountainTooltip("Desert"));
                        break;

                    case ItemID.JungleWaterFountain:
                        if (fargoServerConfig.Fountains)
                            tooltips.Add(FountainTooltip("Jungle"));
                        break;

                    case ItemID.IcyWaterFountain:
                        if (fargoServerConfig.Fountains)
                            tooltips.Add(FountainTooltip("Snow"));
                        break;

                    case ItemID.CorruptWaterFountain:
                        if (fargoServerConfig.Fountains)
                            tooltips.Add(FountainTooltip("Corruption"));
                        break;

                    case ItemID.CrimsonWaterFountain:
                        if (fargoServerConfig.Fountains)
                            tooltips.Add(FountainTooltip("Crimson"));
                        break;

                    case ItemID.HallowedWaterFountain:
                        if (fargoServerConfig.Fountains)
                            tooltips.Add(FountainTooltip("Hallow"));
                        break;

                    //cavern fountain?

                    case ItemID.BugNet:
                    case ItemID.GoldenBugNet:
                    case ItemID.FireproofBugNet:
                        if (fargoServerConfig.CatchNPCs)
                            tooltips.Add(new TooltipLine(Mod, "Tooltip0", $"[s:Fargowiltas/CatchNPCs] [c/AAAAAA:{ExpandedTooltipLoc("CatchNPCs")}]"));
                        break;

                }

                if (fargoServerConfig.ExtraLures)
                {
                    /*if (item.type == ItemID.FishingPotion)
                    {
                        line = new TooltipLine(Mod, "Tooltip1", $"[i:2373] [c/AAAAAA:{ExpandedTooltipLoc("ExtraLure1")}]");
                        tooltips.Insert(3, line);
                    }

                    if (item.type == ItemID.FiberglassFishingPole || item.type == ItemID.FisherofSouls || item.type == ItemID.Fleshcatcher || item.type == ItemID.ScarabFishingRod || item.type == ItemID.BloodFishingRod)
                    {
                        line = new TooltipLine(Mod, "Tooltip1", $"[i:2373] [c/AAAAAA:{ExpandedTooltipLoc("Lures2")}]");
                        tooltips.Insert(3, line);
                    }*/

                    if (item.type == ItemID.MechanicsRod || item.type == ItemID.SittingDucksFishingRod || item.type == ItemID.HotlineFishingHook)
                    {
                        line = new TooltipLine(Mod, "Tooltip1", $"[s:Fargowiltas/ExtraLures] [c/AAAAAA:{ExpandedTooltipLoc("Lures2")}]");
                        tooltips.Add(line);
                    }

                    if (item.type == ItemID.GoldenFishingRod)
                    {
                        line = new TooltipLine(Mod, "Tooltip1", $"[s:Fargowiltas/ExtraLures] [c/AAAAAA:{ExpandedTooltipLoc("Lures3")}]");
                        tooltips.Add(line);
                    }
                }

                if (fargoServerConfig.TorchGodEX && item.type == ItemID.TorchGodsFavor)
                {
                    line = new TooltipLine(Mod, "TooltipTorchGod1", $"[s:Fargowiltas/AbidesTrueTorchLuck] [c/AAAAAA:{ExpandedTooltipLoc("AutoTorch")}]");
                    tooltips.Add(line);
                    line = new TooltipLine(Mod, "TooltipTorchGod2", $"[s:Fargowiltas/AbidesTrueTorchLuck] [c/AAAAAA:{ExpandedTooltipLoc("TrueTorchLuck")}]");
                    tooltips.Add(line);
                }

                if (FargoServerConfig.Instance.PotionCooler && item.maxStack > 1)
                {
                    if (item.buffType != 0 && item.buffTime >= 60 * 60 * 2)
                    {
                        line = new TooltipLine(Mod, "TooltipUnlim", $"[s:Fargowiltas/InfinitePotions] [c/AAAAAA:{Language.GetTextValue($"Mods.Fargowiltas.ExpandedTooltips.UnlimitedBuff30", FargoServerConfig.Instance.UnlimitedPotionBuffsAmount)}]");
                        tooltips.Add(line);
                    }
                    /*else if (item.bait > 0)
                    {
                        line = new TooltipLine(Mod, "TooltipUnlim", $"[i:5139] [c/AAAAAA:{ExpandedTooltipLoc("UnlimitedUse30")}]");
                        tooltips.Add(line);
                    }*/
                }

                if (fargoServerConfig.PermanentStationsNearby && FargoItemSets.BuffStation[item.type] != -1)
                {
                    string text = "";
                    string buff = Lang.GetBuffName(FargoItemSets.BuffStation[item.type]);
                    if (Main.LocalPlayer.FargoMutant().ItemHasBeenOwned[item.type])
                    {
                        string loc = Language.GetTextValue($"Mods.Fargowiltas.ExpandedTooltips.PermanentEffectNearby", buff);
                        text = $"[s:Fargowiltas/PermanentStationsNearby] [c/AAAAAA:{loc}]";
                    }
                    else
                    {
                        string loc = Language.GetTextValue($"Mods.Fargowiltas.ExpandedTooltips.PermanentEffectNearbyPickup", buff);
                        text = $"[s:Fargowiltas/PermanentStationsNearby] [c/AAAAAA:{loc}]";
                    }
                    line = new TooltipLine(Mod, "TooltipUnlim", text);
                    tooltips.Add(line);
                }

                if (fargoServerConfig.PiggyBankAcc && (FargoItemSets.InfoAccessory[item.type] || FargoItemSets.MechanicalAccessory[item.type]))
                {
                    line = new TooltipLine(Mod, "TooltipUnlim", $"[s:Fargowiltas/WorksInPiggy] [c/AAAAAA:{ExpandedTooltipLoc("WorksFromBanks")}]");
                    tooltips.Add(line);
                }

                if (EnchantedTreeTileEntity.IsItemDupable(item.type))
                {
                    line = new TooltipLine(Mod, "TooltipEnchantedTree",
                        $"[s:Fargowiltas/DuplicatableAtTree] [c/AAAAAA:{ExpandedTooltipLoc("EnchantedTreeDupable")}]");
                    tooltips.Add(line);
                }

                int sacCount = FargoItemSets.SacrificeCount[item.type];
                if (Squirrel.EventSacrifice(item, out int consumeCount, false))
                {
                    if (consumeCount > 1)
                    {
                        line = new TooltipLine(Mod, "TooltipSacrificable",
                            $"[h:{TownNPCProfiles.GetHeadIndexSafe(ContentSamples.NpcsByNetId[ModContent.NPCType<Squirrel>()])}] [c/AAAAAA:{Language.GetTextValue($"Mods.Fargowiltas.ExpandedTooltips.SacrificeEventPlural", consumeCount)}]");
                        tooltips.Add(line);

                    }
                    if (item.type == ItemID.LucyTheAxe)
                    {
                        line = new TooltipLine(Mod, "TooltipSacrificable",
                        $"[h:{TownNPCProfiles.GetHeadIndexSafe(ContentSamples.NpcsByNetId[ModContent.NPCType<Squirrel>()])}] [c/AAAAAA:{ExpandedTooltipLoc("Sacrificable")}]");
                        tooltips.Add(line);
                    }
                    else
                    {
                        line = new TooltipLine(Mod, "TooltipSacrificable",
                            $"[h:{TownNPCProfiles.GetHeadIndexSafe(ContentSamples.NpcsByNetId[ModContent.NPCType<Squirrel>()])}] [c/AAAAAA:{ExpandedTooltipLoc("SacrificeEvent")}]");
                        tooltips.Add(line);
                    }

                }
                else if (sacCount > 0)
                {
                    if (sacCount > 1)
                    {
                        line = new TooltipLine(Mod, "TooltipSacrificable",
                        $"[h:{TownNPCProfiles.GetHeadIndexSafe(ContentSamples.NpcsByNetId[ModContent.NPCType<Squirrel>()])}] [c/AAAAAA:{Language.GetTextValue($"Mods.Fargowiltas.ExpandedTooltips.SacrificablePlural", sacCount)}]");
                        tooltips.Add(line);
                    }
                    else
                    {
                        line = new TooltipLine(Mod, "TooltipSacrificable",
                        $"[h:{TownNPCProfiles.GetHeadIndexSafe(ContentSamples.NpcsByNetId[ModContent.NPCType<Squirrel>()])}] [c/AAAAAA:{ExpandedTooltipLoc("Sacrificable")}]");
                        tooltips.Add(line);
                    }
                }

                if (FargoItemSets.TreeTreasureObtainable[item.type])
                {
                    line = new TooltipLine(Mod, "LumberJackTreeTreasure", $"[s:Fargowiltas/TreeTreasure] [c/AAAAAA:{ExpandedTooltipLoc("TreeTreasure")}]");
                    tooltips.Add(line);
                }

                int shimmerItem = ItemID.Sets.ShimmerTransformToItem[item.type];
                int shimmerFromItem = -1;
                if (FargoItemSets.ShimmerTransformsFromItem[item.type] != null)
                {
                    int shimmerTimer = (int)(Main.GlobalTimeWrappedHourly * 60);
                    int shimmerIndex = shimmerTimer / 60;
                    shimmerIndex %= FargoItemSets.ShimmerTransformsFromItem[item.type].Count;
                    shimmerFromItem = FargoItemSets.ShimmerTransformsFromItem[item.type][shimmerIndex];
                }

                string shimmerText = "";
                if (shimmerItem > 0 && shimmerFromItem <= 0)
                    shimmerText = $"[s:Fargowiltas/Shimmer] [c/FFC0CB:{ExpandedTooltipLoc("Shimmerable")}] [i:{shimmerItem}] [c/FFC0CB:{ContentSamples.ItemsByType[shimmerItem].Name}]";
                else if (shimmerItem <= 0 && shimmerFromItem > 0)
                    shimmerText = $"[s:Fargowiltas/Shimmer] [c/FFC0CB:{ExpandedTooltipLoc("ShimmerableFrom")}] [i:{shimmerFromItem}] [c/FFC0CB:{ContentSamples.ItemsByType[shimmerFromItem].Name}]";
                else if (shimmerItem > 0 && shimmerFromItem > 0)
                {
                    if (shimmerItem == shimmerFromItem)
                        shimmerText = $"[s:Fargowiltas/Shimmer] [c/FFC0CB:{ExpandedTooltipLoc("ShimmerableBoth")}] [i:{shimmerItem}] [c/FFC0CB:{ContentSamples.ItemsByType[shimmerItem].Name}]";
                    else
                        shimmerText = $"[s:Fargowiltas/Shimmer] [c/FFC0CB:{ExpandedTooltipLoc("Shimmerable")}] [i:{shimmerItem}] [c/FFC0CB:{ContentSamples.ItemsByType[shimmerItem].Name}], [c/FFC0CB:{ExpandedTooltipLoc("ShimmerableFrom")}] [i:{shimmerFromItem}] [c/FFC0CB:{ContentSamples.ItemsByType[shimmerFromItem].Name}]";
                }

                if (shimmerText.Length > 0)
                {
                    line = new TooltipLine(Mod, "TooltipShimmerable", shimmerText);
                    tooltips.Add(line);
                }

                int bedSpeed = FargoServerConfig.Instance.FasterBedSpeed / 5;
                if (bedSpeed != 1f && item.createTile != -1 && TileID.Sets.CanBeSleptIn[item.createTile])
                {
                    TooltipLine bed = new(Mod, "TooltipFasterBedSpeedConfig",
                        $"[s:Fargowiltas/BedSpeed] [c/AAAAAA:{Language.GetText("Mods.Fargowiltas.ExpandedTooltips.FasterBedSpeed").WithFormatArgs(bedSpeed)}]");
                    tooltips.Add(bed);
                }
            }

            if (FargoClientConfig.Instance.ExactTooltips)
            {
                foreach (var tooltip in tooltips)
                {
                    if (tooltip.Name == "Speed")
                    {
                        int i = tooltip.Text.IndexOf("\n");
                        string text = $" ({item.useAnimation})";
                        if (i >= 0 && i < tooltip.Text.Length)
                            tooltip.Text = tooltip.Text.Insert(i, text);
                        else
                            tooltip.Text += text;
                    }
                    if (tooltip.Name == "Knockback")
                    {
                        float kb = Main.LocalPlayer.GetWeaponKnockback(item, item.knockBack);
                        if (kb > 0 && kb < 1000) // to make it not show when dragonlens does whatever the fuck causes it to skyrocket to infinity
                        {
                            int i = tooltip.Text.IndexOf("\n");
                            string text = $" ({(int)Math.Round(kb * 100) / 100f})";
                            if (i >= 0 && i < tooltip.Text.Length)
                                tooltip.Text = tooltip.Text.Insert(i, text);
                            else
                                tooltip.Text += text;
                        }
                    }
                }
            }
            if (FargoServerConfig.Instance.EasySummons && item.type == ItemID.Abeemination)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    var tooltip = tooltips[i];
                    if (tooltip.Name == "Tooltip0")
                    {
                        tooltips.Insert(i + 1, new TooltipLine(Fargowiltas.Instance, "EnrageWarning", Language.GetTextValue("Mods.Fargowiltas.Items.EnrageWarning.QueenBee")));
                        break;
                    }
                }
            }

            if (FargoClientConfig.Instance.DisableAllScopeView is ScopedBinocularViews.AllDisabled or ScopedBinocularViews.SniperRifleScopeDisabled && item.type == ItemID.SniperRifle)
            {
                TooltipLine line = new(Mod, "TooltipSniperRifleScopeView", $"[s:Fargowiltas/BinocularDisabled] [c/AAAAAA:{ExpandedTooltipLoc("ScopeViewToggle")}]");
                tooltips.Add(line);
            }
            if (FargoClientConfig.Instance.DisableAllScopeView is ScopedBinocularViews.AllDisabled or ScopedBinocularViews.RifleScopeAccessoryDisabled && item.type is ItemID.RifleScope or ItemID.SniperScope or ItemID.ReconScope)
            {
                TooltipLine line = new(Mod, "TooltipRifleScopeView", $"[s:Fargowiltas/BinocularDisabled] [c/AAAAAA:{ExpandedTooltipLoc("ScopeViewToggle")}]");
                tooltips.Add(line);
            }
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.MusicBox || item.Name.Contains(Language.GetTextValue($"ItemName.MusicBox")))
            {
                item.value = Item.sellPrice(0, 0, 22, 50);
            }
        }

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            switch (item.type)
            {
                case ItemID.KingSlimeBossBag:
                    itemLoot.Add(ItemDropRule.Common(ItemID.SlimeStaff, 25));
                    break;

                case ItemID.WoodenCrate:

                    var leadingRule = new LeadingConditionRule(new Conditions.NotRemixSeed());
                    var dropRuleNormal = ItemDropRule.OneFromOptions(40, ItemID.Spear, ItemID.Blowpipe, ItemID.WoodenBoomerang, ItemID.WandofSparking);
                    var dropRuleRemix = ItemDropRule.OneFromOptions(40, ItemID.Spear, ItemID.Blowpipe, ItemID.WoodenBoomerang);
                    leadingRule.OnSuccess(dropRuleNormal);
                    leadingRule.OnFailedConditions(dropRuleRemix);
                    itemLoot.Add(leadingRule);
                    break;

                case ItemID.GoldenCrate:
                    itemLoot.Add(ItemDropRule.OneFromOptions(10, ItemID.BandofRegeneration, ItemID.MagicMirror, ItemID.CloudinaBottle, ItemID.EnchantedBoomerang, ItemID.ShoeSpikes, ItemID.FlareGun, ItemID.HermesBoots));
                    itemLoot.Add(ItemDropRule.Common(ItemID.Sundial, 20));

                    break;
            }

        }
        public override void Update(WorldItem item, ref float gravity, ref float maxFallSpeed)
        {
            if (FromEnchantedTree)
            {
                maxFallSpeed = 0;
                if (Main.myPlayer == Grabbed && !Main.dedServ)
                {
                    item.Center = Vector2.Lerp(item.Center, Main.MouseWorld, 0.07f);
                    if (!Main.LocalPlayer.controlUseItem)
                    {
                        Grabbed = -1;
                        NetMessage.SendData(MessageID.SyncItem, Main.myPlayer, number: item.whoAmI, number2: 1f);

                    }
                }
            }
            base.Update(item, ref gravity, ref maxFallSpeed);
        }
        public override void PostUpdate(WorldItem item)
        {
            if (FargoServerConfig.Instance.Halloween == SeasonSelections.AlwaysOn && FargoServerConfig.Instance.Christmas == SeasonSelections.AlwaysOn && firstTick)
            {
                if (Array.IndexOf(Hearts, item.type) >= 0)
                {
                    item.type = Hearts[Main.rand.Next(Hearts.Length)];
                }

                if (Array.IndexOf(Stars, item.type) >= 0)
                {
                    item.type = Stars[Main.rand.Next(Stars.Length)];
                }

                firstTick = false;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.SiltBlock || item.type == ItemID.SlushBlock || item.type == ItemID.DesertFossil)
            {
                if (FargoServerConfig.Instance.ExtractSpeed && player.GetModPlayer<FargoPlayer>().extractSpeed)
                {
                    item.useTime = 2;
                    item.useAnimation = 3;
                }
                else
                {
                    item.useTime = 10;
                    item.useAnimation = 15;
                }
            }
            return base.CanUseItem(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (ModContent.GetInstance<FargoServerConfig>().EasySummons)
            {
                if (NightSettingSummons.Contains(item.type))
                {
                    Main.time = 0;
                    Main.dayTime = false;
                }
                if (item.type == ItemID.SolarTablet)
                {
                    Main.time = 0;
                    Main.dayTime = true;
                }
            }
            return base.UseItem(item, player);
        }
        public static void TryPiggyBankAcc(Item item, Player player)
        {
            if (item.IsAir || item.maxStack > 1)
                return;
            if (FargoServerConfig.Instance.PiggyBankAcc)
            {
                player.RefreshInfoAccsFromItemType(item);
                player.RefreshMechanicalAccsFromItemType(item.type);
            }
            if (FargoServerConfig.Instance.ModdedPiggyBankAcc)
                item.ModItem?.UpdateInventory(player);
        }
        public override void UpdateInventory(Item item, Player player)
        {
            CheckForIsOldUnlimitedAmmo(item);
            if (Main.netMode != NetmodeID.Server)
            {
                player.FargoMutant().ItemHasBeenOwned[item.type] = true;
                if (player.whoAmI != Main.myPlayer)
                {
                    Main.LocalPlayer.FargoMutant().ItemHasBeenOwned[item.type] = true;
                }
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.MusicBox && Main.curMusic > 0 && Main.curMusic <= 41)
            {
                var itemId = Main.curMusic switch
                {
                    1 => 0 + 562,
                    2 => 1 + 562,
                    3 => 2 + 562,
                    4 => 4 + 562,
                    5 => 5 + 562,
                    6 => 3 + 562,
                    7 => 6 + 562,
                    8 => 7 + 562,
                    9 => 9 + 562,
                    10 => 8 + 562,
                    11 => 11 + 562,
                    12 => 10 + 562,
                    13 => 12 + 562,
                    28 => 1963,
                    29 => 1610,
                    30 => 1963,
                    31 => 1964,
                    32 => 1965,
                    33 => 2742,
                    34 => 3370,
                    35 => 3236,
                    36 => 3237,
                    37 => 3235,
                    38 => 3044,
                    39 => 3371,
                    40 => 3796,
                    41 => 3869,
                    _ => 1596 + Main.curMusic - 14,
                };
                for (int i = 0; i < player.armor.Length; i++)
                {
                    Item accessory = player.armor[i];

                    if (accessory.accessory && accessory.type == item.type)
                    {
                        player.armor[i].SetDefaults(itemId);
                        break;
                    }
                }
            }
        }
        public bool UnlimitedAmmo(Item ammo)
        {
            return FargoServerConfig.Instance.UnlimitedAmmo && Main.hardMode && ammo.ammo != 0 && (ammo.stack >= 3996);
        }

        public override bool CanBeConsumedAsAmmo(Item ammo, Item weapon, Player player)
        {
            if (UnlimitedAmmo(ammo))
                return false;

            return true;
        }

        public override bool? CanConsumeBait(Player player, Item bait)
        {
            //if (FargoServerConfig.Instance.UnlimitedAmmo && bait.stack >= 30)
            //return false;

            return base.CanConsumeBait(player, bait);
        }

        public override bool ConsumeItem(Item item, Player player)
        {
            if (FargoServerConfig.Instance.UnlimitedConsumableWeapons && Main.hardMode && item.damage > 0 && item.ammo == 0 && item.stack >= 3996)
                return false;
            return base.ConsumeItem(item, player);
        }

        public override bool OnPickup(WorldItem item, Player player)
        {
            string dye = "";

            switch (item.type)
            {
                case ItemID.RedHusk:
                    dye = "RedHusk";
                    break;
                case ItemID.OrangeBloodroot:
                    dye = "OrangeBloodroot";
                    break;
                case ItemID.YellowMarigold:
                    dye = "YellowMarigold";
                    break;
                case ItemID.LimeKelp:
                    dye = "LimeKelp";
                    break;
                case ItemID.GreenMushroom:
                    dye = "GreenMushroom";
                    break;
                case ItemID.TealMushroom:
                    dye = "TealMushroom";
                    break;
                case ItemID.CyanHusk:
                    dye = "CyanHusk";
                    break;
                case ItemID.SkyBlueFlower:
                    dye = "SkyBlueFlower";
                    break;
                case ItemID.BlueBerries:
                    dye = "BlueBerries";
                    break;
                case ItemID.PurpleMucos:
                    dye = "PurpleMucos";
                    break;
                case ItemID.VioletHusk:
                    dye = "VioletHusk";
                    break;
                case ItemID.PinkPricklyPear:
                    dye = "PinkPricklyPear";
                    break;
                case ItemID.BlackInk:
                    dye = "BlackInk";
                    break;
            }

            if (dye != "")
            {
                player.GetModPlayer<FargoPlayer>().FirstDyeIngredients[dye] = true;
            }

            return base.OnPickup(item, player);
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.wingSlot != 0 && incomingItem.wingSlot != 0)
                player.GetModPlayer<FargoPlayer>().ResetStatSheetWings();

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            player.GetModPlayer<FargoPlayer>().StatSheetMaxAscentMultiplier = maxAscentMultiplier;
        }

        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            player.GetModPlayer<FargoPlayer>().StatSheetWingSpeed = speed;
        }

        public override void GrabRange(WorldItem item, Player player, ref int grabRange)
        {
            if (player.FargoMutant().bigSuck && !ItemID.Sets.IsAPickup[item.type])
                grabRange += Main.MaxWorldViewSize.X * 3; //360 blocks
        }

        public override bool GrabStyle(WorldItem item, Player player)
        {
            if (player.FargoMutant().bigSuck && !ItemID.Sets.IsAPickup[item.type])
            {
                item.position += (player.MountedCenter - item.Center) / 15f;
                item.position += player.position - player.oldPosition;
            }
            return base.GrabStyle(item, player);
        }
        public override void HoldItem(Item item, Player player)
        {
            if (item.type == ItemID.Binoculars) //the amount of nesting here exists to prevent excessive lag
            {
                if (NPC.AnyNPCs(NPCID.TownCat))
                {
                    for (int j = 0; j < Main.maxNPCs; j++)
                    {
                        if (Main.npc[j].active && Main.npc[j].type == NPCID.TownCat)
                        {
                            NPC cat = Main.npc[j];
                            for (int i = 0; i < Main.maxItems; i++)
                            {
                                if (Main.item[i].active && Main.item[i].type == ItemID.CellPhone)
                                {
                                    if (cat.Distance(Main.item[i].Center) < cat.Size.Length() && Main.MouseWorld.Distance(cat.Center) < cat.Size.Length())
                                    {
                                        Item.NewItem(player.GetSource_ItemUse(item), cat.Center, ItemType<WiresPainting>());
                                        //Main.item[i].active = false;
                                        Main.item[i] = null;
                                                   
                                        cat.active = false;
                                        EmoteBubble.MakeLocalPlayerEmote(ModContent.EmoteBubbleType<WiresEmote>());
                                        return;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            base.HoldItem(item, player);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Grabbed);
            writer.Write(FromEnchantedTree);
            base.NetSend(item, writer);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Grabbed = reader.ReadInt32();
            FromEnchantedTree = reader.ReadBoolean();
            base.NetReceive(item, reader);
        }
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (RecipeGroupAnimationItems != null)
            {
                // the config disabled state is used here to instantly revert the item to the default type if it's not at the default type
                int index = RecipeGroupAnimationItems.IndexOf(item.type);
                int timer = (int)(Main.GlobalTimeWrappedHourly * 60);
                if ((index != 0 && !FargoClientConfig.Instance.AnimatedRecipeGroups) || FargoClientConfig.Instance.AnimatedRecipeGroups && timer % 60 == 0)
                {
                    index++;
                    if (!FargoClientConfig.Instance.AnimatedRecipeGroups || index >= RecipeGroupAnimationItems.Count)
                        index = 0;
                    string name = item.Name;
                    int stack = item.stack;
                    item.ChangeItemType(RecipeGroupAnimationItems[index]);
                    item.GetGlobalItem<FargoGlobalItem>().RecipeGroupAnimationItems = RecipeGroupAnimationItems;
                    item.SetNameOverride(name);
                    item.stack = stack;
                }
            }
            if (UnlimitedAmmo(item) && !item.IsACoin)
            {
                //ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, "∞", position + new Vector2(8f, -24f) * scale, drawColor, 0f, Vector2.Zero, new Vector2(scale), -1f, scale);

                Texture2D texture = ModContent.Request<Texture2D>("Fargowiltas/Assets/Symbols/Infinity").Value;
                Main.EntitySpriteDraw(texture, position + new Vector2(14f, -16f) * scale, null, Color.White, 0, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
            }
            return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ItemID.GraniteWall] = ModContent.ItemType<UnsafeGraniteWall>();
            ItemID.Sets.ShimmerTransformToItem[ItemID.MarbleWall] = ModContent.ItemType<UnsafeMarbleWall>();
            ItemID.Sets.ShimmerTransformToItem[ItemID.ReleaseLantern] = ModContent.ItemType<MatsuriLantern>();
            base.SetStaticDefaults();
        }
        public static Dictionary<string, int> OldUnlimitedAmmos = new()
        {
            { "GelPack", ItemID.Gel },
            { "StarPouch", ItemID.FallenStar },

            { "BoneQuiver", ItemID.BoneArrow },
            { "ChlorophyteQuiver", ItemID.ChlorophyteArrow },
            { "CursedQuiver", ItemID.CursedArrow },
            { "FlameQuiver", ItemID.FlamingArrow },
            { "FrostburnQuiver", ItemID.FrostburnArrow },
            { "HellfireQuiver", ItemID.HellfireArrow },
            { "HolyQuiver", ItemID.HolyArrow },
            { "IchorQuiver", ItemID.IchorArrow },
            { "JesterQuiver", ItemID.JestersArrow },
            { "LuminiteQuiver", ItemID.MoonlordArrow },
            { "UnholyQuiver", ItemID.UnholyArrow },
            { "VenomQuiver", ItemID.VenomArrow },

            { "ChlorophytePouch", ItemID.ChlorophyteBullet },
            { "CrystalPouch", ItemID.CrystalBullet },
            { "CursedPouch", ItemID.CursedBullet },
            { "ExplosivePouch", ItemID.ExplodingBullet },
            { "GoldenPouch", ItemID.GoldenBullet },
            { "IchorPouch", ItemID.IchorBullet },
            { "LuminitePouch", ItemID.MoonlordBullet },
            { "MeteorPouch", ItemID.MeteorShot },
            { "NanoPouch", ItemID.NanoBullet },
            { "PartyPouch", ItemID.PartyBullet },
            { "SilverPouch", ItemID.SilverBullet },
            { "TungstenPouch", ItemID.TungstenBullet },
            { "VelocityPouch", ItemID.HighVelocityBullet },
            { "VenomPouch", ItemID.VenomBullet },

            { "CopperCoinBag", ItemID.CopperCoin },
            { "SilverCoinBag", ItemID.SilverCoin },
            { "GoldCoinBag", ItemID.GoldCoin },
            { "PlatinumCoinBag", ItemID.PlatinumCoin },

            { "CrystalDartBox", ItemID.CrystalDart },
            { "CursedDartBox", ItemID.CursedDart },
            { "IchorDartBox", ItemID.IchorDart },
            { "PoisonDartBox", ItemID.PoisonDart },

            { "ClusterRocket1Box", ItemID.ClusterRocketI },
            { "ClusterRocket2Box", ItemID.ClusterRocketII },
            { "DryRocketBox", ItemID.DryRocket },
            { "HoneyRocketBox", ItemID.HoneyRocket },
            { "LavaRocketBox", ItemID.LavaRocket },
            { "MiniNuke1Box", ItemID.MiniNukeI },
            { "MiniNuke2Box", ItemID.MiniNukeII },
            { "Rocket1Box", ItemID.RocketI },
            { "Rocket2Box", ItemID.RocketII },
            { "Rocket3Box", ItemID.RocketIII },
            { "Rocket4Box", ItemID.RocketIV },
            { "WetRocketBox", ItemID.WetRocket },
        };
        public static void CheckForIsOldUnlimitedAmmo(Item item)
        {
            if (item.ModItem is UnloadedItem unloadedItem && OldUnlimitedAmmos.TryGetValue(unloadedItem.ItemName, out var ammoItemType))
            {
                item.TurnToAir();
                item.type = ammoItemType;
                item.stack = 3996;
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not InitializationItemCreationContext)
            {
                FargoPlayer modPlayer = Main.LocalPlayer.FargoMutant();
                if (!modPlayer.ItemHasBeenOwned[item.type])
                {
                    foreach (Player p in Main.ActivePlayers)
                    {
                        p.FargoMutant().ItemHasBeenOwned[item.type] = true;
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket syncOneOwned = Mod.GetPacket();
                        syncOneOwned.Write((byte)Fargowiltas.PacketID.SyncOwnedItem);
                        syncOneOwned.Write(item.type);
                        syncOneOwned.Send();
                    }
                }
            }
        }
    }
}
