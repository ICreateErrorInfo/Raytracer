namespace Raytracer
{
    class lambertain : material
    {
        public lambertain(Vektor a)
        {
            albedo = a;
        }
        public Vektor albedo;

        public override bool scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            var scatter_direction = rec.normal + Vektor.random_unit_vektor();

            //catch degenerate scatter direction
            if (scatter_direction.near_zero())
            {
                scatter_direction = rec.normal;
            }

            scattered = new ray(rec.p, scatter_direction);
            attenuation = albedo;

            zwischenSpeicher.scattered = scattered;
            zwischenSpeicher.attenuation = attenuation;

            return true;
        }
    }
}
