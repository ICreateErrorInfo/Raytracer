using Projection;
using System;
using System.Drawing;

namespace Raytracer
{
    public class Vektor
    {
        public Vektor()
        {

        }
        public Vektor(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vektor(double d)
        {
            X = d;
            Y = d;
            Z = d;
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public static Vektor operator +(Vektor v1, Vektor v2)
        {
            return new Vektor(v1.X + v2.X,
                              v1.Y + v2.Y,
                              v1.Z + v2.Z);
        }
        public static Vektor operator -(Vektor v1, Vektor v2)
        {
            return new Vektor(v1.X - v2.X,
                              v1.Y - v2.Y,
                              v1.Z - v2.Z);
        }
        public static Vektor operator *(Vektor v1, Vektor v2)
        {
            return new Vektor(v1.X * v2.X,
                              v1.Y * v2.Y,
                              v1.Z * v2.Z);
        }
        public static Vektor operator /(Vektor v1, Vektor v2)
        {
            return new Vektor(v1.X / v2.X,
                              v1.Y / v2.Y,
                              v1.Z / v2.Z);
        }

        public static Vektor operator +(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            return new Vektor(v1.X + v2.X,
                              v1.Y + v2.Y,
                              v1.Z + v2.Z);
        }
        public static Vektor operator -(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            return new Vektor(v1.X - v2.X,
                              v1.Y - v2.Y,
                              v1.Z - v2.Z);
        }
        public static Vektor operator *(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            return new Vektor(v1.X * v2.X,
                              v1.Y * v2.Y,
                              v1.Z * v2.Z);
        }
        public static Vektor operator /(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            return new Vektor(v1.X / v2.X,
                              v1.Y / v2.Y,
                              v1.Z / v2.Z);
        }

        public static Vektor operator +(double d2, Vektor v1)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            return new Vektor(v1.X + v2.X,
                              v1.Y + v2.Y,
                              v1.Z + v2.Z);
        }
        public static Vektor operator -(double d2, Vektor v1)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            return new Vektor(v1.X - v2.X,
                              v1.Y - v2.Y,
                              v1.Z - v2.Z);
        }
        public static Vektor operator *(double d2, Vektor v1)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            return new Vektor(v1.X * v2.X,
                              v1.Y * v2.Y,
                              v1.Z * v2.Z);
        }
        public static Vektor operator /(double d2, Vektor v1)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            return new Vektor(v1.X / v2.X,
                              v1.Y / v2.Y,
                              v1.Z / v2.Z);
        }

        public static bool operator >(Vektor v1, Vektor v2)
        {
            if (v1.X > v2.X && v1.Y > v2.Y && v1.Z > v2.Z)
            {
                return true;
            }
            return false;
        }
        public static bool operator <(Vektor v1, Vektor v2)
        {
            if (v1.X < v2.X && v1.Y < v2.Y && v1.Z < v2.Z)
            {
                return true;
            }
            return false;
        }
        public static bool operator ==(Vektor v1, Vektor v2)
        {
            if (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(Vektor v1, Vektor v2)
        {
            if (v1.X != v2.X && v1.Y != v2.Y && v1.Z != v2.Z)
            {
                return true;
            }
            return false;
        }
        public static bool operator >=(Vektor v1, Vektor v2)
        {
            if (v1 > v2 || v1 == v2)
            {
                return true;
            }
            return false;
        }
        public static bool operator <=(Vektor v1, Vektor v2)
        {
            if (v1 < v2 || v1 == v2)
            {
                return true;
            }
            return false;
        }

        public static bool operator >(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            if (v1.X > v2.X && v1.Y > v2.Y && v1.Z > v2.Z)
            {
                return true;
            }
            return false;
        }
        public static bool operator <(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            if (v1.X < v2.X && v1.Y < v2.Y && v1.Z < v2.Z)
            {
                return true;
            }
            return false;
        }
        public static bool operator ==(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            if (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            if (v1.X != v2.X && v1.Y != v2.Y && v1.Z != v2.Z)
            {
                return true;
            }
            return false;
        }
        public static bool operator >=(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            if (v1 > v2 || v1 == v2)
            {
                return true;
            }
            return false;
        }
        public static bool operator <=(Vektor v1, double d2)
        {
            Vektor v2 = new Vektor(d2, d2, d2);
            if (v1 < v2 || v1 == v2)
            {
                return true;
            }
            return false;
        }

        public double length()
        {
            return Math.Sqrt(length_squared());
        }
        public double length_squared()
        {
            return X * X + Y * Y + Z * Z;
        }
        public static double dot(Vektor v, Vektor v1)
        {
            return v.X * v1.X
                 + v.Y * v1.Y
                 + v.Z * v1.Z;
        }
        public static Vektor cross(Vektor u, Vektor v)
        {
            return new Vektor(u.Y * v.Z - u.Z * v.Y,
                              u.Z * v.X - u.X * v.Z,
                              u.X * v.Y - u.Y * v.X);
        }
        public static Vektor unit_Vektor(Vektor v)
        {
            return v / v.length();
        }
        public static Vektor random()
        {
            return new Vektor(Mathe.random_1Tom1(), Mathe.random_1Tom1(), Mathe.random_1Tom1());
        }
        public static Vektor random_in_unit_sphere()
        {
            while (true)
            {
                var p = Vektor.random();
                if (p.length_squared() >= 1) continue;
                return p;
            }
        }
        public static Vektor random_unit_vektor()
        {
            return unit_Vektor(random_in_unit_sphere());
        }
        public bool near_zero()
        {
            var s = 1e-8;
            return (Math.Abs(X) < s) && (Math.Abs(Y) < s) && (Math.Abs(Z) < s);
        }
        public static Vektor reflect(Vektor v, Vektor n)
        {
            return v - 2 * dot(v, n) * n;
        }
        public static Vektor refract(Vektor uv, Vektor n, double etai_over_etat)
        {
            var cos_theta = Math.Min(Vektor.dot(uv * -1, n), 1);
            Vektor r_out_perp = etai_over_etat * (uv + cos_theta * n);
            Vektor r_out_parallel = -Math.Sqrt(Math.Abs(1 - r_out_perp.length_squared())) * n;
            return r_out_perp + r_out_parallel;

        }
        public static Vektor random_in_unit_disk()
        {
            while (true)
            {
                var p = new Vektor(Mathe.random_1Tom1(), Mathe.random_1Tom1(), 0);
                if (p.length_squared() >= 1) continue;
                return p;
            }
        }

        public static Color toColor(Vektor pixel_color, int samples_per_pixel)
        {
            var r = pixel_color.X;
            var g = pixel_color.Y;
            var b = pixel_color.Z;

            var scale = (double)1 / (double)samples_per_pixel;

            r = Math.Sqrt(scale * r);
            g = Math.Sqrt(scale * g);
            b = Math.Sqrt(scale * b);

            return Color.FromArgb(Convert.ToInt32(255 * Mathe.clamp(r, 0, 0.999)),
                                  Convert.ToInt32(255 * Mathe.clamp(g, 0, 0.999)),
                                  Convert.ToInt32(255 * Mathe.clamp(b, 0, 0.999)));
        }

        public override string ToString() => $"({X:F4}, {Y:F4}, {Z:F4})";

    }
}
