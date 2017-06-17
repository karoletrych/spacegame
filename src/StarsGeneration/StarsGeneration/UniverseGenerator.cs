using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StarsGenerator;

namespace StarsGeneration
{
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public class UniverseGenerator
    {
        private static readonly Random Random = new Random();
        private readonly NumericUpDown _minDistance;
        private readonly NumericUpDown _newPointsCount;
        private readonly PictureBox _pictureBox1;

        public UniverseGenerator(PictureBox pictureBox1, NumericUpDown minDistance,
            NumericUpDown newPointsCount)
        {
            _pictureBox1 = pictureBox1;
            _minDistance = minDistance;
            _newPointsCount = newPointsCount;
        }

        public void Paint(PaintEventArgs e)
        {
            var starPositions = GenerateStarPositions();
            var stars = Star.GenerateStars(starPositions);
            Console.WriteLine($"{starPositions.Count} stars generated.");

            foreach (var point in starPositions)
                Draw(e, point, Brushes.Black);
            for (var i = 0; i < 8; ++i)
            {
                var randomStar = starPositions[Random.Next(starPositions.Count)];
                Draw(e, randomStar, PickBrush());
            }
        }


        private List<ValueTuple<double, double>> GenerateStarPositions()
        {
            var generator = new PoissonGenerator();
            var points = generator
                .GeneratePoisson(_pictureBox1.Width,
                    _pictureBox1.Height,
                    (double) _minDistance.Value,
                    (int) _newPointsCount.Value).ToList();
            return points;
        }

        private static void Draw(PaintEventArgs e, ValueTuple<double, double> point, Brush color)
        {
            e.Graphics.FillRectangle(
                color,
                (float) point.Item1,
                (float) point.Item2,
                5,
                5);
        }

        private Brush PickBrush()
        {
            var brushesType = typeof(Brushes);
            var properties = brushesType.GetProperties();
            var random = Random.Next(properties.Length);
            var result = (Brush) properties[random].GetValue(null, null);
            return result;
        }
    }
}