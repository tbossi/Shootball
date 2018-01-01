using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shootball.Utility
{
    public class MapBuilder
    {
        private GameObject[] _housePrefabs;
        private Bounds[] _houseBounds;

        private float _totalPrefabsArea;
        private float _betweenSpace;

        private Bounds[] HouseBounds
        {
            get
            {
                if (_houseBounds != null)
                    return _houseBounds;

                _houseBounds = new Bounds[_housePrefabs.Length];
                for (int i = 0; i < _housePrefabs.Length; i++)
                {
                    var bounds = new Bounds(_housePrefabs[i].transform.position, Vector3.one);
                    MeshFilter[] meshFilters = _housePrefabs[i].GetComponentsInChildren<MeshFilter>();
                    foreach (var meshFilter in meshFilters)
                    {
                        bounds.Encapsulate(meshFilter.sharedMesh.bounds);
                    }
                    bounds.Expand(_betweenSpace / 2);

                    _houseBounds[i] = bounds;
                }
                return _houseBounds;
            }
        }

        private float TotalPrefabsArea
        {
            get
            {
                if (_totalPrefabsArea > 0)
                    return _totalPrefabsArea;

                _totalPrefabsArea = 0;

                foreach (var bounds in HouseBounds)
                {
                    _totalPrefabsArea += bounds.size.x * bounds.size.z;
                }

                return _totalPrefabsArea;
            }
        }

        public MapBuilder(GameObject[] housePrefabs)
        {
            _housePrefabs = housePrefabs;
            _betweenSpace = 3;
        }

        public void Instantiate(Vector2 minMapPosition, Vector2 maxMapPosition, float baseY, float fillingRate)
        {
            var mapArea = (maxMapPosition.x - minMapPosition.x) * (maxMapPosition.y - minMapPosition.y);
            int maxQuantity = computePrefabsQuantity(mapArea, fillingRate);
            var objectsToBuild = fitPrefabs(maxQuantity, minMapPosition, maxMapPosition, baseY);
            foreach (var obj in objectsToBuild)
            {
                obj.Instantiate();
            }
        }

        private int computePrefabsQuantity(float mapArea, float fillingRate)
        {
            if (fillingRate <= 0)
                return 0;
            if (fillingRate > 1)
                fillingRate = 1;

            return (int)Math.Floor(
                (Math.Round(mapArea / TotalPrefabsArea, 2) * _housePrefabs.Length - 1) * 2 / 3 * fillingRate);
        }

        private bool IntersectsAny(Bounds bounds, List<GameObjectIndex> others)
        {
            foreach (var inserted in others)
            {
                if (bounds.Intersects(inserted.ComputedBounds))
                {
                    return true;
                }
            }
            return false;
        }

        private float RandomDistanceFromHouse(GameObjectIndex objectIndex)
        {
            var objectExtension = objectIndex.ComputedBounds.extents;
            objectExtension.y = 0;
            var rangeMin = objectExtension.magnitude;
            return UnityEngine.Random.Range(rangeMin, rangeMin * 1.5f) + _betweenSpace * 5;
        }

        private IEnumerable<GameObjectBuilder> fitPrefabs(int quantity, Vector2 minMapPosition, Vector2 maxMapPosition, float baseY)
        {
            var objectsToBuild = new List<GameObjectIndex>();

            var mapBounds = new Bounds();
            mapBounds.min = new Vector3(minMapPosition.x, 0, minMapPosition.y);
            mapBounds.max = new Vector3(maxMapPosition.x, 3, maxMapPosition.y);

            var distance = 0.0f;

            for (int i = 0; i < quantity; i++)
            {
                var prefabNumber = UnityEngine.Random.Range(0, _housePrefabs.Length);
                var rotation = UnityEngine.Random.Range(0, 4) * 90;

                if (i == 0)
                {
                    var position = new Vector3(mapBounds.center.x, baseY, mapBounds.center.z);
                    var bounds = HouseBounds[prefabNumber].RotatedOnYAxes(rotation);
                    bounds.center = position;
                    var obj = new GameObjectIndex(prefabNumber, position, rotation, bounds.WithShrinkedY(1, 2));
                    objectsToBuild.Add(obj);
                }
                else
                {
                    var previousObject = (objectsToBuild.Count > i / 3)
                            ? objectsToBuild[i / 3]
                            : objectsToBuild.Last();

                    if (i % 3 == 1) { distance = RandomDistanceFromHouse(previousObject); }
                    else if (i % 3 == 2) { distance *= 1.13f; }
                    else { distance *= 1.27f; }

                    var bounds = HouseBounds[prefabNumber].RotatedOnYAxes(rotation);
                    for (int tryCounter = 0; tryCounter < 40; tryCounter++)
                    {
                        var yRotation = UnityEngine.Random.Range(120 * (i % 3) - 20, 120 * (1 + i % 3) + 20);
                        var positionDelta = Quaternion.Euler(0, yRotation, 0) * Vector3.forward * distance;
                        var actualBounds = new Bounds(bounds.center, bounds.extents);
                        actualBounds.center = previousObject.Position + positionDelta;
                        actualBounds = actualBounds.WithShrinkedY(1, 2);

                        if (!mapBounds.IntersectsButNotContains(actualBounds) && !IntersectsAny(actualBounds, objectsToBuild))
                        {
                            var position = new Vector3(actualBounds.center.x, baseY, actualBounds.center.z);
                            objectsToBuild.Add(new GameObjectIndex(prefabNumber, position, rotation, actualBounds));
                            break;
                        }
                    }
                }
            }
            
            return objectsToBuild.Select(o =>
                    new GameObjectBuilder(_housePrefabs[o.Index], o.Position, Quaternion.Euler(0, o.YAngle, 0)));
        }

        private class GameObjectIndex
        {
            public readonly int Index;
            public readonly Vector3 Position;
            public readonly float YAngle;
            public readonly Bounds ComputedBounds;

            public GameObjectIndex(int index, Vector3 position, float yAngle, Bounds computedBounds)
            {
                Index = index;
                Position = position;
                YAngle = yAngle;
                ComputedBounds = computedBounds;
            }
        }
    }
}