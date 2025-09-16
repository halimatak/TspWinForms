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
        private TspInstance _instance;
        private int[] _bestTour;
        private double _bestLen;
        private readonly object _lock = new object();

        private CancellationTokenSource _ctsExact;
        private CancellationTokenSource _ctsGA;

        private int _seed = 123;
        public TspWinForms()
        {
            InitializeComponent();
            UpdateLabels();
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


            nudGenN.Minimum = 3;
            nudGenN.Maximum = 500;
            nudGenN.Value = 50;
        }

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

        private void UpdateExactEnabled()
        {
            if (_instance == null) 
            {
                gbExactSolver.Enabled = false; 
                return; 
            }
            int n = _instance.N;
            btnExactRun.Enabled = n <= 11;
        }

        private void btnLoadPoints_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var pts = File.ReadAllLines(ofd.FileName)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .Select(l => l.Trim())
                            .Select(l => l.Split(','))
                            .Select(a => new Point2D(int.Parse(a[0]), double.Parse(a[1]), double.Parse(a[2])))
                            .ToArray();
                        _instance = new TspInstance(pts);
                        _bestTour = null;
                        _bestLen = double.NaN;
                        UpdateLabels();
                        pictureBox1.Invalidate();
                        UpdateExactEnabled();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("CSV okuma hatası: " + ex.Message);
                    }
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int n = (int)nudGenN.Value;
            int seed = (int)nudGenSeed.Value;
            var rng = new Random(seed);
            var pts = new Point2D[n];
            for (int i = 0; i < n; i++)
            {
                pts[i] = new Point2D(i, rng.Next(0, 1000), rng.Next(0, 1000));
            }
            _instance = new TspInstance(pts);
            _bestTour = null;
            _bestLen = double.NaN;
            UpdateLabels();
            pictureBox1.Invalidate();
            UpdateExactEnabled();
        }

        private void btnExactRun_Click(object sender, EventArgs e)
        {
            if (_instance == null) 
            { 
                MessageBox.Show("Load/generate data first."); 
                return; 
            }
            _ctsExact?.Cancel();
            _ctsExact = new CancellationTokenSource();
            var token = _ctsExact.Token;

            lblSolver.Text = "Exact";
            var solver = new ExactSolver(_instance);
            var sw = Stopwatch.StartNew();
            var progress = new Progress<double>(best =>
            {
                lock (_lock)
                {
                    _bestLen = best;
                }
                lblProgress.Text = $"Best of {best:F2}";
                pictureBox1.Invalidate();
            });

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

        private void btnExactCancel_Click(object sender, EventArgs e)
        {
            _ctsExact?.Cancel();
            lblSolver.Text = "Exact";
        }

        private void btnRunGa_Click(object sender, EventArgs e)
        {
            if (_instance == null) 
            { 
                MessageBox.Show("Load/generate data first."); 
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
            var progress = new Progress<Tuple<int, double>>(tup =>
            {
                lblProgress.Text = $"{tup.Item1}: Best of {tup.Item2:F2}";
                pictureBox1.Invalidate();
            });

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

        private void btnGaCancel_Click(object sender, EventArgs e)
        {
            _ctsGA?.Cancel();
            lblSolver.Text = "GA";
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            
            if (_instance == null) 
                return;

            var pts = _instance.Points;

            if (!pts.Any())
            {
                lblBest.Text = "-";
                lblElapsed.Text = "-";
                lblSolver.Text = "-";
                lblProgress.Text = "-";
                MessageBox.Show("Check data entries");
                return;
            }

            double minX = pts.Min(p => p.X);
            double maxX = pts.Max(p => p.X);
            double minY = pts.Min(p => p.Y);
            double maxY = pts.Max(p => p.Y);
            double padding = 20;

            var w = pictureBox1.ClientSize.Width;
            var h = pictureBox1.ClientSize.Height;
            double scaleX = (w - 2 * padding) / Math.Max(1, (maxX - minX));
            double scaleY = (h - 2 * padding) / Math.Max(1, (maxY - minY));
            double scale = Math.Min(scaleX, scaleY);

            Func<Point2D, PointF> map = p => new PointF(
                (float)(padding + (p.X - minX) * scale),
                (float)(padding + (p.Y - minY) * scale)
            );

            if (_bestTour != null)
            {
                using (var pen = new Pen(Color.DarkBlue, 2))
                {
                    for (int i = 0; i < _bestTour.Length; i++)
                    {
                        int a = _bestTour[i];
                        int b = _bestTour[(i + 1) % _bestTour.Length];
                        e.Graphics.DrawLine(pen, map(pts[a]), map(pts[b]));
                    }
                }
            }

            using (var brush = new SolidBrush(Color.Red))
            {
                foreach (var p in pts)
                {
                    var pt = map(p);
                    e.Graphics.FillEllipse(brush, pt.X - 3, pt.Y - 3, 6, 6);
                }
            }
        }
    }
}
