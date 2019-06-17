using System;

namespace Pandora.Interactions.UI.Renderer
{
    [Flags]
    public enum WindowStyle
    {
        None = 0,
        Titlebar = 1 << 0,
        Resize = 1 << 1,
        Close = 1 << 2,
        Fullscreen = 1 << 3,
        Default = Titlebar | Resize | Close
    }
}
