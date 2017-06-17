using System;
using System.Collections.Generic;
using System.Linq;

namespace StarsGeneration
{
    // TODO use class instead of enum and apply rules inside it instead of matching value
    internal enum StarType
    {
        White = 1,
        Blue = 2,
        Yellow = 3,
        Red = 4
    }

    internal class Star
    {
        private const int MaximumPlanetCount = 5;
        private const int MinimumNumberOfPlanets = 1;

        public static IEnumerable<Star> GenerateStars(IEnumerable<(double, double)> starPositions)
        {
            return starPositions.Select(GenerateStar);
        }

        private static Star GenerateStar((double, double) arg)
        {
            var random = new Random();
            var starType = (StarType) random.Next(1, Enum.GetNames(typeof(StarType)).Length - 1);
            var planetCount = random.Next(MinimumNumberOfPlanets, MaximumPlanetCount + 1);
            var star = new Star();
            for (var orderFromStar = 0; orderFromStar < planetCount; orderFromStar++)
            {
                var planet = Planet.GeneratePlanet(star, starType, orderFromStar);
                star.AddPlanet(orderFromStar, planet);
            }
            return star;
        }

        public IReadOnlyList<Planet> Planets => _planets;

        private readonly List<Planet> _planets = new List<Planet>();

        public void AddPlanet(int orderFromStar, Planet planet)
        {
            if(_planets.ElementAtOrDefault(orderFromStar) != null)
                throw new ArgumentException($"Planet at {orderFromStar} already exists");
            _planets.Insert(orderFromStar, planet);
        }
    }
}