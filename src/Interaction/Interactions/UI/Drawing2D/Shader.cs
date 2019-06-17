using Pandora.Interactions.UI.Drawing.Shader;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML;
using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pandora.Interactions.UI.Drawing2D
{
    public class Shader : ObjectPointer
    {
        public class CurrentTextureType { }

        public static readonly CurrentTextureType CurrentTexture = null;

        public Shader(IntPtr ptr) : base(ptr)
        { }

        public Shader(string vertexShaderFilename, string geometryShaderFilename, string fragmentShaderFilename) : base(NativeSFML.sfShader_createFromFile(vertexShaderFilename, geometryShaderFilename, fragmentShaderFilename))
        {
            if (Pointer == IntPtr.Zero)
                throw new Exception("shader : " + vertexShaderFilename + " " + fragmentShaderFilename);
        }

        public Shader(Stream vertexShaderStream, Stream geometryShaderStream, Stream fragmentShaderStream) : base(IntPtr.Zero)
        {
            // using these funky conditional operators because StreamAdaptor doesn't have some method for dealing with
            // its constructor argument being null
            using (StreamAdaptor vertexAdaptor = vertexShaderStream != null ? new StreamAdaptor(vertexShaderStream) : null,
                                 geometryAdaptor = geometryShaderStream != null ? new StreamAdaptor(geometryShaderStream) : null,
                                 fragmentAdaptor = fragmentShaderStream != null ? new StreamAdaptor(fragmentShaderStream) : null)
            {
                Pointer = NativeSFML.sfShader_createFromStream(vertexAdaptor != null ? vertexAdaptor.InputStreamPtr : IntPtr.Zero,
                                                     geometryAdaptor != null ? geometryAdaptor.InputStreamPtr : IntPtr.Zero,
                                                     fragmentAdaptor != null ? fragmentAdaptor.InputStreamPtr : IntPtr.Zero);
            }

            if (Pointer == IntPtr.Zero)
                throw new Exception("shader");
        }

        public uint NativeHandle
        {
            get { return NativeSFML.sfShader_getNativeHandle(Pointer); }
        }

        public static Shader FromString(string vertexShader, string geometryShader, string fragmentShader)
        {
            IntPtr ptr = NativeSFML.sfShader_createFromMemory(vertexShader, geometryShader, fragmentShader);
            if (ptr == IntPtr.Zero)
                throw new Exception("shader");

            return new Shader(ptr);
        }

        public void SetUniform(string name, float x)
        {
            NativeSFML.sfShader_setFloatUniform(Pointer, name, x);
        }

        public void SetUniform(string name, Vec2 vector)
        {
            NativeSFML.sfShader_setVec2Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, Vec3 vector)
        {
            NativeSFML.sfShader_setVec3Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, Vec4 vector)
        {
            NativeSFML.sfShader_setVec4Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, int x)
        {
            NativeSFML.sfShader_setIntUniform(Pointer, name, x);
        }

        public void SetUniform(string name, IVec2 vector)
        {
            NativeSFML.sfShader_setIvec2Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, IVec3 vector)
        {
            NativeSFML.sfShader_setIvec3Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, IVec4 vector)
        {
            NativeSFML.sfShader_setIvec4Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, bool x)
        {
            NativeSFML.sfShader_setBoolUniform(Pointer, name, x);
        }

        public void SetUniform(string name, BVec2 vector)
        {
            NativeSFML.sfShader_setBvec2Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, BVec3 vector)
        {
            NativeSFML.sfShader_setBvec3Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, BVec4 vector)
        {
            NativeSFML.sfShader_setBvec4Uniform(Pointer, name, vector);
        }

        public void SetUniform(string name, Mat3 matrix)
        {
            NativeSFML.sfShader_setMat3Uniform(Pointer, name, matrix);
        }

        public void SetUniform(string name, Mat4 matrix)
        {
            NativeSFML.sfShader_setMat4Uniform(Pointer, name, matrix);
        }

        public void SetUniform(string name, Texture texture)
        {
            // Keep a reference to the Texture so it doesn't get GC'd
            _textures[name] = texture;
            NativeSFML.sfShader_setTextureUniform(Pointer, name, texture.Pointer);
        }

        public void SetUniform(string name, CurrentTextureType current)
        {
            NativeSFML.sfShader_setCurrentTextureUniform(Pointer, name);
        }

        public unsafe void SetUniformArray(string name, float[] array)
        {
            fixed (float* data = array)
            {
                NativeSFML.sfShader_setFloatUniformArray(Pointer, name, data, (uint)array.Length);
            }
        }

        public unsafe void SetUniformArray(string name, Vec2[] array)
        {
            fixed (Vec2* data = array)
            {
                NativeSFML.sfShader_setVec2UniformArray(Pointer, name, data, (uint)array.Length);
            }
        }

        public unsafe void SetUniformArray(string name, Vec3[] array)
        {
            fixed (Vec3* data = array)
            {
                NativeSFML.sfShader_setVec3UniformArray(Pointer, name, data, (uint)array.Length);
            }
        }

        public unsafe void SetUniformArray(string name, Vec4[] array)
        {
            fixed (Vec4* data = array)
            {
                NativeSFML.sfShader_setVec4UniformArray(Pointer, name, data, (uint)array.Length);
            }
        }

        public unsafe void SetUniformArray(string name, Mat3[] array)
        {
            fixed (Mat3* data = array)
            {
                NativeSFML.sfShader_setMat3UniformArray(Pointer, name, data, (uint)array.Length);
            }
        }

        public unsafe void SetUniformArray(string name, Mat4[] array)
        {
            fixed (Mat4* data = array)
            {
                NativeSFML.sfShader_setMat4UniformArray(Pointer, name, data, (uint)array.Length);
            }
        }

        public static void Bind(Shader shader)
        {
            NativeSFML.sfShader_bind(shader != null ? shader.Pointer : IntPtr.Zero);
        }

        public static bool IsAvailable
        {
            get { return NativeSFML.sfShader_isAvailable(); }
        }

        public static bool IsGeometryAvailable
        {
            get { return NativeSFML.sfShader_isGeometryAvailable(); }
        }

        public override string ToString()
        {
            return "[Shader]";
        }

        protected override void Destroy(bool disposing)
        {
            if (!disposing)
                Context.Global.SetActive(true);

            _textures.Clear();
            NativeSFML.sfShader_destroy(Pointer);

            if (!disposing)
                Context.Global.SetActive(false);
        }

        // Keeps references to used Textures for GC prevention during use
        private Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
    }
}
