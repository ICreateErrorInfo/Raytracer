using System;
using Projection;

namespace Raytracer
{
    class Sphere
    {
        public Vektor center;
        public double radius;
        public double radius2;
        public Vektor surfaceColor;
        public Vektor emissionColor;
        public double transparency;
        public double reflection;
        public double t1;
        public double t0;

        public Sphere(Vektor c, double r, Vektor sc, double refl = 0, double transp = 0, Vektor ec = new Vektor())
        {
            center = c;
            radius = r;
            radius2 = r * r;
            surfaceColor = sc;
            emissionColor = ec;
            transparency = transp;
            reflection = refl;
        }
        public bool intersect(Vektor rayorig, Vektor raydir)
        {
            Vektor l = center - rayorig;
            double tca = Vektor.DotProduct(l, raydir);
            if (tca < 0)
            {
                return false;
            }
            double d2 = Vektor.DotProduct(l, l) - tca * tca;
            if (d2 > radius2)
            {
                return false;
            }
            double thc = Math.Sqrt(radius2 - d2);
            t0 = tca - thc;
            t1 = tca + thc;

            return true;
        }
    }
}
