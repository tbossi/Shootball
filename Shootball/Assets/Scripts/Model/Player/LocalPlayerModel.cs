using Shootball.Model.Robot;
using Shootball.Model.UI;
using Shootball.Motion;
using Shootball.Provider;

namespace Shootball.Model.Player
{
    public class LocalPlayerModel : PlayerModel<PlayerRobotModel>
    {
        private readonly StatisticsHUDModel _statisticsHUD;

        public LocalPlayerModel(PlayerRobotModel robot, StatisticsHUDModel statisticsHUD) : base(robot)
        {
            _statisticsHUD = statisticsHUD;
        }

        public override void OnUpdate()
        {
            if (Inputs.Shoot.Value) { Robot.Shoot(); }
            Robot.Target(Inputs.Target.Value);

            Robot.Turn(Inputs.MouseX.Value);
            Robot.Aim(Inputs.MouseY.Value);

            if (Inputs.MoveForward.Value) { Robot.Move(Direction.Forward); }
            else if (Inputs.MoveBackward.Value) { Robot.Move(Direction.Backward); }

            if (Inputs.MoveLeft.Value) { Robot.Move(Direction.Left); }
            else if (Inputs.MoveRight.Value) { Robot.Move(Direction.Right); }

            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            _statisticsHUD.SetScore(Robot.Statistics.Points);
            _statisticsHUD.SetLife(Robot.Statistics.LifeLeft, Robot.Statistics.MaxLife);
            _statisticsHUD.SetShots(Robot.Statistics.ShotsLeft, Robot.Statistics.MaxShots);            
        }
    }
}