﻿using Shootball.Model;
using Shootball.Utility;
using UnityEngine;

namespace Shootball
{
    public class Robot : MonoBehaviour
    {
        public GameObject RobotBody;
        public GameObject RobotHead;
        public GameObject LaserRaySpawn;
        public GameObject ShotPrefab;
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
		public float ShotRechargeTime = 0.8f;
		public float FireRate = 0.3f;
		public float MaxLife = 100;
		public int MaxShots = 60;

        [HideInInspector]
        public RobotModel RobotModel;

        void Start()
        {
            var settings = new RobotSettings(MoveSpeed, FixedMoveSpeed, TurnSpeed, AimSpeed, StartingAimDegree,
                    LowerAimDegree, UpperAimDegree, LaserRaySpeed, ShotRechargeTime, FireRate);
            var components = new RobotComponents(transform, RobotBody, RobotHead, RobotHeadCamera, RobotTargetCamera,
                    LaserRaySpawn, ShotPrefab);
			var statistics = new RobotStatistics(MaxLife, MaxShots);

            RobotModel = IsPlayer
                    ? new PlayerRobotModel(settings, components, statistics) as RobotModel
                    : new EnemyRobotModel(settings, components, statistics);
			
			StartCoroutine(RobotModel.RechargeShot());
        }

        void FixedUpdate()
        {
            RobotModel.UpdateRelativePositions();
        }
    }
}