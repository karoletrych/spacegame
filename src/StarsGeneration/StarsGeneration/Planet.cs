using System;
using System.Collections.Generic;

namespace StarsGeneration
{
    enum PlanetClass
    {
        None = 0,
        Terran = 1,
        Barren = 2,
        Arid = 3,
        Aquatic = 4,
        GasGiant = 5
    }

    interface IPlanetView
    {
        int MaxPopulation { get; }
        int NaturalGrowthRate { get; }
        int IndustryPerPopulation { get; }
        int FarmingPerPopulation { get; }
        int Population { get; }
        int Industry { get; }
        int Farming { get; }
        int IndustryPopulation { get;}
        int FarmingPopulation { get;}
        Building CurrentBuildingBeingBuilt { get; }
        int AccumulatedIndustry { get; }
    }

    interface IPlanetSimulation : IPlanetView
    {
        void NextTurn();
    }

    interface IRulerPlanetOrderListener
    {
        void ApplyAction(IRulerOrder rulerOrder);
    }

    class Planet : IPlanetSimulation, IRulerPlanetOrderListener
    {
        private const int MinimumMaxPopulation = 1;
        private const int MaximumMaxPopulation = 10;
        private const int GrowthRateConst = 1;

        private Planet(int maxPopulation, int ipp, int fpp, int naturalGrowthRate)
        {
            MaxPopulation = maxPopulation;
            IndustryPerPopulation = ipp;
            FarmingPerPopulation = fpp;
            NaturalGrowthRate = naturalGrowthRate;
        }

        public int MaxPopulation { get; }
        public int NaturalGrowthRate { get; }
        public int IndustryPerPopulation { get; }
        public int FarmingPerPopulation { get; }
        public int Population { get; private set; }
        public int Industry => IndustryPerPopulation * IndustryPopulation;
        public int Farming => FarmingPerPopulation * FarmingPopulation;
        public int IndustryPopulation { get; private set; }
        public int FarmingPopulation { get; private set; }
        public Building CurrentBuildingBeingBuilt { get; private set; }
        public int AccumulatedIndustry { get; private set; }

        //TODO planetOrderFromStar is pointless
        public static Planet GeneratePlanet(Star star, StarType starType, int planetOrderFromStar)
        {
            var random = new Random();
            var ipp = random.Next(3);
            var fpp = random.Next(3);
            var maxPopulation = random.Next(MinimumMaxPopulation, MaximumMaxPopulation);
            switch (starType)
            {
                case StarType.White:
                    ipp+=2;
                    fpp-=2;
                    break;
                case StarType.Blue:
                    ++ipp;
                    --fpp;
                    break;
                case StarType.Yellow:
                    --ipp;
                    ++fpp;
                    break;
                case StarType.Red:
                    ipp -= 2;
                    fpp -= 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(starType), starType, null);
            }
            return new Planet(maxPopulation, ipp, fpp, GrowthRateConst);
        }

        public void NextTurn()
        {
            if (Population < MaxPopulation)
                Population += NaturalGrowthRate;
            AccumulatedIndustry += Industry;
//            if (AccumulatedIndustry >= IndustryNecessaryForBuilding)
//            {
//                RaiseBuildingBuiltEvent;
//            }
        }

        public void ApplyAction(IRulerOrder rulerOrder)
        {
            switch (rulerOrder)
            {
                case ChangeColonistsAssignmentOrder order:
                    switch (order.From)
                    {
                        case ColonistAssignment.Farming:
                            FarmingPopulation -= order.Number;
                            break;
                        case ColonistAssignment.Industry:
                            IndustryPopulation -= order.Number;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    switch (order.To)
                    {
                        case ColonistAssignment.Farming:
                            FarmingPopulation += order.Number;
                            break;
                        case ColonistAssignment.Industry:
                            IndustryPopulation += order.Number;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case SetCurrentBuildingOrder order:
                    CurrentBuildingBeingBuilt = order.NewBuilding;
                    break;
            }
        }
    }
}
