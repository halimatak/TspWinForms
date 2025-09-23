using System;

namespace TspCore
{
    public static class TourUtils
    {
        public static int[] IdentityTour(int n)
        {
            var t = new int[n];

            for (int i = 0; i < n; i++) 
                t[i] = i;

            return t;
        }

        public static int[] Copy(int[] tour)
        {
            var t = new int[tour.Length];
            Array.Copy(tour, t, tour.Length);
            return t;
        }

        public static void Shuffle<T>(T[] array, Random rng)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
            }
        }

        public static int[] TwoOptSwap(int[] tour, int i, int k)
        {
            var n = tour.Length;
            var newTour = new int[n];

            for (int c = 0; c < i; c++) 
                newTour[c] = tour[c];

            int dec = 0;

            for (int c = i; c <= k; c++) 
            { 
                newTour[c] = tour[k - dec]; 
                dec++; 
            }

            for (int c = k + 1; c < n; c++) 
                newTour[c] = tour[c];

            return newTour;
        }

        public static double Evaluate(double[,] dist, int[] tour) => DistanceMatrix.TourLength(dist, tour);
    }
}