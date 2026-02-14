using Fargowiltas.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Fargowiltas
{
    public class FargoPlayerBuffDrawLayer : PlayerDrawLayer
    {
        public override bool IsHeadLayer => false;

        private readonly int[] debuffsToIgnore = [
            BuffID.Campfire,
            BuffID.HeartLamp,
            BuffID.Sunflower,
            BuffID.PeaceCandle,
            BuffID.StarInBottle,
            BuffID.Tipsy,
            BuffID.MonsterBanner,
            BuffID.Werewolf,
            BuffID.Merfolk,
            BuffID.CatBast,
            BuffID.BrainOfConfusionBuff,
            BuffID.NeutralHunger
        ];

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            => !Main.hideUI 
            && drawInfo.drawPlayer.whoAmI == Main.myPlayer 
            && drawInfo.drawPlayer.active 
            && !drawInfo.drawPlayer.dead 
            && !drawInfo.drawPlayer.ghost
            && drawInfo.shadow == 0 
            && FargoClientConfig.Instance.DebuffOpacity > 0 
            && drawInfo.drawPlayer.buffType.Count(d => Main.debuff[d] && !debuffsToIgnore.Contains(d)) > 0;

        public override Position GetDefaultPosition() => new Between();

        //key is buff id
        //value is <old duration, max duration>
        //purpose of knowing old duration: get debuffed for 15sec, it decrease to 4sec, debuffed again for 10sec, recalculate ratio to match
        private Dictionary<int, Tuple<int, int>> memorizedDebuffDurations = new Dictionary<int, Tuple<int, int>>();

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            List<int> debuffs = player.buffType.Where(d => Main.debuff[d]).Except(debuffsToIgnore).ToList();
            const int maxPerLine = 10;
            int yOffset = 0;
            for (int j = 0; j < debuffs.Count; j += maxPerLine)
            {
                int maxForThisLine = Math.Min(maxPerLine, debuffs.Count - j);
                float midpoint = maxForThisLine / 2f - 0.5f;
                for (int i = 0; i < maxForThisLine; i++)
                {
                    int debuffID = debuffs[j + i];

                    Vector2 drawPos = (player.gravDir > 0 ? player.Top : player.Bottom);
                    drawPos.Y -= (32f + yOffset) * player.gravDir;
                    drawPos.X += 32f * (i - midpoint);

                    drawPos -= player.MountedCenter; //turn it into just the offset from player center
                    drawPos = drawPos.RotatedBy(-player.fullRotation); //correct for player rotation????
                    drawPos += player.MountedCenter;
                    drawPos -= Main.screenPosition;
                    drawPos += Vector2.UnitY * player.gfxOffY;
    

                    if (!TextureAssets.Buff[debuffID].IsLoaded)
                        continue;

                    Texture2D buffIcon = TextureAssets.Buff[debuffID].Value;
                    Color buffColor = Color.White * FargoClientConfig.Instance.DebuffOpacity;


                    int index = Array.FindIndex(player.buffType, id => id == debuffID);
                    int currentDuration = player.buffTime[index];

                    float rotation = (player.gravDir > 0 ? 0 : MathHelper.Pi) - player.fullRotation;
                    SpriteEffects effects = player.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                    float faderRatio = FargoClientConfig.Instance.DebuffFaderRatio;
                    if (faderRatio > 0 && !Main.buffNoTimeDisplay[debuffID])
                    {
                        if (currentDuration <= 1) //probably either a persistent debuff or one that will clear soon
                        {
                            if (memorizedDebuffDurations.TryGetValue(debuffID, out Tuple<int, int> knownDurations))
                            {
                                memorizedDebuffDurations.Remove(debuffID); //remove it
                                buffColor *= 1f - faderRatio; //like drawing 0% ratio so it doesnt jumpscare full opacity for 1 tick
                            }
                        }
                        else //is longer
                        {
                            //draw part of the rectangle to represent time remaining
                            if (memorizedDebuffDurations.TryGetValue(debuffID, out Tuple<int, int> knownDurations)
                                && knownDurations.Item1 >= currentDuration && knownDurations.Item2 > currentDuration)
                            {
                                int maxDuration = knownDurations.Item2;
                                float ratio = (float)currentDuration / maxDuration;
                                
                                int x = 0;
                                int y = (int)(buffIcon.Bounds.Height * (1f - ratio));
                                int width = buffIcon.Bounds.Width;
                                int height = (int)(buffIcon.Bounds.Height * ratio);
                                if (y + height > buffIcon.Bounds.Height) //just in case
                                    y = buffIcon.Bounds.Height - height;

                                Rectangle buffIconPortion = new Rectangle(x, y, width, height);
                                Vector2 drawPortion = drawPos + y * Vector2.UnitY.RotatedBy(rotation);
                                Color portionColor = buffColor * faderRatio;

                                drawInfo.DrawDataCache.Add(new DrawData(
                                    buffIcon, drawPortion.Floor(), buffIconPortion, buffColor,
                                    rotation, buffIcon.Bounds.Size() / 2,
                                    1f, effects, 0));

                                buffColor *= 1f - faderRatio;

                                //update known duration
                                memorizedDebuffDurations[debuffID] = new Tuple<int, int>(currentDuration, maxDuration);
                            }
                            else //if just got this debuff for the first time or it reapplied for longer, update max duration and draw at 100% opacity ratio
                            {
                                memorizedDebuffDurations[debuffID] = new Tuple<int, int>(currentDuration, currentDuration);
                            }
                        }
                    }

                    drawInfo.DrawDataCache.Add(new DrawData(
                        buffIcon, drawPos.Floor(), buffIcon.Bounds, buffColor,
                        rotation, buffIcon.Bounds.Size() / 2,
                        1f, effects, 0));

                    //if (ModContent.GetInstance<FargoConfig>().DebuffCountdown)
                    //{
                    //    Vector2 textPos = drawPos;
                    //    ChatManager.DrawColorCodedStringWithShadow(
                    //        Main.spriteBatch, 
                    //        FontAssets.ItemStack.Value, 
                    //        Math.Round(currentDuration / 60.0, MidpointRounding.AwayFromZero).ToString(), 
                    //        textPos,
                    //        Color.White, 
                    //        0f,
                    //        Vector2.Zero,
                    //        Vector2.One);
                    //}
                }
                yOffset += (int)(32 * player.gravDir);
            }
        }
    }
}
