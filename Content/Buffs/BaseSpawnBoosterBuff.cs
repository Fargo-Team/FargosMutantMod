using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Fargowiltas.Fargowiltas;

namespace Fargowiltas.Content.Buffs;

public abstract class BaseSpawnBoosterBuff : ModBuff
{
    public override string Texture => "Fargowiltas/Content/Items/Summons/Deviantt/" + this.GetType().Name.Replace("Buff", "Active");
    public Func<List<int>> NPCTypes;
    public Func<bool> SpawnCondition;
    public float SpawnRate;

    protected BaseSpawnBoosterBuff(Func<List<int>> npcTypes, Func<bool> spawnCondition, float spawnRate)
    {
        NPCTypes = npcTypes;
        SpawnCondition = spawnCondition;
        SpawnRate = spawnRate;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.FargoMutant().ActiveSpawnBoosters.Add(this);
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            ModPacket packet = Fargowiltas.Instance.GetPacket();
            packet.Write((byte)PacketID.AddDeviSummon);
            packet.Write(this.Type);
            packet.Write((byte)player.whoAmI);
        }
    }
    public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
    {
        Texture2D ourTexture = drawParams.Texture;
        if (!SpawnCondition.Invoke())
        {
            ourTexture = ModContent.Request<Texture2D>(Texture.Replace("Active", "Inactive")).Value;
        }
        spriteBatch.Draw(ourTexture, drawParams.Position, drawParams.SourceRectangle, drawParams.DrawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        return false;
    }
}
