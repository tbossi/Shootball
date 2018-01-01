using UnityEngine;

namespace Shootball.Utility
{
    public class GameObjectBuilder
    {
        public readonly GameObject Prefab;
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public GameObjectBuilder(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            Prefab = prefab;
            Position = position;
            Rotation = rotation;
        }

        public GameObject Instantiate()
        {
            return GameObject.Instantiate(Prefab, Position, Rotation);
        }
    }
}