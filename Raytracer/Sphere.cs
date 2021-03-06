using System;

namespace Raytracer
{
    class sphere : hittable
    {
        public sphere() { }
        public sphere(Vektor cen, double r, material m)
        {
            center = cen;
            radius = r;
            mat_ptr = m;
        }
        Vektor center;
        double radius;
        material mat_ptr;

        public override zwischenSpeicher Hit(ray r, double t_min, double t_max, hit_record rec)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            Vektor oc = r.Origin - center;
            var a = r.Direction.length_squared();
            var half_b = Vektor.dot(oc, r.Direction);
            var c = oc.length_squared() - radius * radius;

            var discriminant = half_b * half_b - a * c;
            if (discriminant < 0)
            {
                zw.IsTrue = false;
                return zw;
            }
            var sqrtd = Math.Sqrt(discriminant);

            var root = (-half_b - sqrtd) / a;
            if (root < t_min || t_max < root)
            {
                root = (-half_b + sqrtd) / a;
                if (root < t_min || t_max < root)
                {
                    zw.IsTrue = false;
                    return zw;
                }
            }

            rec.t = root;
            rec.p = r.at(rec.t);
            Vektor outward_normal = (rec.p - center) / radius;
            rec.set_face_normal(r, outward_normal);
            rec.mat_ptr = mat_ptr;

            zw.rec = rec;
            zw.IsTrue = true;

            return zw;
        }

    }
}
