using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Fargowiltas
{
    internal class DevianttDialogueTracker
    {
        public static class HelpDialogueType
        {
            public static readonly byte BossOrEvent = 0;
            public static readonly byte Environment = 1;
            public static readonly byte Misc = 2;
        }

        public struct HelpDialogue
        {
            public readonly LocalizedText Message;
            public readonly byte Type;
            public readonly Predicate<string> Predicate;

            public HelpDialogue(LocalizedText message, byte type, Predicate<string> predicate)
            {
                Message = message;
                Type = type;
                Predicate = predicate;
            }

            public bool CanDisplay(string deviName) => Predicate(deviName);
        }

        public List<HelpDialogue> PossibleDialogue;
        private int lastDialogueType;

        public DevianttDialogueTracker()
        {
            PossibleDialogue = new List<HelpDialogue>();
        }

        public void AddDialogue(string messageKey, byte type, Predicate<string> predicate, string keyPath = "Fargowiltas.NPCs.Deviantt.HelpDialogue")
        {
            LocalizedText message = Language.GetText($"Mods.{keyPath}.{messageKey}");
            PossibleDialogue.Add(new HelpDialogue(message, type, predicate));
        }

        public string GetDialogue(string deviName)
        {
            WeightedRandom<string> dialogueChooser = new();
            (List<HelpDialogue> sortedDialogue, int type) = SortDialogue(deviName);

            foreach (HelpDialogue dialogue in sortedDialogue)
            {
                dialogueChooser.Add(dialogue.Message.Value);
            }

            lastDialogueType = type;
            return dialogueChooser;
        }

        private (List<HelpDialogue> sortedDialogue, int type) SortDialogue(string deviName)
        {
            List<HelpDialogue> sortedDialogue = new List<HelpDialogue>();
            int typeChoice = 0;
            int attempts = 0;
            while (true)
            {
                attempts++;
                typeChoice = Main.rand.Next(3);
                if (typeChoice != lastDialogueType || typeChoice == HelpDialogueType.Misc) // There's a lot more misc so allow repeats
                {
                    sortedDialogue = PossibleDialogue.Where((dialogue) => dialogue.Type == typeChoice && dialogue.CanDisplay(deviName)).ToList();

                    if (sortedDialogue.Count != 0)
                        break;
                }
                
                if (attempts == 100)
                {
                    typeChoice = HelpDialogueType.BossOrEvent;
                    sortedDialogue = PossibleDialogue.Where((dialogue) => dialogue.Type == typeChoice && dialogue.CanDisplay(deviName)).ToList();
                    break;
                }
            }

            return (sortedDialogue, typeChoice);
        }

        public void AddVanillaDialogue()
        {
            AddDialogue("DownedMutant", HelpDialogueType.BossOrEvent,
                (name) => (bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant") ?? false));

            AddDialogue("DownedAbom", HelpDialogueType.BossOrEvent,
                (name) => (bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedAbom") ?? false) && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant") ?? false));

            AddDialogue("DownedEridanus", HelpDialogueType.Misc,
                (name) => (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedEridanus") && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedAbom"));

            AddDialogue("PostML", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedMoonlord && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedEridanus"));

            AddDialogue("MoonLord", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedAncientCultist && !NPC.downedMoonlord);

            AddDialogue("Cultist", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedFishron && !NPC.downedAncientCultist);

            AddDialogue("Fishron", HelpDialogueType.BossOrEvent,
                (name) => FargoWorld.DownedBools["betsy"] && !NPC.downedFishron);

            AddDialogue("Betsy", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedGolemBoss && !FargoWorld.DownedBools["betsy"]);

            AddDialogue("Golem", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedPlantBoss && !NPC.downedGolemBoss);

            AddDialogue("Plantera", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedPlantBoss);

            //AddDialogue("Watch out when you break your fourth altar! It might attract the pirates, so be sure you're ready when you do it.", HelpDialogueType.BossOrEvent, (name) => Main.hardMode && !NPC.downedPirates);

            AddDialogue("Destroyer", HelpDialogueType.BossOrEvent,
                (name) => Main.hardMode && !NPC.downedMechBoss1);

            AddDialogue("Twins", HelpDialogueType.BossOrEvent,
                (name) => Main.hardMode && !NPC.downedMechBoss2);

            AddDialogue("Prime", HelpDialogueType.BossOrEvent,
                (name) => Main.hardMode && !NPC.downedMechBoss3);

            AddDialogue("WOF", HelpDialogueType.BossOrEvent,
                (name) => (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedDevi") && !Main.hardMode);

            AddDialogue("Deviantt", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedBoss3 && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedDevi"));

            AddDialogue("Skeletron", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedQueenBee && !NPC.downedBoss3);

            AddDialogue("QueenBee", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedBoss2 && !NPC.downedQueenBee);

            AddDialogue("Brain", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedBoss1 && !NPC.downedBoss2 && WorldGen.crimson);

            AddDialogue("Eater", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedBoss1 && !NPC.downedBoss2 && !WorldGen.crimson);

            AddDialogue("GoblinsCrimson", HelpDialogueType.BossOrEvent,
                (name) => !NPC.downedGoblins && WorldGen.crimson);

            AddDialogue("GoblinsCorruption", HelpDialogueType.BossOrEvent,
                (name) => !NPC.downedGoblins && !WorldGen.crimson);

            // I added this because, if there isn't always dialogue available for a boss, the dialogue chooser self destructs
            AddDialogue("EOC", HelpDialogueType.BossOrEvent,
                (name) => NPC.downedSlimeKing && !NPC.downedBoss1);

            AddDialogue("Slime", HelpDialogueType.BossOrEvent,
                (name) => !NPC.downedSlimeKing);

            AddDialogue("Auras", HelpDialogueType.Misc,
                (name) => true);

            AddDialogue("Debuffs", HelpDialogueType.Misc,
                (name) => true);

            AddDialogue("SoulToggle", HelpDialogueType.Misc,
                (name) => true);

            AddDialogue("CactusDamage", HelpDialogueType.Environment,
                (name) => true);

            AddDialogue("RainLightning", HelpDialogueType.Environment,
                (name) => true);

            //AddDialogue("Just so you know, ammos are less effective. Only a bit of their damage contributes to your total output!",
            //    HelpDialogueType.Misc, (name) => Main.LocalPlayer.HeldItem.CountsAsClass(DamageClass.Ranged));

            //AddDialogue("Found any Top Hat Squirrels yet? Keep one in your inventory and maybe a special friend will show up!",
            //    HelpDialogueType.Misc, (name) => !NPC.AnyNPCs(ModContent.NPCType<Squirrel>()));

            AddDialogue("LifeCrystals", HelpDialogueType.Misc,
                (name) => Main.LocalPlayer.statLifeMax < 400);

            AddDialogue("Fish", HelpDialogueType.Misc,
                (name) => !Main.hardMode);

            AddDialogue("Underwater", HelpDialogueType.Environment,
                (name) => !Main.LocalPlayer.GetJumpState(ExtraJump.Flipper).Enabled && !Main.LocalPlayer.gills && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("MutantAntibodies") ?? false));

            AddDialogue("Underworld", HelpDialogueType.Environment,
                (name) => !Main.LocalPlayer.fireWalk && !(Main.LocalPlayer.lavaMax > 0) && !Main.LocalPlayer.buffImmune[BuffID.OnFire] && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            static bool SuffocationImmune()
            {
                Player player = Main.LocalPlayer;
                Item helm = player.armor[0];
                return helm.type == ItemID.FishBowl || helm.type == ItemID.GoldGoldfishBowl || player.accDivingHelm || player.spaceGun || player.buffImmune[BuffID.Suffocation];
            }
            AddDialogue("SpaceSuffocation", HelpDialogueType.Environment,
                (name) => !SuffocationImmune() && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            //AddDialogue("The spirits of light and dark stopped by and they sounded pretty upset with you. Don't be too surprised if something happens to you for entering their territory!", HelpDialogueType.Environment, (name) => Main.hardMode && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            //AddDialogue("Why not go hunting for some rare monsters every once in a while? Plenty of treasure to be looted and all that.", HelpDialogueType.Misc, (name) => Main.hardMode);

            AddDialogue("UndergroundShadowHands", HelpDialogueType.Environment,
                (name) => !NPC.downedDeerclops && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            AddDialogue("UndergroundHallow", HelpDialogueType.Environment,
                (name) => Main.hardMode && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? false));

            AddDialogue("LifeFruit", HelpDialogueType.Misc,
                (name) => Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.LocalPlayer.statLifeMax2 < 500);

            // This is much more possible than before because of how branching works, so I just decided to remove it.
            //AddDialogue("Ever tried out those 'enchantment' thingies? Try breaking a couple altars and see what you can make.",
            //    HelpDialogueType.Misc, (name) => Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);
        }
    }
}
