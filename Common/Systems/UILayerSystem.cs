using Fargowiltas.Content.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Common.Systems
{
    public class UILayerSystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int optionsIndex = layers.FindIndex((layer) => layer.Name == "Vanilla: Ingame Options");
            if (optionsIndex != -1)
            {
                layers.Insert(optionsIndex, new LegacyGameInterfaceLayer("Fargo's Mutant: Buff Overlay", delegate ()
                {
                    FargoBuffOverlay.Draw(Main.spriteBatch, Main.LocalPlayer);
                    return true;
                }, InterfaceScaleType.UI));
            }

            int rulerIndex = layers.FindIndex((layer) => layer.Name == "Vanilla: Ruler");
            if (rulerIndex != -1)
            {
                layers.Insert(rulerIndex, new LegacyGameInterfaceLayer("Fargo's Mutant: Build Range Overlay", delegate ()
                {
                    PlacementReachOverlay.Draw(Main.spriteBatch, Main.LocalPlayer);
                    return true;
                }, InterfaceScaleType.Game));
            }
        }
    }
}
