using System;

namespace Astrophysical_Console
{
    /// <summary>
    /// Double that can take values in range -180 -> 180
    /// </summary>
    public struct Angle
    {
        private double _value;

        public Angle(double value)
        {
            if (value > 180 || value < -180)
                throw new Exception("Angle valuee can only take values in range (-180 => 180)");

            _value = value;
        }

        public static Angle operator +(Angle left, Angle right)
        {
            double v1 = ((left._value + 360) + (right._value + 360)) % 360;

            return (v1 < 180) ? v1 : (v1 - 360);
        }
        public static Angle operator -(Angle left, Angle right)
        {
            return (left + -right);
        }
        public static Angle operator -(Angle a)
        {
            return -a._value;
        }
        public static implicit operator double(Angle x)
        {
            return x._value;
        }
        public static implicit operator Angle(double x)
        {
            return new Angle(x);
        }
    }
}
