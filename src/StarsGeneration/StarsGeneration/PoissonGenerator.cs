using System;
using System.Collections.Generic;
using static System.Math;

namespace StarsGeneration
{
    internal class PoissonGenerator
    {
        private readonly Random _random = new Random();

        public IEnumerable<(double, double)> GeneratePoisson(
            int width,
            int height,
            double minDist,
            int newPointsCount)
        {
            //Create the grid
            var cellSize = minDist / Sqrt(2);

            var grid = new Grid2D(
                (int) Ceiling(width / cellSize),
                (int) Ceiling(height / cellSize));

            //RandomQueue works like a queue, except that it
            //pops a random element from the queue instead of
            //the element at the head of the queue
            var processList = new RandomQueue<(double, double)>();

            //generate the first point randomly
            //and updates 
            var firstPoint = (_random.Next(width), _random.Next(height));

            //update containers
            processList.Enqueue(firstPoint);
            grid[ImageToGrid(firstPoint, cellSize)] = firstPoint;

            //generate other points from points in queue.
            while (!processList.IsEmpty())
            {
                var point = processList.Dequeue();
                for (var i = 0; i < newPointsCount; i++)
                {
                    var newPoint = GenerateRandomPointAround(point, minDist);
                    //check that the point is in the image region
                    //and no points exists in the point's neighbourhood
                    if (InRectangle(width, height, newPoint) &&
                        !InNeighbourhood(grid, newPoint, minDist, cellSize))
                    {
                        //update containers
                        processList.Enqueue(newPoint);
                        var gridCoords = ImageToGrid(newPoint, cellSize);
                        grid[gridCoords] = newPoint;
                        yield return newPoint;
                    }
                }
            }
        }

        private (int x, int y) ImageToGrid((double X, double Y) point, double cellSize)
        {
            var gridX = (int) (point.X / cellSize);
            var gridY = (int) (point.Y / cellSize);
            return (gridX, gridY);
        }

        private bool InRectangle(int width, int height, (double x, double y) newPoint)
        {
            return newPoint.x < width
                   && newPoint.y < height
                   && newPoint.x > 0
                   && newPoint.y > 0;
        }

        private bool InNeighbourhood(Grid2D grid, (double, double) point, double minDist, double cellSize)
        {
            //get the neighbourhood if the point in the grid
            var cellsAroundPoint = SquareAroundPoint(grid, ImageToGrid(point, cellSize), 5);
            foreach (var cell in cellsAroundPoint)
                if (Distance(cell, point) < minDist)
                    return true;
            return false;
        }

        private double Distance((double x, double y) point1, (double x, double y) point2)
        {
            return Sqrt(Pow(point1.x - point2.x, 2) + Pow(point1.y - point2.y, 2));
        }

        private IEnumerable<(double, double)> SquareAroundPoint(Grid2D grid, (int x, int y) gridPoint,
            int diameter)
        {
            for (var i = gridPoint.x - diameter / 2; i < gridPoint.x + diameter / 2; ++i)
            for (var j = gridPoint.y - diameter / 2; j < gridPoint.y + diameter / 2; ++j)
                if (i >= 0 && j >= 0 && i < grid.Width && j < grid.Height)
                    yield return grid[i, j];
        }

        private (double x, double y) GenerateRandomPointAround((double X, double Y) point, double minDist)
        {
            //non-uniform, favours points closer to the inner ring, leads to denser packings
            var r1 = _random.NextDouble(); //random point between 0 and 1
            var r2 = _random.NextDouble();
            //random radius between mindist and 2 * mindist
            var radius = minDist * (r1 + 1);
            //random angle
            var angle = 2 * PI * r2;
            //the new point is generated around the point (x, y)
            var newX = point.X + radius * Cos(angle);
            var newY = point.Y + radius * Sin(angle);
            return (newX, newY);
        }
    }

    internal class RandomQueue<TValueType>
    {
        private readonly List<TValueType> _internalQueue = new List<TValueType>();
        private readonly Random _random = new Random();

        public void Enqueue(TValueType value)
        {
            _internalQueue.Add(value);
        }

        public TValueType Dequeue()
        {
            if (_internalQueue.Count == 0)
                throw new InvalidOperationException("Empty queue");
            var index = _random.Next(_internalQueue.Count - 1);
            var item = _internalQueue[index];

            _internalQueue.RemoveAt(index);
            return item;
        }

        public bool IsEmpty()
        {
            return _internalQueue.Count == 0;
        }
    }

    internal class Grid2D
    {
        private readonly (double, double)[,] _grid;

        public Grid2D(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new (double, double)
                [Width, Height];
        }

        public int Width { get; }
        public int Height { get; }


        public (double, double) this[(int x, int y) point]
        {
            set => _grid[point.x, point.y] = value;
        }

        public (double, double) this[int x, int y] => _grid[x, y];
    }
}