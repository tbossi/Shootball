using System;
using System.Collections.Generic;
using Shootball.Model;
using UnityEngine;

namespace Shootball.Utility.Map
{
    public class MapToFill
    {
        public readonly Vector2 MinPosition;
        public readonly Vector2 MaxPosition;
        public readonly float BaseY;
        private float? _area;
        private Bounds? _bounds;

        public IEnumerable<GameObjectIndex> HouseToBuildIndexes;
        public IEnumerable<GameObjectBuilder> HouseToBuild;
        public ICollection<GameObjectBuilder> SpawnPointsToBuild;
        public IEnumerable<GameObjectBuilder> BordersToBuild;
        public GameObjectBuilder GroundToBuild;
        public GameObjectBuilder LightToBuild;

        public float Area
        {
            get
            {
                if (_area.HasValue) { return _area.Value; }
                _area = Math.Abs((MaxPosition.x - MinPosition.x) * (MaxPosition.y - MinPosition.y));
                return _area.Value;
            }
        }

        public Bounds Bounds
        {
            get
            {
                if (_bounds.HasValue) { return _bounds.Value; }

                var center = new Vector3(
                    (MaxPosition.x + MinPosition.x) / 2,
                    25 + BaseY,
                    (MaxPosition.y + MinPosition.y) / 2
                );
                var size = new Vector3(MaxPosition.x - MinPosition.x + 50, 55, MaxPosition.y - MinPosition.y + 50);
                _bounds = new Bounds(center, size);
                return _bounds.Value;
            }
        }

        public Bounds TemporaryBounds
        {
            get
            {
                var bounds = new Bounds();
                bounds.min = new Vector3(MinPosition.x, 0, MinPosition.y);
                bounds.max = new Vector3(MaxPosition.x, 3, MaxPosition.y);
                return bounds;
            }
        }

        public MapToFill(Vector2 minMapPosition, Vector2 maxMapPosition, float baseY)
        {
            MinPosition = minMapPosition;
            MaxPosition = maxMapPosition;
            BaseY = baseY;
        }

        public MapModel ToMapModel()
        {
            return new MapModel(SpawnPointsToBuild, HouseToBuild, BordersToBuild, Bounds,
                GroundToBuild, LightToBuild);
        }

        public class GameObjectIndex
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