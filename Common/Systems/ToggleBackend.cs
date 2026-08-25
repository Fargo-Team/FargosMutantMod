using Terraria;

namespace Fargowiltas.Common.Systems;

public abstract class ToggleBackend
{
    public abstract void TryLoad();

    public abstract void Save();

    public abstract void LoadPlayerToggles(Player player);

    /*public Dictionary<string, int> UnpackPosition() => new Dictionary<string, int>() {
        { "X", TogglerPosition.X },
        { "Y", TogglerPosition.Y }
    };*/

    public abstract void SetAll(bool value);

    public abstract void SomeEffects();

    public abstract void MinimalEffects();

    public abstract void SaveCustomPreset(int slot);

    public abstract void LoadCustomPreset(int slot);
}
