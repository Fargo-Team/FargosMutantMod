using Fargowiltas.Common.Configs;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Tiles
{
    public class FargoGlobalPylon : GlobalPylon
    {
        public override bool? ValidTeleportCheck_PreAnyDanger(TeleportPylonInfo pylonInfo)
        {
            if (FargoServerConfig.Instance.PylonsIgnoreEvents && !Main.npc.Any(n => n.CountsAsBoss()))
                return true;

            return base.ValidTeleportCheck_PreAnyDanger(pylonInfo);
        }
    }
}