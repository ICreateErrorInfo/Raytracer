﻿namespace Raytracer
{
    class material
    {
        public virtual zwischenSpeicher scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            return zw;
        }
    }
}
