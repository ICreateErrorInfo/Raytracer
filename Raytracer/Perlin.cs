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
            ranfloat = new Vektor[point_count];
            for(int i = 0; i < point_count; i++)
            {
                ranfloat[i] = Vektor.unit_Vektor(Vektor.random());
            }

            perm_x = perlin_generate_perm();
            perm_y = perlin_generate_perm();
            perm_z = perlin_generate_perm();
        }
        ~Perlin()
        {
            ranfloat = new Vektor[0];
            perm_x = new int[0];
            perm_y = new int[0];
            perm_z = new int[0];
        }
        private static int point_count = 256;
        private Vektor[] ranfloat;
        int[] perm_x;
        int[] perm_y;
        int[] perm_z;

        public double noise(Vektor p)
        {
            var u = p.X - Math.Floor(p.X);
            var v = p.Y - Math.Floor(p.Y);
            var w = p.Z - Math.Floor(p.Z);

            var i = Convert.ToInt32(Math.Floor(p.X));
            var j = Convert.ToInt32(Math.Floor(p.Y));
            var k = Convert.ToInt32(Math.Floor(p.Z));
            Vektor[,,] c = new Vektor[2,2,2];

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

            return perlin_interp(c, u, v, w);
        }
        private static double perlin_interp(Vektor[,,] c, double u, double v, double w)
        {
            var uu = u * u * (3 - 2 * u);
            var vv = v * v * (3 - 2 * v);
            var ww = w * w * (3 - 2 * w);
            double accum = 0;

            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Vektor weight_v = new Vektor(u-i, v-j, w-k);
                        accum += (i * uu + (1 - i) * (1 - uu)) *
                                 (j * vv + (1 - j) * (1 - vv)) *
                                 (k * ww + (1 - k) * (1 - ww)) * 
                                 Vektor.dot(c[i,j,k], weight_v);
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
