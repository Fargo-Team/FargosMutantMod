using Fargowiltas.Content.Projectiles.Explosives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Explosives;

public class AutoHouseTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileLighted[Type] = true;
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            int p = Player.FindClosest(new Vector2(i * 16 + 8, j * 16 + 8), 0, 0);
            if (p != -1)
                Projectile.NewProjectile(new EntitySource_TileBreak(i, j), i * 16 + 8, (j + 2) * 16, 0f, 0f, ModContent.ProjectileType<AutoHouseProj>(), 0, 0, p);
        }

        noItem = true;
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        WorldGen.KillTile(i, j);
        if (Main.netMode != NetmodeID.SinglePlayer)
            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        return false;
    }
}