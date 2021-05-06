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
            var i = Convert.ToInt32(4 * p.X) & 255;
            var j = Convert.ToInt32(4 * p.Y) & 255;
            var k = Convert.ToInt32(4 * p.Z) & 255;

            return ranfloat[perm_x[i] ^ perm_y[j] ^ perm_z[k]];
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
