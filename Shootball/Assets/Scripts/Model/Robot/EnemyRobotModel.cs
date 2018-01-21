using Shootball.Motion;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model.Robot
{
    public class EnemyRobotModel : RobotModel
    {
        private float _smoothAngle;

        public EnemyRobotModel(RobotSettings settings, RobotComponents components, RobotStatistics statistics,
                Color laserColor) : base(settings, components, statistics, Color.red, laserColor)
        {
            Components.TargetCamera.GetComponent<Camera>().enabled = false;
            Components.HeadCamera.GetComponent<Camera>().enabled = false;
            _smoothAngle = 0;
        }

        private float AngleFromDirection(Vector3 direction)
        {
            var shootDirection = ShootDirection;
            shootDirection.y = 0;
            shootDirection.Normalize();
            direction.y = 0;
            direction.Normalize();

            var sign = Vector3.Dot(RotationAxis, Vector3.Cross(shootDirection, direction)) > 0 ? 1 : -1;
            return Vector3.Angle(shootDirection, direction) * sign;
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

        public void RotateTowardsSmooth(Vector3 direction)
        {
            var angle = AngleFromDirection(direction);
            _smoothAngle = Mathf.LerpAngle(angle + 180, _smoothAngle + 180, 0.15f) - 180;

            Turn(_smoothAngle);
        }

        public void RotateTowardsApproximately(Vector3 direction, float maxDegreeVariation)
        {
            var angle = AngleFromDirection(direction) +
                    Extensions.Random.Range(-maxDegreeVariation, maxDegreeVariation);
            Turn(angle);
        }

        public void SlowDown()
        {
            MoveTowards(-Components.RobotBodyRigidBody.velocity.normalized);
        }

        public bool SeeEnemy()
        {
            GameObject temp;
            return SeeEnemy(out temp);
        }

        public bool SeeEnemy(out GameObject enemyObject)
        {
            var camera = Components.HeadCamera.GetComponent<Camera>();
            var widthIncrement = camera.pixelWidth / 18;
            var heightIncrement = camera.pixelHeight / 14;
            const float maxDistance = 60;

            for (var x = 0; x < camera.pixelWidth; x += widthIncrement)
            {
                for (var y = 0; y < camera.pixelHeight; y += heightIncrement)
                {
                    var ray = camera.ScreenPointToRay(new Vector3(x, y, 0));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, maxDistance))
                    {
                        var hitObject = hit.transform.gameObject;
                        var maybeRobot = hitObject.GetComponent<GlobalScripts.Robot>()
                            ?? hitObject.GetComponentInParent<GlobalScripts.Robot>();

                        if (maybeRobot != null && maybeRobot.RobotModel.Statistics.IsAlive)
                        {
                            enemyObject = maybeRobot.gameObject;
                            return true;
                        }
                    }
                }
            }
            enemyObject = null;
            return false;
        }
    }
}