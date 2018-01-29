using UnityEngine;

namespace Shootball.Utility.Map
{
    public class LightFiller : MapFiller
    {
        private readonly GameObject _lightPrefab;

        public LightFiller(GameObject lightPrefab)
        {
            _lightPrefab = lightPrefab;
        }

        public override void Fill(MapToFill mapToFill)
        {
            var groundPosition = mapToFill.Bounds.center;
            groundPosition.y = mapToFill.BaseY;
            var lightPosition = groundPosition + Vector3.up * 160;
            mapToFill.LightToBuild =
                new GameObjectBuilder(_lightPrefab, lightPosition, Quaternion.Euler(77, -98, -82));
        }
    }
}