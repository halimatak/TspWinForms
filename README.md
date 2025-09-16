README — TSP WinForms (.NET Framework 4.8)
==========================================

Nasıl derlenir/çalıştırılır
---------------------------
1) Visual Studio 2019/2022 ve .NET Framework 4.8 yüklü olmalı.
2) `TspWinFormsSolution.sln` dosyasını açın.
3) Startup Project: **TspWinForms**.
4) Build & Run (F5).

Ne var içinde?
--------------
- **TspCore** (Class Library): 
  - `ExactSolver` — küçük N için Branch & Bound (başlangıç 0'a sabit, hafif alt sınırla budama).
  - `GASolver` — büyük N için Genetik Algoritma (Order Crossover (OX), swap mutasyon, turnuva seçimi, elitizm).
  - `DistanceMatrix` — Öklid uzaklık matrisi + tur uzunluğu.
  - `TourUtils` — tur yardımcıları.
  - `Models` — veri modelleri.
- **TspWinForms** (WinForms App):
  - CSV yükle (`id,x,y`)
  - Rastgele nokta üret (N, Seed).
  - PictureBox çizim (noktalar + en iyi tur çizgileri).
  - Bilgi etiketleri: **N**, **Best Length**, **Elapsed**, **Solver**.
  - Exact paneli (N ≤ 11 olduğunda etkin): **Run Exact**, **Cancel**.
  - GA paneli: **Seed**, **Population**, **Generations**, **MutationRate**; **Run GA**, **Cancel**.
  - İlerleme etiketi: GA'da her ~10 jenarasyonda en iyi uzunluk.
  - Cancel temiz durdurma (CancellationToken).
  - Tüm RNG'ler verilen **Seed** ile deterministik.

Ticari/3. parti bağımlılık yoktur (yalnızca BCL).

Dosya formatı
-------------
CSV: `id,x,y` (başlık yok).
Örnekler `data/` klasöründe.

Trade-off'lar
-------------
- Exact çözüm Branch & Bound hafif bir alt sınır (her düğüm için en küçük çıkış kenarı toplamı) kullanır. N≈11'e kadar pratik.
- GA, OX crossover + basit swap mutasyon ile yeterli yakınsama sağlar; 2-opt gibi yerel arama eklenmedi (isteğe bağlı iyileştirilebilir).
- Başlangıcı 0'a sabit tutarak arama uzayını n!/n katına indirir ve görselleştirme/karşılaştırma kolaylaşır.
- Çizim, ölçekleme basit ve yeterli; kenar kalınlığı temel seviye.

Kullanım
--------
1) **Load Points...** ile `data/small10.csv` veya `data/med50.csv` yükleyin **ya da**
2) **Generate** ile N/Seed verip rastgele nokta üretin.
3) **Exact**: N ≤ 11 ise **Run Exact** aktif olur. Çalışırken **Cancel** ile durdurabilirsiniz.
4) **GA**: parametreleri girip **Run GA**'ya basın. İlerleme etiketi 10 jenerasyonda bir güncellenir. **Cancel** ile durdurabilirsiniz.
5) En iyi tur ve noktalar PictureBox'ta anlık çizilir.
