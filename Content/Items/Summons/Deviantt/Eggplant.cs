using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt;

public class Eggplant : BaseSpawnBooster
{
    public override int BuffType => ModContent.BuffType<EggplantBuff>();


    public override void AddRecipes()
    {
        void Recipe(int fruit)
        {
            CreateRecipe()
                .AddIngredient(fruit)
                .AddIngredient(ItemID.JungleSpores, 4)
                .AddIngredient(ItemID.Vine, 2)
                .AddIngredient(ItemID.JungleGrassSeeds, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }

        Recipe(ItemID.Mango);
        Recipe(ItemID.Pineapple);
    }
}
public class EggplantBuff : BaseSpawnBoosterBuff
{
    //vanilla uses !Main.daytime instead of IsItDay for non-boss npc spawns
    public EggplantBuff() : base(() => [NPCID.DoctorBones], () => !Main.dayTime && Main.LocalPlayer.ZoneJungle && Main.LocalPlayer.ZoneJungle && !Main.LocalPlayer.ZoneLihzhardTemple, 0.15f)
    {
    }
}