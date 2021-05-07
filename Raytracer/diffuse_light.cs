using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer
{
    class diffuse_light : material
    {
        public diffuse_light(Texture a)
        {
            emit = a;
        }
        public diffuse_light(Vektor c)
        {
            emit = new solid_color(c);
        }
        public Texture emit;

        public override zwischenSpeicher scatter(ray r_in, hit_record rec, Vektor attenuation, ray scattered)
        {
            zwischenSpeicher zw = new zwischenSpeicher();
            zw.IsTrue = false;
            return zw;
        }

        public override Vektor emitted(double u, double v, Vektor p)
        {
            return emit.value(u, v, p);
        }
    }
}
