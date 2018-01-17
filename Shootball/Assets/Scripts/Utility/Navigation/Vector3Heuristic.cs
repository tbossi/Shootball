using UnityEngine;

namespace Shootball.Utility.Navigation
{
    public class Vector3Heuristic : AStarHeuristic<Vector3>
    {
        public override float GetHeuristicScore(Vector3 start, Vector3 end)
        {
            return Vector3.Distance(start, end);
        }
    }
}
