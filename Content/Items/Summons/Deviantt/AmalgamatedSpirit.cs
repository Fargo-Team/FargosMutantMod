using Fargowiltas.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Summons.Deviantt
{
    public class AmalgamatedSpirit : BaseSpawnBooster
    {
        public override int BuffType => ModContent.BuffType<AmalgamatedSpiritBuff>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AncientBattleArmorMaterial) //forbidden fragment
                .AddIngredient(ItemID.Ectoplasm, 12)
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class AmalgamatedSpiritBuff : BaseSpawnBoosterBuff
    {
        public AmalgamatedSpiritBuff() : base(() => [NPCID.Necromancer, NPCID.NecromancerArmored, NPCID.DiabolistRed, NPCID.DiabolistWhite, NPCID.RaggedCaster, NPCID.RaggedCasterOpenCoat], () => Main.LocalPlayer.ZoneDungeon, 0.2f)
        {
        }
    }
}