using Fargowiltas.Common.Configs;
using Fargowiltas.Content.Items;
using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas;

public class FargowiltasDetours : ModSystem
{
    internal static bool BetsyEggUsed;
    public override void Load()
    {
        BetsyEggUsed = false;

        On_DD2Event.DropMedals += BetsyMedals;

        On_Item.GetShimmered += FixRecipeGroupsShimmerInteraction;

        On_Main.DoUpdateInWorld += UpdateEnchantedTreeFruit;
        On_Main.DrawPlayers_AfterProjectiles += DrawEnchantedTrees;
        On_Main.UpdateTime_StartDay += UpdatePortableSundialCooldown_Day;
        On_Main.UpdateTime_StartNight += UpdatePortableSundialCooldown_Night;

        On_NPC.CountKillForBannersAndDropThem += PreventBannerDrop;

        //On_Player.AddBuff += AddBuff;
        On_Player.DoCommonDashHandle += OnVanillaDash;
        On_Player.dropItemCheck += EnchantedTreeMouseClick;
        On_Player.DropTombstone += DisableTombstones;
        On_Player.HasUnityPotion += OnHasUnityPotion;
        On_Player.ItemCheck_CheckCanUse += AllowUseSummons;
        On_Player.ItemCheck_UseBossSpawners += AllowUseSummons2EvilEdition;
        On_Player.ItemCheck_UseEventItems += AllowUseEventSummons;
        On_Player.KeyDoubleTap += OnVanillaDoubleTapSetBonus;
        On_Player.KeyHoldDown += OnVanillaHoldSetBonus;
        On_Player.SummonItemCheck += AllowMultipleBosses;
        On_Player.TakeUnityPotion += OnTakeUnityPotion;

        On_Recipe.FindRecipes += FindRecipes_ElementalAssemblerGraveyardHack;

        On_SceneMetrics.ExportTileCountsToMain += ExportTileCountsToMain_PurityTotemHack;

        On_WorldGen.CountTileTypesInArea += CountTileTypesInArea_PurityTotemHack;

        On_LucyAxeMessage.SpawnPopupText += AddMessage;
    }

    private void AddMessage(On_LucyAxeMessage.orig_SpawnPopupText orig, LucyAxeMessage.MessageSource source, int variationUnwrapped, Vector2 position, Vector2 velocity)
    {
        LocalizedText LucyTheAxeAdditions = Language.GetText("Mods.Fargowiltas.Items.LucyTheAxe.Eaten");
        string lucyTheAxeAdditions = LucyTheAxeAdditions.Value;
        if (source == (LucyAxeMessage.MessageSource)8)
        {
            AdvancedPopupRequest request = default(AdvancedPopupRequest);
            request.Text = lucyTheAxeAdditions;
            request.DurationInFrames = 420;
            request.Velocity = velocity;
            request.Color = new Color(184, 96, 98) * 1.15f;
            PopupText.NewText(request, position);
            return;
        }
        orig(source, variationUnwrapped, position, velocity);
    }
    private static Item[] GetWormholes(Player self)
    {
        var wormholes = self.inventory
            .Concat(self.bank.item)
            .Concat(self.bank2.item)
            .Concat(self.bank3.item);
        if (self.useVoidBag())
        {
            wormholes = wormholes.Concat(self.bank4.item);
        }

        return wormholes.Where(x => x.type == ItemID.WormholePotion && x.stack > 0).ToArray();
    }

    private static void OnTakeUnityPotion(On_Player.orig_TakeUnityPotion orig, Player self)
    {
        var wormholes = GetWormholes(self);
        if (wormholes.Length == 0)
            return;
        Item item = wormholes.First();
        if (ItemLoader.ConsumeItem(item, self))
            item.stack--;
        if (item.stack <= 0)
            item.TurnToAir();
    }

