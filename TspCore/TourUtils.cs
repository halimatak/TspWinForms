using System;

namespace TspCore
{
    public static class TourUtils
    {
        /// <summary>
        /// �ehirlerin s�ral� bir turunu olu�turur (0, 1, 2, ..., n-1).
        /// </summary>
        /// <param name="n">�ehir say�s�.</param>
        /// <returns>S�ral� bir tur yolu d�nd�r�r (�rne�in: [0, 1, 2, ..., n-1]).</returns>
        public static int[] IdentityTour(int n)
        {
            var t = new int[n];

            // S�ral� bir dizi olu�tur (0, 1, 2, ..., n-1)
            for (int i = 0; i < n; i++)
                t[i] = i;

            return t;
        }

        /// <summary>
        /// Verilen turu (�ehir s�ras�n�) kopyalar.
        /// </summary>
        /// <param name="tour">Kopyalanacak mevcut tur.</param>
        /// <returns>Yeni bir kopyalanm�� tur dizisi.</returns>
        public static int[] Copy(int[] tour)
        {
            var t = new int[tour.Length];  // Yeni bir dizi olu�tur
            Array.Copy(tour, t, tour.Length);  // Mevcut turu kopyala
            return t;  // Yeni diziyi d�nd�r
        }

        /// <summary>
        /// Verilen diziyi rastgele kar��t�r�r (Fisher-Yates algoritmas�).
        /// </summary>
        /// <typeparam name="T">Dizinin eleman tipi.</typeparam>
        /// <param name="array">Kar��t�r�lacak dizi.</param>
        /// <param name="rng">Rastgele say� �reteci.</param>
        public static void Shuffle<T>(T[] array, Random rng)
        {
            // Fisher-Yates algoritmas� ile diziyi kar��t�r�r
            for (int i = array.Length - 1; i > 0; i--)
            {
                // [0, i] aras�nda rastgele bir indeks se�
                int j = rng.Next(i + 1);
                // Elemanlar� takas yap
                var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
            }
        }

        /// <summary>
        /// Two-Opt algoritmas�n� uygular. Verilen iki �ehir aras�ndaki yolu ters �evirir.
        /// </summary>
        /// <param name="tour">�yile�tirilecek mevcut tur.</param>
        /// <param name="i">�lk �ehir indeksini belirtir (ba�lang�� noktas�).</param>
        /// <param name="k">Son �ehir indeksini belirtir (biti� noktas�).</param>
        /// <returns>Yeni olu�turulmu� iyile�tirilmi� turu d�nd�r�r.</returns>
        public static int[] TwoOptSwap(int[] tour, int i, int k)
        {
            var n = tour.Length;  // Toplam �ehir say�s�n� al
            var newTour = new int[n];  // Yeni bir tur dizisi olu�tur

            // [0, i) aras�ndaki �ehirleri aynen kopyala
            for (int c = 0; c < i; c++)
                newTour[c] = tour[c];

            int dec = 0;

            // [i, k] aras�ndaki �ehirleri ters s�rayla kopyala (Two-Opt)
            for (int c = i; c <= k; c++)
            {
                newTour[c] = tour[k - dec];
                dec++;
            }

            // [k+1, n) aras�ndaki �ehirleri aynen kopyala
            for (int c = k + 1; c < n; c++)
                newTour[c] = tour[c];

            return newTour;  // Yeni iyile�tirilmi� turu d�nd�r
        }

        /// <summary>
        /// Verilen bir tur yolunun uzunlu�unu hesaplar.
        /// </summary>
        /// <param name="dist">�ehirler aras� mesafeleri i�eren matris.</param>
        /// <param name="tour">�ehir s�ralamas� (tur yolu).</param>
        /// <returns>Toplam tur uzunlu�unu (mesafesini) d�nd�r�r.</returns>
        public static double Evaluate(double[,] dist, int[] tour) => DistanceMatrix.TourLength(dist, tour);
    }
}
