using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Buffs
{
    public class BigSuckBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.FargoMutant().bigSuck = true;
        }
    }
}