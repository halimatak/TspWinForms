using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TspCore
{
    public class GASolver
    {
        private readonly TspInstance _inst;  // TSP örneði, þehirler ve mesafeleri tutar
        private readonly Random _rng;        // Rastgele sayý üreteci
        private readonly double[,] _dist;    // Þehirler arasý mesafe matrisi
        private readonly int _n;             // Þehir sayýsý

        // Genetik algoritma parametreleri
        public int Population { get; set; } = 200;      // Popülasyon büyüklüðü
        public int Generations { get; set; } = 1000;     // Nesil sayýsý
        public double MutationRate { get; set; } = .05;  // Mutasyon oraný
        public int Elites { get; set; } = 2;             // Elitlerin sayýsý (doðrudan geçiþ yapan bireyler)
        public int TournamentSize { get; set; } = 3;     // Turnuva büyüklüðü

        // Yapýcý metod, TSP örneðini alýr ve gerekli parametreleri baþlatýr
        public GASolver(TspInstance inst, int seed)
        {
            _inst = inst ?? throw new ArgumentNullException(nameof(inst));  // TSP örneði null olmamalý
            _rng = new Random(seed);  // Rastgele sayý üreticisini baþlat
            _dist = inst.Dist;  // Mesafe matrisini al
            _n = inst.N;  // Þehir sayýsýný al
        }

        /// <summary>
        /// Genetik algoritmayý çalýþtýrarak TSP problemini çözer.
        /// </summary>
        /// <param name="ct">Ýptal token'ý (kullanýcý iptal isteði için)</param>
        /// <param name="progress">Ýlerlemeyi raporlayacak bir delegat</param>
        /// <returns>En iyi çözümü ve çözüm süresini içeren bir TspResult döndürür</returns>
        public TspResult Solve(CancellationToken ct, IProgress<Tuple<int, double>> progress = null)
        {
            try
            {
                var sw = Stopwatch.StartNew();  // Çözüm süresini baþlat

                // Baþlangýç popülasyonunu oluþtur
                var pop = new List<int[]>(Population);
                var baseTour = TourUtils.IdentityTour(_n);  // Basit sýralý bir tur baþlat
                for (int i = 0; i < Population; i++)
                {
                    var t = TourUtils.Copy(baseTour);

                    // Turun þehirlerini rastgele karýþtýr
                    for (int j = 1; j < _n; j++)
                    {
                        int k = 1 + _rng.Next(_n - 1);  // Rastgele iki þehir seç
                        var tmp = t[j];
                        t[j] = t[k];
                        t[k] = tmp;
                    }
                    pop.Add(t);  // Karýþtýrýlmýþ turu popülasyona ekle
                }

                // Fitness (uyum) deðerlerini hesapla
                var fitness = pop.Select(p => TourUtils.Evaluate(_dist, p)).ToArray();
                int gen = 0;

                // Nesil baþýna döngü
                while (gen < Generations && !ct.IsCancellationRequested)
                {
                    // Popülasyonu ve fitness'ý sýrala (en iyi tur önce gelir)
                    Array.Sort(fitness, pop.ToArray());
                    var pairs = pop.Select((t, idx) => new { t, f = fitness[idx] }).OrderBy(x => x.f).ToList();
                    pop = pairs.Select(x => x.t).ToList();  // Yeni popülasyonu oluþtur
                    fitness = pairs.Select(x => x.f).ToArray();  // Fitness deðerlerini güncelle

                    // Ýlerlemeyi raporla (her 10. nesilde)
                    if (gen % 10 == 0)
                        progress?.Report(Tuple.Create(gen, fitness[0]));  // Ýlerlemeyi raporla

                    // Yeni popülasyonu oluþtur
                    var nextPop = new List<int[]>(Population);
                    for (int e = 0; e < Elites && e < pop.Count; e++)
                        nextPop.Add(TourUtils.Copy(pop[e]));  // Elit bireyleri doðrudan ekle

                    // Yeni bireyler oluþtur (cross-over ve mutasyon)
                    while (nextPop.Count < Population && !ct.IsCancellationRequested)
                    {
                        var p1 = Tournament(pop, fitness, TournamentSize);  // Turnuva ile iki ebeveyn seç
                        var p2 = Tournament(pop, fitness, TournamentSize);
                        var child = OX(p1, p2);  // Crossover (OX) ile çocuk oluþtur
                        Mutate(child, MutationRate);  // Mutasyon uygula
                        nextPop.Add(child);  // Çocuðu yeni popülasyona ekle
                    }

                    pop = nextPop;  // Yeni popülasyonu güncelle
                    fitness = pop.Select(p => TourUtils.Evaluate(_dist, p)).ToArray();  // Fitness hesapla
                    gen++;  // Nesil sayýsýný artýr
                }

                // En iyi çözümü bul
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

                sw.Stop();  // Zamaný durdur

                return new TspResult
                {
                    Tour = pop[bestIdx],  // En iyi tur
                    Length = bestFit,     // En iyi tur uzunluðu
                    Elapsed = sw.Elapsed, // Çözüm süresi
                    Solver = "GA",        // Kullanýlan çözüm yöntemi
                    IsSuccess = true      // Baþarý durumu
                };
            }
            catch
            {
                // Hata durumunda boþ bir sonuç döndür
            }
            return new TspResult();
        }

        /// <summary>
        /// Turnuva seçimi yaparak en iyi iki ebeveyn turunu seçer.
        /// </summary>
        private int[] Tournament(List<int[]> pop, double[] fitness, int k)
        {
            int best = -1;
            double bestFit = double.PositiveInfinity;

            // Turnuva büyüklüðü kadar rastgele seçim yap
            for (int i = 0; i < k; i++)
            {
                int idx = _rng.Next(pop.Count);

                if (fitness[idx] < bestFit)
                {
                    bestFit = fitness[idx];
                    best = idx;
                }
            }

            return pop[best];  // En iyi bireyi döndür
        }

        /// <summary>
        /// Ýki ebeveynin tur sýralamalarýný kullanarak çocuk oluþturur (OX crossover).
        /// </summary>
        private int[] OX(int[] p1, int[] p2)
        {
            int n = p1.Length;
            int[] child = Enumerable.Repeat(-1, n).ToArray();  // Çocuk dizisini baþlat (-1 ile doldur)
            child[0] = 0;  // Ýlk þehir sabit

            int a = 1 + _rng.Next(n - 1);  // Baþlangýç noktasý
            int b = 1 + _rng.Next(n - 1);  // Bitiþ noktasý

            if (a > b)
            {
                int tmp = a;
                a = b;
                b = tmp;  // Eðer a > b ise, a ve b'yi takas et
            }

            // a ve b arasýndaki þehirleri p1'den al
            for (int i = a; i <= b; i++)
                child[i] = p1[i];

            // Geriye kalan þehirleri p2'den al
            int cur = (b + 1) % n;

            if (cur == 0)
                cur = 1;

            for (int i = 1; i < n; i++)
            {
                int gene = p2[(b + i) % n];

                if (gene == 0)  // Ýlk þehir zaten dolu
                    continue;

                if (Array.IndexOf(child, gene) == -1)  // Eðer þehir zaten yoksa, ekle
                {
                    child[cur] = gene;
                    cur++;

                    if (cur >= n)
                        cur = 1;  // Eðer index n'yi geçerse, baþa dön
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
        /// Turda mutasyon uygular (iki þehri yer deðiþtirir).
        /// </summary>
        private void Mutate(int[] tour, double rate)
        {
            if (_rng.NextDouble() >= rate)
                return;  // Mutasyon yapýlmayacaksa, geri dön

            // Ýki þehir seç ve yer deðiþtir
            int i = 1 + _rng.Next(_n - 1);
            int j = 1 + _rng.Next(_n - 1);
            int tmp = tour[i];
            tour[i] = tour[j];
            tour[j] = tmp;
        }
    }
}
