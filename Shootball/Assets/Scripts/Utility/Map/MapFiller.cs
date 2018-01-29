using System.Collections.Generic;
using UnityEngine;

namespace Shootball.Utility.Map
{
    public abstract class MapFiller
    {
        protected bool IntersectsAny(Bounds bounds, IEnumerable<MapToFill.GameObjectIndex> others)
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

        public abstract void Fill(MapToFill mapToFill);
    }
}