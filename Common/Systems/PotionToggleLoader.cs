using System;
using System.Collections.Generic;
using System.Linq;
using Fargowiltas.Content.Items.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems
{
    public class PotionToggleLoader : ModSystem
    {
        public static Dictionary<int, PotionToggle> LoadedToggles
        {
            get;
            set;
        }

        public override void PostSetupContent()
        {
            LoadedToggles = [];

            for (int i = 0; i < ContentSamples.ItemsByType.Count; i++)
            {
                Item item = ContentSamples.ItemsByType[i];
                if (item.buffTime > 60 * 60 * 2 && item.ModItem is not BaseSpawnBooster)
                {
                    RegisterToggle(new PotionToggle(i, item.buffType));
                }
            }
        }

        public override void Unload()
        {
            LoadedToggles?.Clear();
        }


        public static void RegisterToggle(PotionToggle toggle)
        {
            LoadedToggles ??= [];
            if (LoadedToggles.ContainsKey(toggle.ItemID)) throw new Exception("Toggle of item id " + toggle.ItemID + " is already registered");
            if (LoadedToggles.Any(t => t.Value.BuffID == toggle.BuffID)) return; // duplicate buffs are fine to exist; just don't do it. handled by the already-existing item's toggle with the same buff id throw new Exception("Toggle of buff id " + toggle.BuffID + " is already registered");

            LoadedToggles.Add(toggle.ItemID, toggle);

        }
    }
}
