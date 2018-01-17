using System;
using System.Collections.Generic;
using System.Linq;
using Shootball.Extensions;
using Shootball.Utility;
using Shootball.Utility.Navigation;
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

        public Graph<Vector3> Instantiate(GameObject parent)
        {
            var collider = parent.AddComponent<BoxCollider>();
            collider.size = _mapBorders.size;
            collider.center = _mapBorders.center;
            collider.isTrigger = true;

            var collisionScript = parent.AddComponent<MapBoundsCollision>();
            collisionScript.MapModel = this;

            var ground = _ground.Instantiate(parent.transform);
            _borders.ForEach(b => b.Instantiate(parent.transform));
            _light.Instantiate(parent.transform);
            _houses.ForEach(h => h.Instantiate(parent.transform));

            return GenerateNavPoints(ground.transform.position.y + 1);
        }

        private Graph<Vector3> GenerateNavPoints(float baseY)
        {
            var distance = 4;
            var xDiff = _mapBorders.size.x - distance / 2 - Mathf.Floor(_mapBorders.size.x / distance) * distance;
            var xMin = Mathf.FloorToInt(_mapBorders.min.x + xDiff);
            var xMax = Mathf.CeilToInt(_mapBorders.max.x - xDiff);
            var zDiff = _mapBorders.size.z - distance / 2 - Mathf.Floor(_mapBorders.size.z / distance) * distance;
            var zMin = Mathf.FloorToInt(_mapBorders.min.z + zDiff);
            var zMax = Mathf.CeilToInt(_mapBorders.max.z - zDiff);

            var xDirection = Vector3.right;
            var zDirection = Vector3.forward;
            var diagonalDirection = xDirection + zDirection;
            var diagonalReverseDirection = xDirection - zDirection;
            var xStep = xDirection * distance;
            var zStep = zDirection * distance;
            var diagonalStep = diagonalDirection * distance;
            var diagonalReverseStep = diagonalReverseDirection * distance;

            var graph = new Graph<Vector3>();
            for (var x = xMin; x < xMax; x += distance)
            {
                for (var z = zMin; z < zMax; z += distance)
                {
                    var current = new Vector3(x, baseY, z);
                    graph.AddNode(current);
                    if (z > zMin && !Physics.Linecast(current, current - zStep)
                        && !Physics.Linecast(current - zStep, current))
                    {
                        graph.AddDoubleLink(current, current - zStep, 1);
                    }
                    if (x > xMin)
                    {
                        if (!Physics.Linecast(current, current - xStep)
                            && !Physics.Linecast(current - xStep, current))
                        {
                            graph.AddDoubleLink(current, current - xStep, 1);
                        }
                        if (z > zMin && !Physics.Linecast(current,  current - diagonalStep)
                            && !Physics.Linecast(current - diagonalStep, current))
                        {
                            graph.AddDoubleLink(current, current - diagonalStep, 1.4f);
                        }
                        if (z < zMax - distance && !Physics.Linecast(current, current - diagonalReverseStep)
                            && !Physics.Linecast(current - diagonalReverseStep, current))
                        {
                            graph.AddDoubleLink(current, current - diagonalReverseStep, 1.4f);
                        }
                    }
                }
            }
            graph.RemoveIsolatedSubGraphs(300);
            return graph;
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