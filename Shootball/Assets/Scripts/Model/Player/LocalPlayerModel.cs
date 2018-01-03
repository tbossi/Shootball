using Shootball.Model.Robot;
using Shootball.Motion;
using Shootball.Provider;
using UnityEngine;

namespace Shootball.Model.Player
{
    public class LocalPlayerModel : PlayerModel<PlayerRobotModel>
    {
        public LocalPlayerModel(PlayerRobotModel robot) : base(robot)
        { }

        public override void OnUpdate()
        {
            if (Inputs.Shoot.Active) { Robot.Shoot(); }
            Robot.Target(Inputs.Target.Active);

            if (Inputs.RotateLeft.Active) { Robot.Turn(Spin.Left); }
            else if (Inputs.RotateRight.Active) { Robot.Turn(Spin.Right); }

            if (Inputs.RotateUp.Active) { Robot.Aim(Spin.Up); }
            else if (Inputs.RotateDown.Active) { Robot.Aim(Spin.Down); }

            if (Inputs.MoveForward.Active) { Robot.Move(Direction.Forward); }
            else if (Inputs.MoveBackward.Active) { Robot.Move(Direction.Backward); }

            if (Inputs.MoveLeft.Active) { Robot.Move(Direction.Left); }
            else if (Inputs.MoveRight.Active) { Robot.Move(Direction.Right); }
        }
    }
}