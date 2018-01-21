using Shootball.Model;
using UnityEngine;
using VolumetricLines;

namespace Shootball.Utility
{
    public class LaserShooter
    {
        private readonly IShooter _shooter;

        public LaserShooter(IShooter shooter)
        {
            _shooter = shooter;
        }

        public void Shoot(GameObject shotPrefab, Vector3 origin, Quaternion rotation, Vector3 direction, float speed)
        {
            var shot = GameObject.Instantiate(shotPrefab, origin, rotation);
            Physics.IgnoreCollision(shot.GetComponent<Collider>(), _shooter.Collider);
            shot.transform.forward = direction;
            shot.GetComponent<Light>().color = _shooter.LaserColor;
            shot.GetComponent<Renderer>().material.SetColor("_Color", _shooter.LaserColor);
            shot.GetComponent<VolumetricLineBehavior>().LineColor = _shooter.LaserColor;

            var shotScript = shot.GetComponent<GlobalScripts.Shot>();
            shotScript.ShotModel = new ShotModel(shot, _shooter, speed);
            shotScript.enabled = true;
        }
    }
}