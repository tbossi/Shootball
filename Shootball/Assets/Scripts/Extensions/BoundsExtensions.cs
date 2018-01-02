using UnityEngine;

namespace Shootball.Extensions
{

    public static class BoundsExtensions
    {
        public static bool IntersectsButNotContains(this Bounds bound1, Bounds bound2)
        {
            return bound1.Intersects(bound2) && (bound1.Contains(bound2.max) ^ bound1.Contains(bound2.min));
        }

        public static Bounds WithShrinkedY(this Bounds bound, float minY, float maxY)
        {
            var newBound = new Bounds(bound.center, bound.size);
            newBound.min = new Vector3(newBound.min.x, minY, newBound.min.z);
            newBound.max = new Vector3(newBound.max.x, maxY, newBound.max.z);
            return newBound;
        }

        public static Bounds RotatedOnYAxes(this Bounds bound, float angle)
        {
            var rotation = Quaternion.AngleAxis(angle, Vector3.up);
            var newBound = new Bounds(Vector3.zero, bound.size);
            var vert = new Vector3[]
            {
            rotation * newBound.max,
            rotation * newBound.min,
            rotation * new Vector3(newBound.max.x, newBound.max.y, newBound.min.z),
            rotation * new Vector3(newBound.min.x, newBound.max.y, newBound.max.z)
            };

            var min = new Vector2(vert[0].x, vert[0].z);
            var max = new Vector2(vert[0].x, vert[0].z);

            for (int i = 1; i < vert.Length; i++)
            {
                if (vert[i].x < min.x) { min.x = vert[i].x; }
                else if (vert[i].x > max.x) { max.x = vert[i].x; }

                if (vert[i].z < min.y) { min.y = vert[i].z; }
                else if (vert[i].z > max.y) { max.y = vert[i].z; }
            }

            newBound.min = new Vector3(min.x, newBound.min.y, min.y);
            newBound.max = new Vector3(max.x, newBound.max.y, max.y);
            newBound.center = bound.center;
            return newBound;
        }
    }
}