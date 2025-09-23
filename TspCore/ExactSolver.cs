using System;
using System.Diagnostics;
using System.Threading;

namespace TspCore
{
    public class ExactSolver
    {
        private readonly TspInstance _inst;
        private double _best;
        private int[] _bestTour;
        private double[,] _dist;
        private int _n;

        public ExactSolver(TspInstance instance)
        {
            _inst = instance ?? throw new ArgumentNullException(nameof(instance));
            _dist = instance.Dist;
            _n = instance.N;
        }

        public TspResult Solve(CancellationToken ct, IProgress<double> progress = null)
        {
            var sw = Stopwatch.StartNew();
            _bestTour = TourUtils.IdentityTour(_n);
            _best = DistanceMatrix.TourLength(_dist, _bestTour);

            var current = new int[_n];
            current[0] = 0;
            var used = new bool[_n];
            used[0] = true;

            DFS(1, 0.0, current, used, ct, progress, sw);
            sw.Stop();
            return new TspResult { Tour = _bestTour, Length = _best, Elapsed = sw.Elapsed, Solver = "Exact (Branch & Bound)", IsSuccess = true };
        }

        private void DFS(int depth, double partialLen, int[] current, bool[] used, CancellationToken ct, IProgress<double> progress, Stopwatch sw)
        {
            if (ct.IsCancellationRequested) 
                return;

            if (depth == _n)
            {
                double total = partialLen + _dist[current[_n - 1], current[0]];
                if (total < _best)
                {
                    _best = total;
                    _bestTour = TourUtils.Copy(current);
                    progress?.Report(_best);
                }
                return;
            }

            for (int next = 1; next < _n; next++)
            {
                if (used[next]) 
                    continue;

                double edge = _dist[current[depth - 1], next];
                double newLen = partialLen + edge;

                if (newLen >= _best) 
                    continue;

                double bound = newLen;

                for (int j = 1; j < _n; j++)
                {
                    if (used[j] || j == next) 
                        continue;

                    bound += MinOutgoing(j);
                    if (bound >= _best) 
                        break;
                }

                if (bound >= _best) 
                    continue;

                used[next] = true;
                current[depth] = next;
                DFS(depth + 1, newLen, current, used, ct, progress, sw);
                used[next] = false;
            }
        }

        private double MinOutgoing(int i)
        {
            double m = double.PositiveInfinity;
            for (int j = 0; j < _n; j++)
            {
                if (j == i) 
                    continue;

                var d = _dist[i, j];

                if (d < m) 
                    m = d;
            }
            return m;
        }
    }
}