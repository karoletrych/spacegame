namespace StarsGeneration
{
    internal enum ColonistAssignment
    {
        None = 0,
        Farming = 1,
        Industry = 2
    }

    internal interface IRulerOrder
    {
    }

    internal class ChangeColonistsAssignmentOrder : IRulerOrder
    {
        public ChangeColonistsAssignmentOrder(
            ColonistAssignment from,
            ColonistAssignment to,
            int number)
        {
            From = from;
            To = to;
            Number = number;
        }

        public ColonistAssignment From { get; }
        public ColonistAssignment To { get; }
        public int Number { get; }
    }

    internal class SetCurrentBuildingOrder : IRulerOrder
    {
        public Building NewBuilding { get; }

        public SetCurrentBuildingOrder(
            Building newBuilding)
        {
            NewBuilding = newBuilding;
        }

    }
}