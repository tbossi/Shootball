using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model.Robot
{
    public class PlayerRobotModel : RobotModel
    {
        private readonly float _maxTargetFieldOfView;
        private float _currentFieldOfView;
        private float _smoothMouseX = 0;
        private float _smoothMouseY = 0;
        private float _targetSensitivityReducingAmount = 0.45f;

        public PlayerRobotModel(RobotSettings settings, RobotComponents components, RobotStatistics statistics,
                Color laserColor) : base(settings, components, statistics, Color.blue, laserColor)
        {
            Components.RobotHead.AddComponent<AudioListener>();
            _maxTargetFieldOfView = Components.TargetCamera.GetComponent<Camera>().fieldOfView;
            _currentFieldOfView = Components.HeadCamera.GetComponent<Camera>().fieldOfView;
        }

        public void TurnMouse(float rawX, bool useTarget)
        {

            var x = rawX * Settings.MouseSensitivity.x * Settings.MouseSmoothing.x;
            if (useTarget) { x *= _targetSensitivityReducingAmount; }
            _smoothMouseX = Mathf.Lerp(_smoothMouseX, x, 1f / Settings.MouseSmoothing.x);
            var turnAmount = _smoothMouseX * Settings.TurnSpeed * Time.deltaTime;

            Turn(turnAmount);
        }

        public void AimMouse(float rawY, bool useTarget)
        {
            var y = rawY * Settings.MouseSensitivity.y * Settings.MouseSmoothing.y;
            if (useTarget) { y *= _targetSensitivityReducingAmount; }
            _smoothMouseY = Mathf.Lerp(_smoothMouseY, y, 1f / Settings.MouseSmoothing.y);
            var aimAmount = _smoothMouseY * Settings.AimSpeed * Time.deltaTime;

            Aim(aimAmount);
        }

        public void Target(bool useTarget)
        {
            if (useTarget)
            {
                _currentFieldOfView = Mathf.Lerp(_maxTargetFieldOfView, _currentFieldOfView, 0.5f);
                Components.HeadCamera.GetComponent<Camera>().enabled = false;
                Components.TargetCamera.GetComponent<Camera>().enabled = true;
            }
            else
            {
                var minFieldOfView = Components.HeadCamera.GetComponent<Camera>().fieldOfView;
                _currentFieldOfView = Mathf.Lerp(minFieldOfView, _currentFieldOfView, 0.4f);

                if (Mathf.Abs(_currentFieldOfView - minFieldOfView) < 5)
                {
                    _currentFieldOfView = minFieldOfView;
                    Components.TargetCamera.GetComponent<Camera>().enabled = false;
                    Components.HeadCamera.GetComponent<Camera>().enabled = true;
                }
            }

            Components.TargetCamera.GetComponent<Camera>().fieldOfView = _currentFieldOfView;
        }
    }
}