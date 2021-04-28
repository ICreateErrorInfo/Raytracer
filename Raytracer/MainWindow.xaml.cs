using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Projection;

namespace Raytracer
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            List<Sphere> spheres = new List<Sphere>();

            // position, radius, surface color, reflectivity, transparency, emission color
            spheres.Add(new Sphere(new Vektor( 0.0, -10004, -20), 10000, new Vektor(.7,.7,.7), .3, 0.0));
            spheres.Add(new Sphere(new Vektor(0.0, 0, -20), 4, new Vektor(1.00, 0.32, 0.36), 1, 0.5));
            spheres.Add(new Sphere(new Vektor(5.0, -1, -15), 2, new Vektor(0.90, 0.76, 0.46), 1, 0.0));
            spheres.Add(new Sphere(new Vektor(5.0, 0, -25), 3, new Vektor(0.65, 0.77, 0.97), 1, 0.0));
            spheres.Add(new Sphere(new Vektor(-5.5, 0, -15), 3, new Vektor(0.90, 0.90, 0.90), 1, 0.0));

            //Light
            spheres.Add(new Sphere(new Vektor(0.0, 20, -30), 3, new Vektor(0.00, 0.00, 0.00), 0, 0.0, new Vektor(3)));
            render(spheres);

        }
        void render(List<Sphere> spheres)
        {
            int height = 1080;
            int width = 1920;
            System.Drawing.Color[,] colArr = new System.Drawing.Color[width, height];
            double invWidth = (double)1 / (double)width;
            double invHeight = (double)1 / (double)height;
            double fov = 50;
            double aspectratio = (double)width / (double)height;
            double angle = Math.Tan(Math.PI * 0.5 * fov / 180);


            Parallel.For(0, height, y =>
             {

                 for (int x = 0; x < width; x++)
                 {
                     double xx = (2 * ((x + 0.5) * invWidth) - 1) * angle * aspectratio;
                     double yy = (1 - 2 * ((y + 0.5) * invHeight)) * angle;
                     Vektor rayDir = new Vektor(xx, yy, -1);
                     rayDir = rayDir.Normalise();
                     Vektor pixel = trace(new Vektor(0), rayDir, spheres, 0);
                     System.Drawing.Color col = System.Drawing.Color.FromArgb(Convert.ToInt32(Math.Min(1, pixel.X) * 255), Convert.ToInt32(Math.Min(1, pixel.Y) * 255), Convert.ToInt32(Math.Min(1, pixel.Z) * 255));
                     colArr[x, y] = col;
                 }

             });

            Bitmap bmp = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bmp.SetPixel(x,y, colArr[x,y]);
                }
            }

            image.Source = BitmapToImageSource(bmp);
        }
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        int maxRayDepth = 10;
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
                Vektor reflection = trace(phit + nhit * bias, refldir, spheres, depth + 1);
                Vektor refraction = new Vektor(0, 0, 0);

                if (sphere.transparency != 0)
                {
                    double ior = 1.1;
                    double eta = (inside) ? ior : 1 / ior;
                    double cosi = -Vektor.DotProduct(nhit, raydir);
                    double k = 1 - eta * eta * (1 - cosi * cosi);
                    Vektor refrdir = raydir * new Vektor(eta, eta, eta) + nhit * new Vektor(eta * cosi - Math.Sqrt(k), eta * cosi - Math.Sqrt(k), eta * cosi - Math.Sqrt(k));
                    refrdir = refrdir.Normalise();
                    refraction = trace(phit - nhit * new Vektor(bias, bias, bias), refrdir, spheres, depth + 1);
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
