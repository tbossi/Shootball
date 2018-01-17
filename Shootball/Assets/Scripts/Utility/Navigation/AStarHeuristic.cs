namespace Shootball.Utility.Navigation
{
    public abstract class AStarHeuristic<T>
    {
        public abstract float GetHeuristicScore(T start, T end);
    }
}
