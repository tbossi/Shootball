using UnityEngine;

namespace Shootball.Extensions
{
    public static class CameraExtensions
    {
        public static bool CanSee(this Camera camera, Vector3 worldPointPosition)
        {
            var position = camera.WorldToViewportPoint(worldPointPosition);
            return position.x >= 0 && position.x <= 1 && position.y >= 0 && position.y <= 1 && position.z > 0;
        }
    }
}