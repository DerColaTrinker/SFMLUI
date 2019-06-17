using Pandora.SFML.Native;

namespace Pandora.Interactions.Controller
{
    public static class Keyboard
    {
        public static bool IsKeyPressed(KeyboardKey key)
        {
            return NativeSFML.sfKeyboard_isKeyPressed(key);
        }

        public static void SetVirtualKeyboardVisible(bool visible)
        {
            NativeSFML.sfKeyboard_setVirtualKeyboardVisible(visible);
        }
    }
}
