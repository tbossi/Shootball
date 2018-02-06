using Shootball.Model.Robot;
using Shootball.Model.UI;
using Shootball.Motion;
using Shootball.Provider;
using UnityEngine;

namespace Shootball.Model.Player
{
    public class LocalPlayerModel : PlayerModel<PlayerRobotModel>
    {
        private readonly StatisticsHUDModel _statisticsHUD;
        private readonly Camera _minimapCamera;

        public LocalPlayerModel(string name, PlayerRobotModel robot, StatisticsHUDModel statisticsHUD,
            Camera minimapCamera) : base(name, robot)
        {
            _statisticsHUD = statisticsHUD;
            _minimapCamera = minimapCamera;
        }

        public override void OnUpdate()
        {
            if (Inputs.Shoot.Value) { Robot.Shoot(); }
            Robot.Target(Inputs.Target.Value);

            Robot.TurnMouse(Inputs.MouseX.Value, Inputs.Target.Value);
            Robot.AimMouse(Inputs.MouseY.Value, Inputs.Target.Value);

            if (Inputs.MoveForward.Value) { Robot.Move(Direction.Forward); }
            else if (Inputs.MoveBackward.Value) { Robot.Move(Direction.Backward); }

            if (Inputs.MoveLeft.Value) { Robot.Move(Direction.Left); }
            else if (Inputs.MoveRight.Value) { Robot.Move(Direction.Right); }

            UpdateMinimapCameraPosition();
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            _statisticsHUD.SetScore(Robot.Statistics.Points);
            _statisticsHUD.SetLife(Robot.Statistics.LifeLeft, Robot.Statistics.MaxLife);
            _statisticsHUD.SetShots(Robot.Statistics.ShotsLeft, Robot.Statistics.MaxShots);
        }

        private void UpdateMinimapCameraPosition()
        {
            var position = Robot.Components.RobotPosition.position;
            position.y = _minimapCamera.transform.position.y;
            _minimapCamera.transform.position = position;
            _minimapCamera.transform.rotation = Quaternion.Euler(90,
                    Robot.Components.RobotPosition.eulerAngles.y, 0);
        }
    }
}