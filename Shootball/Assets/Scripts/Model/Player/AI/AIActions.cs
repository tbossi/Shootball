using Shootball.Model.Behavior;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player.AI
{
    public abstract class AIActions
    {
        protected readonly EnemyRobotModel RobotModel;
        private readonly Graph<Vector3> _navGraph;

        private AIPath _aiPath;
        protected AIPath AIPath => _aiPath ?? (_aiPath = new AIPath(_navGraph));

        public AIActions(EnemyRobotModel robotModel, Graph<Vector3> navGraph)
        {
            RobotModel = robotModel;
            _navGraph = navGraph;
        }

        private RepeatedAction _slowDown;
        public BehaviorState SlowDown()
        {
            if (_slowDown == null) { _slowDown = new RepeatedAction(RobotModel.SlowDown, 5); }
            var result = _slowDown.RunOnce();
            if (result == BehaviorState.Complete) { _slowDown = null; }
            return result;
        }
    }
}