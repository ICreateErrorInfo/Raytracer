using Projection;
using System;

namespace Raytracer
{
    public class ray
    {
        public ray()
        {

        }
        public ray(Vektor origin, Vektor direction, double time = 0)
        {
            Origin = origin;
            Direction = direction;
            tm = time;
        }

        public Vektor Origin;
        public Vektor Direction;
        public double tm;

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
        public static Vektor ray_color(ray r, Vektor background, hittable world, int depth)
        {
            hit_record rec = new hit_record();

            if (depth <= 0)
            {
                return new Vektor(0, 0, 0);
            }

            zwischenSpeicher zw = world.Hit(r, 0.0001, Mathe.infinity, rec);
            if (!zw.IsTrue)
            {
                return background;
            }
            ray scattered = new ray();
            Vektor attenuation = new Vektor();
            Vektor emitted = zw.rec.mat_ptr.emitted(zw.rec.u, zw.rec.v, zw.rec.p);

            zw = zw.rec.mat_ptr.scatter(r, zw.rec, attenuation, scattered);
            if (!zw.IsTrue)
            {
                return emitted;
            }

            return emitted + zw.attenuation * ray_color(zw.scattered, background, world, depth - 1);
        }
    }
}
