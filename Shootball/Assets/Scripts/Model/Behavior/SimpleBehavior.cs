using System;

namespace Shootball.Model.Behavior
{
    public class SimpleBehavior : AIBehavior
    {
        private readonly Func<BehaviorState> _action;

        public SimpleBehavior(Func<BehaviorState> action) : base()
        {
            _action = action;
        }

        public override BehaviorState Run()
        {
            State = _action.Invoke();
            return State;
        }
    }
}