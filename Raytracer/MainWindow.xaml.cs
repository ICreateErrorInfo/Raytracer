﻿using System;
using System.Diagnostics;

using Projection;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Raytracer
{
    public partial class MainWindow : Window
    {

        public MainWindow() {
            InitializeComponent();

            RenderScene();
        }

        private async void RenderScene() {

            var progress        = new Progress<ProgressData>(OnProgress);
          

            var imageData = await Task.Run(() => RenderImageData(progress));

            var bmp = ToBitmap(imageData);

            image.Source = BitmapToImageSource(bmp);

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        void OnProgress(ProgressData d) {

            ProgressBar.Maximum = d.TotalCount;
            ProgressBar.Value   = d.Current;
        }

        readonly struct ProgressData {

            public ProgressData(int totalCount, int current) {
                TotalCount   = totalCount;
                Current = current;
            }

            public int    TotalCount {get;}
            public int    Current    {get;}

        }

        readonly struct ImageData {

            public ImageData(Vektor[,] data, int width, int height, int samplesPerPixel) {
                Data            = data;
                Width           = width;
                Height          = height;
                SamplesPerPixel = samplesPerPixel;
            }

            public Vektor[,] Data            { get; }
            public int       Width           { get; }
            public int       Height          { get; }
            public int       SamplesPerPixel { get; }

        }

        private ImageData RenderImageData(IProgress<ProgressData> progress) {

            int samplesPerPixel;
            int max_depth       = 50;

            //World
            var world = new hittable_list();

            Vektor lookfrom   = new Vektor();
            Vektor lookat     = new Vektor();
            var    vfov       = 40;
            double aperture   = 0;
            Vektor background = new Vektor(0, 0, 0);

            int imageWidth  = 400;
            int imageHeight = 236;

            switch (5) {
                case 1:
                    var checker = new checker_texture(new Vektor(.2, .3, .1), new Vektor(.9, .9, .9));

                    var material  = new Metal(new Vektor(.7,   .7,   .7),   0.7);
                    var material1 = new Metal(new Vektor(1,    0.32, 0.36), 0);
                    var material2 = new Metal(new Vektor(0.90, 0.76, 0.46), 0);
                    var material3 = new Metal(new Vektor(0.65, 0.77, 0.97), 0);
                    var material4 = new Metal(new Vektor(0.90, 0.90, 0.90), 0);

                    var center2 = new Vektor(0, 0, -20) + new Vektor(0, Mathe.random(0, .5, 1000), 0);

                    world.Add(new sphere(new Vektor(0.0,      -10004, -20), 10000, new lambertian(checker)));
                    world.Add(new moving_sphere(new Vektor(0, 0,      -20), center2, 0, 1, 4, material1));
                    world.Add(new sphere(new Vektor(5,        -1,     -15), 2, material2));
                    world.Add(new sphere(new Vektor(5,        0,      -25), 3, material3));
                    world.Add(new sphere(new Vektor(-5.5,     0,      -15), 3, material4));

                    lookfrom   = new Vektor(0, 0, 0);
                    lookat     = new Vektor(0, 0, -1);
                    vfov       = 50;
                    aperture   = 0.1;
                    background = new Vektor(.7, .8, 1);
                    break;

                case 2:
                    world    = two_spheres();
                    lookfrom = new Vektor(13, 2, 3);
                    lookat   = new Vektor(0,  0, 0);
                    vfov     = 20;
                    break;
                case 3:
                    world      = two_perlin_spheres();
                    background = new Vektor(.7, .8, 1);
                    lookfrom   = new Vektor(13, 2,  3);
                    lookat     = new Vektor(0,  0,  0);
                    vfov       = 20;
                    break;
                case 4:
                    world      = earth();
                    background = new Vektor(.7, .8, 1);
                    lookfrom   = new Vektor(13, 2,  3);
                    lookat     = new Vektor(0,  0,  0);
                    vfov       = 20;
                    break;
                case 5:
                    world             = simple_light();
                    samplesPerPixel = 10;
                    background        = new Vektor(0,  0, 0);
                    lookfrom          = new Vektor(26, 3, 6);
                    lookat            = new Vektor(0,  2, 0);
                    vfov              = 20;
                    break;
                case 6:
                    world             = cornell_box();
                    imageWidth       = 600;
                    imageHeight      = 600;
                    samplesPerPixel = 100;
                    background        = new Vektor(0,   0,   0);
                    lookfrom          = new Vektor(278, 278, -800);
                    lookat            = new Vektor(278, 278, 0);
                    vfov              = 40;
                    break;
            }

            double aspectRatio = imageWidth / (double) imageHeight;

            //Camera

            Vektor vup           = new Vektor(0, 1, 0);
            var    dist_to_focus = 20;

            Camera cam = new Camera(lookfrom, lookat, vup, vfov, aspectRatio, aperture, dist_to_focus, 0, 1);

            Vektor[,] vArr = new Vektor[imageHeight, imageWidth];

            var totalCount = imageHeight;
            var current    = 0;
            progress.Report(new ProgressData(totalCount, current));

            Parallel.For(0, imageHeight, j => 
            {
                Interlocked.Increment(ref current);
                progress.Report(new ProgressData(totalCount, current));

                for (int i = 0; i < imageWidth; i++) {

                    

                    Vektor pixelColor = new Vektor(0, 0, 0);
                    for (int s = 0; s < samplesPerPixel; s++) 
                    {
                        var u = (i + Mathe.random_double()) / (imageWidth  - 1);
                        var v = (j + Mathe.random_double()) / (imageHeight - 1);
                        ray r = cam.get_ray(u, v);
                        pixelColor += ray.ray_color(r, background, world, max_depth);
                    }

                    vArr[j, i] = pixelColor;
                }

            });
            return new ImageData(
                data: vArr, 
                width: imageWidth,
                height: imageHeight, 
                samplesPerPixel: samplesPerPixel);
        }

        private static Bitmap ToBitmap(ImageData imageData) {

            Bitmap bmp = new Bitmap(imageData.Width, imageData.Height);

            for (int j = 0; j < imageData.Height; j++) {
                for (int i = 0; i < imageData.Width; i++) {
                    bmp.SetPixel(i, (j - (imageData.Height - 1)) * -1, Vektor.toColor(imageData.Data[j, i], imageData.SamplesPerPixel));
                }
            }

            return bmp;
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
            var earthTexture = new image_texture("C:/Users/Moritz/source/repos/Raytracer/Resources/earthmap.jpg");

            var earthSurface = new lambertian(earthTexture);
            var globe = new sphere(new Vektor(0, 0, 0), 2, earthSurface);
            var ret = new hittable_list();
            ret.Add(globe);

            return ret;
        }
        hittable_list simple_light()
        {
            hittable_list objekts = new hittable_list();

            var pertext = new noise_texture(4);
            objekts.Add(new sphere(new Vektor(0, -1000, 0), 1000, new lambertian(pertext)));
            objekts.Add(new sphere(new Vektor(0,2,0), 2, new lambertian(pertext)));

            var difflight = new diffuse_light(new Vektor(4,4,4));
            objekts.Add(new xy_rect(3,5,1,3,-2, difflight));

            return objekts;
        }
        hittable_list cornell_box()
        {
            hittable_list objects = new hittable_list();

            var red = new lambertian(new Vektor(.65, .05, .05));
            var white = new lambertian(new Vektor(.73, .73, .73));
            var green = new lambertian(new Vektor(.12, .45, .15));
            var light = new diffuse_light(new Vektor(15, 15, 15));

            objects.Add(new yz_rect (0, 555, 0, 555, 555, green));
            objects.Add(new yz_rect (0, 555, 0, 555, 0, red));
            objects.Add(new xz_rect (213, 343, 227, 332, 554, light));
            objects.Add(new xz_rect (0, 555, 0, 555, 0, white));
            objects.Add(new xz_rect (0, 555, 0, 555, 555, white));
            objects.Add(new xy_rect (0, 555, 0, 555, 555, white));

            return objects;
        }
    }
}
