using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model
{
    public class EnemyRobotModel : RobotModel
    {
        public EnemyRobotModel(RobotSettings settings, RobotComponents components, RobotStatistics statistics)
                : base(settings, components, statistics)
        {
			Components.TargetCamera.GetComponent<Camera>().enabled = false;
			Components.HeadCamera.GetComponent<Camera>().enabled = false;
        }
    }
}