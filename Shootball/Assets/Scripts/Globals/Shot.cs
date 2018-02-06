using UnityEngine;
using Shootball.Model;

namespace Shootball.GlobalScripts
{
    public class Shot : MonoBehaviour
    {
        public GameObject BurnPrefab;
        public GameObject SparklesPrefab;

        [HideInInspector]
        public ShotModel ShotModel;

        void Update()
        {
            ShotModel.Move();
        }

        void OnCollisionEnter(Collision collisionInfo)
        {
            ShotModel.Hit(collisionInfo.gameObject, collisionInfo.contacts[0].point,
                collisionInfo.contacts[0].normal);
        }
    }
}