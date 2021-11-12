using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer
{
    class box : hittable
    {
        public box()
        {

        }
        public box(Vektor p0, Vektor p1, material ptr)
        {
            box_min = p0;
            box_max = p1;

            sides.Add(new xy_rect(p0.X, p1.X, p0.Y, p1.Y, p1.Z, ptr));
            sides.Add(new xy_rect(p0.X, p1.X, p0.Y, p1.Y, p0.Z, ptr));
                                 
            sides.Add(new xz_rect(p0.X, p1.X, p0.Z, p1.Z, p1.Y, ptr));
            sides.Add(new xz_rect(p0.X, p1.X, p0.Z, p1.Z, p0.Y, ptr));
                                   
            sides.Add(new yz_rect(p0.Y, p1.Y, p0.Z, p1.Z, p1.X, ptr));
            sides.Add(new yz_rect(p0.Y, p1.Y, p0.Z, p1.Z, p0.X, ptr));

        }
        public Vektor box_min;
        public Vektor box_max;
        public hittable_list sides = new hittable_list();

        public override zwischenSpeicher Hit(ray r, double t_min, double t_max, hit_record rec)
        {
            return sides.Hit(r, t_min, t_max, rec);
        }
        public override zwischenSpeicherAABB bounding_box(double time0, double time1, aabb output_box)
        {
            zwischenSpeicherAABB zw = new zwischenSpeicherAABB();
            zw.outputBox = new aabb(box_min, box_max);
            zw.isTrue = true;
            return zw;
        }
    }
}
