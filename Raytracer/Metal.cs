namespace Raytracer
{
    class Metal : material
    {
        public Metal(Vektor a, double f)
        {
            albedo = a;
            fuzz = f < 1 ? f : 1;
        }
        public Vektor albedo;
        public double fuzz;

        public override zwischenSpeicher scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            Vektor reflected = Vektor.reflect(Vektor.unit_Vektor(r_in.Direction), rec.normal);
            scattered = new ray(rec.p, reflected + fuzz * Vektor.random_in_unit_sphere(), r_in.tm);
            attenuation = albedo;

            zw.scattered = scattered;
            zw.attenuation = attenuation;
            zw.IsTrue = (Vektor.dot(scattered.Direction, rec.normal) > 0.0);

            return zw;
        }
    }
}
