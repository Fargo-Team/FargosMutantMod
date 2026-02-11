using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Buffs.SpawnBoosters
{
    public abstract class BaseSpawnBoosterBuff : ModBuff
    {
        public List<int> NPCTypes;
        public Condition SpawnCondition;
        public float SpawnRate;

        protected BaseSpawnBoosterBuff(List<int> npcTypes, Condition spawnCondition, float spawnRate)
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
            if (SpawnCondition.IsMet())
            {
                var color = Color.Purple;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Texture2D glowBorder = ModContent.Request<Texture2D>("Fargowiltas/Content/Buffs/BuffGlowBorder").Value;
                spriteBatch.Draw(glowBorder, drawParams.Position - Vector2.One * 2, null, color * drawParams.DrawColor.A, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            spriteBatch.Draw(ourTexture, drawParams.Position, drawParams.SourceRectangle, drawParams.DrawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }

    public class TestBoosterBuff : BaseSpawnBoosterBuff // Remove this when the real ones are implemented
    {
        public override string Texture => "Fargowiltas/Content/Buffs/PlaceholderBuff";
        public TestBoosterBuff() : base([NPCID.GoblinScout], Condition.TimeDay, 3f)
        {
        }
    }
}
