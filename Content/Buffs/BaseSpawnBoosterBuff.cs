using Fargowiltas.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Buffs
{
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
}
