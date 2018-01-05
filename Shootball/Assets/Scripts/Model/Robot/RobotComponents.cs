using UnityEngine;

namespace Shootball.Model.Robot
{
    public class RobotComponents
	{
		public readonly Transform RobotPosition;
        public readonly GameObject RobotBody;
		public readonly GameObject RobotHead;
		public readonly GameObject LaserRaySpawn;
		public readonly GameObject ShotPrefab;		
		public readonly GameObject HeadCamera;
		public readonly GameObject TargetCamera;
		public readonly GameObject DieEffectsPrefab;
		

		public Rigidbody RobotBodyRigidBody => RobotBody.GetComponent<Rigidbody>();

		public RobotComponents(Transform robotPosition, GameObject robotBody, GameObject robotHead,
				GameObject headCamera, GameObject targetCamera, GameObject laserRaySpawn, GameObject shotPrefab,
				GameObject dieEffectsPrefab)
        {
            RobotPosition = robotPosition;
            RobotBody = robotBody;
            RobotHead = robotHead;
            HeadCamera = headCamera;
			TargetCamera = targetCamera;
            LaserRaySpawn = laserRaySpawn;
			ShotPrefab = shotPrefab;
			DieEffectsPrefab = dieEffectsPrefab;
		}
	}
}