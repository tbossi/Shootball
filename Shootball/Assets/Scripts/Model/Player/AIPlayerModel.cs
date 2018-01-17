using Shootball.Model.Behavior;
using Shootball.Model.Player.AI;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player
{
    public class AIPlayerModel : PlayerModel<EnemyRobotModel>
    {
        private readonly Graph<Vector3> _navGraph;
        private readonly AIBehavior _patrolBehavior; 

        public AIPlayerModel(EnemyRobotModel robot, Graph<Vector3> navGraph) : base(robot)
        {
            _navGraph = navGraph;
            var behaviorFactory = new AIPlayerBehaviorFactory(robot, _navGraph);
            _patrolBehavior = behaviorFactory.CreatePatrolBehavior();
        }

        public override void OnUpdate()
        {
            _patrolBehavior.Run();
        }
    }
}