using Shootball.Model;
using UnityEngine;

namespace Shootball.Utility
{
    public class LaserShooter
    {
        private readonly IShooter shooter;

        public LaserShooter(IShooter shooter)
        {
            this.shooter = shooter;
        }

        public void Shoot(GameObject shotPrefab, Vector3 origin, Quaternion rotation, Vector3 direction, float speed)
        {
            var shot = GameObject.Instantiate(shotPrefab, origin, rotation);
            Physics.IgnoreCollision(shot.GetComponent<Collider>(), shooter.Collider);
            shot.transform.forward = direction;

            var shotScript = shot.GetComponent<GlobalScripts.Shot>();
            shotScript.ShotModel = new ShotModel(shot, shooter, speed);
            shotScript.enabled = true;
        }
    }
}