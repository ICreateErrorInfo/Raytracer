using Projection;
using System;

namespace Raytracer
{
    class Camera
    {
        public Camera(Vektor lookfrom, Vektor lookat, Vektor vup, double vfov, double aspect_ratio, double aperture, double focus_dist)
        {
            var theta = Mathe.ToRad(vfov);
            var h = Math.Tan(theta / 2);
            var viewport_height = 2 * h;
            var viewport_width = aspect_ratio * viewport_height;

            w = Vektor.unit_Vektor(lookfrom - lookat);
            u = Vektor.unit_Vektor(Vektor.cross(vup, w));
            v = Vektor.cross(w, u);

            origin = lookfrom;
            horizontal = focus_dist * viewport_width * u;
            vertical = focus_dist * viewport_height * v;
            lower_left_corner = origin - horizontal / 2 - vertical / 2 - focus_dist * w;

            lens_radius = aperture / 2;
        }

        public ray get_ray(double s, double t)
        {
            Vektor rd = lens_radius * Vektor.random_in_unit_disk();
            Vektor offset = u * rd.X + v * rd.Y;

            return new ray(origin + offset, lower_left_corner + s * horizontal + t * vertical - origin - offset);
        }

        Vektor origin;
        Vektor lower_left_corner;
        Vektor horizontal;
        Vektor vertical;
        Vektor u;
        Vektor v;
        Vektor w;
        double lens_radius;
    }
}
