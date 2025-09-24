using System;
using System.Diagnostics;
using System.Threading;

namespace TspCore
{
    public class ExactSolver
    {
        private readonly TspInstance _inst;   // TSP �rne�i (�ehirlere ve mesafeye dair bilgi)
        private double _best;                 // En iyi (min) ��z�m�n uzunlu�u
        private int[] _bestTour;              // En iyi ��z�m�n tur yolu
        private double[,] _dist;              // �ehirler aras�ndaki mesafeleri i�eren matris
        private int _n;                       // �ehir say�s�

        // Constructor: TSP �rne�i al�r ve gerekli verileri ba�lat�r
        public ExactSolver(TspInstance instance)
        {
            _inst = instance ?? throw new ArgumentNullException(nameof(instance)); // TSP �rne�ini al�r
            _dist = instance.Dist;  // Mesafe matrisini al�r
            _n = instance.N;        // �ehir say�s�n� al�r
        }

        /// <summary>
        /// Branch and Bound y�ntemini kullanarak TSP problemini ��zer.
        /// </summary>
        /// <param name="ct">�ptal token'� (kullan�c� iptal iste�i i�in)</param>
        /// <param name="progress">�lerleme bildirimleri i�in kullan�lacak delegate</param>
        /// <returns>En iyi turu ve s�resini i�eren bir TspResult d�nd�r�r</returns>
        public TspResult Solve(CancellationToken ct, IProgress<double> progress = null)
        {
            var sw = Stopwatch.StartNew();  // ��z�mleme s�resini ba�lat

            // Ba�lang��ta basit s�ral� bir tur olu�tur ve en iyi ��z�m olarak kabul et
            _bestTour = TourUtils.IdentityTour(_n);  // Ba�lang�� turu
            _best = DistanceMatrix.TourLength(_dist, _bestTour);  // Ba�lang�� turunun uzunlu�unu hesapla

            // DFS i�in ge�ici diziler
            var current = new int[_n];
            current[0] = 0;  // Ba�lang�� �ehri 0
            var used = new bool[_n];
            used[0] = true;  // Ba�lang�� �ehri kullan�ld�

            // Derinlik �ncelikli arama (DFS) ba�lat
            DFS(1, 0.0, current, used, ct, progress, sw);

            sw.Stop();  // ��z�m s�resi bitir

            // Sonu�lar� d�nd�r
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
        /// Derinlik �ncelikli arama (DFS) ile olas� t�m yollar� ke�feder.
        /// </summary>
        /// <param name="depth">�u anki derinlik (ka��nc� �ehirdeyiz)</param>
        /// <param name="partialLen">�u ana kadar gidilen mesafe</param>
        /// <param name="current">Ge�ici tur yolu (�u anki �ehir s�ralamas�)</param>
        /// <param name="used">Hangi �ehirlerin kullan�ld���n� belirten bir bayrak dizisi</param>
        /// <param name="ct">�ptal token'�</param>
        /// <param name="progress">�lerlemeyi raporlama</param>
        /// <param name="sw">Zamanlay�c� (��z�m s�resi)</param>
        private void DFS(int depth, double partialLen, int[] current, bool[] used, CancellationToken ct, IProgress<double> progress, Stopwatch sw)
        {
            if (ct.IsCancellationRequested)
                return;  // E�er iptal edilmi�se, geri d�n

            // E�er t�m �ehirler ziyaret edildiyse, ��z�m bulduk
            if (depth == _n)
            {
                // Son �ehirden ilk �ehire kadar olan mesafeyi de ekleyerek toplam mesafeyi hesapla
                double total = partialLen + _dist[current[_n - 1], current[0]];
                // E�er bu yeni mesafe daha iyiyse, en iyisini g�ncelle
                if (total < _best)
                {
                    _best = total;
                    _bestTour = TourUtils.Copy(current);  // Yeni en iyi turu kopyala
                    progress?.Report(_best);  // �lerlemeyi raporla
                }
                return;  // Derinlik yeterli oldu�u i�in geri d�n
            }

            // �imdi bir sonraki �ehirleri deneyelim
            for (int next = 1; next < _n; next++)
            {
                if (used[next])
                    continue;  // E�er bu �ehir zaten kullan�ld�ysa, ge�

                // Bu �ehir ile olan mesafeyi al
                double edge = _dist[current[depth - 1], next];
                double newLen = partialLen + edge;  // Yeni toplam mesafeyi hesapla

                // E�er yeni mesafe mevcut en iyi ��z�mden daha b�y�kse, bu yolu ge�
                if (newLen >= _best)
                    continue;

                // Tahmin yap (en k���k ��k��� ekleyerek)
                double bound = newLen;

                // Geriye kalan her �ehir i�in en k���k ��k��� ekle
                for (int j = 1; j < _n; j++)
                {
                    if (used[j] || j == next)
                        continue;

                    bound += MinOutgoing(j);  // Minimum ��k��� ekle
                    if (bound >= _best)
                        break;  // E�er tahmin bile ge�erse, dur
                }

                // E�er tahmin edilen ��z�m bile ge�erli de�ilse, bu yolu ge�
                if (bound >= _best)
                    continue;

                // Bu �ehri kullanmaya karar verdik
                used[next] = true;
                current[depth] = next;  // �u anki �ehri ekle
                DFS(depth + 1, newLen, current, used, ct, progress, sw);  // Derinlemesine aramay� s�rd�r
                used[next] = false;  // Geri d�n�yoruz, dolay�s�yla �ehir kullan�labilir
            }
        }

        /// <summary>
        /// Verilen �ehirden ��kan en k���k kenar� bulur.
        /// </summary>
        /// <param name="i">�ehir indeksi</param>
        /// <returns>�ehirden ��kan en k���k kenar�n uzunlu�u</returns>
        private double MinOutgoing(int i)
        {
            double m = double.PositiveInfinity;
            for (int j = 0; j < _n; j++)
            {
                if (j == i)
                    continue;  // Ayn� �ehirden ��k��� hesaba katma

                var d = _dist[i, j];  // Mesafeyi al

                if (d < m)
                    m = d;  // E�er bu mesafe �u ana kadar g�rd���m�z en k���kse, g�ncelle
            }
            return m;  // En k���k ��k��� d�nd�r
        }
    }
}
