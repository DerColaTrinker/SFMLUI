using System;

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
