namespace Raytracer
{
    public struct hit_record
    {
        public Vektor p;
        public Vektor normal;
        public material mat_ptr;
        public double t;
        public double u;
        public double v;
        public bool front_face;

        public void set_face_normal(ray r, Vektor outward_normal)
        {
            front_face = Vektor.dot(r.Direction, outward_normal) < 0;
            normal = front_face ? outward_normal : (outward_normal * -1);
        }
    }

    public class hittable
    {
        public virtual zwischenSpeicher Hit(ray r, double t_min, double t_max, hit_record rec)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            return zw; 
        }
        public virtual zwischenSpeicherAABB bounding_box(double time0, double time1, aabb output_box)
        {
            zwischenSpeicherAABB zw = new zwischenSpeicherAABB();
            return zw;
        }
    }

    public struct zwischenSpeicher
    {
        public bool IsTrue;
        public hit_record rec;
        public ray scattered;
        public Vektor attenuation;
    }
}
