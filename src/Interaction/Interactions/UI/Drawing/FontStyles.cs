using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing
{
    public enum FontStyles
    {
        Regular = 0,

        Bold = 1 << 0,

        Italic = 1 << 1,

        Underlined = 1 << 2,

        StrikeThrough = 1 << 3
    }
}
