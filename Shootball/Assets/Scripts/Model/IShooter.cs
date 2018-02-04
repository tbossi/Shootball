using System.Collections;
using UnityEngine;

namespace Shootball.Model
{
    public interface IShooter
    {
        Collider HeadCollider { get; }
        Color LaserColor { get; }
        void Shoot();
        IEnumerator RechargeShot();
        void OnEnemyHit(float effectiveness);
    }
}