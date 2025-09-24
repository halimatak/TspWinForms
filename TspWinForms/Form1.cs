using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TspCore;

namespace TspWinForms
{
    public partial class TspWinForms : Form
    {
        private TspInstance _instance;        // TSP örneği (şehirlere ve mesafeye dair bilgi)
        private int[] _bestTour;              // En iyi tur yolu
        private double _bestLen;              // En iyi tur uzunluğu
        private readonly object _lock = new object(); // Çözümdeki verileri güvenli bir şekilde paylaşmak için kilit

        private CancellationTokenSource _ctsExact;  // ExactSolver için iptal token
        private CancellationTokenSource _ctsGA;     // GASolver için iptal token

        public TspWinForms()
        {
            InitializeComponent();

            // UI bileşenlerinin maksimum ve minimum değerlerini ayarlama
            nudGenSeed.Maximum = 1000000;
            nudGASeed.Maximum = 1000000;
            nudGen.Minimum = 1;
            nudGen.Maximum = 20000;
            nudPop.Minimum = 10;
            nudPop.Maximum = 10000;
            nudMut.DecimalPlaces = 2;
            nudMut.Increment = 0.01M;
            nudMut.Minimum = 0;
            nudMut.Maximum = 1;
            nudGenN.Minimum = 10;
            nudGenN.Maximum = 200;
        }

        /// <summary>
        /// Arayüzdeki etiketleri günceller.
        /// </summary>
        private void UpdateLabels()
        {
            if (_instance == null)
            {
                lblN.Text = "-";
                lblBest.Text = "-";
                lblElapsed.Text = "-";
                lblSolver.Text = "-";
                lblProgress.Text = "-";
            }
            else
            {
                lblN.Text = $"{_instance.N}";
                lblBest.Text = double.IsNaN(_bestLen) ? "-" : $"{_bestLen:F2}";
            }
        }

        /// <summary>
        /// Exact Solver için butonun etkinlik durumunu ayarlar.
        /// </summary>
        private void UpdateExactEnabled()
        {
            if (_instance == null)
            {
                gbExactSolver.Enabled = false;
                return;
            }
            int n = _instance.N;
            btnExactRun.Enabled = n <= 11;  // Exact Solver, 11'den fazla şehir için çalışmaz
        }

