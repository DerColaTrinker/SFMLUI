using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.SFML.Graphics
{
    [Flags]
    public enum Attribute
    {
        Default = 0,

        Core = 1 << 0,

        Debug = 1 << 2
    }
}
