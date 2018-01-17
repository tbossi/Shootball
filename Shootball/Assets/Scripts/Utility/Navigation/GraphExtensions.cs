using System.Collections.Generic;

namespace Shootball.Utility.Navigation
{
    public static class GraphExtensions
    {
        public static List<T> FindAStarPath<T>(this Graph<T> graph, T start, T end, AStarHeuristic<T> heuristic)
        {
            var pathFinder = new AStarPathFinder<T>(heuristic, graph);
            return pathFinder.FindPath(start, end);
        }

        public static T FindNearestNode<T>(this Graph<T> graph, T nearPoint, AStarHeuristic<T> heuristic)
        {
            var nodes = graph.GetNodes();
            float distance = float.PositiveInfinity;
            var result = default(T);
            foreach (var node in nodes)
            {
                var heuristicDistance = heuristic.GetHeuristicScore(nearPoint, node);
                if (distance > heuristicDistance)
                {
                    distance = heuristicDistance;
                    result = node;
                }
            }
            return result;
        }
    }
}
