namespace Raytracer
{
    class lambertain : material
    {
        public lambertain(Vektor a)
        {
            albedo = a;
        }
        public Vektor albedo;

        public override zwischenSpeicher scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            var scatter_direction = rec.normal + Vektor.random_unit_vektor();

            //catch degenerate scatter direction
            if (scatter_direction.near_zero())
            {
                scatter_direction = rec.normal;
            }

            scattered = new ray(rec.p, scatter_direction);
            attenuation = albedo;

            zw.scattered = scattered;
            zw.attenuation = attenuation;
            zw.IsTrue = true;
                
            return zw;
        }
    }
}
