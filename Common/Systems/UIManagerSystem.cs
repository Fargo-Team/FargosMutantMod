using Fargowiltas.Content.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Common.Systems
{
    public class UIManagerSystem : ModSystem
    {
        public override void UpdateUI(GameTime gameTime)
        {
            FargoUIManager.UpdateUI(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            FargoUIManager.ModifyInterfaceLayers(layers);
        }
    }
}
