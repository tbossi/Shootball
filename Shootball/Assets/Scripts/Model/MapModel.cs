using System;
using System.Collections.Generic;
using System.Linq;
using Shootball.Extensions;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model
{
    public class MapModel
    {
        private readonly ICollection<GameObjectBuilder> _spawnPoints;
        private readonly IEnumerable<GameObjectBuilder> _houses;
        private readonly Bounds _mapBorders;
        private readonly GameObjectBuilder _ground;

        public MapModel(ICollection<GameObjectBuilder> spawnPoints, IEnumerable<GameObjectBuilder> houses,
                Bounds mapBorders, GameObjectBuilder ground)
        {
            _spawnPoints = spawnPoints;
            _houses = houses;
            _mapBorders = mapBorders;
            _ground = ground;
        }

        public void Instantiate(GameObject parent)
        {
            var collider = parent.AddComponent<BoxCollider>();
            collider.size = _mapBorders.size;
            collider.center = _mapBorders.center;
            collider.isTrigger = true;

            var collisionScript = parent.AddComponent<MapBoundsCollision>();
            collisionScript.MapModel = this;

            _ground.Instantiate(parent.transform);
            _houses.ForEach(h => h.Instantiate(parent.transform));
        }

        public GameObjectBuilder GetSpawnPoint()
        {
            return Extensions.Random.FromCollection(_spawnPoints);
        }

        public void OnBorderReached(Collider otherObject)
        {
            var gameObject = otherObject.gameObject;
            var maybeRobot = gameObject.GetComponent<GlobalScripts.Robot>()
                    ?? gameObject.GetComponentInParent<GlobalScripts.Robot>();
            if (maybeRobot != null)
            {
                maybeRobot.RobotModel.MapBorderReached();
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        [RequireComponent(typeof(BoxCollider))]
        private class MapBoundsCollision : MonoBehaviour
        {
            [HideInInspector]
            public MapModel MapModel;

            void OnTriggerExit(Collider other)
            {
                MapModel.OnBorderReached(other);
            }
        }
    }
}