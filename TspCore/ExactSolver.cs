using System;
using System.Diagnostics;
using System.Threading;

namespace TspCore
{
    public class ExactSolver
    {
        private readonly TspInstance _inst;   // TSP örneði (þehirlere ve mesafeye dair bilgi)
        private double _best;                 // En iyi (min) çözümün uzunluðu
        private int[] _bestTour;              // En iyi çözümün tur yolu
        private double[,] _dist;              // Þehirler arasýndaki mesafeleri içeren matris
        private int _n;                       // Þehir sayýsý

        // Constructor: TSP örneði alýr ve gerekli verileri baþlatýr
        public ExactSolver(TspInstance instance)
        {
            _inst = instance ?? throw new ArgumentNullException(nameof(instance)); // TSP örneðini alýr
            _dist = instance.Dist;  // Mesafe matrisini alýr
            _n = instance.N;        // Þehir sayýsýný alýr
        }

        /// <summary>
        /// Branch and Bound yöntemini kullanarak TSP problemini çözer.
        /// </summary>
        /// <param name="ct">Ýptal token'ý (kullanýcý iptal isteði için)</param>
        /// <param name="progress">Ýlerleme bildirimleri için kullanýlacak delegate</param>
        /// <returns>En iyi turu ve süresini içeren bir TspResult döndürür</returns>
        public TspResult Solve(CancellationToken ct, IProgress<double> progress = null)
        {
            var sw = Stopwatch.StartNew();  // Çözümleme süresini baþlat

            // Baþlangýçta basit sýralý bir tur oluþtur ve en iyi çözüm olarak kabul et
            _bestTour = TourUtils.IdentityTour(_n);  // Baþlangýç turu
            _best = DistanceMatrix.TourLength(_dist, _bestTour);  // Baþlangýç turunun uzunluðunu hesapla

            // DFS için geçici diziler
            var current = new int[_n];
            current[0] = 0;  // Baþlangýç þehri 0
            var used = new bool[_n];
            used[0] = true;  // Baþlangýç þehri kullanýldý

            // Derinlik öncelikli arama (DFS) baþlat
            DFS(1, 0.0, current, used, ct, progress, sw);

            sw.Stop();  // Çözüm süresi bitir

            // Sonuçlarý döndür
            return new TspResult
            {
                Tour = _bestTour,
                Length = _best,
                Elapsed = sw.Elapsed,
                Solver = "Exact (Branch & Bound)",
                IsSuccess = true
            };
        }

        /// <summary>
        /// Derinlik öncelikli arama (DFS) ile olasý tüm yollarý keþfeder.
        /// </summary>
        /// <param name="depth">Þu anki derinlik (kaçýncý þehirdeyiz)</param>
        /// <param name="partialLen">Þu ana kadar gidilen mesafe</param>
        /// <param name="current">Geçici tur yolu (þu anki þehir sýralamasý)</param>
        /// <param name="used">Hangi þehirlerin kullanýldýðýný belirten bir bayrak dizisi</param>
        /// <param name="ct">Ýptal token'ý</param>
        /// <param name="progress">Ýlerlemeyi raporlama</param>
        /// <param name="sw">Zamanlayýcý (çözüm süresi)</param>
        private void DFS(int depth, double partialLen, int[] current, bool[] used, CancellationToken ct, IProgress<double> progress, Stopwatch sw)
        {
            if (ct.IsCancellationRequested)
                return;  // Eðer iptal edilmiþse, geri dön

            // Eðer tüm þehirler ziyaret edildiyse, çözüm bulduk
            if (depth == _n)
            {
                // Son þehirden ilk þehire kadar olan mesafeyi de ekleyerek toplam mesafeyi hesapla
                double total = partialLen + _dist[current[_n - 1], current[0]];
                // Eðer bu yeni mesafe daha iyiyse, en iyisini güncelle
                if (total < _best)
                {
                    _best = total;
                    _bestTour = TourUtils.Copy(current);  // Yeni en iyi turu kopyala
                    progress?.Report(_best);  // Ýlerlemeyi raporla
                }
                return;  // Derinlik yeterli olduðu için geri dön
            }

            // Þimdi bir sonraki þehirleri deneyelim
            for (int next = 1; next < _n; next++)
            {
                if (used[next])
                    continue;  // Eðer bu þehir zaten kullanýldýysa, geç

                // Bu þehir ile olan mesafeyi al
                double edge = _dist[current[depth - 1], next];
                double newLen = partialLen + edge;  // Yeni toplam mesafeyi hesapla

                // Eðer yeni mesafe mevcut en iyi çözümden daha büyükse, bu yolu geç
                if (newLen >= _best)
                    continue;

                // Tahmin yap (en küçük çýkýþý ekleyerek)
                double bound = newLen;

                // Geriye kalan her þehir için en küçük çýkýþý ekle
                for (int j = 1; j < _n; j++)
                {
                    if (used[j] || j == next)
                        continue;

                    bound += MinOutgoing(j);  // Minimum çýkýþý ekle
                    if (bound >= _best)
                        break;  // Eðer tahmin bile geçerse, dur
                }

                // Eðer tahmin edilen çözüm bile geçerli deðilse, bu yolu geç
                if (bound >= _best)
                    continue;

                // Bu þehri kullanmaya karar verdik
                used[next] = true;
                current[depth] = next;  // Þu anki þehri ekle
                DFS(depth + 1, newLen, current, used, ct, progress, sw);  // Derinlemesine aramayý sürdür
                used[next] = false;  // Geri dönüyoruz, dolayýsýyla þehir kullanýlabilir
            }
        }

        /// <summary>
        /// Verilen þehirden çýkan en küçük kenarý bulur.
        /// </summary>
        /// <param name="i">Þehir indeksi</param>
        /// <returns>Þehirden çýkan en küçük kenarýn uzunluðu</returns>
        private double MinOutgoing(int i)
        {
            double m = double.PositiveInfinity;
            for (int j = 0; j < _n; j++)
            {
                if (j == i)
                    continue;  // Ayný þehirden çýkýþý hesaba katma

                var d = _dist[i, j];  // Mesafeyi al

                if (d < m)
                    m = d;  // Eðer bu mesafe þu ana kadar gördüðümüz en küçükse, güncelle
            }
            return m;  // En küçük çýkýþý döndür
        }
    }
}
