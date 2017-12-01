using System;
using System.Collections;
using System.Collections.Generic;
using Shootball.Motion;
using UnityEngine;

namespace Shootball.Model
{
    public abstract class RobotModel
    {
        protected readonly RobotSettings Settings;
        protected readonly RobotComponents Components;
        private readonly LaserShooter _shooter;
        private readonly Vector3 _distanceBodyHead;
        private float _aimDegree;
        private List<GameObject> _laserRays;

        private LineRenderer LaserLine => Components.LaserRaySpawn.GetComponent<LineRenderer>();

        protected Vector3 MoveAxis => Components.RobotPosition.forward;

        protected Vector3 RotationAxis => Components.RobotHead.transform.up;

        protected Vector3 ShootDirection
        {
            get
            {
                var axis = Vector3.Cross(MoveAxis, RotationAxis);
                var rotation = Quaternion.AngleAxis(_aimDegree, axis);
                return (rotation * MoveAxis).normalized;
            }
        }

        public RobotModel(RobotSettings settings, RobotComponents components, LaserShooter shooter)
        {
            Settings = settings;
            Components = components;
            _shooter = shooter;

            _distanceBodyHead = Components.RobotBody.transform.position - Components.RobotHead.transform.position;
            _aimDegree = Settings.StartingAimDegree;
            _laserRays = new List<GameObject>();

            Physics.IgnoreCollision(Components.RobotBody.GetComponent<Collider>(), Components.RobotHead.GetComponent<Collider>());
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

            //var laserRay = UnityEngine.Object.Instantiate(Components.LaserRayPrefab, Components.LaserRaySpawn.position, 
            //		Quaternion.LookRotation(ShootDirection)) as GameObject;
            //
            //laserRay.GetComponent<Rigidbody>().velocity = ShootDirection * Settings.LaserRaySpeed;
            //Physics.IgnoreCollision(Components.RobotHead.GetComponent<Collider>(), laserRay.GetComponent<Collider>());
            //Physics.IgnoreCollision(Components.RobotBody.GetComponent<Collider>(), laserRay.GetComponent<Collider>());
            //
            //_laserRays.RemoveAll(ray => ray == null);
            //_laserRays.ForEach(ray => {
            //	Physics.IgnoreCollision(ray.GetComponent<Collider>(), laserRay.GetComponent<Collider>());
            //});
            //_laserRays.Add(laserRay);

            _shooter.Shoot(UnityEngine.Object.Instantiate(LaserLine) as LineRenderer,
					Components.LaserRaySpawn.transform.position, ShootDirection);
        }
    }

    public class LaserShooter
    {
        private readonly MonoBehaviour _monoBehaviour;
        private WaitForSeconds shotDuration = new WaitForSeconds(3f);

        public LaserShooter(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }

        public void Shoot(LineRenderer line, Vector3 origin, Vector3 direction)
        {
			line.useWorldSpace = true;
            _monoBehaviour.StartCoroutine(ShotEffect(line));

            line.SetPosition(0, origin);

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit))
            {
                line.SetPosition(1, hit.point);
            }
            else
            {
                line.SetPosition(1, origin + direction);
            }
        }

        private IEnumerator ShotEffect(LineRenderer line)
        {
            line.enabled = true;
            yield return shotDuration;
            line.enabled = false;
        }
    }
}