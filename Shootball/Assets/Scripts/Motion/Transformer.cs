using UnityEngine;

namespace Shootball.Motion
{
    public class Transformer {
		public static Quaternion DirectionRotation(Direction direction, Vector3 axis) {
            float angle = 0;
            switch(direction) {
				case Direction.Forward:
					angle = 0;
                    break;
				case Direction.Backward:
					angle = 180;
					break;
				case Direction.Right:
					angle = 90;
					break;
				case Direction.Left:
					angle = -90;
					break;
			}
            return Quaternion.AngleAxis(angle, axis);
        }

		public static Quaternion SpinRotation(Spin spin, Vector3 axis, float amount) {
			float rotation = 0;
			switch(spin) {
				case Spin.Right:
					rotation = amount;
					break;
				case Spin.Left:
					rotation = -amount;
					break;
			}
			return Quaternion.AngleAxis(rotation, axis);
		}
	}
}
