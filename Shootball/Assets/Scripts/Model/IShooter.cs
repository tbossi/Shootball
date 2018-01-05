using System.Collections;
using UnityEngine;

namespace Shootball.Model
{
    public interface IShooter
    {
        Collider Collider { get; }
        void Shoot();
        IEnumerator RechargeShot();
        void OnEnemyHit();
    }
}