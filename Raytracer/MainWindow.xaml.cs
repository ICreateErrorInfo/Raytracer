using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
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

            var sw = Stopwatch.StartNew();

            Raytrace raytrace = new Raytrace();
            image.Source = BitmapToImageSource(raytrace.render(spheres));

            Stats.Content = $"render time: {sw.Elapsed:c}";
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
