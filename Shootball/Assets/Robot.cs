using Shootball.Model;
using UnityEngine;

namespace Shootball
{
    public class Robot : MonoBehaviour
	{
		public GameObject RobotBody;
		public GameObject RobotHead;
		public GameObject LaserRaySpawn;
		public GameObject LaserRayPrefab;
		public GameObject RobotHeadCamera;
		public GameObject RobotTargetCamera;
		public bool IsPlayer = false;
		public float MoveSpeed = 1080;
		public float FixedMoveSpeed = 55;
		public float TurnSpeed = 170;
		public float AimSpeed = 90;
		public float StartingAimDegree = 0;
		public float LowerAimDegree = -45;
		public float UpperAimDegree = 50;
		public float LaserRaySpeed = 185;
		
		[HideInInspector]
		public RobotModel RobotModel;

		void Start ()
		{
			var settings = new RobotSettings(MoveSpeed, FixedMoveSpeed, TurnSpeed, AimSpeed, StartingAimDegree,
					LowerAimDegree, UpperAimDegree, LaserRaySpeed);
			var components = new RobotComponents(transform, RobotBody, RobotHead, RobotHeadCamera, RobotTargetCamera,
					LaserRaySpawn, LaserRayPrefab);

			var shooter = new LaserShooter(this);
			
			RobotModel = IsPlayer
					? new PlayerRobotModel(settings, components, shooter) as RobotModel
					: new EnemyRobotModel(settings, components, shooter);
		}

		void FixedUpdate()
		{
			RobotModel.UpdateRelativePositions();		
		}
	}
}