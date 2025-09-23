using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TspCore
{
    public class GASolver
    {
        private readonly TspInstance _inst;
        private readonly Random _rng;
        private readonly double[,] _dist;
        private readonly int _n;

        public int Population { get; set; } = 200;
        public int Generations { get; set; } = 1000;
        public double MutationRate { get; set; } = .05;
        public int Elites { get; set; } = 2;
        public int TournamentSize { get; set; } = 3;

        public GASolver(TspInstance inst, int seed)
        {
            _inst = inst ?? throw new ArgumentNullException(nameof(inst));
            _rng = new Random(seed);
            _dist = inst.Dist;
            _n = inst.N;
        }

        public TspResult Solve(CancellationToken ct, IProgress<Tuple<int, double>> progress = null)
        {
            try
            {
                var sw = Stopwatch.StartNew();

                var pop = new List<int[]>(Population);
                var baseTour = TourUtils.IdentityTour(_n);
                for (int i = 0; i < Population; i++)
                {
                    var t = TourUtils.Copy(baseTour);

                    for (int j = 1; j < _n; j++)
                    {
                        int k = 1 + _rng.Next(_n - 1);
                        var tmp = t[j]; 
                        t[j] = t[k]; 
                        t[k] = tmp;
                    }
                    pop.Add(t);
                }

                var fitness = pop.Select(p => TourUtils.Evaluate(_dist, p)).ToArray();
                int gen = 0;

                while (gen < Generations && !ct.IsCancellationRequested)
                {
                    Array.Sort(fitness, pop.ToArray());
                    var pairs = pop.Select((t, idx) => new { t, f = fitness[idx] }).OrderBy(x => x.f).ToList();
                    pop = pairs.Select(x => x.t).ToList();
                    fitness = pairs.Select(x => x.f).ToArray();

                    if (gen % 10 == 0)
                        progress?.Report(Tuple.Create(gen, fitness[0]));

                    var nextPop = new List<int[]>(Population);
                    for (int e = 0; e < Elites && e < pop.Count; e++)
                        nextPop.Add(TourUtils.Copy(pop[e]));

                    while (nextPop.Count < Population && !ct.IsCancellationRequested)
                    {
                        var p1 = Tournament(pop, fitness, TournamentSize);
                        var p2 = Tournament(pop, fitness, TournamentSize);
                        var child = OX(p1, p2);
                        Mutate(child, MutationRate);
                        nextPop.Add(child);
                    }
                    pop = nextPop;
                    fitness = pop.Select(p => TourUtils.Evaluate(_dist, p)).ToArray();
                    gen++;
                }

                var bestIdx = 0;
                var bestFit = fitness[0];

                for (int i = 1; i < fitness.Length; i++)
                {
                    if (fitness[i] < bestFit)
                    {
                        bestFit = fitness[i];
                        bestIdx = i;
                    }
                }
                sw.Stop();
                return new TspResult
                {
                    Tour = pop[bestIdx],
                    Length = bestFit,
                    Elapsed = sw.Elapsed,
                    Solver = "GA",
                    IsSuccess = true
                };
            }
            catch
            {
            }
            return new TspResult();
        }

        private int[] Tournament(List<int[]> pop, double[] fitness, int k)
        {
            int best = -1;
            double bestFit = double.PositiveInfinity;

            for (int i = 0; i < k; i++)
            {
                int idx = _rng.Next(pop.Count);

                if (fitness[idx] < bestFit)
                {
                    bestFit = fitness[idx];
                    best = idx;
                }
            }
            return pop[best];
        }

        private int[] OX(int[] p1, int[] p2)
        {
            int n = p1.Length;
            int[] child = Enumerable.Repeat(-1, n).ToArray();
            child[0] = 0;

            int a = 1 + _rng.Next(n - 1);
            int b = 1 + _rng.Next(n - 1);

            if (a > b)
            {
                int tmp = a;
                a = b;
                b = tmp;
            }

            for (int i = a; i <= b; i++)
                child[i] = p1[i];

            int cur = (b + 1) % n;

            if (cur == 0)
                cur = 1;

            for (int i = 1; i < n; i++)
            {
                int gene = p2[(b + i) % n];

                if (gene == 0)
                    continue;

                if (Array.IndexOf(child, gene) == -1)
                {
                    child[cur] = gene;
                    cur++;

                    if (cur >= n)
                        cur = 1;
                }
            }

            for (int i = 1; i < n; i++)
            {
                if (child[i] == -1)
                {
                    for (int g = 1; g < n; g++)
                    {
                        if (Array.IndexOf(child, g) == -1)
                        {
                            child[i] = g;
                            break;
                        }
                    }
                }
            }

            return child;
        }

        private void Mutate(int[] tour, double rate)
        {
            if (_rng.NextDouble() >= rate) 
                return;

            int i = 1 + _rng.Next(_n - 1);
            int j = 1 + _rng.Next(_n - 1);
            int tmp = tour[i]; 
            tour[i] = tour[j]; 
            tour[j] = tmp;
        }
    }
}