namespace Raytracer
{
    public class material
    {
        public virtual Vektor emitted(double u, double v, Vektor p)
        {
            return new Vektor(0,0,0);
        }
        public virtual zwischenSpeicher scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            return zw;
        }
    }
}
