using System;
using System.Collections;
using System.Collections.Generic;
using Shootball.Motion;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model.Robot
{
    public abstract class RobotModel : IShooter, IMortal
    {
        protected readonly RobotSettings Settings;
        public readonly RobotComponents Components;
        public readonly RobotStatistics Statistics;
        private readonly LaserShooter _shooter;
        private readonly Vector3 _distanceBodyHead;
        private float _smoothMouseX = 0;
        private float _smoothMouseY = 0;
        private float _aimDegree;
        private float _nextFire;
        private List<Action> _deathCallbacks;

        public Collider Collider => Components.RobotHead.GetComponent<Collider>();

        protected Vector3 MoveAxis => Components.RobotPosition.forward;

        public Vector3 RotationAxis => Components.RobotHead.transform.up;

        protected Quaternion ShootRotation => Quaternion.AngleAxis(_aimDegree, Vector3.Cross(MoveAxis, RotationAxis));

        public Vector3 ShootDirection => (ShootRotation * MoveAxis).normalized;

        public Color LaserColor { get; }

        public RobotModel(RobotSettings settings, RobotComponents components, RobotStatistics statistics,
                Color minimapInidicator, Color laserColor)
        {
            Settings = settings;
            Components = components;
            Statistics = statistics;
            LaserColor = laserColor;
            Physics.IgnoreCollision(Components.RobotBody.GetComponent<Collider>(), Components.RobotHead.GetComponent<Collider>());

            _distanceBodyHead = Components.RobotBody.transform.position - Components.RobotHead.transform.position;
            _aimDegree = Settings.StartingAimDegree;
            _nextFire = 0;
            _shooter = new LaserShooter(this);
            _deathCallbacks = new List<Action>();
            Components.MinimapIndicator.GetComponent<Renderer>().material.SetColor("_Color", minimapInidicator);
            Components.RobotBody.AddComponent<RobotCollision>().RobotModel = this;
            Components.RobotHead.AddComponent<RobotCollision>().RobotModel = this;
        }

        public void UpdateRelativePositions()
        {
            var bodyPosition = Components.RobotBody.transform.position;
            Components.RobotHead.transform.position = bodyPosition - _distanceBodyHead;

            var oldParentPosition = Components.RobotPosition.position;
            Components.RobotPosition.position = bodyPosition;
            Components.RobotBody.transform.position += oldParentPosition - bodyPosition;
        }

        public IEnumerator RechargeShot()
        {
            while (true)
            {
                yield return new WaitForSeconds(Settings.ShotRechargeTime);
                if (Statistics.IsAlive)
                {
                    Statistics.RechargeShot();
                }
            }
        }

        public void OnEnemyHit(float effectiveness)
        {
            Statistics.IncreasePoints(effectiveness);
        }

        public void GetDamaged(float effectiveness)
        {
            if (Statistics.IsAlive)
            {
                var isAlive = Statistics.GetDamaged(effectiveness);
                if (!isAlive)
                {
                    Die();
                }
            }
        }

        public void Die()
        {
            Components.RobotBodyRigidBody.drag *= 5;
            Components.RobotBodyRigidBody.angularDrag *= 5;
            GameObject.Instantiate(Components.DieEffectsPrefab, Components.RobotPosition);
            Components.MinimapIndicator.SetActive(false);
            _deathCallbacks.ForEach(c => c.Invoke());
        }

        public void SetOnDeathListener(Action callback)
        {
            _deathCallbacks.Add(callback);
        }

        public void MapBorderReached()
        {
            Die();
        }

        public void Move(Direction direction)
        {
            if (Statistics.IsAlive)
            {
                var directionVector = RotationFromDirection(direction, RotationAxis) * MoveAxis;
                var oldDirection = Vector3.Dot(directionVector, Components.RobotBodyRigidBody.velocity);
                if (oldDirection <= Settings.MaxSpeed * Time.deltaTime)
                {
                    var moveAmount = Settings.MoveSpeed * Time.deltaTime + Settings.FixedMoveSpeed
                            + Math.Abs(oldDirection) * 4;

                    Components.RobotBodyRigidBody.AddForce(directionVector * moveAmount);
                }
            }
        }

        private Quaternion RotationFromDirection(Direction direction, Vector3 axis)
        {
            float angle = 0;
            switch (direction)
            {
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

        public void TurnMouse(float rawX)
        {

            var x = rawX * Settings.MouseSensitivity.x * Settings.MouseSmoothing.x;
            _smoothMouseX = Mathf.Lerp(_smoothMouseX, x, 1f / Settings.MouseSmoothing.x);
            var turnAmount = _smoothMouseX * Settings.TurnSpeed * Time.deltaTime;

            Turn(turnAmount);
        }

        public void Turn(float turnAmount)
        {
            if (Statistics.IsAlive)
            {
                Components.RobotPosition.Rotate(RotationAxis, turnAmount);
            }
        }

        public void Aim(float rawY)
        {
            if (Statistics.IsAlive)
            {
                var y = rawY * Settings.MouseSensitivity.y * Settings.MouseSmoothing.y;
                _smoothMouseY = Mathf.Lerp(_smoothMouseY, y, 1f / Settings.MouseSmoothing.y);
                var aimAmount = _smoothMouseY * Settings.AimSpeed * Time.deltaTime;

                if (_aimDegree + aimAmount > Settings.UpperAimDegree)
                {
                    _aimDegree = Settings.UpperAimDegree;
                }
                else if (_aimDegree + aimAmount < Settings.LowerAimDegree)
                {
                    _aimDegree = Settings.LowerAimDegree;
                }
                else
                {
                    _aimDegree += aimAmount;
                }

                Components.HeadCamera.transform.forward = ShootDirection;
                Components.TargetCamera.transform.forward = ShootDirection;
            }
        }

        public void Shoot()
        {
            if (Statistics.IsAlive)
            {
                if (Time.time > _nextFire && Statistics.Shoot())
                {
                    _nextFire = Time.time + Settings.FireRate;
                    _shooter.Shoot(Components.ShotPrefab, Components.LaserRaySpawn.transform.position,
                        ShootRotation, ShootDirection, Settings.LaserRaySpeed);
                }
            }
        }

        [RequireComponent(typeof(Collider))]
        private class RobotCollision : MonoBehaviour
        {
            [HideInInspector]
            public RobotModel RobotModel;

            private const float threshold = 80f;
            void OnCollisionEnter(Collision collisionInfo)
            {
                var rawMagnitude = collisionInfo.impulse.magnitude;
                var impactEffectiveness = Mathf.Atan(rawMagnitude - threshold) / 3 + 1 / 2;
                RobotModel.GetDamaged(impactEffectiveness);
            }
        }
    }
}