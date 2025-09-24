using System;

namespace TspCore
{
    public static class DistanceMatrix
    {
        /// <summary>
        /// Verilen noktalar aras�ndaki mesafe matrisini olu�turur.
        /// </summary>
        /// <param name="pts">�ehirlerin koordinatlar�n� i�eren dizi.</param>
        /// <returns>�ehirler aras�ndaki mesafeleri tutan iki boyutlu bir dizi d�nd�r�r.</returns>
        public static double[,] Build(Point2D[] pts)
        {
            int n = pts.Length;  // �ehir say�s�n� al
            var dist = new double[n, n];  // Mesafelerin tutulaca�� matris (n x n)

            // T�m �ehirler aras�ndaki mesafeleri hesapla
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    // X ve Y farklar� hesaplanarak mesafe hesaplan�r
                    var dx = pts[i].X - pts[j].X;
                    var dy = pts[i].Y - pts[j].Y;
                    var d = Math.Sqrt(dx * dx + dy * dy);  // �klidyen mesafe hesaplama
                    dist[i, j] = dist[j, i] = d;  // Mesafeyi iki y�nl� olarak (simetrik) kaydet
                }
            }

            return dist;  // Hesaplanm�� mesafe matrisini d�nd�r
        }

        /// <summary>
        /// Verilen bir turun (�ehir s�ralamas�n�n) uzunlu�unu (toplam mesafeyi) hesaplar.
        /// </summary>
        /// <param name="dist">Mesafe matrisi.</param>
        /// <param name="tour">�ehir s�ralamas�n� i�eren dizi (tur yolu).</param>
        /// <returns>Toplam mesafeyi (tur uzunlu�unu) d�nd�r�r.</returns>
        public static double TourLength(double[,] dist, int[] tour)
        {
            double sum = 0.0;  // Ba�lang��ta toplam mesafeyi s�f�rla
            int n = tour.Length;  // Turda ka� �ehir oldu�unu al

            // Turda s�ras�yla her iki �ehir aras�ndaki mesafeyi topla
            for (int i = 0; i < n - 1; i++)
                sum += dist[tour[i], tour[i + 1]];  // Her iki �ehir aras�ndaki mesafe

            // Turun son �ehrinden ilk �ehire olan mesafeyi de ekle (kapal� d�ng�)
            sum += dist[tour[n - 1], tour[0]];  // Son �ehirden ilk �ehire mesafe

            return sum;  // Toplam mesafeyi d�nd�r
        }
    }
}
