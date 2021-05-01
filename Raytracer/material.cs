namespace Raytracer
{
    class material
    {
        public virtual bool scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            return true;
        }
    }
}
