using System.Collections.Generic;
using UnityEngine;

namespace Shootball.Utility.Map
{
    public class BordersFiller : MapFiller
    {
        private readonly GameObject[] _borderPrefabs;

        public BordersFiller(GameObject[] borderPrefabs)
        {
            _borderPrefabs = borderPrefabs;
        }

        private IEnumerable<GameObjectBuilder> Borders(Vector2 minMapPosition, Vector2 maxMapPosition, float baseY)
        {
            var borders = new List<GameObjectBuilder>();

            var prefab = _borderPrefabs[Extensions.Random.Range(0, _borderPrefabs.Length)];
            var position = new Vector3((minMapPosition.x + maxMapPosition.x) / 2, baseY, minMapPosition.y);
            var border = new GameObjectBuilder(prefab, position, new Quaternion());
            borders.Add(border);

            prefab = _borderPrefabs[Extensions.Random.Range(0, _borderPrefabs.Length)];
            position = new Vector3((minMapPosition.x + maxMapPosition.x) / 2, baseY, maxMapPosition.y);
            border = new GameObjectBuilder(prefab, position, Quaternion.Euler(0, 180, 0));
            borders.Add(border);

            prefab = _borderPrefabs[Extensions.Random.Range(0, _borderPrefabs.Length)];
            position = new Vector3(minMapPosition.x, baseY, (minMapPosition.y + maxMapPosition.y) / 2);
            border = new GameObjectBuilder(prefab, position, Quaternion.Euler(0, 90, 0));
            borders.Add(border);

            prefab = _borderPrefabs[Extensions.Random.Range(0, _borderPrefabs.Length)];
            position = new Vector3(maxMapPosition.x, baseY, (minMapPosition.y + maxMapPosition.y) / 2);
            border = new GameObjectBuilder(prefab, position, Quaternion.Euler(0, 270, 0));
            borders.Add(border);

            return borders;
        }

        public override void Fill(MapToFill mapToFill)
        {
            mapToFill.BordersToBuild =
                Borders(mapToFill.MinPosition, mapToFill.MaxPosition, mapToFill.BaseY);
        }
    }
}