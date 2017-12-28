using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model
{
    public class PlayerRobotModel : RobotModel
    {
        public PlayerRobotModel(RobotSettings settings, RobotComponents components) : 
				base(settings, components)
        {
			Components.RobotHead.AddComponent<AudioListener>();
        }

		public void Target(bool useTarget)
		{
			if (useTarget)
			{
				Components.HeadCamera.GetComponent<Camera>().enabled = false;
				Components.TargetCamera.GetComponent<Camera>().enabled = true;
			}
			else
			{
				Components.TargetCamera.GetComponent<Camera>().enabled = false;
				Components.HeadCamera.GetComponent<Camera>().enabled = true;
			}
		}
    }
}