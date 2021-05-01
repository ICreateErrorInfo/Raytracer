using Projection;
using System;

namespace Raytracer
{
    class ray
    {
        public ray()
        {

        }
        public ray(Vektor origin, Vektor direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vektor Origin;
        public Vektor Direction;

        public Vektor at(double t)
        {
            return Origin + t * Direction;
        }
        public static double hit_sphere(Vektor center, double radius, ray r)
        {
            Vektor oc = r.Origin - center;
            var a = r.Direction.length_squared();
            var half_b = Vektor.dot(oc, r.Direction);
            var c = oc.length_squared() - radius * radius;
            var discriminant = half_b * half_b - a * c;

            if (discriminant < 0)
            {
                return -1;
            }
            else
            {
                return (-half_b - Math.Sqrt(discriminant)) / a;
            }
        }
        public static Vektor ray_color(ray r, hittable world, int depth)
        {
            hit_record rec = new hit_record();

            if (depth <= 0)
            {
                return new Vektor(0, 0, 0);
            }

            zwischenSpeicher zw = world.Hit(r, 0.0001, Mathe.infinity, rec);
            if (zw.IsTrue)
            {
                rec = zw.rec;
                ray scattered = new ray();
                Vektor attenuation = new Vektor();
                zwischenSpeicher zw1 = rec.mat_ptr.scatter(r, rec, attenuation, scattered);

                if (zw1.IsTrue)
                {
                    attenuation = zw1.attenuation;
                    scattered = zw1.scattered;

                    return attenuation * ray_color(scattered, world, depth - 1);
                }
                return new Vektor(0, 0, 0);
            }

            Vektor unit_direction = Vektor.unit_Vektor(r.Direction);
            var t = 0.5 * (unit_direction.Y + 1);
            Vektor col = (1 - t) * new Vektor(1, 1, 1) + t * new Vektor(0.5, 0.7, 1);

            return col;
        }
    }
}
