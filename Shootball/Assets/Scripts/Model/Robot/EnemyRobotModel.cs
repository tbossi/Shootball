using Shootball.Motion;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model.Robot
{
    public class EnemyRobotModel : RobotModel
    {
        public EnemyRobotModel(RobotSettings settings, RobotComponents components, RobotStatistics statistics)
                : base(settings, components, statistics, Color.red)
        {
            Components.TargetCamera.GetComponent<Camera>().enabled = false;
            Components.HeadCamera.GetComponent<Camera>().enabled = false;
        }

        private float AngleFromDirection(Vector3 direction)
        {
            var shootDirection = ShootDirection;
            shootDirection.y = 0;
            direction.y = 0;
            return Vector3.Angle(shootDirection.normalized, direction.normalized);
        }

        public void MoveTowards(Vector3 direction)
        {
            var angle = AngleFromDirection(direction);

            if (angle > -45 && angle < 45) { Move(Direction.Forward); }
            else if (angle < -135 || angle > 135) { Move(Direction.Backward); }

            if (angle > 45 && angle < 135)
            {
                Move(Direction.Left);
            }
            else if (angle < -45 && angle > -135)
            {
                Move(Direction.Right);
            }
        }

        public void RotateTowards(Vector3 direction)
        {
            var angle = AngleFromDirection(direction);

            if (angle > 30 && angle < 150 || angle < -30 && angle > -150)
            {
                Turn(angle * 2 / 5);
            }
        }
    }
}