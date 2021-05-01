namespace Raytracer
{
    struct hit_record
    {
        public Vektor p;
        public Vektor normal;
        public material mat_ptr;
        public double t;
        public bool front_face;

        public void set_face_normal(ray r, Vektor outward_normal)
        {
            front_face = Vektor.dot(r.Direction, outward_normal) < 0;
            normal = front_face ? outward_normal : (outward_normal * -1);
        }
    }

    class hittable
    {
        public virtual bool Hit(ray r, double t_min, double t_max, hit_record rec)
        {
            return false; 
        }
    }

    struct zwischenSpeicher
    {
        public static hit_record rec;
        public static ray scattered;
        public static Vektor attenuation;
    }
}