    private static void DisableTombstones(On_Player.orig_DropTombstone orig, Player self, long coinsOwned, NetworkText deathText, int hitDirection)
    {
        if (FargoServerConfig.Instance.DisableTombstones)
            return;

        orig(self, coinsOwned, deathText, hitDirection);
    }

    private static bool OnHasUnityPotion(On_Player.orig_HasUnityPotion orig, Player self)
    {
        return GetWormholes(self).Length > 0;
    }

    private static void FindRecipes_ElementalAssemblerGraveyardHack(
        On_Recipe.orig_FindRecipes orig,
        bool canDelayCheck)
    {
        bool oldZoneGraveyard = Main.LocalPlayer.ZoneGraveyard;

        if (!Main.gameMenu && Main.LocalPlayer.active && Main.LocalPlayer.FargoMutant().ElementalAssemblerNearby > 0)
            Main.LocalPlayer.ZoneGraveyard = true;

        orig(canDelayCheck);

        Main.LocalPlayer.ZoneGraveyard = oldZoneGraveyard;
    }

    //for town npc housing check, independent from player biome
    private static void CountTileTypesInArea_PurityTotemHack(
        On_WorldGen.orig_CountTileTypesInArea orig,
        int[] tileTypeCounts, int startX, int endX, int startY, int endY)
    {
        orig(tileTypeCounts, startX, endX, startY, endY);

        if (tileTypeCounts[ModContent.TileType<PurityTotemSheet>()] > 0)
        {
            const int sunflowerWeight = 5;
            tileTypeCounts[TileID.Sunflower] += PurityTotemSheet.TILES_NEGATED / sunflowerWeight;
        }
    }

    //for current biome
    private void ExportTileCountsToMain_PurityTotemHack(
        On_SceneMetrics.orig_ExportTileCountsToMain orig,
        SceneMetrics self)
    {
        orig(self);

        //for visible biome effect
        if (self.GetTileCount((ushort)ModContent.TileType<PurityTotemSheet>()) > 0)
        {
            const int tilesNegated = PurityTotemSheet.TILES_NEGATED;

            //reduce biome counts, floor at zero
            self.BloodTileCount = Math.Max(self.BloodTileCount - tilesNegated, 0);
            self.EvilTileCount = Math.Max(self.EvilTileCount - tilesNegated, 0);
            self.GraveyardTileCount = Math.Max(self.GraveyardTileCount - tilesNegated, 0);

            //reenable if disabled by graveyard
            if (self.GetTileCount(TileID.Sunflower) > 0)
                self.HasSunflower = true;
        }
    }
    private static void OnVanillaDash(On_Player.orig_DoCommonDashHandle orig, Player player, out int dir, out bool dashing, Player.DashStartAction dashStartAction)
    {
        if (FargoClientConfig.Instance.DoubleTapDashDisabled)
        {
            player.dashTime = 0;
            /*
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                Main.NewText(calamity);
                if (calamity.TryFind("CalamityPlayer", out ModPlayer modPlayer))
                {
                    FieldInfo dashTimeMod = modPlayer.GetType().GetField("dashTimeMod");
                    Main.NewText(dashTimeMod.Name);
                    if (dashTimeMod != null)
                        dashTimeMod.SetValue(modPlayer, 0);
                }
            }
            */
        }


        orig.Invoke(player, out dir, out dashing, dashStartAction);

        if (player.whoAmI == Main.myPlayer && Fargowiltas.DashKey.JustPressed && !player.CCed)
        {
            InputManager modPlayer = player.GetModPlayer<InputManager>();
            if (player.controlRight && player.controlLeft)
            {
                dir = modPlayer.latestXDirPressed;
            }
            else if (player.controlRight)
            {
                dir = 1;
            }
            else if (player.controlLeft)
            {
                dir = -1;
            }
            if (dir == 0) // this + commented out below because changed to not have an effect when not holding any movement keys; primarily so it's affected by stun effects
                return;
            //else if (modPlayer.latestXDirReleased != 0)
            //{
            //    dir = modPlayer.latestXDirReleased;
            //}
            //else
            //{
            //    dir = player.direction;
            //}
            player.timeSinceLastDashStarted = 0;
            player.direction = dir;
            dashing = true;
            if (player.dashTime > 0)
            {
                player.dashTime--;
            }
            if (player.dashTime < 0)
            {
                player.dashTime++;
            }
            if ((player.dashTime <= 0 && player.direction == -1) || (player.dashTime >= 0 && player.direction == 1))
            {
                player.dashTime = 15;
                return;
            }
            dashing = true;
            player.dashTime = 0;

            if (dashStartAction != null)
                dashStartAction?.Invoke(dir);
        }

    }
    private static void OnVanillaDoubleTapSetBonus(On_Player.orig_KeyDoubleTap orig, Player player, int keyDir)
    {
        if (!FargoClientConfig.Instance.DoubleTapSetBonusDisabled || Fargowiltas.SetBonusKey.JustPressed)
        {
            orig.Invoke(player, keyDir);
        }
    }
    private static void OnVanillaHoldSetBonus(On_Player.orig_KeyHoldDown orig, Player player, int keyDir, int holdTime)
    {
        if (!FargoClientConfig.Instance.DoubleTapSetBonusDisabled || Fargowiltas.SetBonusKey.Current)
        {
            orig.Invoke(player, keyDir, holdTime);
        }
    }

