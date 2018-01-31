using System;
using System.Collections;
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
        private bool _isInstantiated;
        private GameObject _parentInstantiated;

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
            _isInstantiated = false;
        }

        public void InstantiateObjects(GameObject parent)
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

            _isInstantiated = true;
            _parentInstantiated = parent;
        }

        private bool SafeLinecast(Vector3 start, Vector3 end)
        {
            return Physics.Linecast(start, end) || Physics.Linecast(end, start);
        }

        private void TryAddLink(Graph<Vector3> graph, Vector3 start, Vector3 end, float weight)
        {
            if (graph.ContainsNode(end) && !SafeLinecast(start, end))
            {
                graph.AddDoubleLink(start, end, weight);
            }
        }

        public IEnumerator GenerateNavPoints(Action<Graph<Vector3>> callback)
        {
            if (!_isInstantiated) { throw new Exception("Must call InstantiateObjects() first."); }

            const float distance = 4;
            var baseY = _parentInstantiated.transform.position.y + 0.01f;
            var xDiff = _mapBorders.size.x - distance / 2 - Mathf.Floor(_mapBorders.size.x / distance) * distance;
            var xMin = Mathf.Floor(_mapBorders.min.x + xDiff);
            var xMax = Mathf.Ceil(_mapBorders.max.x - xDiff);
            var zDiff = _mapBorders.size.z - distance / 2 - Mathf.Floor(_mapBorders.size.z / distance) * distance;
            var zMin = Mathf.Floor(_mapBorders.min.z + zDiff);
            var zMax = Mathf.Ceil(_mapBorders.max.z - zDiff);

            var xDirection = Vector3.right;
            var zDirection = Vector3.forward;

            var xStep = xDirection * distance;
            var zStep = zDirection * distance;
            var diagonalStep = (xDirection + zDirection) * distance;
            var diagonalReverseStep = (xDirection - zDirection) * distance;

            var graph = new Graph<Vector3>();
            for (var x = xMin; x < xMax; x += distance)
            {
                for (var z = zMin; z < zMax; z += distance)
                {
                    var current = new Vector3(x, baseY, z);
                    graph.AddNode(current);
                    if (z > zMin)
                    {
                        TryAddLink(graph, current, current - zStep, 1);
                    }
                    if (x > xMin)
                    {
                        TryAddLink(graph, current, current - xStep, 1);

                        if (z > zMin)
                        {
                            TryAddLink(graph, current, current - diagonalStep, 1.4f);
                        }
                        if (z < zMax - distance)
                        {
                            TryAddLink(graph, current, current - diagonalReverseStep, 1.4f);
                        }
                    }
                }
            }

            yield return null;
            graph.RemoveIsolatedSubGraphs(300);

            yield return null;
            callback.Invoke(graph);
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