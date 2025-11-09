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
            InitializeCustomGlyphs();
        }

        private List<string> CustomGlyphs = new()
        {
            "PotionToggler",
            "StatSheet"
        };

        private void InitializeCustomGlyphs()
        {
            string modName = Fargowiltas.Instance.Name;
            foreach (var glyph in CustomGlyphs)
            {
                string shorthand = $"{modName}/{glyph}";
                string fileName = $"{modName}/Assets/Glyphs/{glyph}";
                GlyphRegistry.Register(shorthand, fileName);
            }
        }

        internal void FinalizeGlyphs()
        {
            GlyphsFinalized = true;
        }

        public void AddGlyph(string key, string fileName)
        {
            if (!GlyphsFinalized)
                GlyphRegistry.Register(key, fileName);
        }
    }
}
