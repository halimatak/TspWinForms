using System;

namespace TspCore
{
    [Serializable]
    public struct Point2D
    {
        public int Id;
        public double X;
        public double Y;
        public Point2D(int id, double x, double y)
        {
            Id = id; 
            X = x; 
            Y = y;
        }
        public override string ToString() => $"{Id}({X},{Y})";
    }

    [Serializable]
    public class TspInstance
    {
        public Point2D[] Points { get; private set; }
        public double[,] Dist { get; private set; }
        public int N => Points.Length;

        public TspInstance(Point2D[] points, bool precomputeDist = true)
        {
            Points = points ?? throw new ArgumentNullException(nameof(points));
            if (precomputeDist)
                Dist = DistanceMatrix.Build(Points);
        }
    }

    public class TspResult
    {
        public int[] Tour;
        public double Length;
        public TimeSpan Elapsed;
        public string Solver;
        public bool IsSuccess { get; set; }
    }
}