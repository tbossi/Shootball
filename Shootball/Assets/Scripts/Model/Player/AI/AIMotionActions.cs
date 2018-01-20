using System.Collections.Generic;
using Shootball.Model.Behavior;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player.AI
{
    public class AIMotionActions : AIActions
    {
        private Vector3 _lastPointToReach;
        private float timeSpent;

        public AIMotionActions(EnemyRobotModel robotModel, Graph<Vector3> navGraph, IList<IPlayer> playersList)
            : base(robotModel, navGraph, playersList)
        {
        }

        public BehaviorState PatrolMove()
        {
            var position = RobotModel.Components.RobotPosition.position;
            var pointToReach = AIPath.NextPoint(position);

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
                AIPath.ForceNextPoint();
            }
            else
            {
                //Debug.DrawLine(position, pointToReach, Color.blue);
                if (Vector3.Distance(position, pointToReach) > 1.2f)
                {
                    RobotModel.MoveTowards(pointToReach - position);
                }
                RobotModel.RotateTowardsSmooth(pointToReach - position);
            }

            return AIPath.IsCheckPointReached(position) ? BehaviorState.Complete : BehaviorState.Running;
        }
    }
}