using Fargowiltas.Content.Items.Summons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using static Fargowiltas.Fargowiltas;

namespace Fargowiltas.Content.Items.Summons.Abom
{
    public class BetsyEgg : BaseSummon
    {
        public override int NPCType => NPCID.DD2Betsy;

        public override bool CanShoot(Player player) => false;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 17; // Places it right after Solar Tablet
		}

        public override bool CanUseItem(Player player)
        {
            if (DD2Event.Ongoing || !DD2Event.ReadyForTier3)
                return false;

            Point playerPos = player.Center.ToTileCoordinates();
            Point standPos = new Point(-1, -1);
            int radius = 20;
            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    Point p = playerPos + new Point(i, j);
                    Tile t = Main.tile[p];
                    if (t.HasTile && t.TileType == TileID.ElderCrystalStand)
                    {
                        if (DD2Event.WouldFailSpawningHere(p.X, p.Y))
                        {
                            return false;
                        }
                        standPos = p;
                        break;
                    }
                }
            }

            if (standPos.X < 0 || standPos.X > Main.maxTilesX || standPos.Y < 0 || standPos.Y > Main.maxTilesY)
            {
                return false;
            }

            return base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {
            Point playerPos = player.Center.ToTileCoordinates();
            Point standPos = new Point(-1, -1);
            int radius = 20;
            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    Point p = playerPos + new Point(i, j);
                    Tile t = Main.tile[p];
                    if (t.HasTile && t.TileType == TileID.ElderCrystalStand)
                    {
                        if (DD2Event.WouldFailSpawningHere(p.X, p.Y))
                        {
                            DD2Event.FailureMessage(player.whoAmI);
                            return null;
                        }
                        standPos = p;
                        break;
                    }
                }
            }

            if (standPos.X < 0 || standPos.X > Main.maxTilesX || standPos.Y < 0 || standPos.Y > Main.maxTilesY)
            {
                return null;
            }


            // actually spawn
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                BetsyEggUsed = true;
                DD2Event.SummonCrystal(standPos.X, standPos.Y, player.whoAmI);
                DD2Event.TimeLeftBetweenWaves = 0;
                NPC.waveNumber = 6;
                NPC.waveKills = 220;
                DD2Event.CheckProgress(NPCID.DD2GoblinT3);
                player.QuickSpawnItem(Item.GetSource_FromThis(), ItemID.DD2EnergyCrystal, 140); // give all missing crystals
                BetsyEggUsed = false;
            }
            else
            {
                var netMessage = Instance.GetPacket();
                netMessage.Write((byte)PacketID.BetsySummon);
                netMessage.Write(player.whoAmI);
                netMessage.Write(standPos.X);
                netMessage.Write(standPos.Y);
                netMessage.Send();
            }

            return true;
        }
    }
}