    private bool AllowUseSummons(On_Player.orig_ItemCheck_CheckCanUse orig, Player self, Item item)
    {
        if (FargoGlobalItem.AlwaysUsableVanillaSummons.Contains(item.type) && FargoServerConfig.Instance.EasySummons)
        {
            if (!((item.type == ItemID.BloodMoonStarter && Main.bloodMoon) ||
                (item.type == ItemID.NaughtyPresent && Main.snowMoon) ||
                (item.type == ItemID.PumpkinMoonMedallion && Main.pumpkinMoon) ||
                (item.type == ItemID.GoblinBattleStandard && Main.invasionType == InvasionID.GoblinArmy) ||
                (item.type == ItemID.SolarTablet && Main.eclipse) ||
                (item.type == ItemID.PirateMap && Main.invasionType == InvasionID.PirateInvasion) ||
                (item.type == ItemID.SnowGlobe && Main.invasionType == InvasionID.SnowLegion)))
            {
                return true;
            }
        }
        return orig(self, item);
    }
    private bool AllowMultipleBosses(On_Player.orig_SummonItemCheck orig, Player self, Item item)
    {
        if (FargoServerConfig.Instance.EasySummons && self.itemAnimation == self.itemAnimationMax)
        {
            return true;
        }
        return orig(self, item);
    }

    /*private void AddBuff(On_Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet, bool foodHack)
    {
        orig(self, type, timeToAdd, quiet, foodHack);
    }*/

