using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Biomes;

public class SkyBiome : IShoppingBiome, ILoadable
{
    public string NameKey => "Mods.Fargowiltas.Biome.Sky";

    public bool IsInBiome(Player player) => player.ZoneSkyHeight;

    void ILoadable.Load(Mod mod)
    {
    }

    void ILoadable.Unload()
    {
    }
}
