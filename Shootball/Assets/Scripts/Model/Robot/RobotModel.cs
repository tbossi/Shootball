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
        private float _aimDegree;
        private float _nextFire;
        private List<Action> _deathCallbacks;

        public Collider HeadCollider => Components.RobotHead.GetComponent<Collider>();
        protected Vector3 MoveAxis => Components.RobotPosition.forward;
        public Vector3 RotationAxis => Components.RobotPosition.up;
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
            _shooter = new LaserShooter(this, Components.ShotPrefab);
            _deathCallbacks = new List<Action>();
            Components.MinimapIndicator.GetComponent<Renderer>().material.SetColor("_Color", minimapInidicator);
            Components.RobotBody.AddComponent<RobotCollision>().RobotModel = this;
            Components.RobotHead.AddComponent<RobotCollision>().RobotModel = this;
        }

        public void UpdateHeadAndBobyRelativePositions()
        {
            if (Statistics.IsAlive)
            {
                var bodyPosition = Components.RobotBody.transform.position;
                Components.RobotHead.transform.position = bodyPosition - _distanceBodyHead;
                Components.RobotHead.transform.rotation = new Quaternion();

                var oldParentPosition = Components.RobotPosition.position;
                Components.RobotPosition.position = bodyPosition;
                Components.RobotBody.transform.position += oldParentPosition - bodyPosition;

                var s = Settings.MaxSpeed * Time.deltaTime;
                var angle = Mathf.Clamp(Components.RobotBodyRigidBody.velocity.magnitude, 0, s);
                angle = angle / s - 0.5f;
                angle = 4 * angle * angle * angle + 0.5f;

                var headRotationAxis = Vector3.Cross(RotationAxis, MoveAxis).normalized;
                Components.RobotHead.transform.RotateAround(Components.RobotBody.transform.position,
                    headRotationAxis, 15f * angle);
            }
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
                else
                {
                    Components.RobotBody.GetComponents<AudioSource>()[3].Play();
                }
            }
        }

        public void Die()
        {
            GameObject.Instantiate(Components.DieEffectsPrefab, Components.RobotPosition);
            Components.RobotBody.GetComponents<AudioSource>()[2].Play();
            Components.MinimapIndicator.SetActive(false);
            Components.RobotBodyRigidBody.drag *= 3;
            Components.RobotBodyRigidBody.angularDrag *= 3;
            var robotHeadRigidBody = Components.RobotHead.GetComponent<Rigidbody>();
            robotHeadRigidBody.useGravity = true;
            robotHeadRigidBody.isKinematic = false;
            var direction = Extensions.Random.VectorRange(5, 25);
            direction.y = 300;
            robotHeadRigidBody.AddForce(direction);
            robotHeadRigidBody.AddTorque(Extensions.Random.VectorRange(15, 90));
            Physics.IgnoreCollision(Components.RobotBody.GetComponent<Collider>(),
                Components.RobotHead.GetComponent<Collider>(), false);
            _deathCallbacks.ForEach(c => c.Invoke());
        }

        public void SetOnDeathListener(Action callback)
        {
            _deathCallbacks.Add(callback);
        }

        public void MapBorderReached()
        {
            Components.RobotBodyRigidBody.velocity *= -0.3f;
            Components.RobotBodyRigidBody.angularVelocity *= -0.3f;
        }

        public void Move(Direction direction)
        {
            if (Statistics.IsAlive)
            {
                var directionVector = (RotationFromDirection(direction, RotationAxis) * MoveAxis).normalized;
                var oldDirection = Vector3.Dot(directionVector, Components.RobotBodyRigidBody.velocity);
                if (oldDirection <= Settings.MaxSpeed * Time.deltaTime)
                {
                    var moveAmount = (Settings.MoveSpeed * Time.deltaTime + Settings.FixedMoveSpeed
                        + Math.Abs(oldDirection) * 4.2f) * 70;

                    Components.RobotBodyRigidBody.AddForce(directionVector * moveAmount * Time.deltaTime);
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

        public void Turn(float turnAmount)
        {
            if (Statistics.IsAlive)
            {
                Components.RobotPosition.Rotate(RotationAxis, turnAmount);
            }
        }

        public void Aim(float aimAmount)
        {
            if (Statistics.IsAlive)
            {
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
                    _shooter.Shoot(Components.LaserRaySpawn.transform.position, ShootRotation, ShootDirection,
                        Settings.LaserRaySpeed);
                    Components.RobotHead.GetComponent<AudioSource>().Play();
                }
            }
        }

        [RequireComponent(typeof(Collider))]
        private class RobotCollision : MonoBehaviour
        {
            [HideInInspector]
            public RobotModel RobotModel;

            private const float threshold = 75f;
            void OnCollisionEnter(Collision collisionInfo)
            {
                var rawMagnitude = collisionInfo.impulse.magnitude;
                var impactEffectiveness = Mathf.Atan(rawMagnitude - threshold) / 3 + 0.5f;

                var other = collisionInfo.gameObject;

                var robot = other.GetComponent<GlobalScripts.Robot>()
                        ?? other.GetComponentInParent<GlobalScripts.Robot>();

                var audioSource = robot == null
                        ? RobotModel.Components.RobotBody.GetComponents<AudioSource>()[0]
                        : RobotModel.Components.RobotBody.GetComponents<AudioSource>()[1];

                audioSource.volume = impactEffectiveness * 0.58f + 0.42f;
                audioSource.Play();

                if (impactEffectiveness > 0.01)
                {
                    RobotModel.GetDamaged(impactEffectiveness);
                }
            }
        }
    }
}