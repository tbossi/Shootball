using System;
using System.Collections.Generic;
using System.Linq;
using Shootball.Extensions;
using Shootball.Model;
using UnityEngine;

namespace Shootball.Utility
{
    public class MapBuilder
    {
        private readonly GameObject[] _housePrefabs;
        private readonly int[] _weights;
        private readonly GameObject _spawnPointPrefab;
        private readonly float _spawnPointWidth;
        private readonly GameObject _groundPrefab;
        private readonly float _betweenSpace;
        private Dictionary<Tuple<Bounds, int>, int> _weightedList;
        private Bounds[] _houseBounds;
        private float _totalPrefabsArea;

        private Dictionary<Tuple<Bounds, int>, int> HouseBoundsIndexedWeightedList
        {
            get
            {
                if (_weightedList != null)
                    return _weightedList;

                _weightedList = new Dictionary<Tuple<Bounds, int>, int>();
                for (int i = 0; i < _housePrefabs.Length; i++)
                {
                    var t = new Tuple<Bounds, int>(HouseBounds[i], i);
                    _weightedList.Add(t, _weights[i]);
                }
                return _weightedList;
            }
        }

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

        public MapBuilder(GameObject[] housePrefabs, int[] weights, GameObject spawnPointPrefab,
                float spawnPointWidth, GameObject groundPrefab)
        {
            if (housePrefabs.Length != weights.Length)
            {
                throw new System.Exception("House Prefabs quantity and Weights quantity are different.");
            }
            _housePrefabs = housePrefabs;
            _weights = weights;
            _spawnPointPrefab = spawnPointPrefab;
            _spawnPointWidth = spawnPointWidth;
            _groundPrefab = groundPrefab;
            _betweenSpace = 3;
        }

        public MapModel Generate(Vector2 minMapPosition, Vector2 maxMapPosition, float baseY,
                float fillingRate, int spawnPointsQuantity)
        {
            var mapArea = Math.Abs((maxMapPosition.x - minMapPosition.x) * (maxMapPosition.y - minMapPosition.y));
            int maxQuantity = ComputePrefabsQuantity(mapArea, fillingRate);

            var temporaryMapBounds = new Bounds();
            temporaryMapBounds.min = new Vector3(minMapPosition.x, 0, minMapPosition.y);
            temporaryMapBounds.max = new Vector3(maxMapPosition.x, 3, maxMapPosition.y);

            var houseList = FitPrefabs(maxQuantity, temporaryMapBounds, baseY);

            var spawnPoints = FitSpawnPoints(spawnPointsQuantity, temporaryMapBounds, baseY, houseList);
            var houses = houseList.Select(o =>
                    new GameObjectBuilder(_housePrefabs[o.Index], o.Position, Quaternion.Euler(0, o.YAngle, 0)));
            var mapBorders = MapBorders(minMapPosition, maxMapPosition, baseY);

            var groundPosition = mapBorders.center;
            groundPosition.y = baseY;
            var groundScale = Math.Max(maxMapPosition.x - minMapPosition.x, maxMapPosition.y - minMapPosition.y) / 10 + 10;
            var ground = new GameObjectBuilder(_groundPrefab, groundPosition, new Quaternion(),
                    go => { go.transform.localScale = Vector3.one * groundScale; });

            return new MapModel(spawnPoints, houses, mapBorders, ground);
        }

        private Bounds MapBorders(Vector2 minMapPosition, Vector2 maxMapPosition, float baseY)
        {
            var center = new Vector3(
                (maxMapPosition.x + minMapPosition.x) / 2,
                25 + baseY,
                (maxMapPosition.y + minMapPosition.y) / 2
            );
            var size = new Vector3(maxMapPosition.x - minMapPosition.x, 55, maxMapPosition.y - minMapPosition.y);
            return new Bounds(center, size);
        }

