using System;

namespace TspCore
{
    /// <summary>
    /// 2D uzayda bir noktan�n koordinatlar�n� ve kimlik bilgisini tutar.
    /// </summary>
    [Serializable]  // Bu struct serile�tirilebilir (diskte saklanabilir)
    public struct Point2D
    {
        public int Id;      // Noktan�n kimlik numaras� (genellikle �ehir numaras� olarak kullan�l�r)
        public double X;    // Noktan�n X koordinat�
        public double Y;    // Noktan�n Y koordinat�

        /// <summary>
        /// Point2D yap�c�s�, id, x ve y koordinatlar�n� al�r.
        /// </summary>
        /// <param name="id">Noktan�n kimlik numaras�</param>
        /// <param name="x">X koordinat�</param>
        /// <param name="y">Y koordinat�</param>
        public Point2D(int id, double x, double y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Point2D nesnesini string olarak d�nd�r�r.
        /// </summary>
        /// <returns>String format�nda nokta bilgisi</returns>
        public override string ToString() => $"{Id}({X},{Y})";  // "Id(X,Y)" format�nda d�ner
    }

    /// <summary>
    /// TSP �rne�ini temsil eder. �ehirler ve aralar�ndaki mesafeleri i�erir.
    /// </summary>
    [Serializable]  // Bu s�n�f serile�tirilebilir
    public class TspInstance
    {
        public Point2D[] Points { get; private set; }  // �ehirlerin koordinatlar�n� i�eren dizi
        public double[,] Dist { get; private set; }   // �ehirler aras� mesafeleri i�eren matris
        public int N => Points.Length;  // �ehir say�s�n� d�nd�r�r (Points dizisinin uzunlu�u)

        /// <summary>
        /// TspInstance yap�c�s�. �ehirlerin koordinatlar�n� ve iste�e ba�l� olarak mesafe matrisini ba�lat�r.
        /// </summary>
        /// <param name="points">�ehirlerin koordinatlar�n� i�eren dizi</param>
        /// <param name="precomputeDist">Mesafe matrisini �nceden hesapla (varsay�lan: true)</param>
        public TspInstance(Point2D[] points, bool precomputeDist = true)
        {
            Points = points ?? throw new ArgumentNullException(nameof(points));  // E�er city dizisi null ise hata f�rlat
            if (precomputeDist)
                Dist = DistanceMatrix.Build(Points);  // �ehirler aras� mesafeleri hesapla
        }
    }

    /// <summary>
    /// TSP ��z�m�n� temsil eder.
    /// </summary>
    public class TspResult
    {
        public int[] Tour;              // ��z�m turu (�ehir s�ralamas�)
        public double Length;           // Tur uzunlu�u (toplam mesafe)
        public TimeSpan Elapsed;        // ��z�m s�resi
        public string Solver;           // Kullan�lan ��z�m algoritmas�
        public bool IsSuccess;          // ��z�m ba�ar�l� m�?
    }
}
