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
        private readonly IList<GameObjectBuilder> _spawnPoints;
        private readonly IEnumerable<GameObjectBuilder> _houses;
        private readonly IEnumerable<GameObjectBuilder> _borders;
        private readonly Bounds _mapBorders;
        private readonly GameObjectBuilder _ground;
        private readonly GameObjectBuilder _light;
        private int _spawnPointCounter;

        public MapModel(ICollection<GameObjectBuilder> spawnPoints, IEnumerable<GameObjectBuilder> houses,
                IEnumerable<GameObjectBuilder> borders, Bounds mapBorders, GameObjectBuilder ground,
                GameObjectBuilder light)
        {
            _spawnPoints = spawnPoints.ToList();
            _houses = houses;
            _borders = borders;
            _mapBorders = mapBorders;
            _ground = ground;
            _light = light;
            _spawnPointCounter = 0;
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
            _borders.ForEach(b => b.Instantiate(parent.transform));
            _light.Instantiate(parent.transform);
            _houses.ForEach(h => h.Instantiate(parent.transform));
        }

        public GameObjectBuilder GetSpawnPoint()
        {
            _spawnPointCounter = _spawnPointCounter >= _spawnPoints.Count - 1 ? 0 : _spawnPointCounter + 1;
            return _spawnPoints[_spawnPointCounter];
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