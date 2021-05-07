using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer
{
    class xy_rect : hittable
    {
        public xy_rect()
        {

        }
        public xy_rect(double _x0, double _x1, double _y0, double _y1, double _k, material mat)
        {
            x0 = _x0;
            x1 = _x1;
            y0 = _y0;
            y1 = _y1;
            k = _k;
            mp = mat;
        }
        public double x0, x1, y0, y1, k;
        public material mp;
        public override zwischenSpeicher Hit(ray r, double t_min, double t_max, hit_record rec)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            var t = (k - r.Origin.Z) / r.Direction.Z;
            if(t < t_min || t > t_max)
            {
                zw.IsTrue = false;
                return zw;
            }

            var x = r.Origin.X + t * r.Direction.X;
            var y = r.Origin.Y + t * r.Direction.Y;
            if(x < x0 || x > x1 || y < y0 || y > y1)
            {
                zw.IsTrue = false;
                return zw;
            }

            zw.rec.u = (x - x0) / (x1 - x0);
            zw.rec.v = (y - y0) / (y1 - y0);
            rec.t = t;
            var outward_normal = new Vektor(0,0,1);
            zw.rec.set_face_normal(r, outward_normal);
            zw.rec.mat_ptr = mp;
            zw.rec.p = r.at(t);
            zw.IsTrue = true;
            return zw;
        }
        public override zwischenSpeicherAABB bounding_box(double time0, double time1, aabb output_box)
        {
            return base.bounding_box(time0, time1, output_box);
        }
    }
}
