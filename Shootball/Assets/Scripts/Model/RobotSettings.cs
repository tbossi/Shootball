namespace Shootball.Model
{
    public class RobotSettings
    {
        public readonly float MoveSpeed;
		public readonly float FixedMoveSpeed;
		public readonly float TurnSpeed;
		public readonly float AimSpeed;
		public readonly float StartingAimDegree;
		public readonly float LowerAimDegree;
		public readonly float UpperAimDegree;
		public readonly float LaserRaySpeed;

        public RobotSettings(float moveSpeed, float fixedMoveSpeed, float turnSpeed, float aimSpeed,
                float startingAimDegree, float lowerAimDegree, float upperAimDegree, float laserRaySpeed)
        {
            MoveSpeed = moveSpeed;
            FixedMoveSpeed = fixedMoveSpeed;
            TurnSpeed = turnSpeed;
            AimSpeed = aimSpeed;
            StartingAimDegree = startingAimDegree;
            LowerAimDegree = lowerAimDegree;
            UpperAimDegree = upperAimDegree;
            LaserRaySpeed = laserRaySpeed;
        }
    }
}