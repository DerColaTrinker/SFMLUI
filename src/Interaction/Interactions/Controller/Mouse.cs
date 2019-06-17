using Pandora.Interactions.UI;
using Pandora.Interactions.UI.Drawing;
using Pandora.SFML.Native;
using System;

namespace Pandora.Interactions.Controller
{
    public static class Mouse
    {
        public static bool IsButtonPressed(MouseButton button)
        {
            return NativeSFML.sfMouse_isButtonPressed(button);
        }

        public static Vector2 GetPosition()
        {
            return GetPosition(null);
        }

        public static Vector2 GetPosition(SceneHandler relativeTo)
        {
            if (relativeTo != null)
                return NativeSFML.sfMouse_getPosition(relativeTo.Pointer);
            else
                return NativeSFML.sfMouse_getPosition(IntPtr.Zero);
        }

        public static void SetPosition(Vector2 position)
        {
            SetPosition(position, null);
        }

        public static void SetPosition(Vector2 position, SceneHandler relativeTo)
        {
            if (relativeTo != null)
                NativeSFML.sfMouse_setPosition(position, relativeTo.Pointer);
            else
                NativeSFML.sfMouse_setPosition(position, IntPtr.Zero);
        }
    }
}