        /// <summary>
        /// Kullanıcıdan şehir verilerini yükler (CSV dosyasından).
        /// </summary>
        private void btnLoadPoints_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // CSV dosyasını okuyup şehirleri yükler
                        var pts = File.ReadAllLines(ofd.FileName)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .Select(l => l.Trim())
                            .Select(l => l.Split(','))
                            .Select(a => new Point2D(int.Parse(a[0]), double.Parse(a[1]), double.Parse(a[2])))
                            .ToArray();
                        _instance = new TspInstance(pts);  // Yeni TspInstance oluştur
                        _bestTour = null;
                        _bestLen = double.NaN;
                        UpdateLabels();  // Etiketleri güncelle
                        pictureBox1.Invalidate();  // Grafik alanını yeniden çiz
                        UpdateExactEnabled();  // Exact Solver'ı etkinleştir
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("CSV reading error: " + ex.Message);  // Hata mesajı göster
                    }
                }
            }
        }

        /// <summary>
        /// Rastgele şehirler üretir.
        /// </summary>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int n = (int)nudGenN.Value;
            int seed = (int)nudGenSeed.Value;
            var rng = new Random(seed);
            var pts = new Point2D[n];
            for (int i = 0; i < n; i++)
            {
                pts[i] = new Point2D(i, rng.Next(0, 1000), rng.Next(0, 1000));  // Rastgele şehirler üret
            }
            _instance = new TspInstance(pts);  // Yeni TspInstance oluştur
            _bestTour = null;
            _bestLen = double.NaN;
            UpdateLabels();
            pictureBox1.Invalidate();
            UpdateExactEnabled();
        }

        /// <summary>
        /// Exact Solver'ı çalıştırır ve sonucu gösterir.
        /// </summary>
        private void btnExactRun_Click(object sender, EventArgs e)
        {
            if (_instance == null)
            {
                MessageBox.Show("First load/generate data");
                return;
            }
            _ctsExact?.Cancel();
            _ctsExact = new CancellationTokenSource();
            var token = _ctsExact.Token;

            lblSolver.Text = "Exact";
            var solver = new ExactSolver(_instance);
            var sw = Stopwatch.StartNew();
            var progress = GetProgress<double>();

            Task.Run(() =>
            {
                var res = solver.Solve(token, progress);

                if (!res.IsSuccess)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        lblBest.Text = "-";
                        lblElapsed.Text = "-";
                        lblSolver.Text = "-";
                        lblProgress.Text = "-";
                    }));
                    MessageBox.Show("Check data entries");
                    return;
                }

                if (!token.IsCancellationRequested)
                {
                    lock (_lock)
                    {
                        _bestTour = res.Tour;
                        _bestLen = res.Length;
                    }
                    this.BeginInvoke(new Action(() =>
                    {
                        lblBest.Text = $"{res.Length:F2}";
                        lblElapsed.Text = $"{res.Elapsed}";
                        lblSolver.Text = "Exact";
                        pictureBox1.Invalidate();
                    }));
                }
            });
        }

        /// <summary>
        /// Exact Solver'ı iptal eder.
        /// </summary>
        private void btnExactCancel_Click(object sender, EventArgs e) => _ctsExact?.Cancel();

        /// <summary>
        /// İlerleme durumunu günceller.
        /// </summary>
        private IProgress<T> GetProgress<T>()
        {
            return new Progress<T>(t =>
            {
                if (t is Tuple<int, double> tup)
                    lblProgress.Text = $"Generation: {tup.Item1}, Best length: {tup.Item2:F2}";
                else if (t is double d)
                {
                    lock (_lock)
                    {
                        _bestLen = d;
                    }
                    lblProgress.Text = $"{d:F2}";
                }
                pictureBox1.Invalidate();
            });
        }

        /// <summary>
        /// Genetik algoritmayı çalıştırır ve sonucu gösterir.
        /// </summary>
        private void btnRunGa_Click(object sender, EventArgs e)
        {
            if (_instance == null)
            {
                MessageBox.Show("First load/generate data");
                return;
            }
            _ctsGA?.Cancel();
            _ctsGA = new CancellationTokenSource();
            var token = _ctsGA.Token;

            int seed = (int)nudGASeed.Value;
            int pop = (int)nudPop.Value;
            int gens = (int)nudGen.Value;
            double mut = (double)nudMut.Value;

            var solver = new GASolver(_instance, seed)
            {
                Population = pop,
                Generations = gens,
                MutationRate = mut,
                Elites = 2,
                TournamentSize = 3
            };

            lblSolver.Text = "GA";
            var progress = GetProgress<Tuple<int, double>>();

            Task.Run(() =>
            {
                var res = solver.Solve(token, progress);
                if (!res.IsSuccess)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        lblBest.Text = "-";
                        lblElapsed.Text = "-";
                        lblSolver.Text = "-";
                        lblProgress.Text = "-";
                    }));

                    MessageBox.Show("Check data entries");
                    return;
                }

                if (!token.IsCancellationRequested)
                {
                    lock (_lock)
                    {
                        _bestTour = res.Tour;
                        _bestLen = res.Length;
                    }
                    this.BeginInvoke(new Action(() =>
                    {
                        lblBest.Text = $"{res.Length:F2}";
                        lblElapsed.Text = $"{res.Elapsed}";
                        lblSolver.Text = "GA";
                        pictureBox1.Invalidate();
                    }));
                }
            });
        }

        /// <summary>
        /// Genetik algoritmanın çalışmasını iptal eder.
        /// </summary>
        private void btnGaCancel_Click(object sender, EventArgs e) => _ctsGA?.Cancel();

        /// <summary>
        /// Grafik üzerine şehirlerin ve çözümün çizilmesi.
        /// </summary>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);

            if (_instance == null) return;

            var pts = _instance.Points;
            if (pts == null || pts.Length == 0)
            {
                lblBest.Text = "-";
                lblElapsed.Text = "-";
                lblSolver.Text = "-";
                lblProgress.Text = "-";
                return;
            }

            // Grafik için şehirlerin harita üzerinden konumlandırılması
            double minX = pts.Min(p => p.X);
            double maxX = pts.Max(p => p.X);
            double minY = pts.Min(p => p.Y);
            double maxY = pts.Max(p => p.Y);
            const double padding = 20;

            int w = pictureBox1.ClientSize.Width;
            int h = pictureBox1.ClientSize.Height;

            double scaleX = (w - 2 * padding) / Math.Max(1, (maxX - minX));
            double scaleY = (h - 2 * padding) / Math.Max(1, (maxY - minY));
            double scale = Math.Min(scaleX, scaleY);

            PointF Map(Point2D p) => new PointF((float)(padding + (p.X - minX) * scale), (float)(padding + (p.Y - minY) * scale));

            // En iyi tur varsa, onu çiz
            if (_bestTour != null)
            {
                using (var pen = new Pen(Color.DarkBlue, 2))
                {
                    for (int i = 0; i < _bestTour.Length; i++)
                    {
                        int a = _bestTour[i];
                        int b = _bestTour[(i + 1) % _bestTour.Length];
                        e.Graphics.DrawLine(pen, Map(pts[a]), Map(pts[b]));
                    }
                }
            }

            // Şehirleri çiz
            var font = this.Font;
            using (var dotBrush = new SolidBrush(Color.Red))
            {
                foreach (var p in pts)
                {
                    var pt = Map(p);
                    e.Graphics.FillEllipse(dotBrush, pt.X - 3, pt.Y - 3, 6, 6);

                    if (chkShowLocation.Checked)
                    {
                        string label = $"({p.X}, {p.Y})";

                        Size size = TextRenderer.MeasureText(label, font);

                        float tx = pt.X + 5;
                        float ty = pt.Y - size.Height - 4;

                        tx = Math.Max(2f, Math.Min(tx, w - size.Width - 2f));
                        ty = Math.Max(2f, Math.Min(ty, h - size.Height - 2f));

                        var rect = new Rectangle((int)tx - 2, (int)ty - 1, size.Width + 4, size.Height + 2);
                        using (var bgBrush = new SolidBrush(Color.FromArgb(100, Color.White)))
                            e.Graphics.FillRectangle(bgBrush, rect);

                        TextRenderer.DrawText(e.Graphics, label, font, new Point((int)tx, (int)ty), Color.Black);
                    }
                }
            }
        }

        /// <summary>
        /// Tohum değerini değiştirir.
        /// </summary>
        private void nudGenSeed_ValueChanged(object sender, EventArgs e) => nudGASeed.Value = nudGenSeed.Value;

        /// <summary>
        /// Form yüklendiğinde başlatıcı ayarları yapar.
        /// </summary>
        private void TspWinForms_Load(object sender, EventArgs e)
        {
            chkShowLocation.Checked = true;
            nudGenSeed.Value = 123;
            nudGenN.Value = 10;
            nudMut.Value = .05m;
            UpdateLabels();  // Etiketleri güncelle
        }

        private void chkShowLocation_CheckedChanged(object sender, EventArgs e) => pictureBox1.Invalidate();
    }
}
