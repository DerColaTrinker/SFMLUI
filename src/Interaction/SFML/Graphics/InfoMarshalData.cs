using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.SFML.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct InfoMarshalData
    {
        public IntPtr Family;
    }
}
