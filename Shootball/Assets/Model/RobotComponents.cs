using UnityEngine;

namespace Shootball.Model
{
    public class RobotComponents
	{
		public readonly Transform RobotPosition;
        public readonly GameObject RobotBody;
		public readonly GameObject RobotHead;
		public readonly GameObject LaserRaySpawn;
		public readonly GameObject LaserRayPrefab;
		public readonly GameObject HeadCamera;
		public readonly GameObject TargetCamera;

		public Rigidbody RobotBodyRigidBody => RobotBody.GetComponent<Rigidbody>();

		public RobotComponents(Transform robotPosition, GameObject robotBody, GameObject robotHead,
				GameObject headCamera, GameObject targetCamera, GameObject laserRaySpawn, GameObject laserRayPrefab)
        {
            RobotPosition = robotPosition;
            RobotBody = robotBody;
            RobotHead = robotHead;
            HeadCamera = headCamera;
			TargetCamera = targetCamera;
            LaserRaySpawn = laserRaySpawn;
            LaserRayPrefab = laserRayPrefab;
		}
	}
}