    private void AllowUseEventSummons(On_Player.orig_ItemCheck_UseEventItems orig, Player self, Item item)
    {
        if (!FargoServerConfig.Instance.EasySummons)
        {
            orig(self, item);
            return;
        }
        bool day = Main.dayTime;
        bool hardmode = Main.hardMode;
        bool dd2event = DD2Event.Ongoing;
        bool pumpkin = Main.pumpkinMoon;
        bool frost = Main.snowMoon;
        int lifecrystals = self.ConsumedLifeCrystals;
        if (self.ItemTimeIsZero && self.itemAnimation > 0)
        {
            if (FargoGlobalItem.NightSettingSummons.Contains(item.type))
            {
                Main.dayTime = false;
            }
            if (item.type == ItemID.SolarTablet)
            {
                Main.dayTime = true;
                Main.hardMode = true;
            }
            if (item.type == ItemID.PumpkinMoonMedallion)
            {
                DD2Event.Ongoing = false;
                Main.snowMoon = false;
            }
            if (item.type == ItemID.NaughtyPresent)
            {
                DD2Event.Ongoing = false;
                Main.pumpkinMoon = false;
            }
            if (item.type == ItemID.GoblinBattleStandard || item.type == ItemID.PirateMap || item.type == ItemID.SnowGlobe)
            {
                if (self.ConsumedLifeCrystals < 5) self.ConsumedLifeCrystals = 5;
            }
            if (item.type == ItemID.PirateMap || item.type == ItemID.SnowGlobe)
            {
                Main.hardMode = true;
            }
            //with this one its just easier to redo the whole thing
            if (item.type == ItemID.CelestialSigil)
            {
                SoundEngine.PlaySound(SoundID.Roar, self.position);
                self.ApplyItemTime(item);
                if (Main.netMode == NetmodeID.SinglePlayer)
                    WorldGen.StartImpendingDoom(60);
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, self.whoAmI, -8f);
                return;
            }

        }
        orig(self, item);

