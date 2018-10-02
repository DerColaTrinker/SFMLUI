using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Mat3
    {
        public Mat3(float a00, float a01, float a02,
                    float a10, float a11, float a12,
                    float a20, float a21, float a22)
        {
            fixed (float* m = array)
            {
                m[0] = a00; m[3] = a01; m[6] = a02;
                m[1] = a10; m[4] = a11; m[7] = a12;
                m[2] = a20; m[5] = a21; m[8] = a22;
            }
        }

        public Mat3(Transform transform)
        {
            fixed (float* m = array)
            {
                m[0] = transform.m00; m[3] = transform.m01; m[6] = transform.m02;
                m[1] = transform.m10; m[4] = transform.m11; m[7] = transform.m12;
                m[2] = transform.m20; m[5] = transform.m21; m[8] = transform.m22;
            }
        }

        private fixed float array[3 * 3];
    }
}
