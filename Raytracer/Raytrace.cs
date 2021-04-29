using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Projection;

namespace Raytracer
{
    class Raytrace
    {
        public Bitmap render(List<Sphere> spheres)
        {
            int height = 1080;
            int width = 1920;
            Color[,] colArr = new Color[width, height];
            double invWidth = (double)1 / (double)width;
            double invHeight = (double)1 / (double)height;
            double fov = 50;
            double aspectratio = (double)width / (double)height;
            double angle = Math.Tan(Math.PI * 0.5 * fov / 180);


            Parallel.For(0, height, y =>
            {
                Parallel.For(0, width, x =>
                {
                    double xx = (2 * ((x + 0.5) * invWidth) - 1) * angle * aspectratio;
                    double yy = (1 - 2 * ((y + 0.5) * invHeight)) * angle;
                    Vektor rayDir = new Vektor(xx, yy, -1);
                    rayDir = rayDir.Normalise();
                    Vektor pixel = trace(new Vektor(0), rayDir, spheres, 0);
                    Color col = Color.FromArgb(Convert.ToInt32(Math.Min(1, pixel.X) * 255), Convert.ToInt32(Math.Min(1, pixel.Y) * 255), Convert.ToInt32(Math.Min(1, pixel.Z) * 255));
                    colArr[x, y] = col;
                });

            });

            Bitmap bmp = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bmp.SetPixel(x, y, colArr[x, y]);
                }
            }

            return bmp;
        }
        int maxRayDepth = 5;
        double mix(double a, double b, double mix)
        {
            return b * mix + a * (1 - mix);
        }
        Vektor trace(Vektor rayorig, Vektor raydir, List<Sphere> spheres, int depth)
        {
            double tnear = 100000000;
            Sphere sphere = null;

            for (int i = 0; i < spheres.Count; i++)
            {
                double t0;
                double t1;
                if (spheres[i].intersect(rayorig, raydir))
                {
                    t0 = spheres[i].t0;
                    t1 = spheres[i].t1;
                    if (t0 < 0)
                    {
                        t0 = t1;
                    }
                    if (t0 < tnear)
                    {
                        tnear = t0;
                        sphere = spheres[i];
                    }
                }
            }
            if (sphere == null)
            {
                return new Vektor(2, 2, 2);
            }
            Vektor surfaceColor = new Vektor(0, 0, 0);
            Vektor phit = rayorig + raydir * new Vektor(tnear, tnear, tnear);
            Vektor nhit = phit - sphere.center;
            nhit = nhit.Normalise();

            double bias = 0.0001;
            bool inside = false;
            if (Vektor.DotProduct(raydir, nhit) > 0)
            {
                nhit = nhit * new Vektor(-1, -1, -1);
                inside = true;
            }
            if ((sphere.transparency > 0 || sphere.reflection > 0) && depth < maxRayDepth)
            {
                double facingratio = -Vektor.DotProduct(raydir, nhit);
                double fresneleffect = mix(Math.Pow(1 - facingratio, 3), 1, 0.1);
                Vektor refldir = raydir - nhit * 2 * Vektor.DotProduct(raydir, nhit);
                refldir = refldir.Normalise();
                if (double.IsNaN(refldir.X) || double.IsNaN(refldir.Y) || double.IsNaN(refldir.Z))
                {

                }
                Vektor reflection = trace(phit + nhit * bias, refldir, spheres, depth + 1);
                Vektor refraction = new Vektor(0, 0, 0);

                if (sphere.transparency != 0)
                {
                    double ior = 1.1;
                    double eta = (inside) ? ior : (double)1 / (double)ior;
                    if (eta < 0)
                    {
                        eta *= -1;
                    }
                    double cosi = -Vektor.DotProduct(nhit, raydir);
                    double k = 1 - eta * eta * (1 - cosi * cosi);

                    Vektor refrdir = raydir * eta + nhit * new Vektor(eta * cosi - Math.Sqrt(k));
                    refrdir = refrdir.Normalise();
                    refraction = trace(phit - nhit * bias, refrdir, spheres, depth + 1);
                }
                surfaceColor = (
                                reflection * fresneleffect +
                                refraction * (1 - fresneleffect) * sphere.transparency) * sphere.surfaceColor;
            }
            else
            {
                for (int i = 0; i < spheres.Count; i++)
                {
                    if (spheres[i].emissionColor.X > 0)
                    {
                        Vektor transmission = new Vektor(1);
                        Vektor lightDirection = spheres[i].center - phit;
                        lightDirection = lightDirection.Normalise();
                        for (int j = 0; j < spheres.Count; j++)
                        {
                            if (i != j)
                            {
                                double t0;
                                double t1;
                                if (spheres[j].intersect(phit + nhit * bias, lightDirection))
                                {
                                    t0 = spheres[j].t0;
                                    t1 = spheres[j].t1;
                                    transmission = new Vektor(0);
                                    break;
                                }
                            }
                        }
                        surfaceColor += sphere.surfaceColor * transmission * Math.Max(0, Vektor.DotProduct(nhit, lightDirection)) * spheres[i].emissionColor;
                    }
                }
            }
            return surfaceColor + sphere.emissionColor;
        }
    }
}
