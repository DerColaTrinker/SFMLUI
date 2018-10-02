using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.SFML.System
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Time : IEquatable<Time>
    {
        private long _microseconds;

        public static readonly Time Zero = FromMicroseconds(0);

        public static Time FromSeconds(float seconds)
        {
            return NativeSFML.sfSeconds(seconds);
        }

        public static Time FromMilliseconds(int milliseconds)
        {
            return NativeSFML.sfMilliseconds(milliseconds);
        }

        public static Time FromMicroseconds(long microseconds)
        {
            return NativeSFML.sfMicroseconds(microseconds);
        }

        public float AsSeconds()
        {
            return NativeSFML.sfTime_asSeconds(this);
        }

        public int AsMilliseconds()
        {
            return NativeSFML.sfTime_asMilliseconds(this);
        }

        public long AsMicroseconds()
        {
            return NativeSFML.sfTime_asMicroseconds(this);
        }

        public static bool operator ==(Time left, Time right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Time left, Time right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return (obj is Time) && Equals((Time)obj);
        }

        public bool Equals(Time other)
        {
            return _microseconds == other._microseconds;
        }

        public static bool operator <(Time left, Time right)
        {
            return left.AsMicroseconds() < right.AsMicroseconds();
        }

        public static bool operator <=(Time left, Time right)
        {
            return left.AsMicroseconds() <= right.AsMicroseconds();
        }

        public static bool operator >(Time left, Time right)
        {
            return left.AsMicroseconds() > right.AsMicroseconds();
        }

        public static bool operator >=(Time left, Time right)
        {
            return left.AsMicroseconds() >= right.AsMicroseconds();
        }

        public static Time operator -(Time left, Time right)
        {
            return FromMicroseconds(left.AsMicroseconds() - right.AsMicroseconds());
        }

        public static Time operator +(Time left, Time right)
        {
            return FromMicroseconds(left.AsMicroseconds() + right.AsMicroseconds());
        }

        public static Time operator *(Time left, float right)
        {
            return FromSeconds(left.AsSeconds() * right);
        }

        public static Time operator *(Time left, long right)
        {
            return FromMicroseconds(left.AsMicroseconds() * right);
        }

        public static Time operator *(float left, Time right)
        {
            return FromSeconds(left * right.AsSeconds());
        }

        public static Time operator *(long left, Time right)
        {
            return FromMicroseconds(left * right.AsMicroseconds());
        }

        public static Time operator /(Time left, Time right)
        {
            return FromMicroseconds(left.AsMicroseconds() / right.AsMicroseconds());
        }

        public static Time operator /(Time left, float right)
        {
            return FromSeconds(left.AsSeconds() / right);
        }

        public static Time operator /(Time left, long right)
        {
            return FromMicroseconds(left.AsMicroseconds() / right);
        }

        public static Time operator %(Time left, Time right)
        {
            return FromMicroseconds(left.AsMicroseconds() % right.AsMicroseconds());
        }

        public override int GetHashCode()
        {
            return _microseconds.GetHashCode();
        }
    }

}
