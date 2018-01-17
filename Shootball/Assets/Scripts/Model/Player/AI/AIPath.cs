using System.Collections.Generic;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player.AI
{
    public class AIPath
    {
        private readonly Graph<Vector3> _navGraph;
        private readonly Vector3Heuristic _heuristic;
        private Vector3 _checkPoint;
        private int _pointCounter = 0;
        private List<Vector3> _currentPath;

        public AIPath(Graph<Vector3> navGraph)
        {
            _navGraph = navGraph;
            _heuristic = new Vector3Heuristic();
        }

        private Vector3 CheckPoint(bool generateNew)
        {
            if (!generateNew) return _checkPoint;

            var point = Extensions.Random.VectorRange(0, 250);
            point.y = 0;

            _checkPoint = _navGraph.FindNearestNode(point, _heuristic);
            return _checkPoint;
        }

        private List<Vector3> GetPath(Vector3 robotPosition, Vector3 checkPoint)
        {
            var nearest = _navGraph.FindNearestNode(robotPosition, _heuristic);
            return _navGraph.FindAStarPath(nearest, checkPoint, _heuristic);
        }

        public Vector3 NextPoint(Vector3 robotPosition)
        {
            if (_currentPath == null || _pointCounter >= _currentPath.Count - 1)
            {
                do
                {
                    _currentPath = GetPath(robotPosition, CheckPoint(true));
                    _pointCounter = 0;
                }
                while (_currentPath.Count == 0);
            }
            else if (Physics.Linecast(robotPosition, _currentPath[_pointCounter]))
            {
                var recoverPath = GetPath(robotPosition, _currentPath[_pointCounter]);
                for (var i = _pointCounter + 1; i < _currentPath.Count; i++)
                {
                    recoverPath.Add(_currentPath[i]);
                }
                _currentPath = recoverPath;
                _pointCounter = 0;
            }

            Vector3? old = null;
            foreach (var node in _currentPath)
            {
                if (!old.HasValue)
                {
                    old = node;
                    continue;
                }
                Debug.DrawLine(old.Value, node, Color.green, 10);
                old = node;
            }

            if (Vector3.Distance(robotPosition, _currentPath[_pointCounter]) <= 3)
            {
                _pointCounter++;
            }
            return _currentPath[_pointCounter];
        }

        public void ForceNextPoint()
        {
            _pointCounter++;
        }
    }
}