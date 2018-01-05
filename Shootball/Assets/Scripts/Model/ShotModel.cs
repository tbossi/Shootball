using UnityEngine;

namespace Shootball.Model
{
    public class ShotModel
    {
        private GameObject _shot;
        private IShooter _shooter;
        private float _speed;

        public ShotModel(GameObject shot, IShooter shooter, float speed)
        {
            _shot = shot;
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
                Hit(hit.transform.gameObject);
            }

            _shot.transform.position = newPosition;
        }

        public void Hit(GameObject hitObject)
        {
            var maybeRobot = hitObject.GetComponent<GlobalScripts.Robot>()
                    ?? hitObject.GetComponentInParent<GlobalScripts.Robot>();

            if (maybeRobot != null)
            {
                maybeRobot.RobotModel.GetDamaged();
                _shooter.OnEnemyHit();
            }

            GameObject.Destroy(_shot);
        }
    }
}