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

        public override bool scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            Vektor reflected = Vektor.reflect(Vektor.unit_Vektor(r_in.Direction), rec.normal);
            scattered = new ray(rec.p, reflected + fuzz * Vektor.random_in_unit_sphere());
            attenuation = albedo;

            zwischenSpeicher.scattered = scattered;
            zwischenSpeicher.attenuation = attenuation;

            return (Vektor.dot(scattered.Direction, rec.normal) > 0.0);
        }
    }
}
