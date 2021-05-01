using Projection;
using System.Drawing;
using System.IO;
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
            const int samples_per_pixel = 10;
            const int max_depth = 50;

            //World
            var world = new hittable_list();

            var material = new Metal(new Vektor(.7, .7, .7), 0.7);
            var material1 = new Metal(new Vektor(1, 0.32, 0.36), 0);
            var material2 = new Metal(new Vektor(0.90, 0.76, 0.46), 0);
            var material3 = new Metal(new Vektor(0.65, 0.77, 0.97), 0);
            var material4 = new Metal(new Vektor(0.90, 0.90, 0.90), 0);

            world.Add(new sphere(new Vektor(0.0, -10004, -20), 10000, material));
            world.Add(new sphere(new Vektor(0, 0, -20), 4, material1));
            world.Add(new sphere(new Vektor(5, -1, -15), 2, material2));
            world.Add(new sphere(new Vektor(5, 0, -25), 3, material3));
            world.Add(new sphere(new Vektor(-5.5, 0, -15), 3, material4));

            //Camera

            Vektor lookfrom = new Vektor(0, 0, 0);
            Vektor lookat = new Vektor(0, 0, -1);
            Vektor vup = new Vektor(0, 1, 0);
            var dist_to_focus = 10;
            var aperture = 0.1;

            Camera cam = new Camera(lookfrom, lookat, vup, 50, aspect_ratio, aperture, dist_to_focus);

            Bitmap bmp = new Bitmap(image_width, image_height);
            for (int j = (image_height - 1); j >= 0; j--)
            {
                for (int i = 0; i < image_width; i++)
                {
                    Vektor pixel_color = new Vektor(0, 0, 0);
                    for (int s = 0; s < samples_per_pixel; s++)
                    {
                        var u = ((double)i + Mathe.random_double()) / (image_width - 1);
                        var v = ((double)j + Mathe.random_double()) / (image_height - 1);
                        ray r = cam.get_ray(u, v);
                        pixel_color += ray.ray_color(r, world, max_depth);
                    }
                    bmp.SetPixel(i, (j - (image_height - 1)) * -1, Vektor.toColor(pixel_color, samples_per_pixel));
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
    }
}
