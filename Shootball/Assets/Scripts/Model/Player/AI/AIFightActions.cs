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
            else if (RobotModel.Statistics.ShotsLeft <= RobotModel.Statistics.MaxShots / 7
                || RobotModel.Statistics.LifeLeft <= RobotModel.Statistics.MaxLife / 9)
            {
                if (Extensions.Random.Coin(0.16f))
                {
                    return BehaviorState.Failed;
                }
            }

            var nearestEnemy = NearestEnemy;
            if (nearestEnemy == null)
            {
                return BehaviorState.Complete;
            }
            var position = RobotModel.Components.RobotPosition.position;
            var direction = nearestEnemy.Components.RobotPosition.position - position;
            direction.Normalize();

            RobotModel.RotateTowardsApproximately(direction, 2);
            RobotModel.Shoot();

            return Extensions.Random.Coin(0.90f) ? BehaviorState.Complete : BehaviorState.Running;
        }

        public BehaviorState Flee()
        {
            var nearestEnemy = NearestEnemy;
            if (nearestEnemy == null)
            {
                return BehaviorState.Complete;
            }
            var position = RobotModel.Components.RobotPosition.position;
            var directionFromEnemy = position - nearestEnemy.Components.RobotPosition.position;
            directionFromEnemy.Normalize();
            if (Vector3.Dot(directionFromEnemy, nearestEnemy.ShootDirection) > 0.975f)
            {
                return BehaviorState.Complete;
            }

            if (Vector3.Dot(nearestEnemy.RotationAxis,
                    Vector3.Cross(nearestEnemy.ShootDirection, directionFromEnemy)) > 0)
            {
                RobotModel.MoveTowards(RobotModel.Components.RobotPosition.right);
            }
            else
            {
                RobotModel.MoveTowards(-RobotModel.Components.RobotPosition.right);
            }
            RobotModel.RotateTowardsSmooth(-directionFromEnemy);

            return BehaviorState.Running;
        }

        public BehaviorState Follow()
        {
            var nearestEnemy = NearestEnemy;
            if (nearestEnemy == null)
            {
                return BehaviorState.Failed;
            }
            var position = RobotModel.Components.RobotPosition.position;
            if (Vector3.Distance(position, nearestEnemy.Components.RobotPosition.position) < 10)
            {
                return BehaviorState.Complete;
            }
            var direction = nearestEnemy.Components.RobotPosition.position - position;
            RobotModel.MoveTowards(direction);
            RobotModel.RotateTowardsSmooth(direction);

            return Extensions.Random.Coin(0.6f) ? BehaviorState.Complete : BehaviorState.Running;
        }
    }
}