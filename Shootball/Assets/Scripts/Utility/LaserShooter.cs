using Shootball.Model;
using UnityEngine;
using VolumetricLines;

namespace Shootball.Utility
{
    public class LaserShooter
    {
        private readonly IShooter _shooter;
        private readonly GameObject _shotPrefab;

        public LaserShooter(IShooter shooter, GameObject shotPrefab)
        {
            _shooter = shooter;
            _shotPrefab = shotPrefab;
        }

        public void Shoot(Vector3 origin, Quaternion rotation, Vector3 direction, float speed)
        {
            var shot = GameObject.Instantiate(_shotPrefab, origin, rotation);
            Physics.IgnoreCollision(shot.GetComponent<Collider>(), _shooter.HeadCollider);
            shot.transform.forward = direction;
            shot.GetComponent<Light>().color = _shooter.LaserColor;
            shot.GetComponent<Renderer>().material.SetColor("_Color", _shooter.LaserColor);
            shot.GetComponent<VolumetricLineBehavior>().LineColor = _shooter.LaserColor;

            var shotScript = shot.GetComponent<GlobalScripts.Shot>();
            shotScript.ShotModel = new ShotModel(shot, shotScript.BurnPrefab, _shooter, speed);
            shotScript.enabled = true;
        }
    }
}