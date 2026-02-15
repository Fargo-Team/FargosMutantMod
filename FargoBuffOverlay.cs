using Fargowiltas.Assets.Textures;
using Fargowiltas.Common.Configs;
using Fargowiltas.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Fargowiltas
{
    public class FargoBuffOverlay
    {

        public static bool ShouldDraw(Player player)
            => !Main.hideUI 
            && player.whoAmI == Main.myPlayer 
            && player.active 
            && !player.dead 
            && !player.ghost
            && FargoClientConfig.Instance.DebuffOpacity > 0 
            && FargoClientConfig.Instance.DebuffDisplayMode != DebuffDisplayMode.Disabled
            && player.buffType.Any(d => Main.debuff[d] && !FargoSets.Buffs.BuffDisplayBlacklist[d]);


        //key is buff id
        //value is <old duration, max duration>
        //purpose of knowing old duration: get debuffed for 15sec, it decrease to 4sec, debuffed again for 10sec, recalculate ratio to match
        private static Dictionary<int, Tuple<int, int>> memorizedDebuffDurations = new Dictionary<int, Tuple<int, int>>();

        public static void Draw(SpriteBatch spriteBatch, Player player)
        {
            if (!ShouldDraw(player))
                return;

            
            List<int> debuffs = player.buffType.Where(d => Main.debuff[d] && !FargoSets.Buffs.BuffDisplayBlacklist[d]).ToList();
            const int maxPerLine = 10;
            int yOffset = 0;
            for (int j = 0; j < debuffs.Count; j += maxPerLine)
            {
                int maxForThisLine = Math.Min(maxPerLine, debuffs.Count - j);
                float midpoint = maxForThisLine / 2f - 0.5f;
                for (int i = 0; i < maxForThisLine; i++)
                {
                    int debuffID = debuffs[j + i];

                    float position = 32f;
                    if (FargoClientConfig.Instance.DebuffDisplayPosition == DebuffDisplayPosition.Bottom)
                        position = -74f;
                    if (Main.ingameOptionsWindow || Main.InGameUI.IsVisible)
                    {
                        if (FargoClientConfig.Instance.DebuffDisplayPosition == DebuffDisplayPosition.Bottom)
                        {
                            position = -300;
                        }
                        else
                            position = 260;
                    }
                   
                    //Main.NewText(Main.menuMode);
                    if (player.lavaTime != player.lavaMax || player.breath != player.breathMax)
                    {
                        if (FargoClientConfig.Instance.DebuffDisplayPosition == DebuffDisplayPosition.Top && !Main.playerInventory)
                            position = 88f;
                        if (Main.playerInventory && FargoClientConfig.Instance.DebuffDisplayPosition == DebuffDisplayPosition.Bottom)
                            position = -96f;

                        if (Main.ingameOptionsWindow || Main.InGameUI.IsVisible)
                        {
                            if (FargoClientConfig.Instance.DebuffDisplayPosition == DebuffDisplayPosition.Bottom)
                            {
                                position = -330;
                            }
                        }
                            
                    }
                    
                    Vector2 drawPos = player.Top;
                    drawPos.Y -= (position * Main.UIScale + yOffset);
                    drawPos.X += (36f * Main.UIScale) * (i - midpoint);              

                    drawPos -= player.MountedCenter; //turn it into just the offset from player center
                    drawPos = drawPos.RotatedBy(-player.fullRotation); //correct for player rotation????
                    drawPos += player.MountedCenter;
                    drawPos -= Main.screenPosition;
                    drawPos += Vector2.UnitY * player.gfxOffY;

                    drawPos.Y = Vector2.Transform(drawPos.Floor(), Matrix.Invert(Main.GameViewMatrix.ZoomMatrix)).Y;
                    drawPos.Y = Vector2.Transform(drawPos.Floor(), Main.GameViewMatrix.ZoomMatrix).Y;
                    drawPos.Y = (int)drawPos.Y;
                    drawPos.X = (int)drawPos.X;
                    drawPos /= Main.UIScale;

                    if (!TextureAssets.Buff[debuffID].IsLoaded)
                        continue;

                    Texture2D buffIcon = TextureAssets.Buff[debuffID].Value;
                    Color buffColor = Color.White * FargoClientConfig.Instance.DebuffOpacity;


                    int index = Array.FindIndex(player.buffType, id => id == debuffID);
                    int currentDuration = player.buffTime[index];

                    float rotation = 0;
                    SpriteEffects effects = SpriteEffects.None;

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

                                Texture2D line = FargoMutantAssets.UI.DebuffOverlayLine.Value;
                               
                                spriteBatch.Draw(
                                    buffIcon, drawPortion.Floor(), buffIconPortion, buffColor,
                                    rotation, buffIcon.Bounds.Size() / 2,
                                    1, effects, 0);

                                Color lineColor = (Color.White * FargoClientConfig.Instance.DebuffOpacity) * (Main.cursorAlpha * 1.2f);

                                if (buffIconPortion.Y >= 30)
                                    lineColor = new(0, 0, 0, 0);

                                spriteBatch.Draw(
                                    line, new Vector2(drawPortion.X, drawPortion.Y + (buffIconPortion.Y / 34)).Floor(), null, lineColor,
                                    rotation, buffIcon.Bounds.Size() / 2,
                                    1, effects, 0);

                                if (FargoClientConfig.Instance.DebuffDisplayMode == DebuffDisplayMode.IconTimer)
                                {
                                    string text = Math.Round(currentDuration / 60.0, MidpointRounding.AwayFromZero).ToString();

                                    Vector2 textSize = FontAssets.ItemStack.Value.MeasureString(text);
                                    Vector2 textPos = drawPos - new Vector2(textSize.X / 2, textSize.Y * 1.5f);

                                    if (FargoClientConfig.Instance.DebuffDisplayPosition == DebuffDisplayPosition.Bottom)
                                        textPos = drawPos + new Vector2(-textSize.X / 2, textSize.Y / 1.5f);

                                    ChatManager.DrawColorCodedStringWithShadow(
                                        Main.spriteBatch,
                                        FontAssets.ItemStack.Value,
                                        Math.Round(currentDuration / 60.0, MidpointRounding.AwayFromZero).ToString(),
                                        textPos.Floor(),
                                        Color.Lerp(Color.Red, Color.White, (Main.cursorAlpha * 0.9f)),
                                        0f,
                                        Vector2.Zero,
                                        Vector2.One);
                                }

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

                    spriteBatch.Draw(
                        buffIcon, drawPos.Floor(), buffIcon.Bounds, buffColor,
                        rotation, buffIcon.Bounds.Size() / 2,
                        1, effects, 0);
                }
                yOffset += 32;
            }
        }
    }
}
