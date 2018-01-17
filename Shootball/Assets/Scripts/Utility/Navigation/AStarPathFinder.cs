using System;
using System.Collections.Generic;
using System.Linq;
using Shootball.Extensions;

namespace Shootball.Utility.Navigation
{
    public class AStarPathFinder<T>
    {
        private readonly AStarHeuristic<T> _AStarHeuristic;
        private readonly Graph<T> _graph;

        public AStarPathFinder(AStarHeuristic<T> AStarHeuristic, Graph<T> graph)
        {
            _AStarHeuristic = AStarHeuristic;
            _graph = graph;
        }

        public List<T> FindPath(T start, T goal)
        {
            var nodes = _graph.GetNodes();
            if (!(nodes.Contains(start) || nodes.Contains(goal)))
            {
                throw new Exception();
            }

            var evaluatedNodes = new HashSet<T>();
            var notEvaluatedNodes = new HashSet<T>();
            notEvaluatedNodes.Add(start);
            var cameFrom = new Dictionary<T, T>();
            var globalScoreFromStart = new Dictionary<T, float>();
            globalScoreFromStart[start] = 0;
            var scoreByPassingNode = new Dictionary<T, float>();
            scoreByPassingNode[start] = _AStarHeuristic.GetHeuristicScore(start, goal);

            while (notEvaluatedNodes.Count != 0)
            {
                var current = GetBestNodeToEvaluate(notEvaluatedNodes, scoreByPassingNode);
                if (current.Equals(goal))
                    return ReconstructPath(cameFrom, current);

                notEvaluatedNodes.Remove(current);
                evaluatedNodes.Add(current);

                foreach (var pathToNeighbor in _graph.GetNeighbors(current))
                {
                    var neighbor = pathToNeighbor.Key;
                    if (evaluatedNodes.Contains(neighbor))
                        continue;

                    if (!notEvaluatedNodes.Contains(neighbor))
                        notEvaluatedNodes.Add(neighbor);

                    var score = globalScoreFromStart[current] + pathToNeighbor.Value;
                    var oldScore = globalScoreFromStart.ContainsKey(neighbor)
                            ? globalScoreFromStart[neighbor]
                            : float.PositiveInfinity;
                    if (score >= oldScore)
                        continue;

                    cameFrom[neighbor] = current;
                    globalScoreFromStart[neighbor] = score;
                    scoreByPassingNode[neighbor] = score + _AStarHeuristic.GetHeuristicScore(neighbor, goal);
                }
            }
            return new List<T>();
        }

        private T GetBestNodeToEvaluate(HashSet<T> notEvaluatedNodes, Dictionary<T, float> scoreByPassingNode)
        {
            var current = default(T);
            var cost = float.PositiveInfinity;
            foreach (var node in notEvaluatedNodes)
            {
                var nodeCost = scoreByPassingNode.ContainsKey(node)
                        ? scoreByPassingNode[node]
                        : float.PositiveInfinity;
                if (nodeCost < cost)
                {
                    current = node;
                    cost = nodeCost;
                }
            }
            return current;
        }

        private List<T> ReconstructPath(Dictionary<T, T> cameFrom, T current)
        {
            var path = new List<T>();
            var currentNode = current;
            while (cameFrom.ContainsKey(currentNode))
            {
                currentNode = cameFrom[currentNode];
                path.Add(currentNode);
            }
            path.Reverse();
            return path;
        }
    }
}
