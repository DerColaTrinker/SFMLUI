using Pandora.SFML.System;
using System;
using System.Runtime.InteropServices;
using System.Security;

#pragma warning disable IDE1006 // Benennungsstile

namespace Pandora.SFML.Native
{
    internal static partial class NativeSFML
    {
        #region System

        #region Clock

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr sfClock_create();

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern void sfClock_destroy(IntPtr CPointer);

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern Time sfClock_getElapsedTime(IntPtr Clock);

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern Time sfClock_restart(IntPtr Clock);

        #endregion

        #region Time

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern Time sfSeconds(float Amount);

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern Time sfMilliseconds(int Amount);

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern Time sfMicroseconds(long Amount);

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern float sfTime_asSeconds(Time time);

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern int sfTime_asMilliseconds(Time time);

        [DllImport("csfml-system-2", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        internal static extern long sfTime_asMicroseconds(Time time);

        #endregion

        #endregion
    }
}
