using System;

namespace TspCore
{
    public static class TourUtils
    {
        /// <summary>
        /// Þehirlerin sýralý bir turunu oluþturur (0, 1, 2, ..., n-1).
        /// </summary>
        /// <param name="n">Þehir sayýsý.</param>
        /// <returns>Sýralý bir tur yolu döndürür (örneðin: [0, 1, 2, ..., n-1]).</returns>
        public static int[] IdentityTour(int n)
        {
            var t = new int[n];

            // Sýralý bir dizi oluþtur (0, 1, 2, ..., n-1)
            for (int i = 0; i < n; i++)
                t[i] = i;

            return t;
        }

        /// <summary>
        /// Verilen turu (þehir sýrasýný) kopyalar.
        /// </summary>
        /// <param name="tour">Kopyalanacak mevcut tur.</param>
        /// <returns>Yeni bir kopyalanmýþ tur dizisi.</returns>
        public static int[] Copy(int[] tour)
        {
            var t = new int[tour.Length];  // Yeni bir dizi oluþtur
            Array.Copy(tour, t, tour.Length);  // Mevcut turu kopyala
            return t;  // Yeni diziyi döndür
        }

        /// <summary>
        /// Verilen diziyi rastgele karýþtýrýr (Fisher-Yates algoritmasý).
        /// </summary>
        /// <typeparam name="T">Dizinin eleman tipi.</typeparam>
        /// <param name="array">Karýþtýrýlacak dizi.</param>
        /// <param name="rng">Rastgele sayý üreteci.</param>
        public static void Shuffle<T>(T[] array, Random rng)
        {
            // Fisher-Yates algoritmasý ile diziyi karýþtýrýr
            for (int i = array.Length - 1; i > 0; i--)
            {
                // [0, i] arasýnda rastgele bir indeks seç
                int j = rng.Next(i + 1);
                // Elemanlarý takas yap
                var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
            }
        }

        /// <summary>
        /// Two-Opt algoritmasýný uygular. Verilen iki þehir arasýndaki yolu ters çevirir.
        /// </summary>
        /// <param name="tour">Ýyileþtirilecek mevcut tur.</param>
        /// <param name="i">Ýlk þehir indeksini belirtir (baþlangýç noktasý).</param>
        /// <param name="k">Son þehir indeksini belirtir (bitiþ noktasý).</param>
        /// <returns>Yeni oluþturulmuþ iyileþtirilmiþ turu döndürür.</returns>
        public static int[] TwoOptSwap(int[] tour, int i, int k)
        {
            var n = tour.Length;  // Toplam þehir sayýsýný al
            var newTour = new int[n];  // Yeni bir tur dizisi oluþtur

            // [0, i) arasýndaki þehirleri aynen kopyala
            for (int c = 0; c < i; c++)
                newTour[c] = tour[c];

            int dec = 0;

            // [i, k] arasýndaki þehirleri ters sýrayla kopyala (Two-Opt)
            for (int c = i; c <= k; c++)
            {
                newTour[c] = tour[k - dec];
                dec++;
            }

            // [k+1, n) arasýndaki þehirleri aynen kopyala
            for (int c = k + 1; c < n; c++)
                newTour[c] = tour[c];

            return newTour;  // Yeni iyileþtirilmiþ turu döndür
        }

        /// <summary>
        /// Verilen bir tur yolunun uzunluðunu hesaplar.
        /// </summary>
        /// <param name="dist">Þehirler arasý mesafeleri içeren matris.</param>
        /// <param name="tour">Þehir sýralamasý (tur yolu).</param>
        /// <returns>Toplam tur uzunluðunu (mesafesini) döndürür.</returns>
        public static double Evaluate(double[,] dist, int[] tour) => DistanceMatrix.TourLength(dist, tour);
    }
}
