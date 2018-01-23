using UnityEngine;

namespace Shootball.Model
{
    public class ShotModel
    {
        private readonly GameObject _shot;
        private readonly GameObject _burnEffect;
        private readonly IShooter _shooter;
        private readonly float _speed;

        public ShotModel(GameObject shot, GameObject burnEffect, IShooter shooter, float speed)
        {
            _shot = shot;
            _burnEffect = burnEffect;
            _shooter = shooter;
            _speed = speed;
        }

        public void Move()
        {
            var oldPosition = _shot.transform.position;
            var newPosition = oldPosition + _shot.transform.forward * Time.deltaTime * _speed;

            RaycastHit hit;
            if (Physics.Raycast(oldPosition, (newPosition - oldPosition).normalized, out hit,
                    Vector3.Distance(newPosition, oldPosition)))
            {
                Hit(hit.transform.gameObject, hit.point, hit.normal);
            }

            _shot.transform.position = newPosition;
        }

        public void Hit(GameObject hitObject, Vector3 position, Vector3 normal)
        {
            var maybeRobot = hitObject.GetComponent<GlobalScripts.Robot>()
                    ?? hitObject.GetComponentInParent<GlobalScripts.Robot>();

            if (maybeRobot != null)
            {
                if (maybeRobot.RobotModel.Statistics.IsAlive)
                {
                    var effectiveness = Vector3.Dot(_shot.transform.forward, -normal);
                    maybeRobot.RobotModel.GetDamaged(effectiveness);
                    _shooter.OnEnemyHit(effectiveness);
                }
            }
            else
            {
                var rotation = Quaternion.FromToRotation(Vector3.up, normal);
                var burn = GameObject.Instantiate(_burnEffect, position + normal * 0.001f, rotation);
                GameObject.Destroy(burn, 10);
            }

            GameObject.Destroy(_shot);
        }
    }
}