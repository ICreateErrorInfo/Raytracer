namespace Raytracer
{
    class lambertian : material
    {
        public lambertian(Vektor a)
        {
            albedo = new solid_color(a);
        }
        public lambertian(Texture a)
        {
            albedo = a;
        }
        public Texture albedo;

        public override zwischenSpeicher scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            var scatter_direction = rec.normal + Vektor.random_unit_vektor();

            //catch degenerate scatter direction
            if (scatter_direction.near_zero())
            {
                scatter_direction = rec.normal;
            }

            scattered = new ray(rec.p, scatter_direction, r_in.tm);

            zw.scattered = scattered;
            zw.attenuation = albedo.value(rec.u, rec.v, rec.p);
            zw.IsTrue = true;
                
            return zw;
        }
    }
}
