using System;
using System.Collections.Generic;
using System.Linq;

namespace Shootball.Utility.Navigation
{
    public class Graph<T>
    {
        private IDictionary<T, IDictionary<T, float>> _nodesWithWeightedNeighbors;
        public Graph()
        {
            _nodesWithWeightedNeighbors = new Dictionary<T, IDictionary<T, float>>();
        }

        public void AddNode(T node)
        {
            _nodesWithWeightedNeighbors.Add(node, new Dictionary<T, float>());
        }

        public void AddLink(T start, T end, float weight)
        {
            if (!_nodesWithWeightedNeighbors.ContainsKey(end))
                throw new ArgumentException();

            _nodesWithWeightedNeighbors[start].Add(end, weight);
        }

        public void AddDoubleLink(T start, T end, float weight)
        {
            AddLink(start, end, weight);
            AddLink(end, start, weight);
        }

        public IDictionary<T, float> GetNeighbors(T node)
        {
            return _nodesWithWeightedNeighbors.ContainsKey(node)
                ? _nodesWithWeightedNeighbors[node]
                : new Dictionary<T, float>();
        }

        public IEnumerable<T> GetNodes()
        {
            return _nodesWithWeightedNeighbors.Select(n => n.Key);
        }

        public void RemoveIsolatedSubGraphs(int maxSubGraphToRemoveSize)
        {
            var subgraphs = new List<HashSet<T>>();

            foreach (var nodeWithNeighbor in _nodesWithWeightedNeighbors)
            {
                HashSet<T> subgraph = null;
                foreach (var graph in subgraphs)
                {
                    if (graph.Contains(nodeWithNeighbor.Key))
                    {
                        subgraph = graph;
                        break;
                    }
                }

                if (subgraph == null)
                {
                    subgraph = new HashSet<T>();
                    subgraph.Add(nodeWithNeighbor.Key);
                    subgraphs.Add(subgraph);
                }

                nodeWithNeighbor.Value.Select(k => k.Key).ToList().ForEach(n => subgraph.Add(n));
            }

            foreach (var subgraph in subgraphs)
            {
                if (subgraph.Count <= maxSubGraphToRemoveSize)
                {
                    foreach (var node in subgraph)
                    {
                        _nodesWithWeightedNeighbors.Remove(node);
                    }
                }
            }
        }
    }
}
