using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Dusts
{
    public class BigSuckDust : ModDust
    {
        public override string Texture => null;
        public override void OnSpawn(Dust dust)
        {
            //to give off the effect of sucking the entire world, dust sprite is random from many options
            //its not really gonna make much sense, like snow coming from the jungle side, but convoluting this even more is probably a bad idea
            int dusttype;

            if (!Main.rand.NextBool(3)) 
                dusttype = Main.rand.NextFromList(DustID.Dirt, DustID.Stone, Main.rand.NextBool() ? DustID.Grass : DustID.GrassBlades, DustID.WoodFurniture, 
                    DustID.Sand, DustID.Mud, DustID.Snow, Dust.dustWater());

            else if (!Main.rand.NextBool(3)) 
                dusttype = Main.rand.NextFromList(DustID.Torch, DustID.GreenMoss, DustID.SlimeBunny, DustID.Copper, DustID.Iron, DustID.Silver, 
                    DustID.Gold, DustID.Ice, DustID.Cloud, WorldGen.crimson ? DustID.Crimson : DustID.Corruption, DustID.Pot, DustID.DungeonBlue);

            else dusttype = Main.rand.NextFromList(DustID.FoodPiece, DustID.CopperCoin, DustID.GoldCoin, DustID.Blood, DustID.GlowingMushroom, 
                DustID.Granite, DustID.Marble, DustID.Lihzahrd);
            
            int desiredVanillaDustTexture = dusttype;
            int frameX = desiredVanillaDustTexture * 10 % 1000;
            int frameY = desiredVanillaDustTexture * 10 / 1000 * 30 + Main.rand.Next(3) * 10;
            dust.frame = new Rectangle(frameX, frameY, 8, 8);

            if (dusttype is DustID.Torch or DustID.CopperCoin or DustID.GoldCoin or DustID.GlowingMushroom)
                dust.noLight = false;
            else dust.noLight = true;
            dust.noGravity = true;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            if (!dust.noLight) lightColor = Color.White;
            return base.GetAlpha(dust, lightColor);
        }

        public override bool Update(Dust dust)
        {
            dust.alpha -= 30;
            if (dust.customData is Player p)
            {
                Vector2 dist = dust.position - p.Center;
                dist.Normalize();
                float speed = (3f - dust.scale) * 20f;
                dist *= 0f - speed;
                dust.velocity = (dust.velocity * 4f + dist) / 5f;

                if (p.Hitbox.Distance(dust.position) < 40)
                    dust.scale -= 0.4f;
                else dust.scale += 0.04f;

                dust.position += (p.position - p.oldPosition) / 2;
            }
            return true;
        }
    }
}