using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model.Robot
{
    public class PlayerRobotModel : RobotModel
    {
        private readonly float _maxTargetFieldOfView;
        private float _currentFieldOfView;

        public PlayerRobotModel(RobotSettings settings, RobotComponents components, RobotStatistics statistics,
                Color laserColor) : base(settings, components, statistics, Color.blue, laserColor)
        {
            Components.RobotHead.AddComponent<AudioListener>();
            _maxTargetFieldOfView = Components.TargetCamera.GetComponent<Camera>().fieldOfView;
            _currentFieldOfView = Components.HeadCamera.GetComponent<Camera>().fieldOfView;
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