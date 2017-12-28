using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model
{
    public class EnemyRobotModel : RobotModel
    {
        public EnemyRobotModel(RobotSettings settings, RobotComponents components) :
                base(settings, components)
        {
			Components.TargetCamera.GetComponent<Camera>().enabled = false;
			Components.HeadCamera.GetComponent<Camera>().enabled = false;
        }
    }
}