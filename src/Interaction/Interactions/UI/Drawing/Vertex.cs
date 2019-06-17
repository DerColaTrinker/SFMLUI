using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vertex(Vector2F position) : this(position, Color.White, new Vector2F(0, 0))
        { }

        public Vertex(Vector2F position, Color color) : this(position, color, new Vector2F(0, 0))
        { }

        public Vertex(Vector2F position, Vector2F texCoords) : this(position, Color.White, texCoords)
        { }

        public Vertex(Vector2F position, Color color, Vector2F texCoords)
        {
            Position = position;
            Color = color;
            TexCoords = texCoords;
        }

        public override string ToString()
        {
            return "[Vertex]" + " Position(" + Position + ")" + " Color(" + Color + ")" + " TexCoords(" + TexCoords + ")";
        }

        public Vector2F Position;

        public Color Color;

        public Vector2F TexCoords;
    }
}
