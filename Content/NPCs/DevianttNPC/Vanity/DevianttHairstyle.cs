using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.NPCs.DevianttNPC.Vanity;

public class DevianttHairstyle : ModHair
{
    public override Gender RandomizedCharacterCreationGender => Gender.Unspecified;

    public override bool AvailableDuringCharacterCreation => true;
}
