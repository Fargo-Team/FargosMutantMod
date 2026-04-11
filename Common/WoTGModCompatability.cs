using Fargowiltas.Content.NPCs;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Common
{
    public class WoTGModCompatability : GlobalNPC
    {
        public override void GetChat(NPC npc, ref string chat)
        {
            Mod wotg = Fargowiltas.WoTG;
            if (wotg != null)
            {
                bool eclipseActive = (bool)wotg.Call("GetRiftEclipseActive");
                bool solynInWorld = NPC.AnyNPCs(wotg.Find<ModNPC>("Solyn").Type);
                bool defeatedAvatar = (bool)wotg.Call("GetBossDefeated", "avatarofemptiness");

                /*Main.NewText("eclipse: " + eclipseActive);
                Main.NewText("solyn: " + solynInWorld);
                Main.NewText("defeated avatar: " + defeatedAvatar);*/

                if (npc.type == ModContent.NPCType<Squirrel>() && eclipseActive)
                {
                    chat = Language.GetTextValue("Mods.Fargowiltas.NPCs.Squirrel.WoTGDialogue", npc.GivenName);
                }

                if (npc.type == ModContent.NPCType<LumberJack>())
                {
                    if (eclipseActive)
                    {
                        chat = Language.GetTextValue($"Mods.Fargowiltas.NPCs.LumberJack.WoTGDialogue.RiftEclipse{Main.rand.Next(1, 5)}");
                        if (Main.raining && Main.rand.NextBool(5))
                        {
                            chat = Language.GetTextValue("Mods.Fargowiltas.NPCs.LumberJack.WoTGDialogue.RiftEclipseSnow");
                        }
                    }
                }

                if (npc.type == ModContent.NPCType<Deviantt>())
                {
                    if (eclipseActive)
                    {
                        chat = Language.GetTextValue($"Mods.Fargowiltas.NPCs.Deviantt.WoTGDialogue.RiftEclipse{Main.rand.Next(1, 5)}", npc.GivenName);
                        if (Main.rand.NextBool(5))
                        {
                            chat = Language.GetTextValue($"Mods.Fargowiltas.NPCs.Deviantt.WoTGDialogue.RiftEclipseAlien", NPC.downedMartians ? Language.GetTextValue("Mods.Fargowiltas.NPCs.Deviantt.WoTGDialogue.RiftEclipseAlienExtension") : "");
                        }
                    }
                    if (solynInWorld && Main.rand.NextBool(7))
                    {
                        chat = Language.GetTextValue($"Mods.Fargowiltas.NPCs.Deviantt.WoTGDialogue.Solyn");
                    }
                }

                if (npc.type == ModContent.NPCType<Abominationn>())
                {
                    if (eclipseActive)
                    {
                        chat = Language.GetTextValue($"Mods.Fargowiltas.NPCs.Abominationn.WoTGDialogue.RiftEclipse{Main.rand.Next(1, 6)}");
                    }
                    if (solynInWorld && Main.rand.NextBool(7))
                    {
                        chat = Language.GetTextValue($"Mods.Fargowiltas.NPCs.Abominationn.WoTGDialogue.Solyn");
                    }
                }

                if (npc.type == ModContent.NPCType<Mutant>())
                {
                    if (eclipseActive)
                    {
                        chat = Language.GetTextValue($"Mods.Fargowiltas.NPCs.Mutant.WoTGDialogue.RiftEclipse{Main.rand.Next(1, 6)}");
                    }
                    if ((solynInWorld || defeatedAvatar) && Main.rand.NextBool(7))
                    {
                        chat = Language.GetTextValue($"Mods.Fargowiltas.NPCs.Mutant.WoTGDialogue.Solyn",  defeatedAvatar? Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.WoTGDialogue.SolynExtension2") : Language.GetTextValue("Mods.Fargowiltas.NPCs.Mutant.WoTGDialogue.SolynExtension1"));
                    }
                }

            }
            base.GetChat(npc, ref chat);
        }
    }
}
