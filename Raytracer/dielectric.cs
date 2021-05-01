using System;

namespace Raytracer
{
    class dielectric : material
    {
        public dielectric(double index_of_refraction)
        {
            ir = index_of_refraction;
        }
        public double ir;

        public override zwischenSpeicher scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            attenuation = new Vektor(1, 1, 1);
            double refraction_ratio = rec.front_face ? (1 / ir) : ir;

            Vektor unit_direction = Vektor.unit_Vektor(r_in.Direction);
            double cos_theta = Math.Min(Vektor.dot(unit_direction * -1, rec.normal), 1);
            double sin_theta = Math.Sqrt(1 - cos_theta * cos_theta);

            bool cannot_refract = refraction_ratio * sin_theta > 1;
            Vektor direction;

            if (cannot_refract)
            {
                direction = Vektor.reflect(unit_direction, rec.normal);
            }
            else
            {
                direction = Vektor.refract(unit_direction, rec.normal, refraction_ratio);
            }

            scattered = new ray(rec.p, direction);

            zw.attenuation = attenuation;
            zw.scattered = scattered;
            zw.IsTrue = true;

            return zw;
        }
    }
}
