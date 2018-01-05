using UnityEngine;

namespace Shootball.Model.Robot
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
        public readonly float ShotRechargeTime;
        public readonly float FireRate;
        public readonly Vector2 MouseSensitivity;
        public readonly Vector2 MouseSmoothing;

        public RobotSettings(float moveSpeed, float fixedMoveSpeed, float turnSpeed, float aimSpeed,
                float startingAimDegree, float lowerAimDegree, float upperAimDegree, float laserRaySpeed,
                float shotRechargeTime, float fireRate, Vector2 mouseSensitivity, Vector2 mouseSmoothing)
        {
            MoveSpeed = moveSpeed;
            FixedMoveSpeed = fixedMoveSpeed;
            TurnSpeed = turnSpeed;
            AimSpeed = aimSpeed;
            StartingAimDegree = startingAimDegree;
            LowerAimDegree = lowerAimDegree;
            UpperAimDegree = upperAimDegree;
            LaserRaySpeed = laserRaySpeed;
            ShotRechargeTime = shotRechargeTime;
            FireRate = fireRate;
            MouseSensitivity = mouseSensitivity;
            MouseSmoothing = mouseSmoothing;
        }
    }
}