using Shootball.Extensions;
using Shootball.Model.Robot;
using Shootball.Motion;

namespace Shootball.Model.Player
{
    public class AIPlayerModel : PlayerModel<EnemyRobotModel>
    {
        public AIPlayerModel(EnemyRobotModel robot) : base(robot)
        { }

        public override void OnUpdate()
        {
            if (Random.Range(-3, 3) >= 1)
            {
                Robot.Shoot();
            }
            else
            {
                var i = Random.Range(0, 3);
                if (i == 0) Robot.Move(Direction.Backward);
                else if (i == 1) Robot.Move(Direction.Forward);
                else if (i == 2) Robot.Move(Direction.Left);
                else if (i == 3) Robot.Move(Direction.Right);
            }
        }
    }
}