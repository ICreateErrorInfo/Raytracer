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

        public override bool Hit(ray r, double t_min, double t_max, hit_record rec)
        {
            hit_record temp_rec = new hit_record();
            bool hit_anything = false;
            var closest_so_far = t_max;

            foreach (var Object in objects)
            {
                if (Object.Hit(r, t_min, closest_so_far, temp_rec))
                {
                    temp_rec = zwischenSpeicher.rec;
                    hit_anything = true;
                    closest_so_far = temp_rec.t;
                    rec = temp_rec;
                }
            }

            zwischenSpeicher.rec = rec;
            return hit_anything;
        }
    }
}
