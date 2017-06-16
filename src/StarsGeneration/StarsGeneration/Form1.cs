using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StarsGeneration
{
    public partial class Form1 : Form
    {
        private static readonly Random Random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var generator = new PoissonGenerator();
            var points = generator
                .GeneratePoisson(
                    width: pictureBox1.Width,
                    height: pictureBox1.Height,
                    minDist: (double)minDistance.Value,
                    newPointsCount: (int) newPointsCount.Value).ToList();

            foreach (var point in points)
                Draw(e, point, Brushes.Black);
            for (var i = 0; i < 8; ++i)
            {
                var randomStar = points[Random.Next(points.Count)];
                Draw(e, randomStar, PickBrush());
            }
        }

        private static void Draw(PaintEventArgs e, ValueTuple<double, double> point, Brush color)
        {
            e.Graphics.FillRectangle(
                brush: color,
                x: (float) point.Item1,
                y: (float) point.Item2,
                width: 5,
                height: 5);
        }

        private Brush PickBrush()
        {
            var brushesType = typeof(Brushes);
            var properties = brushesType.GetProperties();
            var random = Random.Next(properties.Length);
            var result = (Brush)properties[random].GetValue(null, null);
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
    }
}