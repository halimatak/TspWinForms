using System;

namespace TspCore
{
    /// <summary>
    /// 2D uzayda bir noktanýn koordinatlarýný ve kimlik bilgisini tutar.
    /// </summary>
    [Serializable]  // Bu struct serileþtirilebilir (diskte saklanabilir)
    public struct Point2D
    {
        public int Id;      // Noktanýn kimlik numarasý (genellikle þehir numarasý olarak kullanýlýr)
        public double X;    // Noktanýn X koordinatý
        public double Y;    // Noktanýn Y koordinatý

        /// <summary>
        /// Point2D yapýcýsý, id, x ve y koordinatlarýný alýr.
        /// </summary>
        /// <param name="id">Noktanýn kimlik numarasý</param>
        /// <param name="x">X koordinatý</param>
        /// <param name="y">Y koordinatý</param>
        public Point2D(int id, double x, double y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Point2D nesnesini string olarak döndürür.
        /// </summary>
        /// <returns>String formatýnda nokta bilgisi</returns>
        public override string ToString() => $"{Id}({X},{Y})";  // "Id(X,Y)" formatýnda döner
    }

    /// <summary>
    /// TSP örneðini temsil eder. Þehirler ve aralarýndaki mesafeleri içerir.
    /// </summary>
    [Serializable]  // Bu sýnýf serileþtirilebilir
    public class TspInstance
    {
        public Point2D[] Points { get; private set; }  // Þehirlerin koordinatlarýný içeren dizi
        public double[,] Dist { get; private set; }   // Þehirler arasý mesafeleri içeren matris
        public int N => Points.Length;  // Þehir sayýsýný döndürür (Points dizisinin uzunluðu)

        /// <summary>
        /// TspInstance yapýcýsý. Þehirlerin koordinatlarýný ve isteðe baðlý olarak mesafe matrisini baþlatýr.
        /// </summary>
        /// <param name="points">Þehirlerin koordinatlarýný içeren dizi</param>
        /// <param name="precomputeDist">Mesafe matrisini önceden hesapla (varsayýlan: true)</param>
        public TspInstance(Point2D[] points, bool precomputeDist = true)
        {
            Points = points ?? throw new ArgumentNullException(nameof(points));  // Eðer city dizisi null ise hata fýrlat
            if (precomputeDist)
                Dist = DistanceMatrix.Build(Points);  // Þehirler arasý mesafeleri hesapla
        }
    }

    /// <summary>
    /// TSP çözümünü temsil eder.
    /// </summary>
    public class TspResult
    {
        public int[] Tour;              // Çözüm turu (þehir sýralamasý)
        public double Length;           // Tur uzunluðu (toplam mesafe)
        public TimeSpan Elapsed;        // Çözüm süresi
        public string Solver;           // Kullanýlan çözüm algoritmasý
        public bool IsSuccess;          // Çözüm baþarýlý mý?
    }
}
