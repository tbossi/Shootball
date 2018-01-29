using System;
using UnityEngine;

namespace Shootball.Utility.Map
{
    public class GroundFiller : MapFiller
    {
        private readonly GameObject _groundPrefab;

        public GroundFiller(GameObject groundPrefab)
        {
            _groundPrefab = groundPrefab;
        }

        public override void Fill(MapToFill mapToFill)
        {
            var groundPosition = mapToFill.Bounds.center;
            groundPosition.y = mapToFill.BaseY;
            var groundScale =
                Math.Max(mapToFill.MaxPosition.x - mapToFill.MinPosition.x,
                    mapToFill.MaxPosition.y - mapToFill.MinPosition.y) / 10 + 20;

            mapToFill.GroundToBuild = new GameObjectBuilder(_groundPrefab, groundPosition, new Quaternion(),
                    go => { go.transform.localScale = Vector3.one * groundScale; });
        }
    }
}