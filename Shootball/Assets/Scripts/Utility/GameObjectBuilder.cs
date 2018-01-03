using System;
using UnityEngine;

namespace Shootball.Utility
{
    public class GameObjectBuilder
    {
        public readonly GameObject Prefab;
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly Action<GameObject> PostInitAction;

        public GameObjectBuilder(GameObject prefab, Vector3 position, Quaternion rotation = new Quaternion(),
                Action<GameObject> postInitAction = null)
        {
            Prefab = prefab;
            Position = position;
            Rotation = rotation;
            PostInitAction = postInitAction;
        }

        public GameObject Instantiate(Transform parent = null)
        {
            var gameObject = parent == null
                    ? GameObject.Instantiate(Prefab, Position, Rotation)
                    : GameObject.Instantiate(Prefab, Position, Rotation, parent);
            
            if (PostInitAction != null)
            {
                PostInitAction.Invoke(gameObject);
            }
            return gameObject;
        }
    }
}