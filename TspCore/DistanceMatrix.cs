using System;

namespace TspCore
{
    public static class DistanceMatrix
    {
        public static double[,] Build(Point2D[] pts)
        {
            int n = pts.Length;
            var dist = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    var dx = pts[i].X - pts[j].X;
                    var dy = pts[i].Y - pts[j].Y;
                    var d = Math.Sqrt(dx * dx + dy * dy);
                    dist[i, j] = dist[j, i] = d;
                }
            }
            return dist;
        }

        public static double TourLength(double[,] dist, int[] tour)
        {
            double sum = 0.0;
            int n = tour.Length;
            for (int i = 0; i < n - 1; i++)
                sum += dist[tour[i], tour[i + 1]];
            sum += dist[tour[n - 1], tour[0]];
            return sum;
        }
    }
}