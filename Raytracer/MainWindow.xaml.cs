using Projection;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Raytracer
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            const int image_width = 400;
            const int image_height = 236;
            double aspect_ratio = (double)image_width / (double)image_height;
            const int samples_per_pixel = 200;
            const int max_depth = 50;

            //World
            var world = new hittable_list();

            Vektor lookfrom = new Vektor();
            Vektor lookat = new Vektor();
            var vfov = 40;
            double aperture = 0;
            Vektor background = new Vektor(0, 0, 0);

            switch (5)
            {
                case 1:
                    var checker = new checker_texture(new Vektor(.2, .3, .1), new Vektor(.9, .9, .9));

                    var material = new Metal(new Vektor(.7, .7, .7), 0.7);
                    var material1 = new Metal(new Vektor(1, 0.32, 0.36), 0);
                    var material2 = new Metal(new Vektor(0.90, 0.76, 0.46), 0);
                    var material3 = new Metal(new Vektor(0.65, 0.77, 0.97), 0);
                    var material4 = new Metal(new Vektor(0.90, 0.90, 0.90), 0);

                    var center2 = new Vektor(0, 0, -20) + new Vektor(0, Mathe.random(0, .5, 1000), 0);

                    world.Add(new sphere(new Vektor(0.0, -10004, -20), 10000, new lambertian(checker)));
                    world.Add(new moving_sphere(new Vektor(0, 0, -20), center2, 0, 1, 4, material1));
                    world.Add(new sphere(new Vektor(5, -1, -15), 2, material2));
                    world.Add(new sphere(new Vektor(5, 0, -25), 3, material3));
                    world.Add(new sphere(new Vektor(-5.5, 0, -15), 3, material4));

                    lookfrom = new Vektor(0, 0, 0);
                    lookat = new Vektor(0, 0, -1);
                    vfov = 50;
                    aperture = 0.1;
                    background = new Vektor(.7,.8,1);
                    break;

                case 2:
                    world = two_spheres();
                    lookfrom = new Vektor(13, 2, 3);
                    lookat = new Vektor(0,0,0);
                    vfov = 20;
                    break;
                case 3:
                    world = two_perlin_spheres();
                    background = new Vektor(.7,.8,1);
                    lookfrom = new Vektor(13, 2, 3);
                    lookat = new Vektor(0, 0, 0);
                    vfov = 20;
                    break;
                case 4:
                    world = earth();
                    background = new Vektor(.7, .8, 1);
                    lookfrom = new Vektor(13, 2, 3);
                    lookat = new Vektor(0, 0, 0);
                    vfov = 20;
                    break;
                case 5:
                    background = new Vektor(0,0,0);
                    break;
            }

            //Camera

            Vektor vup = new Vektor(0, 1, 0);
            var dist_to_focus = 20;

            Camera cam = new Camera(lookfrom, lookat, vup, vfov, aspect_ratio, aperture, dist_to_focus, 0, 1);

            Vektor[,] vArr = new Vektor[image_height, image_width];
            Bitmap bmp = new Bitmap(image_width, image_height);
            Parallel.For(0, image_height, j =>
            {
                for (int i = 0; i < image_width; i++)
                {
                    Vektor pixel_color = new Vektor(0, 0, 0);
                    for (int s = 0; s < samples_per_pixel; s++)
                    {
                        var u = ((double)i + Mathe.random_double()) / (image_width - 1);
                        var v = ((double)j + Mathe.random_double()) / (image_height - 1);
                        ray r = cam.get_ray(u, v);
                        pixel_color += ray.ray_color(r, background, world, max_depth);
                    }
                    vArr[j, i] = pixel_color;
                }
            
            });

            for (int j = 0; j < image_height; j++)
            {
                for(int i = 0; i < image_width; i++)
                {
                    bmp.SetPixel(i, (j - (image_height - 1)) * -1, Vektor.toColor(vArr[j, i], samples_per_pixel));   
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
        hittable_list two_spheres()
        {
            hittable_list objects = new hittable_list();

            var checker = new checker_texture(new Vektor(0.2, 0.3, 0.1), new Vektor(0.9, 0.9, 0.9));

            objects.Add(new sphere(new Vektor(0, -10, 0), 10, new lambertian(checker)));
            objects.Add(new sphere(new Vektor(0, 10, 0), 10, new lambertian(checker)));

            return objects;
        }
        hittable_list two_perlin_spheres()
        {
            hittable_list objects = new hittable_list();

            var pertext = new noise_texture(4);

            objects.Add(new sphere(new Vektor(0, -1000, 0), 1000, new lambertian(pertext)));
            objects.Add(new sphere(new Vektor(0, 2, 0), 2, new lambertian(pertext)));

            return objects;
        }
        hittable_list earth()
        {
            var earth_texture = new image_texture("C:/Users/Moritz/source/repos/Raytracer/Resources/earthmap.jpg");

            var earth_surface = new lambertian(earth_texture);
            var globe = new sphere(new Vektor(0, 0, 0), 2, earth_surface);
            var ret = new hittable_list();
            ret.Add(globe);

            return ret;
        }
    }
}
