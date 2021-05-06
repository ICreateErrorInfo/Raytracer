using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer
{
    class moving_sphere : hittable
    {
        public moving_sphere()
        {

        }
        public moving_sphere(Vektor cen0, Vektor cen1, double _time0, double _time1, double r, material m)
        {
            center0 = cen0;
            center1 = cen1;
            time0 = _time0;
            time1 = _time1;
            radius = r;
            mat_ptr = m;
        }
        public Vektor center0;
        public Vektor center1;
        public double time0;
        public double time1;
        public double radius;
        public material mat_ptr;

        public override zwischenSpeicher Hit(ray r, double t_min, double t_max, hit_record rec)
        {
            zwischenSpeicher zw = new zwischenSpeicher();

            Vektor oc = r.Origin - center(r.tm);
            var a = r.Direction.length_squared();
            var half_b = Vektor.dot(oc, r.Direction);
            var c = oc.length_squared() - radius * radius;

            var discriminant = half_b * half_b - a * c;
            if(discriminant < 0)
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
            Vektor outward_normal = (rec.p - center(r.tm)) / radius;
            rec.set_face_normal(r, outward_normal);
            rec.mat_ptr = mat_ptr;

            zw.rec = rec;
            zw.IsTrue = true;

            return zw;
        }
        public virtual Vektor center(double time)
        {
            return center0 + ((time - time0) / (time1 - time0)) * (center1 - center0);
        }
    }
}
