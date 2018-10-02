using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Renderer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BlendMode : IEquatable<BlendMode>
    {
        public static readonly BlendMode Alpha = new BlendMode(Factor.SrcAlpha, Factor.OneMinusSrcAlpha, Equation.Add, Factor.One, Factor.OneMinusSrcAlpha, Equation.Add);

        public static readonly BlendMode Add = new BlendMode(Factor.SrcAlpha, Factor.One, Equation.Add, Factor.One, Factor.One, Equation.Add);

        public static readonly BlendMode Multiply = new BlendMode(Factor.DstColor, Factor.Zero);

        public static readonly BlendMode None = new BlendMode(Factor.One, Factor.Zero);

        public BlendMode(Factor SourceFactor, Factor DestinationFactor) : this(SourceFactor, DestinationFactor, Equation.Add)
        { }

        public BlendMode(Factor SourceFactor, Factor DestinationFactor, Equation BlendEquation) : this(SourceFactor, DestinationFactor, BlendEquation, SourceFactor, DestinationFactor, BlendEquation)
        { }

        public BlendMode(Factor ColorSourceFactor, Factor ColorDestinationFactor, Equation ColorBlendEquation, Factor AlphaSourceFactor, Factor AlphaDestinationFactor, Equation AlphaBlendEquation)
        {
            ColorSrcFactor = ColorSourceFactor;
            ColorDstFactor = ColorDestinationFactor;
            ColorEquation = ColorBlendEquation;
            AlphaSrcFactor = AlphaSourceFactor;
            AlphaDstFactor = AlphaDestinationFactor;
            AlphaEquation = AlphaBlendEquation;
        }

        public static bool operator ==(BlendMode left, BlendMode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlendMode left, BlendMode right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return (obj is BlendMode) && Equals((BlendMode)obj);
        }

        public bool Equals(BlendMode other)
        {
            return (ColorSrcFactor == other.ColorSrcFactor) &&
                   (ColorDstFactor == other.ColorDstFactor) &&
                   (ColorEquation == other.ColorEquation) &&
                   (AlphaSrcFactor == other.AlphaSrcFactor) &&
                   (AlphaDstFactor == other.AlphaDstFactor) &&
                   (AlphaEquation == other.AlphaEquation);
        }

        public override int GetHashCode()
        {
            return ColorSrcFactor.GetHashCode() ^
                   ColorDstFactor.GetHashCode() ^
                   ColorEquation.GetHashCode() ^
                   AlphaSrcFactor.GetHashCode() ^
                   AlphaDstFactor.GetHashCode() ^
                   AlphaEquation.GetHashCode();
        }

        public Factor ColorSrcFactor;

        public Factor ColorDstFactor;

        public Equation ColorEquation;

        public Factor AlphaSrcFactor;

        public Factor AlphaDstFactor;

        public Equation AlphaEquation;
    }
}
