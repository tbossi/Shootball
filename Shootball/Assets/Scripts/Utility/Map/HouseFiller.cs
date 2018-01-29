using System;
using System.Collections.Generic;
using System.Linq;
using Shootball.Extensions;
using UnityEngine;

namespace Shootball.Utility.Map
{
    public class HouseFiller : MapFiller
    {
        private readonly GameObject[] _housePrefabs;
        private readonly int[] _weights;
        private readonly float _betweenSpace;
        private readonly float _fillingRate;
        private Dictionary<Tuple<Bounds, int>, int> _weightedList;
        private Bounds[] _houseBounds;
        private float _totalPrefabsArea;
        private readonly Dictionary<int, int> _availableHouseRotations = new Dictionary<int, int>
        {
            {90, 16}, {30, 3}, {45, 1}, {60, 3}
        };

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

        private int RandomHouseRotation =>
            Extensions.Random.FromWeightedList(_availableHouseRotations) * Extensions.Random.Range(0, 4);

        public HouseFiller(GameObject[] housePrefabs, int[] weights, float fillingRate)
        {
            if (housePrefabs.Length != weights.Length)
            {
                throw new System.Exception("House Prefabs quantity and Weights quantity are different.");
            }
            _housePrefabs = housePrefabs;
            _weights = weights;
            _fillingRate = fillingRate;
            _betweenSpace = 3;
        }

        private float RandomDistanceFromHouse(MapToFill.GameObjectIndex objectIndex)
        {
            var objectExtension = objectIndex.ComputedBounds.extents;
            objectExtension.y = 0;
            var rangeMin = objectExtension.magnitude;
            return Extensions.Random.Range(rangeMin, rangeMin * 1.5f) + _betweenSpace * 5;
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

        private IEnumerable<MapToFill.GameObjectIndex> FitPrefabs(int quantity, Bounds mapBounds, float baseY)
        {
            var objectsToBuild = new List<MapToFill.GameObjectIndex>();
            var distance = 0.0f;

            for (int i = 0; i < quantity; i++)
            {
                var chosen = Extensions.Random.FromWeightedList(HouseBoundsIndexedWeightedList);
                var rotation = RandomHouseRotation;
                int prefabNumber = chosen.Item2;
                var bounds = chosen.Item1.RotatedOnYAxes(rotation);

                if (i == 0)
                {
                    bounds.center = new Vector3(mapBounds.center.x, baseY, mapBounds.center.z);
                    var obj = new MapToFill.GameObjectIndex(prefabNumber, bounds.center, rotation, bounds.WithShrinkedY(1, 2));
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
                            objectsToBuild.Add(new MapToFill.GameObjectIndex(prefabNumber, position, rotation, actualBounds));
                            break;
                        }
                    }
                }
            }

            return objectsToBuild;
        }

        public override void Fill(MapToFill mapToFill)
        {
            int maxQuantity = ComputePrefabsQuantity(mapToFill.Area, _fillingRate);

            mapToFill.HouseToBuildIndexes = FitPrefabs(maxQuantity, mapToFill.TemporaryBounds, mapToFill.BaseY);
            mapToFill.HouseToBuild = mapToFill.HouseToBuildIndexes.Select(o =>
                new GameObjectBuilder(_housePrefabs[o.Index], o.Position, Quaternion.Euler(0, o.YAngle, 0)));
        }
    }
}