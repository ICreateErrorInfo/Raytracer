using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer
{
    class Texture
    {
        public virtual Vektor value(double u, double v, Vektor p)
        {
            return null;
        }
    }
    class solid_color : Texture
    {
        public solid_color()
        {

        }
        public solid_color(Vektor c)
        {
            color_value = c;
        }

        public solid_color(double red, double green, double blue)
        {
            color_value = new Vektor(red, green, blue);
        }
        private Vektor color_value;

        public override Vektor value(double u, double v, Vektor p)
        {
            return color_value;
        }
    }
    class checker_texture : Texture
    {
        public checker_texture()
        {

        }
        public checker_texture(Texture _even, Texture _odd)
        {
            even = _even;
            odd = _odd;
        }
        public checker_texture(Vektor c1, Vektor c2)
        {
            even = new solid_color(c1);
            odd = new solid_color(c2);
        }
        public Texture odd;
        public Texture even;

        public override Vektor value(double u, double v, Vektor p)
        {
            var sines = Math.Sin(10 * p.X) * Math.Sin(10 * p.Y) * Math.Sin(10 * p.Z);
            if(sines < 0)
            {
                return odd.value(u, v, p);
            }
            else
            {
                return even.value(u, v, p);
            }
        }
    }
    class noise_texture : Texture
    {
        public noise_texture()
        {

        }
        public noise_texture(double sc)
        {
            scale = sc;
        }
        public override Vektor value(double u, double v, Vektor p)
        {
            return new Vektor(1,1,1) * noise.turb(scale * p);
        }

        public Perlin noise = new Perlin();
        double scale;
    }
}
