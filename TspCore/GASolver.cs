using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TspCore
{
    public class GASolver
    {
        private readonly TspInstance _inst;  // TSP �rne�i, �ehirler ve mesafeleri tutar
        private readonly Random _rng;        // Rastgele say� �reteci
        private readonly double[,] _dist;    // �ehirler aras� mesafe matrisi
        private readonly int _n;             // �ehir say�s�

        // Genetik algoritma parametreleri
        public int Population { get; set; } = 200;      // Pop�lasyon b�y�kl���
        public int Generations { get; set; } = 1000;     // Nesil say�s�
        public double MutationRate { get; set; } = .05;  // Mutasyon oran�
        public int Elites { get; set; } = 2;             // Elitlerin say�s� (do�rudan ge�i� yapan bireyler)
        public int TournamentSize { get; set; } = 3;     // Turnuva b�y�kl���

        // Yap�c� metod, TSP �rne�ini al�r ve gerekli parametreleri ba�lat�r
        public GASolver(TspInstance inst, int seed)
        {
            _inst = inst ?? throw new ArgumentNullException(nameof(inst));  // TSP �rne�i null olmamal�
            _rng = new Random(seed);  // Rastgele say� �reticisini ba�lat
            _dist = inst.Dist;  // Mesafe matrisini al
            _n = inst.N;  // �ehir say�s�n� al
        }

        /// <summary>
        /// Genetik algoritmay� �al��t�rarak TSP problemini ��zer.
        /// </summary>
        /// <param name="ct">�ptal token'� (kullan�c� iptal iste�i i�in)</param>
        /// <param name="progress">�lerlemeyi raporlayacak bir delegat</param>
        /// <returns>En iyi ��z�m� ve ��z�m s�resini i�eren bir TspResult d�nd�r�r</returns>
        public TspResult Solve(CancellationToken ct, IProgress<Tuple<int, double>> progress = null)
        {
            try
            {
                var sw = Stopwatch.StartNew();  // ��z�m s�resini ba�lat

                // Ba�lang�� pop�lasyonunu olu�tur
                var pop = new List<int[]>(Population);
                var baseTour = TourUtils.IdentityTour(_n);  // Basit s�ral� bir tur ba�lat
                for (int i = 0; i < Population; i++)
                {
                    var t = TourUtils.Copy(baseTour);

                    // Turun �ehirlerini rastgele kar��t�r
                    for (int j = 1; j < _n; j++)
                    {
                        int k = 1 + _rng.Next(_n - 1);  // Rastgele iki �ehir se�
                        var tmp = t[j];
                        t[j] = t[k];
                        t[k] = tmp;
                    }
                    pop.Add(t);  // Kar��t�r�lm�� turu pop�lasyona ekle
                }

                // Fitness (uyum) de�erlerini hesapla
                var fitness = pop.Select(p => TourUtils.Evaluate(_dist, p)).ToArray();
                int gen = 0;

                // Nesil ba��na d�ng�
                while (gen < Generations && !ct.IsCancellationRequested)
                {
                    // Pop�lasyonu ve fitness'� s�rala (en iyi tur �nce gelir)
                    Array.Sort(fitness, pop.ToArray());
                    var pairs = pop.Select((t, idx) => new { t, f = fitness[idx] }).OrderBy(x => x.f).ToList();
                    pop = pairs.Select(x => x.t).ToList();  // Yeni pop�lasyonu olu�tur
                    fitness = pairs.Select(x => x.f).ToArray();  // Fitness de�erlerini g�ncelle

                    // �lerlemeyi raporla (her 10. nesilde)
                    if (gen % 10 == 0)
                        progress?.Report(Tuple.Create(gen, fitness[0]));  // �lerlemeyi raporla

                    // Yeni pop�lasyonu olu�tur
                    var nextPop = new List<int[]>(Population);
                    for (int e = 0; e < Elites && e < pop.Count; e++)
                        nextPop.Add(TourUtils.Copy(pop[e]));  // Elit bireyleri do�rudan ekle

                    // Yeni bireyler olu�tur (cross-over ve mutasyon)
                    while (nextPop.Count < Population && !ct.IsCancellationRequested)
                    {
                        var p1 = Tournament(pop, fitness, TournamentSize);  // Turnuva ile iki ebeveyn se�
                        var p2 = Tournament(pop, fitness, TournamentSize);
                        var child = OX(p1, p2);  // Crossover (OX) ile �ocuk olu�tur
                        Mutate(child, MutationRate);  // Mutasyon uygula
                        nextPop.Add(child);  // �ocu�u yeni pop�lasyona ekle
                    }

                    pop = nextPop;  // Yeni pop�lasyonu g�ncelle
                    fitness = pop.Select(p => TourUtils.Evaluate(_dist, p)).ToArray();  // Fitness hesapla
                    gen++;  // Nesil say�s�n� art�r
                }

                // En iyi ��z�m� bul
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

                sw.Stop();  // Zaman� durdur

                return new TspResult
                {
                    Tour = pop[bestIdx],  // En iyi tur
                    Length = bestFit,     // En iyi tur uzunlu�u
                    Elapsed = sw.Elapsed, // ��z�m s�resi
                    Solver = "GA",        // Kullan�lan ��z�m y�ntemi
                    IsSuccess = true      // Ba�ar� durumu
                };
            }
            catch
            {
                // Hata durumunda bo� bir sonu� d�nd�r
            }
            return new TspResult();
        }

        /// <summary>
        /// Turnuva se�imi yaparak en iyi iki ebeveyn turunu se�er.
        /// </summary>
        private int[] Tournament(List<int[]> pop, double[] fitness, int k)
        {
            int best = -1;
            double bestFit = double.PositiveInfinity;

            // Turnuva b�y�kl��� kadar rastgele se�im yap
            for (int i = 0; i < k; i++)
            {
                int idx = _rng.Next(pop.Count);

                if (fitness[idx] < bestFit)
                {
                    bestFit = fitness[idx];
                    best = idx;
                }
            }

            return pop[best];  // En iyi bireyi d�nd�r
        }

        /// <summary>
        /// �ki ebeveynin tur s�ralamalar�n� kullanarak �ocuk olu�turur (OX crossover).
        /// </summary>
        private int[] OX(int[] p1, int[] p2)
        {
            int n = p1.Length;
            int[] child = Enumerable.Repeat(-1, n).ToArray();  // �ocuk dizisini ba�lat (-1 ile doldur)
            child[0] = 0;  // �lk �ehir sabit

            int a = 1 + _rng.Next(n - 1);  // Ba�lang�� noktas�
            int b = 1 + _rng.Next(n - 1);  // Biti� noktas�

            if (a > b)
            {
                int tmp = a;
                a = b;
                b = tmp;  // E�er a > b ise, a ve b'yi takas et
            }

            // a ve b aras�ndaki �ehirleri p1'den al
            for (int i = a; i <= b; i++)
                child[i] = p1[i];

            // Geriye kalan �ehirleri p2'den al
            int cur = (b + 1) % n;

            if (cur == 0)
                cur = 1;

            for (int i = 1; i < n; i++)
            {
                int gene = p2[(b + i) % n];

                if (gene == 0)  // �lk �ehir zaten dolu
                    continue;

                if (Array.IndexOf(child, gene) == -1)  // E�er �ehir zaten yoksa, ekle
                {
                    child[cur] = gene;
                    cur++;

                    if (cur >= n)
                        cur = 1;  // E�er index n'yi ge�erse, ba�a d�n
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

        /// <summary>
        /// Turda mutasyon uygular (iki �ehri yer de�i�tirir).
        /// </summary>
        private void Mutate(int[] tour, double rate)
        {
            if (_rng.NextDouble() >= rate)
                return;  // Mutasyon yap�lmayacaksa, geri d�n

            // �ki �ehir se� ve yer de�i�tir
            int i = 1 + _rng.Next(_n - 1);
            int j = 1 + _rng.Next(_n - 1);
            int tmp = tour[i];
            tour[i] = tour[j];
            tour[j] = tmp;
        }
    }
}
