using System;

namespace Pandora.Interactions.Dispatcher
{
    internal enum EventType
    {
        Closed,

        Resized,

        LostFocus,

        GainedFocus,

        TextEntered,

        KeyPressed,

        KeyReleased,

        [Obsolete("MouseWheelMoved is deprecated, please use MouseWheelScrolled instead")]
        MouseWheelMoved,

        MouseWheelScrolled,

        MouseButtonPressed,

        MouseButtonReleased,

        MouseMoved,

        MouseEntered,

        MouseLeft,

        JoystickButtonPressed,

        JoystickButtonReleased,

        JoystickMoved,

        JoystickConnected,

        JoystickDisconnected,

        TouchBegan,

        TouchMoved,

        TouchEnded,

        SensorChanged
    }
}
