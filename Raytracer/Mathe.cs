using System;

namespace Projection {

    static class Mathe {
        public static double ToRad(double deg) => deg * Math.PI / 180;
        public static double infinity = 9999999999999999999;
        public static double random_double()
        {
            Random random = new Random();
            double ran = random.NextDouble();
            return ran;
        }
        public static double random_1Tom1()
        {
            Random random = new System.Random();
            int rand_num = random.Next(0, 2000);
            rand_num -= 1000;

            return Convert.ToDouble((double)rand_num / (double)1000);
        }
        public static double clamp(double x, double min, double max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }
        public static double random(int min, int max, int durch)
        {
            Random random = new System.Random();
            int rand_num = random.Next(min * durch, max * durch);
            rand_num -= durch;
            return Convert.ToDouble((double)rand_num / (double)durch);
        }
    }

}