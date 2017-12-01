using UnityEngine;

namespace Shootball.Model
{
    public class EnemyRobotModel : RobotModel
    {
        public EnemyRobotModel(RobotSettings settings, RobotComponents components, LaserShooter shooter) :
                base(settings, components, shooter)
        {
			Components.TargetCamera.GetComponent<Camera>().enabled = false;
			Components.HeadCamera.GetComponent<Camera>().enabled = false;
        }
    }
}