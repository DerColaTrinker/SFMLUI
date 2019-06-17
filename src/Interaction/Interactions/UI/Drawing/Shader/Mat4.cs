using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Mat4
    {
        public static Mat4 Identity
        {
            get
            {
                return new Mat4(1, 0, 0, 0,
                                0, 1, 0, 0,
                                0, 0, 1, 0,
                                0, 0, 0, 1);
            }
        }

        public Mat4(float a00, float a01, float a02, float a03,
                    float a10, float a11, float a12, float a13,
                    float a20, float a21, float a22, float a23,
                    float a30, float a31, float a32, float a33)
        {
            fixed (float* m = array)
            {
                // transpose to column major
                m[0] = a00; m[4] = a01; m[8] = a02; m[12] = a03;
                m[1] = a10; m[5] = a11; m[9] = a12; m[13] = a13;
                m[2] = a20; m[6] = a21; m[10] = a22; m[14] = a23;
                m[3] = a30; m[7] = a31; m[11] = a32; m[15] = a33;
            }
        }

        public Mat4(Transform transform)
        {
            fixed (float* m = array)
            {
                // swapping to column-major (OpenGL) from row-major (SFML) order
                // in addition, filling in the blanks (from expanding to a mat4) with values from
                // an identity matrix
                m[0] = transform.m00; m[4] = transform.m01; m[8] = 0; m[12] = transform.m02;
                m[1] = transform.m10; m[5] = transform.m11; m[9] = 0; m[13] = transform.m12;
                m[2] = 0; m[6] = 0; m[10] = 1; m[14] = 0;
                m[3] = transform.m20; m[7] = transform.m21; m[11] = 0; m[15] = transform.m22;
            }
        }

        // column major!
        private fixed float array[4 * 4];
    }
}
