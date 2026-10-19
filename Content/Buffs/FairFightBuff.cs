using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Buffs;

public class FairFightBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }
}
