using Shootball.Model.Player.AI;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player
{
    public class AIPlayerModel : PlayerModel<EnemyRobotModel>
    {
        private readonly Graph<Vector3> _navGraph;
        private readonly AIPath _aiPath;
        private Vector3 _lastPointToReach;
        private float timeSpent;

        public AIPlayerModel(EnemyRobotModel robot, Graph<Vector3> navGraph) : base(robot)
        {
            _navGraph = navGraph;
            _aiPath = new AIPath(_navGraph);
        }

        public override void OnUpdate()
        {
            var position = Robot.Components.RobotPosition.position;
            var pointToReach = _aiPath.NextPoint(position);

            if (pointToReach != _lastPointToReach)
            {
                _lastPointToReach = pointToReach;
                timeSpent = Time.deltaTime;
            }
            else
            {
                timeSpent += Time.deltaTime;
            }

            if (timeSpent > 8)
            {
                //Workaround: sometimes robot get stuck trying to reach the point
                _aiPath.ForceNextPoint();
            }
            else
            {
                //Debug.DrawLine(position, pointToReach, Color.blue);
                if (Vector3.Distance(position, pointToReach) > 1.2f)
                {
                    Robot.MoveTowards(pointToReach - position);
                }
                Robot.RotateTowards(pointToReach - position);
            }
        }
    }
}