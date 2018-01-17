using Shootball.Model.Behavior;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player.AI
{
    public class AIPlayerBehaviorFactory
    {
        protected readonly EnemyRobotModel _robotModel;
        private readonly Graph<Vector3> _navGraph;

        public AIPlayerBehaviorFactory(EnemyRobotModel robotModel, Graph<Vector3> navGraph)
        {
            _robotModel = robotModel;
            _navGraph = navGraph;
        }

        public AIBehavior CreatePatrolBehavior()
        {
            var actions = new AIMotionActions(_robotModel, _navGraph);
            var builder = new BehaviorBuilder();
            builder
                .Sequence()
                    .Do(actions.PatrolMove)
                    .Do(actions.SlowDown)
                .End();

            return builder.Build();
        }
    }
}