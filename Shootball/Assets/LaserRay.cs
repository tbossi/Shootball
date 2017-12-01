using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shootball.Laser
{
    public class LaserRay : MonoBehaviour
    {
        void OnCollisionEnter(Collision collisionInfo)
        {
            Destroy(this.gameObject);         
        }
    }
}