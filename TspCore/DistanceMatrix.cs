using System;

namespace TspCore
{
    public static class DistanceMatrix
    {
        /// <summary>
        /// Verilen noktalar arasýndaki mesafe matrisini oluþturur.
        /// </summary>
        /// <param name="pts">Þehirlerin koordinatlarýný içeren dizi.</param>
        /// <returns>Þehirler arasýndaki mesafeleri tutan iki boyutlu bir dizi döndürür.</returns>
        public static double[,] Build(Point2D[] pts)
        {
            int n = pts.Length;  // Þehir sayýsýný al
            var dist = new double[n, n];  // Mesafelerin tutulacaðý matris (n x n)

            // Tüm þehirler arasýndaki mesafeleri hesapla
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    // X ve Y farklarý hesaplanarak mesafe hesaplanýr
                    var dx = pts[i].X - pts[j].X;
                    var dy = pts[i].Y - pts[j].Y;
                    var d = Math.Sqrt(dx * dx + dy * dy);  // Öklidyen mesafe hesaplama
                    dist[i, j] = dist[j, i] = d;  // Mesafeyi iki yönlü olarak (simetrik) kaydet
                }
            }

            return dist;  // Hesaplanmýþ mesafe matrisini döndür
        }

        /// <summary>
        /// Verilen bir turun (þehir sýralamasýnýn) uzunluðunu (toplam mesafeyi) hesaplar.
        /// </summary>
        /// <param name="dist">Mesafe matrisi.</param>
        /// <param name="tour">Þehir sýralamasýný içeren dizi (tur yolu).</param>
        /// <returns>Toplam mesafeyi (tur uzunluðunu) döndürür.</returns>
        public static double TourLength(double[,] dist, int[] tour)
        {
            double sum = 0.0;  // Baþlangýçta toplam mesafeyi sýfýrla
            int n = tour.Length;  // Turda kaç þehir olduðunu al

            // Turda sýrasýyla her iki þehir arasýndaki mesafeyi topla
            for (int i = 0; i < n - 1; i++)
                sum += dist[tour[i], tour[i + 1]];  // Her iki þehir arasýndaki mesafe

            // Turun son þehrinden ilk þehire olan mesafeyi de ekle (kapalý döngü)
            sum += dist[tour[n - 1], tour[0]];  // Son þehirden ilk þehire mesafe

            return sum;  // Toplam mesafeyi döndür
        }
    }
}
