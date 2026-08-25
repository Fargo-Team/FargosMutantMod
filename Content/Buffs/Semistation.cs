using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Buffs;

public class Semistation : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            //sunflower
            Main.SceneMetrics.HasSunflower = false;
            player.buffImmune[BuffID.Sunflower] = true;
            player.moveSpeed += 0.1f;
            player.moveSpeed *= 1.1f;
            player.sunflower = true;

            //campfire
            Main.SceneMetrics.HasCampfire = true;
            player.buffImmune[BuffID.Campfire] = true;

            //heart lantern
            Main.SceneMetrics.HasHeartLantern = false;
            player.buffImmune[BuffID.HeartLamp] = true;
            player.lifeRegen += 2;

            //star bottle
            Main.SceneMetrics.HasStarInBottle = false;
            player.buffImmune[BuffID.StarInBottle] = true;
            player.manaRegenBonus += 2;
        }
    }
}