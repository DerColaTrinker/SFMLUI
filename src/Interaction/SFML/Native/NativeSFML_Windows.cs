using Pandora.Interactions.Controller;
using Pandora.Interactions.Dispatcher;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing.Shader;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static Pandora.Interactions.UI.Drawing.Cursor;

#pragma warning disable IDE1006 // Benennungsstile

namespace Pandora.SFML.Native
{
    internal static partial class NativeSFML
    {
        #region Windows

        #region Mouse

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern bool sfMouse_isButtonPressed(MouseButton button);

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern Vector2 sfMouse_getPosition(IntPtr relativeTo);

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]

        internal static extern void sfMouse_setPosition(Vector2 position, IntPtr relativeTo);
        #endregion

        #region Keyboard

        #region Imports
        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern bool sfKeyboard_isKeyPressed(KeyboardKey Key);

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern void sfKeyboard_setVirtualKeyboardVisible(bool visible);
        #endregion

        #endregion

        #region Events

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern bool sfWindow_pollEvent(IntPtr CPointer, out Event Evt);

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern bool sfWindow_waitEvent(IntPtr CPointer, out Event Evt);

        #endregion

        #endregion

        #region Cursor

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr sfCursor_createFromSystem(CursorType type);

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr sfCursor_createFromPixels(IntPtr pixels, Vector2U size, Vector2U hotspot);

        [DllImport("csfml-window-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern void sfCursor_destroy(IntPtr CPointer);

        #endregion
    }
}
