using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.UI.Chat;

namespace Fargowiltas.Common.Systems
{
    /// <summary>
    /// The toggle's header in the display. <para/>
    /// If the effect shouldn't have a toggle, set this to null.
    /// </summary>
    public class PotionToggle(int itemID, int buffID)
    {
        public int ItemID = itemID;
        public int BuffID = buffID;

        public bool ToggleBool = true;

        public override string ToString() => $"Item ID: {ItemID}, Buff ID: {BuffID}";

        public string GetRawToggleName()
        {
            string baseText = Lang.GetBuffName(BuffID);
            List<TextSnippet> parsedText = ChatManager.ParseMessage(baseText, Color.White);
            string rawText = "";

            foreach (TextSnippet snippet in parsedText)
            {
                if (!snippet.Text.StartsWith("["))
                {
                    rawText += snippet.Text.Trim();
                }
            }

            return rawText;
        }

    }
}
