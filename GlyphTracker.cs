using Fargowiltas.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargowiltas
{
    public class GlyphTracker
    {
        internal bool GlyphsFinalized = false;

        public GlyphTracker()
        {
            Fargowiltas.glyphTracker = this;
            AddGlyphPath(Fargowiltas.Instance.Name, $"{Fargowiltas.Instance.Name}/Assets/Glyphs");
        }

        internal void FinalizeGlyphs()
        {
            GlyphsFinalized = true;
        }

        public void AddGlyphPath(string modName, string filePath)
        {
            if (!GlyphsFinalized)
                GlyphPathRegistry.Register(modName, filePath);
        }
    }
}
