using System.Collections.Generic;
using Shootball.Model.Behavior;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player.AI
{
    public class AIFightActions : AIActions
    {
        public AIFightActions(EnemyRobotModel robotModel, Graph<Vector3> navGraph, IList<IPlayer> playersList)
            : base(robotModel, navGraph, playersList)
        {
        }

        public BehaviorState WaitForRecharge()
        {
            if (RobotModel.Statistics.ShotsLeft > 0)
            {
                return BehaviorState.Complete;
            }
            else
            {
                return BehaviorState.Failed;
            }
        }

        public BehaviorState AimEnemy()
        {
            var nearestEnemy = NearestEnemy;
            if (nearestEnemy == null)
                return BehaviorState.Failed;

            var position = RobotModel.Components.RobotPosition.position;
            var direction = nearestEnemy.Components.RobotPosition.position - position;
            direction.Normalize();
            RobotModel.RotateTowardsSmooth(direction);
            if (Vector3.Dot(direction, RobotModel.ShootDirection) < 0.975f)
            {
                return BehaviorState.Running;
            }
            return BehaviorState.Complete;
        }

        public BehaviorState Shoot()
        {
            if (RobotModel.Statistics.ShotsLeft <= 0)
            {
                return BehaviorState.Failed;
            }

            var nearestEnemy = NearestEnemy;
            if (nearestEnemy == null)
            {
                return BehaviorState.Complete;
            }
            var position = RobotModel.Components.RobotPosition.position;
            var direction = nearestEnemy.Components.RobotPosition.position - position;

            RobotModel.RotateTowardsApproximately(direction, 2);
            RobotModel.Shoot();

            return Extensions.Random.Coin(0.90f) ? BehaviorState.Complete : BehaviorState.Running;
        }

        /*
        public BehaviorState Pursue()
        {
            var nearestEnemy = NearestEnemy;
            if (nearestEnemy == null)
            {
                return BehaviorState.Failed;
            }
            var position = RobotModel.Components.RobotPosition.position;
            var enemyPosition = nearestEnemy.Components.RobotPosition.position;
            var distance = Vector3.Distance(position, enemyPosition);

            if (distance < 15)
            {
                RobotModel.SlowDown();
                return BehaviorState.Complete;
            }
            else if (distance > 50)
            {
                return BehaviorState.Failed;
            }

            var direction = enemyPosition - position;
            RobotModel.MoveTowards(direction);
            RobotModel.RotateTowardsSmooth(direction);
            return BehaviorState.Running;
        }*/
    }
}