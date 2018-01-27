using Shootball.Model.Behavior;
using Shootball.Model.Player.AI;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player
{
    public class AIPlayerModel : PlayerModel<EnemyRobotModel>
    {
        enum AIState { Patrol, Fight }

        private readonly Graph<Vector3> _navGraph;
        private AIState _currentState;
        private AIBehavior _patrolBehavior;
        private AIBehavior _fightBehavior;
        private AIPlayerBehaviorFactory _factory;

        private AIPlayerBehaviorFactory BehaviorFactory =>
            _factory ?? (_factory = new AIPlayerBehaviorFactory(Robot, _navGraph, MatchStatusModel.Players));
        private AIBehavior PatrolBehavior =>
            _patrolBehavior ?? (_patrolBehavior = BehaviorFactory.CreatePatrolBehavior());
        private AIBehavior FightBehavior =>
            _fightBehavior ?? (_fightBehavior = BehaviorFactory.CreateFightBehavior());

        public AIPlayerModel(string name, EnemyRobotModel robot, Graph<Vector3> navGraph) : base(name, robot)
        {
            _navGraph = navGraph;
            _currentState = AIState.Patrol;
        }

        private int _enemyLostCountdown = 20;
        public override void OnUpdate()
        {
            if (Robot.SeeEnemy())
            {
                _currentState = AIState.Fight;
                _enemyLostCountdown = 20;
            }
            else
            {
                if (_enemyLostCountdown > 0)
                {
                    _enemyLostCountdown--;
                }
                else
                {
                    _currentState = AIState.Patrol;
                }
            }

            RunBehavior();
        }

        private void RunBehavior()
        {
            switch (_currentState)
            {
                case AIState.Patrol:
                    PatrolBehavior.Run();
                    break;
                case AIState.Fight:
                    FightBehavior.Run();
                    break;
            }
        }
    }
}