        private int ComputePrefabsQuantity(float mapArea, float fillingRate)
        {
            if (fillingRate <= 0)
                return 0;
            if (fillingRate > 1)
                fillingRate = 1;

            return (int)Math.Floor(
                (Math.Round(mapArea / TotalPrefabsArea, 2) * _housePrefabs.Length - 1) * 2 / 3 * fillingRate);
        }

        private bool IntersectsAny(Bounds bounds, IEnumerable<GameObjectIndex> others)
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
            return Extensions.Random.Range(rangeMin, rangeMin * 1.5f) + _betweenSpace * 5;
        }

        private int RandomHouseRotation()
        {
            var availableHouseRotations = new Dictionary<int, int> {
                {90, 16}, {30, 3}, {45, 1}, {60, 3}
            };
            return Extensions.Random.FromWeightedList(availableHouseRotations) * Extensions.Random.Range(0, 4);
        }

        private IEnumerable<GameObjectIndex> FitPrefabs(int quantity, Bounds mapBounds, float baseY)
        {
            var objectsToBuild = new List<GameObjectIndex>();
            var distance = 0.0f;

            for (int i = 0; i < quantity; i++)
            {
                var chosen = Extensions.Random.FromWeightedList(HouseBoundsIndexedWeightedList);
                var rotation = RandomHouseRotation();
                int prefabNumber = chosen.Item2;
                var bounds = chosen.Item1.RotatedOnYAxes(rotation);

                if (i == 0)
                {
                    bounds.center = new Vector3(mapBounds.center.x, baseY, mapBounds.center.z);
                    var obj = new GameObjectIndex(prefabNumber, bounds.center, rotation, bounds.WithShrinkedY(1, 2));
                    objectsToBuild.Add(obj);
                }
                else
                {
                    var previousObject = (objectsToBuild.Count > i / 3)
                            ? objectsToBuild[i / 3]
                            : objectsToBuild.Last();

                    if (i % 3 == 1) { distance = RandomDistanceFromHouse(previousObject); }
                    else if (i % 3 == 2) { distance *= 1.23f; }
                    else { distance *= 1.37f; }

                    for (int tryCounter = 0; tryCounter < 40; tryCounter++)
                    {
                        var yRotation = Extensions.Random.Range(120 * (i % 3) - 20, 120 * (1 + i % 3) + 20);
                        var positionDelta = Quaternion.Euler(0, yRotation, 0) * Vector3.forward * distance;
                        var actualBounds = new Bounds(bounds.center, bounds.size);
                        actualBounds.center = previousObject.Position + positionDelta;
                        actualBounds = actualBounds.WithShrinkedY(1, 2);

                        if (mapBounds.Contains(actualBounds) && !IntersectsAny(actualBounds, objectsToBuild))
                        {
                            var position = new Vector3(actualBounds.center.x, baseY, actualBounds.center.z);
                            objectsToBuild.Add(new GameObjectIndex(prefabNumber, position, rotation, actualBounds));
                            break;
                        }
                    }
                }
            }

            return objectsToBuild;
        }

        private ICollection<GameObjectBuilder> FitSpawnPoints(int quantity, Bounds mapBounds, float baseY, IEnumerable<GameObjectIndex> houses)
        {
            var spawnPoints = new List<GameObjectIndex>();
            for (int i = 0; i < quantity; i++)
            {
                for (int tryCounter = 0; tryCounter < 40; tryCounter++)
                {
                    var position = Extensions.Random.VectorRange(0, mapBounds.extents.magnitude);
                    position.y = baseY;

                    var spawnBounds = new Bounds(position, Vector3.one * _spawnPointWidth).WithShrinkedY(1, 2);

                    if (mapBounds.Contains(spawnBounds) && !IntersectsAny(spawnBounds, houses)
                            && !IntersectsAny(spawnBounds, spawnPoints))
                    {
                        spawnPoints.Add(new GameObjectIndex(-1, position, 0, spawnBounds));
                        break;
                    }
                }
            }

            return spawnPoints.Select(goi => new GameObjectBuilder(_spawnPointPrefab, goi.Position)).ToList();
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