        //Main.dayTime = day;
        //DD2Event.Ongoing = dd2event;
        //Main.pumpkinMoon = pumpkin;
        //Main.snowMoon = frost;
        Main.hardMode = hardmode;
        self.ConsumedLifeCrystals = lifecrystals;
    }

    private void AllowUseSummons2EvilEdition(On_Player.orig_ItemCheck_UseBossSpawners orig, Player self, int onWhichPlayer, Item item)
    {
        if (!FargoServerConfig.Instance.EasySummons)
        {
            orig(self, onWhichPlayer, item);
            return;
        }
        bool day = Main.dayTime;
        if (self.ItemTimeIsZero && self.itemAnimation > 0)
        {
            if (FargoGlobalItem.NightSettingSummons.Contains(item.type))
            {
                Main.dayTime = false;
            }
            if (item.type == ItemID.SolarTablet)
            {
                Main.dayTime = true;
            }
            if (item.type == ItemID.WormFood)
            {
                self.ZoneCorrupt = true;
            }
            if (item.type == ItemID.BloodySpine)
            {
                self.ZoneCrimson = true;
            }
            if (item.type == ItemID.Abeemination)
            {
                self.ZoneJungle = true;
                self.ZoneRockLayerHeight = true;
            }
            if (item.type == ItemID.DeerThing)
            {
                self.ZoneSnow = true;
            }
            if (item.type == ItemID.QueenSlimeCrystal)
            {
                self.ZoneHallow = true;
            }
        }
        orig(self, onWhichPlayer, item);
        //Main.dayTime = day;

    }
    private void DrawEnchantedTrees(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
    {
        EnchantedTreeTileEntity.DrawEnchantedTrees();
        orig(self);
    }

    private void UpdateEnchantedTreeFruit(On_Main.orig_DoUpdateInWorld orig, Main self, Stopwatch sw)
    {
        orig(self, sw);
        EnchantedTreeTileEntity.UpdateEnchantedTrees();
    }

    private void EnchantedTreeMouseClick(On_Player.orig_dropItemCheck orig, Player self)
    {

        if (self.whoAmI == Main.myPlayer && Main.playerInventory && !Main.mouseItem.IsAir && Main.mouseItem.stack > 0 && !Main.mouseItem.favorited && Main.mouseRight && Main.mouseRightRelease && !self.mouseInterface)
        {
            for (int t = 0; t < EnchantedTreeSheet.EnchantedTrees.Count; t++)
            {

                if (!FargoUtils.TryGetTileEntityAs(EnchantedTreeSheet.EnchantedTrees[t].X, EnchantedTreeSheet.EnchantedTrees[t].Y, out EnchantedTreeTileEntity tree))
                {
                    continue;
                }

                if (tree.ItemType != -1 || tree.Fruits.Count != 0)
                {

                    continue;
                }

                Vector2 treeTopLeft = new Vector2(tree.Position.X, tree.Position.Y) * 16;
                Rectangle treeHitbox = new Rectangle((int)treeTopLeft.X, (int)treeTopLeft.Y, 3 * 16, 4 * 16);

                if (!treeHitbox.Contains(Main.MouseWorld.ToPoint()) || self.Distance(treeTopLeft) >= 400f)
                {
                    continue;
                }

                tree.ItemType = Main.mouseItem.type;
                tree.Prefix = Main.mouseItem.prefix;

                if (EnchantedTreeTileEntity.IsItemDupable(tree.ItemType))
                {

                    tree.Fruits.Add(new EnchantedTreeTileEntity.Fruit(tree.ItemType, tree.Position.ToWorldCoordinates() + new Vector2(16, -12), tree.Position.ToWorldCoordinates() + new Vector2(16, -80), Vector2.Zero));
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    { 
                        FargoNet.SendEnchantedTreeFruitPacket(t);
                    }
                }

                Main.mouseItem.stack -= 1;
                tree.CursorInsertCooldown = 10;
                return;
            }
        }
        orig(self);
    }

    public static void UpdatePortableSundialCooldown_Day(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
    {
        if (FargoWorld.PortableSundialCooldown > 0 && !FargoWorld.BlockPortaDialCooldown && Main.netMode != NetmodeID.MultiplayerClient)
        {
            FargoWorld.PortableSundialCooldown--;
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket syncClientCooldown = Fargowiltas.Instance.GetPacket();
                syncClientCooldown.Write((byte)Fargowiltas.PacketID.SyncPortableSundial);
                syncClientCooldown.Write(FargoWorld.PortableSundialCooldown);
                syncClientCooldown.Send();
            }
        }
        orig(ref stopEvents);
    }

    public static void UpdatePortableSundialCooldown_Night(On_Main.orig_UpdateTime_StartNight orig, ref bool stopEvents)
    {
        if (FargoWorld.PortableSundialCooldown > 0 && !FargoWorld.BlockPortaDialCooldown && Main.netMode != NetmodeID.MultiplayerClient)
        {
            FargoWorld.PortableSundialCooldown--;
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket syncClientCooldown = Fargowiltas.Instance.GetPacket();
                syncClientCooldown.Write((byte)Fargowiltas.PacketID.SyncPortableSundial);
                syncClientCooldown.Write(FargoWorld.PortableSundialCooldown);
                syncClientCooldown.Send();
            }
        }
        orig(ref stopEvents);
    }

    private void FixRecipeGroupsShimmerInteraction(On_Item.orig_GetShimmered orig, Item self)
    {
        if (!FargoClientConfig.Instance.AnimatedRecipeGroups)
        {
            orig(self);
            return;
        }
        foreach (Recipe recipe in Main.recipe.Where(recipe => recipe.HasResult(self.type) && recipe.acceptedGroups.Count != 0))
        {
            foreach (int groupID in recipe.acceptedGroups)
            {
                foreach (Item material in recipe.requiredItem.Where(material => RecipeGroup.recipeGroups[groupID].ContainsItem(material.type) && material.type != RecipeGroup.recipeGroups[groupID].IconicItemId))
                {
                    string name = material.Name;
                    int stack = material.stack;
                    material.ChangeItemType(RecipeGroup.recipeGroups[groupID].IconicItemId);
                    material.SetNameOverride(name);
                    material.stack = stack;
                }
            }
        }
        orig(self);
    }

    private void PreventBannerDrop(On_NPC.orig_CountKillForBannersAndDropThem orig, NPC npc)
    {
        if (FargoServerConfig.Instance.BannerRecipes && npc.SpawnedFromStatue)
            return;

        orig(npc);
    }

    private void BetsyMedals(On_DD2Event.orig_DropMedals orig, int medals)
    {
        if (BetsyEggUsed)
            return;

        orig(medals);
    }
}

