using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.SFML
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long ReadCallbackType(IntPtr data, long size, IntPtr userData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long SeekCallbackType(long position, IntPtr userData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long TellCallbackType(IntPtr userData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long GetSizeCallbackType(IntPtr userData);

    [StructLayout(LayoutKind.Sequential)]
    internal struct InputStream
    {
        public ReadCallbackType Read;

        public SeekCallbackType Seek;

        public TellCallbackType Tell;

        public GetSizeCallbackType GetSize;
    }
}
