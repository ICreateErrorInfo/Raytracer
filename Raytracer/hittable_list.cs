using System.Collections.Generic;

namespace Raytracer
{
    class hittable_list : hittable
    {
        public hittable_list()
        {

        }
        public void Add(hittable obj)
        {
            objects.Add(obj);
        }

        private List<hittable> objects = new List<hittable>();

        public override zwischenSpeicher Hit(ray r, double t_min, double t_max, hit_record rec)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            hit_record temp_rec = new hit_record();
            bool hit_anything = false;
            var closest_so_far = t_max;

            foreach (var Object in objects)
            {
                zwischenSpeicher zw1 = Object.Hit(r, t_min, closest_so_far, temp_rec);
                if (zw1.IsTrue)
                {
                    temp_rec = zw1.rec;
                    hit_anything = true;
                    closest_so_far = temp_rec.t;
                    rec = temp_rec;
                }
            }

            zw.rec = rec;
            zw.IsTrue = hit_anything;

            return zw;
        }
    }
}
