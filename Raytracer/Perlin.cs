using System;
using System.Collections.Generic;
using System.Text;
using Projection;

namespace Raytracer
{
    class Perlin
    {
        public Perlin()
        {
            ranfloat = new double[point_count];
            for(int i = 0; i < point_count; i++)
            {
                ranfloat[i] = Mathe.random_double();
            }

            perm_x = perlin_generate_perm();
            perm_y = perlin_generate_perm();
            perm_z = perlin_generate_perm();
        }
        ~Perlin()
        {
            ranfloat = new double[0];
            perm_x = new int[0];
            perm_y = new int[0];
            perm_z = new int[0];
        }
        private static int point_count = 256;
        private double[] ranfloat;
        int[] perm_x;
        int[] perm_y;
        int[] perm_z;

        public double noise(Vektor p)
        {
            var u = p.X - Math.Floor(p.X);
            var v = p.Y - Math.Floor(p.Y);
            var w = p.Z - Math.Floor(p.Z);
            u = u * u * (3 - 2 * u);
            v = v * v * (3 - 2 * v);
            w = w * w * (3 - 2 * w);

            var i = Convert.ToInt32(Math.Floor(p.X));
            var j = Convert.ToInt32(Math.Floor(p.Y));
            var k = Convert.ToInt32(Math.Floor(p.Z));
            double[,,] c = new double[2,2,2];

            for(int di = 0; di < 2; di++)
            {
                for(int dj = 0; dj < 2; dj++)
                {
                    for (int dk = 0; dk < 2; dk++)
                    {
                        c[di, dj, dk] = ranfloat[
                            perm_x[(i+di) & 255] ^
                            perm_y[(j+dj) & 255] ^
                            perm_z[(k+dk) & 255]
                            ];
                    }
                }
            }

            return trilinear_interp(c, u, v, w);
        }
        private static double trilinear_interp(double[,,] c, double u, double v, double w)
        {
            double accum = 0;
            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        accum += (i * u + (1 - i) * (1 - u)) *
                                 (j * v + (1 - j) * (1 - v)) *
                                 (k * w + (1 - k) * (1 - w)) * c[i,j,k];
                    }
                }
            }
            return accum;
        }
        private static int[] perlin_generate_perm()
        {
            var p = new int[point_count];

            for (int i = 0; i < Perlin.point_count; i++)
            {
                p[i] = i;
            }
            permute(p,point_count);

            return p;
        }
        private static int[] permute(int[] p, int n)
        {
            for(int i = n - 1; i > 0; i--)
            {
                int target = Convert.ToInt32(Mathe.random(1, i, 1));
                int tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }
            return p;
        }
    }
}
