using System;
using System.Collections.Generic;
using Shootball.Motion;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model
{
    public interface IShooter
    {
        Collider Collider { get; }
        void Shoot();
    }

    public abstract class RobotModel : IShooter
    {
        protected readonly RobotSettings Settings;
        protected readonly RobotComponents Components;
        private readonly LaserShooter _shooter;
        private readonly Vector3 _distanceBodyHead;
        private float _aimDegree;

        private LineRenderer _laserLineSingleton;
        private LineRenderer LaserLine =>
                _laserLineSingleton ?? (_laserLineSingleton = Components.LaserRaySpawn.GetComponent<LineRenderer>());

        public Collider Collider => Components.RobotHead.GetComponent<Collider>();

        protected Vector3 MoveAxis => Components.RobotPosition.forward;

        protected Vector3 RotationAxis => Components.RobotHead.transform.up;

        protected Quaternion ShootRotation => Quaternion.AngleAxis(_aimDegree, Vector3.Cross(MoveAxis, RotationAxis));

        protected Vector3 ShootDirection => (ShootRotation * MoveAxis).normalized;

        public RobotModel(RobotSettings settings, RobotComponents components)
        {
            Settings = settings;
            Components = components;
            Physics.IgnoreCollision(Components.RobotBody.GetComponent<Collider>(), Components.RobotHead.GetComponent<Collider>());            

            _distanceBodyHead = Components.RobotBody.transform.position - Components.RobotHead.transform.position;
            _aimDegree = Settings.StartingAimDegree;
            _shooter = new LaserShooter(this);
        }

        public void UpdateRelativePositions()
        {
            var bodyPosition = Components.RobotBody.transform.position;
            Components.RobotHead.transform.position = bodyPosition - _distanceBodyHead;

            var oldParentPosition = Components.RobotPosition.position;
            Components.RobotPosition.position = bodyPosition;
            Components.RobotBody.transform.position += oldParentPosition - bodyPosition;
        }

        public void Move(Direction direction)
        {
            var directionVector = Transformer.DirectionRotation(direction, RotationAxis) * MoveAxis;
            var oldDirection = Vector3.Dot(directionVector, Components.RobotBodyRigidBody.velocity);
            var moveAmount = Settings.MoveSpeed * Time.deltaTime + Settings.FixedMoveSpeed
                    + Math.Abs(oldDirection) * 4;
            Components.RobotBodyRigidBody.AddForce(directionVector * moveAmount);
        }

        public void Turn(Spin spin)
        {
            var turnAmount = Settings.TurnSpeed * Time.deltaTime;

            switch (spin)
            {
                case Spin.Right:
                    Components.RobotPosition.Rotate(RotationAxis, turnAmount);
                    break;
                case Spin.Left:
                    Components.RobotPosition.Rotate(RotationAxis, -turnAmount);
                    break;
                default:
                    throw new ArgumentException("Bad spin value");
            }
        }

        public void Aim(Spin spin)
        {
            var aimAmount = Settings.AimSpeed * Time.deltaTime;

            switch (spin)
            {
                case Spin.Up:
                    _aimDegree = (_aimDegree + aimAmount > Settings.UpperAimDegree)
                            ? Settings.UpperAimDegree
                            : _aimDegree + aimAmount;
                    break;
                case Spin.Down:
                    _aimDegree = (_aimDegree - aimAmount < Settings.LowerAimDegree)
                            ? Settings.LowerAimDegree
                            : _aimDegree - aimAmount;
                    break;
                default:
                    throw new ArgumentException("Bad spin value");
            }

            Components.HeadCamera.transform.forward = ShootDirection;
            Components.TargetCamera.transform.forward = ShootDirection;
        }

        public void Shoot()
        {
            _shooter.Shoot(Components.ShotPrefab, Components.LaserRaySpawn.transform.position,
                    ShootRotation, ShootDirection, Settings.LaserRaySpeed);
        }
    }
}