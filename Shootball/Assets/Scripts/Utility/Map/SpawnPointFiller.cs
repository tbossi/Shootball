using System.Collections.Generic;
using System.Linq;
using Shootball.Extensions;
using UnityEngine;

namespace Shootball.Utility.Map
{
    public class SpawnPointFiller : MapFiller
    {
        private readonly GameObject _spawnPointPrefab;
        private readonly float _spawnPointWidth;
        private readonly int _spawnPointQuantity;

        public SpawnPointFiller(GameObject spawnPointPrefab, float spawnPointWidth, int spawnPointsQuantity)
        {
            _spawnPointPrefab = spawnPointPrefab;
            _spawnPointWidth = spawnPointWidth;
            _spawnPointQuantity = spawnPointsQuantity;
        }

        private ICollection<GameObjectBuilder> FitSpawnPoints(Bounds mapBounds, float baseY,
            IEnumerable<MapToFill.GameObjectIndex> houses)
        {
            var spawnPoints = new List<MapToFill.GameObjectIndex>();
            for (int i = 0; i < _spawnPointQuantity; i++)
            {
                for (int tryCounter = 0; tryCounter < 40; tryCounter++)
                {
                    var position = Extensions.Random.VectorRange(0, mapBounds.extents.magnitude);
                    position.y = baseY;

                    var spawnBounds = new Bounds(position, Vector3.one * _spawnPointWidth).WithShrinkedY(1, 2);

                    if (mapBounds.Contains(spawnBounds) && !IntersectsAny(spawnBounds, houses)
                            && !IntersectsAny(spawnBounds, spawnPoints))
                    {
                        spawnPoints.Add(new MapToFill.GameObjectIndex(-1, position, 0, spawnBounds));
                        break;
                    }
                }
            }

            return spawnPoints.Select(goi => new GameObjectBuilder(_spawnPointPrefab, goi.Position)).ToList();
        }

        public override void Fill(MapToFill mapToFill)
        {
            mapToFill.SpawnPointsToBuild =
                FitSpawnPoints(mapToFill.TemporaryBounds, mapToFill.BaseY, mapToFill.HouseToBuildIndexes);
        }
    }
}