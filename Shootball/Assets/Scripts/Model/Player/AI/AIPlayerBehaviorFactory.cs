using System.Collections.Generic;
using Shootball.Model.Behavior;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player.AI
{
    public class AIPlayerBehaviorFactory
    {
        private readonly EnemyRobotModel _robotModel;
        private readonly Graph<Vector3> _navGraph;
        private readonly IList<IPlayer> _playersList;

        public AIPlayerBehaviorFactory(EnemyRobotModel robotModel, Graph<Vector3> navGraph, IList<IPlayer> playersList)
        {
            _robotModel = robotModel;
            _navGraph = navGraph;
            _playersList = playersList;
        }

        public AIBehavior CreatePatrolBehavior()
        {
            var actions = new AIMotionActions(_robotModel, _navGraph, _playersList);
            var builder = new BehaviorBuilder();
            builder
                .Sequence()
                    .Do(actions.PatrolMove)
                    .Do(actions.SlowDown)
                .End();

            return builder.Build();
        }

        public AIBehavior CreateFightBehavior()
        {
            var actions = new AIFightActions(_robotModel, _navGraph, _playersList);
            var builder = new BehaviorBuilder();
            builder
                .Sequence()
                    .Do(actions.AimEnemy)
                    .Choice()
                        .Do(actions.Shoot)
                        .Do(actions.Flee)
                    .End()
                    .Do(actions.WaitForRecharge)
                .End();

            return builder.Build();
        }
    }
}