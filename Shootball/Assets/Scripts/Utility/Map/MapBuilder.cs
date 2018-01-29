using System.Collections.Generic;
using Shootball.Model;
using UnityEngine;

namespace Shootball.Utility.Map
{
    public class MapBuilder
    {
        private readonly GameObject[] _housePrefabs;
        private readonly int[] _weights;
        private readonly GameObject _spawnPointPrefab;
        private readonly float _spawnPointWidth;
        private readonly GameObject _groundPrefab;
        private readonly GameObject _lightPrefab;
        private readonly GameObject[] _borderPrefabs;
        private readonly float _fillingRate;
        private readonly int _spawnPointsQuantity;

        public MapBuilder(GameObject[] housePrefabs, int[] weights, GameObject spawnPointPrefab,
                float spawnPointWidth, GameObject groundPrefab, GameObject lightPrefab,
                GameObject[] borderPrefabs, float fillingRate, int spawnPointsQuantity)
        {
            _housePrefabs = housePrefabs;
            _weights = weights;
            _spawnPointPrefab = spawnPointPrefab;
            _spawnPointWidth = spawnPointWidth;
            _groundPrefab = groundPrefab;
            _lightPrefab = lightPrefab;
            _borderPrefabs = borderPrefabs;
            _fillingRate = fillingRate;
            _spawnPointsQuantity = spawnPointsQuantity;
        }

        private List<MapFiller> Fillers => new List<MapFiller>
        {
            new HouseFiller(_housePrefabs, _weights, _fillingRate),
            new SpawnPointFiller(_spawnPointPrefab, _spawnPointWidth, _spawnPointsQuantity),
            new BordersFiller(_borderPrefabs),
            new GroundFiller(_groundPrefab),
            new LightFiller(_lightPrefab)
        };

        public MapModel Generate(Vector2 minMapPosition, Vector2 maxMapPosition, float baseY)
        {
            var map = new MapToFill(minMapPosition, maxMapPosition, baseY);
            Fillers.ForEach(f => f.Fill(map));
            return map.ToMapModel();
        }
    }
}